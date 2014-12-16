using System;
using System.Collections.Generic;
using UnityEngine;

/* this comes from:
https://github.com/Mihara/RasterPropMonitor/blob/master/LICENSE.md
RasterPropMonitor  Copyright (C) 2013-2014 
by Mihara (Eugene Medvedev) and other contributors
This program comes with ABSOLUTELY NO WARRANTY!
This is free software, and you are welcome to redistribute it
under certain conditions, as outlined in the full content of
the GNU General Public License (GNU GPL), version 3, revision
date 29 June 2007.

FULL LICENSE CONTENT:

                GNU GENERAL PUBLIC LICENSE
                   Version 3, 29 June 2007
*/

namespace Belerophon
{
	// So this is my attempt at reconstructing sfr's transparent internals plugin from first principles.
	// Because I really don't understand some of the things he's doing, why are they breaking, and
	// I'm going to attempt to recreate this module from scratch.

    internal class JSITransparentPod : PartModule
    {
        [KSPField]
        public string transparentTransforms = string.Empty;
        [KSPField]
        public string transparentShaderName = "Transparent/Specular";
        [KSPField]
        public string opaqueShaderName = string.Empty;
        [KSPField]
        public bool restoreShadersOnIVA = true;
        // I would love to know what exactly possessed Squad to have the IVA space use it's own coordinate system.
        // This rotation adjusts IVA space to match the 'real' space...
        private Quaternion MagicalVoodooRotation = new Quaternion(0, 0.7f, -0.7f, 0);
        private Part knownRootPart;
        private Vessel lastActiveVessel;
        private Quaternion originalRotation;
        private Transform originalParent;
        private Vector3 originalPosition;
        private Shader transparentShader, opaqueShader;
        private bool hasOpaqueShader;
        private readonly Dictionary<Transform, Shader> shadersBackup = new Dictionary<Transform, Shader>();
        public override string GetInfo()
        {
            return "The windows of this capsule have been carefully cleaned.";
        }
        public override void OnStart(StartState state)
        {
            JUtil.LogMessage(this, "Cleaning pod windows...");
            // Apply shaders to transforms on startup.
            if (!string.IsNullOrEmpty(transparentTransforms))
            {
                transparentShader = Shader.Find(transparentShaderName);
                foreach (string transformName in transparentTransforms.Split('|'))
                {
                    try
                    {
                        Transform tr = part.FindModelTransform(transformName.Trim());
                        if (tr != null)
                        {
                            // We both change the shader and backup the original shader so we can undo it later.
                            Shader backupShader = tr.renderer.material.shader;
                            tr.renderer.material.shader = transparentShader;
                            shadersBackup.Add(tr, backupShader);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e, this);
                    }
                }
            }
            if (!string.IsNullOrEmpty(opaqueShaderName))
            {
                opaqueShader = Shader.Find(opaqueShaderName);
                hasOpaqueShader = true;
            }
            // In Editor, the camera we want to change is called "Main Camera". In flight, the camera to change is
            // "Camera 00", i.e. close range camera.
            if (state == StartState.Editor)
            {
                // I'm not sure if this change is actually needed, even. Main Camera's culling mask seems to already include IVA objects,
                // they just don't normally spawn them.
                JUtil.SetCameraCullingMaskForIVA("Main Camera", true);
            }
            // If the internal model has not yet been created, try creating it and log the exception if we fail.
            if (part.internalModel == null)
            {
                try
                {
                    part.CreateInternalModel();
                }
                catch (Exception e)
                {
                    Debug.LogException(e, this);
                }
            }
            // If we ended up with an existing internal model, rotate it now, so that it is shown correctly in the editor.
            if (part.internalModel != null)
            {
                if (state == StartState.Editor)
                {
                    // Just rotating the internal is sufficient in this case.
                    part.internalModel.transform.localRotation = MagicalVoodooRotation;
                }
                else
                {
                    // Else this is our first startup in flight scene, so we reset the IVA.
                    ResetIVA();
                }
            }
            else
            {
                // Some error-proofing. I won't bother doing this every frame, because one error message
                // should suffice, this module is not supposed to be attached to parts that have no internals in the first place.
                JUtil.LogErrorMessage(this, "Wait, where's my internal model?");
            }
        }
        private void SetShaders(bool state)
        {
            if (restoreShadersOnIVA)
            {
                if (state)
                {
                    foreach (KeyValuePair<Transform, Shader> backup in shadersBackup)
                    {
                        backup.Key.renderer.material.shader = transparentShader;
                    }
                }
                else
                {
                    foreach (KeyValuePair<Transform, Shader> backup in shadersBackup)
                    {
                        backup.Key.renderer.material.shader = hasOpaqueShader ? opaqueShader : backup.Value;
                    }
                }
            }
        }
        private void ResetIVA()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                JUtil.LogMessage(this, "Need to reset IVA in part ", part.partName);
                // Now the cruical bit.
                // If the root part changed, we actually need to recreate the IVA forcibly even if it still exists.
                if (vessel.rootPart != knownRootPart)
                {
                    // In this case we also need to kick the user out of IVA if they're currently in our pod,
                    // otherwise lots of things screw up in a bizarre fashion.
                    if (JUtil.UserIsInPod(part))
                    {
                        JUtil.LogMessage(this, "The user is in pod {0} and I need to kick them out.", part.partName);
                        CameraManager.Instance.SetCameraFlight();
                    }
                    // This call not just reinitialises the IVA, but also destroys the existing one, if any,
                    // and reloads all the props and modules.
                    JUtil.LogMessage(this, "Need to actually respawn the IVA model in part {0}", part.partName);
                    part.CreateInternalModel();
                }
                // But otherwise the existing one will serve.
                // If the internal model doesn't yet exist, this call will implicitly create it anyway.
                // It will also initialise it, which in this case implies moving it into the correct location in internal space
                // and populate it with crew, which is what we want.
                part.SpawnCrew();
                // Once that happens, the internal will have the correct location for viewing from IVA relative to the
                // current active vessel. (Yeah, internal space is bizarre like that.) So we make note of it.
                originalParent = part.internalModel.transform.parent;
                originalPosition = part.internalModel.transform.localPosition;
                originalRotation = part.internalModel.transform.localRotation;
                // And then we remember the root part and the active vessel these coordinates refer to.
                knownRootPart = vessel.rootPart;
                lastActiveVessel = FlightGlobals.ActiveVessel;
            }
        }
        // When our part is destroyed, we need to be sure to undo the culling mask change before we leave.
        public void OnDestroy()
        {
            JUtil.SetMainCameraCullingMaskForIVA(false);
        }
        // We also do the same if the part is packed, just in case.
        public virtual void OnPartPack()
        {
            JUtil.SetMainCameraCullingMaskForIVA(false);
        }
        public override void OnUpdate()
        {
            // In the editor, none of this logic should matter, even though the IVA probably exists already.
            if (HighLogic.LoadedSceneIsEditor)
                return;
            // If the root part changed, or the IVA is mysteriously missing, we reset it and take note of where it ended up.
            if (vessel.rootPart != knownRootPart || lastActiveVessel != FlightGlobals.ActiveVessel || part.internalModel == null)
            {
                ResetIVA();
            }
            // Now we need to make sure that the list of portraits in the GUI conforms to what actually is in the active vessel.
            // This is important because IVA/EVA buttons clicked on kerbals that are not in the active vessel cause problems
            // that I can't readily debug, and it shouldn't happen anyway.
            // Only the pods in the active vessel should be doing it since the list refers to them.
            if (vessel.isActiveVessel)
            {
                // First, every pod should check through the list of portaits and remove everyone who is from some other vessel.
                var stowaways = new List<Kerbal>();
                foreach (Kerbal thatKerbal in KerbalGUIManager.ActiveCrew)
                {
                    if (thatKerbal.InVessel != vessel)
                    {
                        stowaways.Add(thatKerbal);
                    }
                }
                foreach (Kerbal thatKerbal in stowaways)
                {
                    KerbalGUIManager.RemoveActiveCrew(thatKerbal);
                }
                // Then, every pod should check the list of seats in itself and see if anyone is missing who should be present.
                foreach (InternalSeat seat in part.internalModel.seats)
                {
                    if (seat.kerbalRef != null && !KerbalGUIManager.ActiveCrew.Contains(seat.kerbalRef))
                    {
                        KerbalGUIManager.AddActiveCrew(seat.kerbalRef);
                    }
                }
            }
            // So we do have an internal model, right?
            if (part.internalModel != null)
            {
                if (JUtil.IsInIVA())
                {
                    // If the user is IVA, we move the internals to the original position,
                    // so that they show up correctly on InternalCamera. This particularly concerns
                    // the pod the user is inside of.
                    part.internalModel.transform.parent = originalParent;
                    part.internalModel.transform.localRotation = originalRotation;
                    part.internalModel.transform.localPosition = originalPosition;
                    if (!JUtil.UserIsInPod(part))
                    {
                        // If the user is in some other pod than this one, we also hide our IVA to prevent them from being drawn above
                        // everything else.
                        part.internalModel.SetVisible(false);
                    }
                    // Unfortunately even if I do that, it means that at least one kerbal on the ship will see his own IVA twice in two different orientations,
                    // one time through the InternalCamera (which I can't modify) and another through the Camera 00.
                    // So we have to also undo the culling mask change as well.
                    JUtil.SetMainCameraCullingMaskForIVA(false);
                    // So once everything is hidden again, we undo the change in shaders to conceal the fact that you can't see other internals.
                    SetShaders(false);
                }
                else
                {
                    // Otherwise, we're out of IVA, so we can proceed with setting up the pods for exterior view.
                    JUtil.SetMainCameraCullingMaskForIVA(true);
                    // Make the internal model visible...
                    part.internalModel.SetVisible(true);
                    // And for a good measure we make sure the shader change has been applied.
                    SetShaders(true);
                    // Now we attach the restored IVA directly into the pod at zero local coordinates and rotate it,
                    // so that it shows up on the main outer view camera in the correct location.
                    part.internalModel.transform.parent = part.transform;
                    part.internalModel.transform.localRotation = MagicalVoodooRotation;
                    part.internalModel.transform.localPosition = Vector3.zero;
                }
            }
        }
    }
    // And this is a stop gap measure.
    // In the particular case where the user is controlling a vessel where the root pod is not
    // a transparent pod, but a transparent pod is within physics range,
    // the IVA of the non-transparent pod will be visible while the user is out of it.
    // And it won't be in the correct position either.
    // Which is why this module need to be added to every non-transparent pod with IVA
    // that does not have a JSITransparentPod module on it with ModuleManager to hide the IVA
    // in the case that happens.
    public class JSINonTransparentPod : PartModule
    {
        /* Correction. You can actually do this, so this method is unnecessary.
        * So much the better.
        // Since apparently, current versions of ModuleManager do not allow multiple
        // "HAS" directives, the easier course of action to only apply this module to
        // pods that are not transparent is to apply it to every pod,
        // and then make it self-destruct if the pod is in fact transparent.
        public override void OnStart(StartState state)
        {
            if (state != StartState.Editor) 
            {
                foreach (PartModule thatModule in part.Modules) {
                    if (thatModule is JSITransparentPod) {
                        Destroy(this);
                    }
                }
            }
        }
        */
        // During the drawing of the GUI, when the portraits are to be drawn, if the internal exists, it should be visible,
        // so that portraits show up correctly.
        public void OnGUI()
        {
            if (JUtil.cameraMaskShowsIVA && vessel.isActiveVessel && part.internalModel != null)
            {
                part.internalModel.SetVisible(true);
            }
        }
        // But before the rest of the world is to be drawn, if the internal exists and is the active internal,
        // it should become invisible.
        public void LateUpdate()
        {
            if (JUtil.cameraMaskShowsIVA && vessel.isActiveVessel && part.internalModel != null && !JUtil.UserIsInPod(part))
            {
                part.internalModel.SetVisible(false);
            }
        }
    }






    public static class JUtil
    {
        public static bool cameraMaskShowsIVA = false;

        public static void SetLayer(this Transform trans, int layer)
        {
            trans.gameObject.layer = layer;
            foreach (Transform child in trans)
                child.SetLayer(layer);
        }
        public static void SetCameraCullingMaskForIVA(string cameraName, bool flag)
        {
            Camera thatCamera = JUtil.GetCameraByName(cameraName);
            if (thatCamera != null)
            {
                if (flag)
                {
                    thatCamera.cullingMask |= 1 << 16 | 1 << 20;
                }
                else
                {
                    thatCamera.cullingMask &= ~(1 << 16 | 1 << 20);
                }
            }
            else
                Debug.Log("Could not find camera \"" + cameraName + "\" to change it's culling mask, check your code.");
        }
        public static bool IsPodTransparent(Part thatPart)
        {
            foreach (PartModule thatModule in thatPart.Modules)
            {
                if (thatModule is JSITransparentPod)
                {
                    return true;
                }
            }
            return false;
        }
        public static void SetMainCameraCullingMaskForIVA(bool flag)
        {
            SetCameraCullingMaskForIVA("Camera 00", flag);
            cameraMaskShowsIVA = flag;
        }
        public static void MakeReferencePart(this Part thatPart)
        {
            if (thatPart != null)
            {
                foreach (PartModule thatModule in thatPart.Modules)
                {
                    var thatNode = thatModule as ModuleDockingNode;
                    var thatPod = thatModule as ModuleCommand;
                    var thatClaw = thatModule as ModuleGrappleNode;
                    if (thatNode != null)
                    {
                        thatNode.MakeReferenceTransform();
                        break;
                    }
                    if (thatPod != null)
                    {
                        thatPod.MakeReference();
                        break;
                    }
                    if (thatClaw != null)
                    {
                        thatClaw.MakeReferenceTransform();
                    }
                }
            }
        }
        /* I wonder why this isn't working.
        * It's like the moment I unseat a kerbal, no matter what else I do,
        * the entire internal goes poof. Although I'm pretty sure it doesn't quite,
        * because the modules keep working and generating errors.
        * What's really going on here, and why the same thing works for Crew Manifest?
        public static void ReseatKerbalInPart(this Kerbal thatKerbal) {
        if (thatKerbal.InPart == null || !JUtil.VesselIsInIVA(thatKerbal.InPart.vessel))
        return;
        InternalModel thatModel = thatKerbal.InPart.internalModel;
        Part thatPart = thatKerbal.InPart;
        int spareSeat = thatModel.GetNextAvailableSeatIndex();
        if (spareSeat >= 0) {
        ProtoCrewMember crew = thatKerbal.protoCrewMember;
        CameraManager.Instance.SetCameraFlight();
        thatPart.internalModel.UnseatKerbal(crew);
        thatPart.internalModel.SitKerbalAt(crew,thatPart.internalModel.seats[spareSeat]);
        thatPart.internalModel.part.vessel.SpawnCrew();
        CameraManager.Instance.SetCameraIVA(thatPart.internalModel.seats[spareSeat].kerbalRef,true);
        }
        }
        */
        public static bool ActiveKerbalIsLocal(this Part thisPart)
        {
            return FindCurrentKerbal(thisPart) != null;
        }
        public static int CurrentActiveSeat(this Part thisPart)
        {
            Kerbal activeKerbal = thisPart.FindCurrentKerbal();
            return activeKerbal != null ? activeKerbal.protoCrewMember.seatIdx : -1;
        }
        public static Kerbal FindCurrentKerbal(this Part thisPart)
        {
            if (thisPart.internalModel == null || !JUtil.VesselIsInIVA(thisPart.vessel))
                return null;
            // InternalCamera instance does not contain a reference to the kerbal it's looking from.
            // So we have to search through all of them...
            Kerbal thatKerbal = null;
            foreach (InternalSeat thatSeat in thisPart.internalModel.seats)
            {
                if (thatSeat.kerbalRef != null)
                {
                    if (thatSeat.kerbalRef.eyeTransform == InternalCamera.Instance.transform.parent)
                    {
                        thatKerbal = thatSeat.kerbalRef;
                        break;
                    }
                }
            }
            return thatKerbal;
        }
        public static Camera GetCameraByName(string name)
        {
            foreach (Camera cam in Camera.allCameras)
            {
                if (cam.name == name)
                {
                    return cam;
                }
            }
            return null;
        }
        public static bool VesselIsInIVA(Vessel thatVessel)
        {
            // Inactive IVAs are renderer.enabled = false, this can and should be used...
            // ... but now it can't because we're doing transparent pods, so we need a more complicated way to find which pod the player is in.
            return IsActiveVessel(thatVessel) && IsInIVA();
        }
        public static bool UserIsInPod(Part thisPart)
        {
            // Just in case, check for whether we're not in flight.
            if (!HighLogic.LoadedSceneIsFlight)
                return false;
            // If we're not in IVA, or the part does not have an instantiated IVA, the user can't be in it.
            if (!VesselIsInIVA(thisPart.vessel) || thisPart.internalModel == null)
                return false;
            // Now that we got that out of the way, we know that the user is in SOME pod on our ship. We just don't know which.
            // Let's see if he's controlling a kerbal in our pod.
            if (ActiveKerbalIsLocal(thisPart))
                return true;
            // There still remains an option of InternalCamera which we will now sort out.
            if (CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Internal)
            {
                // So we're watching through an InternalCamera. Which doesn't record which pod we're in anywhere, like with kerbals.
                // But we know that if the camera's transform parent is somewhere in our pod, it's us.
                // InternalCamera.Instance.transform.parent is the transform the camera is attached to that is on either a prop or the internal itself.
                // The problem is figuring out if it's in our pod, or in an identical other pod.
                // Unfortunately I don't have anything smarter right now than get a list of all transforms in the internal and cycle through it.
                // This is a more annoying computation than looking through every kerbal in a pod (there's only a few of those,
                // but potentially hundreds of transforms) and might not even be working as I expect. It needs testing.
                foreach (Transform thisTransform in thisPart.internalModel.GetComponentsInChildren<Transform>())
                {
                    if (thisTransform == InternalCamera.Instance.transform.parent)
                        return true;
                }
            }
            return false;
        }
        public static bool IsActiveVessel(Vessel thatVessel)
        {
            return (HighLogic.LoadedSceneIsFlight && thatVessel != null && thatVessel.isActiveVessel);
        }
        public static bool IsInIVA()
        {
            return CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.IVA || CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Internal;
        }
        public static void LogMessage(object caller, string line, params object[] list)
        {
                Debug.Log(String.Format(caller.GetType().Name + ": " + line, list));
        }
        public static void LogErrorMessage(object caller, string line, params object[] list)
        {
            Debug.LogError(String.Format(caller.GetType().Name + ": " + line, list));
        }
    }









}

