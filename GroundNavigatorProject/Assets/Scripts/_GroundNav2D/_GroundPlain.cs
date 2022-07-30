using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JlMetroidvaniaProject.MapManagement
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class _GroundPlain : _Ground
    {
        public _GroundNavigator m_navigator;
        public BoxCollider2D m_boxCollider;

        private void OnValidate()
        {
            m_navigator = GetComponentInParent<_GroundNavigator>();
            m_navigator._Validate();
        }

        public override void _Validate()
        {
            m_navigator = GetComponentInParent<_GroundNavigator>();
            m_boxCollider = GetComponent<BoxCollider2D>();
            _GroundNavigatorProperty properties = m_navigator.properties;

            int childIndex = this.transform.GetSiblingIndex();
            int index = 2 * childIndex + 1;

            if(index >= properties.m_count)
            {
                gameObject.SetActive(false);
            }
            else
            {
                transform.position = properties.m_positions[index];
                transform.eulerAngles = Vector3.forward * properties.m_eulerAngles[index];
                transform.localScale = Vector3.one;

                m_boxCollider.size = properties.m_boxSizes[index];

                // gameObject.SetActive(true);
                gameObject.SetActive(properties.m_overlapGrounds[index]);
            }
        }

        private void OnDrawGizmos()
        {
            if(m_boxCollider == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;

            Vector2 center = transform.position;
            float r = m_boxCollider.size.x * 0.5f;
            float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * r;

            Gizmos.DrawLine(center - dir, center + dir);
        }
    }
}