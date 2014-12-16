using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class ObjectScript : MonoBehaviour
{
    public LineRenderer linkLine;
    void Start()
    {
        if (linkLine == null)
            linkLine = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        if (IsGrouped)
        {
            var group = GroupObject;
            var dir = (group.position - transform.position).normalized;
            rigidbody.AddForce(dir*100);
            linkLine.SetPosition(0,transform.position);
            linkLine.SetPosition(1,group.position);
        }
    }

    public virtual bool IsGrouped
    {
        get { return false;}
    }

    public virtual Transform GroupObject
    {
        get { return null; }
    }
    public virtual void SetGroup(Transform O)
    {
        linkLine.enabled = IsGrouped;
        if (IsGrouped)
        {
            linkLine.SetPosition(0, transform.position);
            linkLine.SetPosition(1, O.position);
        }
    }
}
