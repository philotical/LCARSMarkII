using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LCARSMarkII
{
    public class LCARS_AudioLibrary
    {
        private static string LCARS_Plugin_Path = "LCARSMarkII/";
        private string LCARSSoundFilePath = LCARS_Plugin_Path + "sounds/";
        public string LCARSSoundFile = "transporterbeam";
        public float LCARSSoundVolume = 6.2f;
        public float LCARSSoundStartTime = 0f;
        public bool loopLCARSSound = false;
        public FXGroup LCARSSound = null;

        public void init(string SoundFilePath)
        {
            if (LCARSSound == null)
            {
                this.LCARSSoundFilePath = SoundFilePath;
                LCARSSound = new FXGroup("LCARSSound");
                GameObject audioObj = new GameObject();
                audioObj.transform.position = FlightGlobals.ActiveVessel.transform.position;
                audioObj.transform.parent = FlightGlobals.ActiveVessel.transform;	// add to parent
                LCARSSound.audio = audioObj.AddComponent<AudioSource>();
                LCARSSound.audio.dopplerLevel = 0f;
                //LCARSSound.audio.Stop();
                LCARSSound.audio.loop = false;
                //LCARSSound.audio.Play();
                LCARSSound.audio.enabled = true;
                LCARSSound.audio.time = 0;
            }
        }
        public void setSoundFilePath(string SoundFilePath)
        {
            this.LCARSSoundFilePath = SoundFilePath;
        }
        public void setSoundStartTime(float SoundStartTime)
        {
            this.LCARSSoundStartTime = SoundStartTime;
        }

        
        public void play(string SoundFileName, Vessel obj, bool loop=false)
        {
            if (LCARSSound==null)
            {
                init(this.LCARSSoundFilePath);
            }
            string clip = this.LCARSSoundFilePath + SoundFileName;
            LCARSSound.audio.loop = loop;
            LCARSSound.audio.transform.position = obj.transform.position;
            LCARSSound.audio.transform.parent = obj.transform;	// add to parent
            LCARSSound.audio.clip = GameDatabase.Instance.GetAudioClip(clip);

            //AudioSource foo = new AudioSource();
            //foo.clip = GameDatabase.Instance.GetAudioClip(clip);
            //AudioSource.PlayClipAtPoint(LCARSSound.audio.clip, LCARSSound.audio.transform.position);
            //_play();
        }
        public void play(string SoundFileName, Transform transform, bool loop = false)
        {
            if (LCARSSound == null)
            {
                init(this.LCARSSoundFilePath);
            }
            string clip = this.LCARSSoundFilePath + SoundFileName;
            LCARSSound.audio.loop = loop;
            LCARSSound.audio.transform.position = transform.position;
            LCARSSound.audio.transform.parent = transform;	// add to parent
            LCARSSound.audio.clip = GameDatabase.Instance.GetAudioClip(clip);

            //AudioSource foo = new AudioSource();
            //foo.clip = GameDatabase.Instance.GetAudioClip(clip);
            //AudioSource.PlayClipAtPoint(LCARSSound.audio.clip, LCARSSound.audio.transform.position);
            //_play();
        }
        public void play(string SoundFileName, GameObject obj, bool loop = false)
        {
            if (LCARSSound == null)
            {
                init(this.LCARSSoundFilePath);
            }
            string clip = this.LCARSSoundFilePath + SoundFileName;
            LCARSSound.audio.loop = loop;
            LCARSSound.audio.transform.position = obj.transform.position;
            LCARSSound.audio.transform.parent = obj.transform;	// add to parent
            LCARSSound.audio.clip = GameDatabase.Instance.GetAudioClip(clip);

            //AudioSource foo = new AudioSource();
            //foo.clip = GameDatabase.Instance.GetAudioClip(clip);
            //AudioSource.PlayClipAtPoint(LCARSSound.audio.clip, LCARSSound.audio.transform.position);
            //_play();
        }
        public void play(string SoundFileName, Part obj, bool loop = false)
        {
            if (LCARSSound == null)
            {
                init(this.LCARSSoundFilePath);
            }
            string clip = this.LCARSSoundFilePath + SoundFileName;
            LCARSSound.audio.loop = loop;
            LCARSSound.audio.transform.position = obj.transform.position;
            LCARSSound.audio.transform.parent = obj.transform;	// add to parent
            LCARSSound.audio.clip = GameDatabase.Instance.GetAudioClip(clip);

            //AudioSource foo = new AudioSource();
            //foo.clip = GameDatabase.Instance.GetAudioClip(clip);
            //AudioSource.PlayClipAtPoint(LCARSSound.audio.clip, LCARSSound.audio.transform.position);
            //_play();
        }
        public void end()
        {
            LCARSSound.audio.enabled = false;
        }
        private void _play()
        {
            if (LCARSSound != null && LCARSSound.audio != null)
            {
                UnityEngine.Debug.Log("LCARS_AudioLibrary LCARSSound.audio.clip.name " + LCARSSound.audio.clip.name);


                LCARSSound.audio.enabled = true;
                //LCARSSound.audio.enabled = false;
                LCARSSound.audio.loop = this.loopLCARSSound;
                LCARSSound.audio.time = 0;
                float soundVolume = GameSettings.SHIP_VOLUME * LCARSSoundVolume;
                LCARSSound.audio.volume = soundVolume;
                UnityEngine.Debug.Log("LCARS_AudioLibrary audio.Play done ");
                //LCARSSound.audio.enabled = false;
                AudioSource.PlayClipAtPoint(LCARSSound.audio.clip, LCARSSound.audio.transform.position);
            }
            else
            {
                UnityEngine.Debug.Log("LCARS_AudioLibrary LCARSSound.audio failed ");
            }
        }
    }
}
