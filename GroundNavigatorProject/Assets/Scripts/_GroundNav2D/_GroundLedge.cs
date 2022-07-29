using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JlMetroidvaniaProject.MapManagement
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class _GroundLedge : _Ground
    {
        [Header("Ledge Settings")]
        public LedgePivot ledgePivot = LedgePivot.Center;
        public LedgeShape ledgeShape = LedgeShape.Circle;
        public bool syncBySlopeAngle = true;
        [Range(0.01f, 3.0f)]
        public float ledgeSize = 0.1f;

        [Header("Ledge Gizmo Settings")]
        public bool gizmoEnable = true;
        public Color gizmoColor = Color.magenta;
        public float gizmoSize = 0.6f;

        private _GroundNavigator m_navigator;
        private _GroundJoint m_joint;
        private CircleCollider2D m_circleCollider;
        private BoxCollider2D m_boxCollider;

        public enum LedgePivot
        {
            LeftTop, Top, RightTop, Left, Center, Right, LeftBottom, Bottom, RightBottom
        }

        public enum LedgeShape
        {
            Circle, Box
        }

        private float[] ledgePositionWeight = new float[]
        {
            -1.0f, 1.0f,
            0.0f, 1.0f,
            1.0f, 1.0f,
            -1.0f, 0.0f,
            0.0f, 0.0f,
            1.0f, 0.0f,
            -1.0f, -1.0f,
            0.0f, -1.0f,
            1.0f, -1.0f,
        };

        private void OnValidate()
        {
            m_navigator = GetComponentInParent<_GroundNavigator>();
            m_joint = GetComponentInParent<_GroundJoint>();

            m_navigator._Validate();
        }

        public override void _Validate()
        {
            m_navigator = GetComponentInParent<_GroundNavigator>();
            m_joint = GetComponentInParent<_GroundJoint>();
            m_circleCollider = GetComponent<CircleCollider2D>();
            m_boxCollider = GetComponent<BoxCollider2D>();
            m_circleCollider.isTrigger = true;
            m_boxCollider.isTrigger = true;

            m_circleCollider.enabled = ledgeShape == LedgeShape.Circle;
            m_boxCollider.enabled = ledgeShape == LedgeShape.Box;

            m_boxCollider.size = Vector2.one * ledgeSize;
            m_circleCollider.radius = ledgeSize * 0.5f;

            SetPosition();
        }

        private void SetPosition()
        {
            int pivotIndex = (int)ledgePivot;
            float x = m_joint.boxJointSize.x / 2;
            float y = m_joint.boxJointSize.y / 2;
            float weightX = ledgePositionWeight[pivotIndex * 2];
            float weightY = ledgePositionWeight[pivotIndex * 2 + 1];

            if(syncBySlopeAngle)
                transform.localPosition = new Vector2(x * weightX, y * weightY);
            else
                transform.position = m_joint.transform.position + (Vector3)(new Vector2(x * weightX, y * weightY));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;

            if(ledgeShape == LedgeShape.Circle)
                Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0.0f), gizmoSize * 0.5f);
            else if(ledgeShape == LedgeShape.Box)
                Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(gizmoSize, gizmoSize, 0));
        }
    }
}