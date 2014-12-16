using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class ParentScript : ObjectScript
{
    private bool m_IsGrouped = false;
    public override bool IsGrouped
    {
        get { return m_IsGrouped;}
    }

    public override Transform GroupObject
    {
        get { return transform.parent; }
    }

    public override void SetGroup(Transform O)
    {
        m_IsGrouped = O != null;
        transform.parent = O;
        base.SetGroup(O);
    }
}
