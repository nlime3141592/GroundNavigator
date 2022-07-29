using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GroundPlain : Ground
{
    [Header("Plain Collider Settings")]
    public bool plainEnable = true;

    [Header("Plain Gizmo Settings")]
    public bool gizmoEnable = true;
    public Color gizmoColor = Color.yellow;

    private BoxCollider2D boxPlain;
    private float thickness;

    public BoxCollider2D box => boxPlain;

    protected override void SetFields()
    {
        if(boxPlain == null)
            boxPlain = GetComponent<BoxCollider2D>();
    }

    protected override void SetOtherSettings()
    {
        if(boxPlain != null)
        {
            boxPlain.enabled = plainEnable;
            boxPlain.usedByComposite = true;
        }
    }

    private void OnDrawGizmos()
    {
        if(boxPlain == null && !gizmoEnable)
            return;

        Gizmos.color = gizmoColor;

        Vector2 center = transform.position;
        float r = boxPlain.size.x * 0.5f;
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * r;

        Gizmos.DrawLine(center - dir, center + dir);
    }
}