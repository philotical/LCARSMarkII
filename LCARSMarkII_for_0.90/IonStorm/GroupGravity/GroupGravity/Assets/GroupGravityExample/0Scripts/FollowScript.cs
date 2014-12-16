using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FollowScript : ObjectScript
{
    private Transform m_Target = null;

    public override bool IsGrouped
    {
        get { return m_Target != null;}
    }

    public override Transform GroupObject
    {
        get { return m_Target; }
    }

    public override void SetGroup(Transform O)
    {
        m_Target = O;
        base.SetGroup(O);
    }
}
