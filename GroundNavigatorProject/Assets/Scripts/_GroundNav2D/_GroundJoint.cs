using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JlMetroidvaniaProject.MapManagement
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class _GroundJoint : _Ground
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
        public float gizmoSize = 1.0f;

        private _GroundNavigator m_navigator;
        private CircleCollider2D m_circleCollider;
        private BoxCollider2D m_boxCollider;
        public Vector2 boxJointSize => m_boxCollider.size;

        public enum JointShape
        {
            Circle, Box
        }

        public enum LedgePivot
        {
            LeftTop, Top, RightTop, Left, Center, Right, LeftBottom, Bottom, RightBottom
        }

        private void OnValidate()
        {
            m_navigator = GetComponentInParent<_GroundNavigator>();
            transform.position = jointPosition;
            m_navigator._Validate();
        }

        public override void _Validate()
        {
            m_navigator = GetComponentInParent<_GroundNavigator>();
            m_circleCollider = GetComponent<CircleCollider2D>();
            m_boxCollider = GetComponent<BoxCollider2D>();
            _GroundNavigatorProperty properties = m_navigator.properties;

            int childIndex = this.transform.GetSiblingIndex();
            int index = 2 * childIndex;

            m_circleCollider.enabled = jointShape == JointShape.Circle;
            m_boxCollider.enabled = jointShape == JointShape.Box;

            if(index >= properties.m_count)
            {
                transform.eulerAngles = Vector3.zero;
                gameObject.SetActive(false);
            }
            else
            {
                if(syncBySlopeAngle)
                    transform.eulerAngles = Vector3.forward * properties.m_eulerAngles[index];
                else
                    transform.eulerAngles = Vector3.zero;
                transform.localScale = Vector3.one;

                m_boxCollider.size = properties.m_boxSizes[index];
                m_circleCollider.radius = properties._thickness * 0.5f;

                gameObject.SetActive(true);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;

            if(jointShape == JointShape.Circle)
                Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0.0f), gizmoSize * 0.5f);
            else if(jointShape == JointShape.Box)
                Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(gizmoSize, gizmoSize, 0));
        }
    }
}