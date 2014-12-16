using UnityEngine;
using System.Collections;


public class AppScript : MonoBehaviour
{
    private Transform m_DragObject = null;
    private Vector3 m_DragOffset;

    private ObjectScript m_ActiveObject = null;

    public LineRenderer linkLine;
    public Transform GreenPrefab;
    public Transform YellowPrefab;

    private Transform GetObjectUnderCursor()
    { 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform;
        }
        return null;
    }
    private Vector3 GetPosOnCameraPlane(Transform aPlanePos)
    {
        Camera cam = Camera.main;
        Plane plane = new Plane(-cam.transform.forward,aPlanePos.position);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        float dist;
        if (plane.Raycast(ray, out dist))
        {
            return ray.GetPoint(dist);
        }
        // Shouldn't be possible
        return Vector3.zero;
    }

    private void PerformDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_DragObject = GetObjectUnderCursor();
            if (m_DragObject != null)
            {
                m_DragOffset = GetPosOnCameraPlane(m_DragObject) - m_DragObject.position;
                if (m_DragObject.rigidbody != null)
                    m_DragObject.rigidbody.velocity = m_DragObject.rigidbody.angularVelocity = Vector3.zero;
            }
        }
        if (Input.GetMouseButtonUp(0))
            m_DragObject = null;
        if (m_DragObject != null)
        {
            var P = GetPosOnCameraPlane(m_DragObject);
            m_DragObject.position = P - m_DragOffset;
        }
    }

    void PerformParenting()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var O = GetObjectUnderCursor();
            if (O == null)
                return;
            m_ActiveObject = O.GetComponent<ObjectScript>();
        }
        if (m_ActiveObject != null )
        {
            linkLine.enabled = true;
            linkLine.SetPosition(0, m_ActiveObject.transform.position);
            linkLine.SetPosition(1, GetPosOnCameraPlane(m_ActiveObject.transform));
            
            if (Input.GetMouseButtonUp(1))
            {
                var O = GetObjectUnderCursor();
                while(O != null && O.tag != "group" )
                {
                    var objectScript = O.GetComponent<ObjectScript>();
                    if (objectScript != null && objectScript.GroupObject != null)
                        O = objectScript.GroupObject;
                    else
                        O = O.parent;
                }
                m_ActiveObject.SetGroup(O);
                m_ActiveObject = null;
            }
        }
        else
            linkLine.enabled = false;
    }

    private void Update ()
    {
        PerformDrag();
        PerformParenting();
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0,0,Screen.width, Screen.height));
        GUILayout.BeginHorizontal();
        GUILayout.Space(100);        
        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical();
        if (GUILayout.Button("New"))
            Instantiate(GreenPrefab);
        GUILayout.FlexibleSpace();   
        if (GUILayout.Button("New"))
            Instantiate(YellowPrefab);

        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(GUILayout.Width(100));
        if (GUILayout.Button("Reload"))
            Application.LoadLevel(0);
        GUILayout.FlexibleSpace();   
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
}
