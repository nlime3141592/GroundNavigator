using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class GroundJoint : Ground
{
    [Header("Joint Collider Settings")]
    public bool jointEnable = true;
    public JointShape jointShape = JointShape.Circle;
    public bool ledgeGrabAvailable = false;
    public bool syncBySlopeAngle = true;
    public Vector2 jointPosition;

    [Header("Joint Gizmo Settings")]
    public bool gizmoEnable = true;
    public Color gizmoColor = Color.cyan;
    public float gizmoSize = 0.5f;

    public CircleCollider2D circle => circleJoint;
    public BoxCollider2D box => boxJoint;

    private CircleCollider2D circleJoint;
    private BoxCollider2D boxJoint;

    public enum JointShape
    {
        Circle, Box
    }

    protected override void OnValidate()
    {
        transform.position = jointPosition;
        
        base.OnValidate();
    }

    protected override void SetFields()
    {
        if(circleJoint == null)
            circleJoint = GetComponent<CircleCollider2D>();

        if(boxJoint == null)
            boxJoint = GetComponent<BoxCollider2D>();
    }

    protected override void SetOtherSettings()
    {
        if(circleJoint != null)
        {
            circleJoint.enabled = (jointEnable && jointShape == JointShape.Circle);
        }

        if(boxJoint != null)
        {
            boxJoint.enabled = (jointEnable && jointShape == JointShape.Box);
        }
    }

    private void OnDrawGizmos()
    {
        if(gizmoEnable)
        {
            Gizmos.color = gizmoColor;

            if(jointShape == JointShape.Circle)
                Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0), gizmoSize * 0.5f);
            else if(jointShape == JointShape.Box)
                Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(gizmoSize, gizmoSize, 0));
        }
    }
}