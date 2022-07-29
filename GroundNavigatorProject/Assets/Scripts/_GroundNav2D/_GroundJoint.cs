using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JlMetroidvaniaProject.MapManagement
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class _GroundJoint : _Ground
    {
        public _GroundNavigator m_navigator;
        public CircleCollider2D m_circleCollider;
        public BoxCollider2D m_boxCollider;

        public JointShape jointShape = JointShape.Circle;

        public Vector2 position;

        public enum JointShape
        {
            Circle, Box
        }

        private void OnValidate()
        {
            m_navigator = GetComponentInParent<_GroundNavigator>();
            transform.position = position;
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
                gameObject.SetActive(false);
            }
            else
            {
                transform.eulerAngles = Vector3.forward * properties.m_eulerAngles[index];
                transform.localScale = Vector3.one;

                m_boxCollider.size = properties.m_boxSizes[index];
                m_circleCollider.radius = properties._thickness * 0.5f;

                gameObject.SetActive(true);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            if(jointShape == JointShape.Circle)
                Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0.0f), 0.5f);
            else if(jointShape == JointShape.Box)
                Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(1, 1, 0));
        }
    }
}