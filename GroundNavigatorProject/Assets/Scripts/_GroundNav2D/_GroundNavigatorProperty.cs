using System;
using System.Collections.Generic;
using UnityEngine;

namespace JlMetroidvaniaProject.MapManagement
{
    [Serializable]
    public class _GroundNavigatorProperty
    {
        public float _thickness;

        // joint, plain, joint, plain, ... , joint, plain, joint
        public int m_count;
        public List<Vector2> m_positions;
        public List<float> m_eulerAngles;
        public List<Vector2> m_boxSizes;
        public List<Transform> m_childJoints;

        public _GroundNavigatorProperty()
        {
            m_count = 0;
            m_positions = new List<Vector2>();
            m_eulerAngles = new List<float>();
            m_boxSizes = new List<Vector2>();
        }

        public void Calculate(Transform joints, float thickness)
        {
            int jCount = joints.childCount;
            int pCount = jCount - 1;
            _thickness = thickness;

            if(jCount < 1)
            {
                m_positions.Clear();
                m_eulerAngles.Clear();
                m_boxSizes.Clear();
                m_childJoints.Clear();
                m_count = 0;
                return;
            }

            m_count = jCount + pCount;

            m_childJoints.Clear();
            for(int i = 0; i < jCount; i++)
                m_childJoints.Add(joints.GetChild(i));

            m_positions._CheckCapacity(m_count);
            m_eulerAngles._CheckCapacity(m_count);
            m_boxSizes._CheckCapacity(m_count);

            // 1. position setting
            // joint positions
            for(int i = 0; i < jCount; i++)
            {
                m_positions[2 * i] = new Vector2(m_childJoints[i].position.x, m_childJoints[i].position.y);
            }
            // plain positions
            for(int i = 0; i < pCount; i++)
            {
                float x = m_positions[2 * i].x + m_positions[2 * i + 2].x;
                float y = m_positions[2 * i].y + m_positions[2 * i + 2].y;
                x /= 2;
                y /= 2;
                m_positions[2 * i + 1] = new Vector2(x, y);
            }
            // 2. euler angle setting
            // plain euler angles
            for(int i = 0; i < pCount; i++)
            {
                int idx_l = 2 * i;
                int idx_p = idx_l + 1;
                int idx_r = idx_l + 2;

                Vector2 plainVector = m_positions[idx_r] - m_positions[idx_l];
                float angle = Vector2.Angle(Vector2.right, plainVector);
                if(plainVector.y < 0) angle *= -1.0f;

                m_eulerAngles[idx_p] = angle;
            }
            // joint euler angles
            if(m_count >= 3)
            {
                m_eulerAngles[0] = m_eulerAngles[1];
                m_eulerAngles[m_count - 1] = m_eulerAngles[m_count - 2];
            }
            else
            {
                m_eulerAngles[0] = 0.0f;
            }
            for(int i = 1; i < jCount - 1; i++)
            {
                int idx_j = 2 * i;
                int idx_l = idx_j - 1;
                int idx_r = idx_j + 1;

                float angle = m_eulerAngles[idx_l] + m_eulerAngles[idx_r];
                angle *= 0.5f;

                m_eulerAngles[idx_j] = angle;
            }
            // 3. box size setting
            // plain box sizes
            for(int i = 0; i < pCount; i++)
            {
                int idx_l = 2 * i;
                int idx_p = idx_l + 1;
                int idx_r = idx_l + 2;

                float x = Vector2.Distance(m_positions[idx_l], m_positions[idx_r]);
                float y = thickness;

                m_boxSizes[idx_p] = new Vector2(x, y);
            }
            // joint box sizes
            m_boxSizes[0] = new Vector2(thickness, thickness);
            m_boxSizes[m_count - 1] = new Vector2(thickness, thickness);
            for(int i = 1; i < jCount - 1; i++)
            {
                int idx_j = 2 * i;
                int idx_l = idx_j - 1;
                int idx_r = idx_j + 1;

                float angle_l = m_eulerAngles[idx_l] + 90.0f;
                float angle_r = m_eulerAngles[idx_r] + 90.0f;

                while(angle_l < 0.0f) angle_l += 360.0f;
                while(angle_r < 0.0f) angle_r += 360.0f;

                float angle = angle_l - angle_r;

                if(angle < 0.0f) angle = -angle;
                if(angle > 180.0f) angle = 360.0f - angle;

                angle *= Mathf.Deg2Rad;

                float width = thickness * Mathf.Sqrt(2 * (1 - Mathf.Cos(angle))) * 0.5f;
                float height = thickness * Mathf.Cos(angle * 0.5f);

                float sin_theta = Mathf.Sin(angle);
                float cos_theta = 1 - (sin_theta * sin_theta);

                float x = thickness * sin_theta;
                float y = thickness * cos_theta;

                m_boxSizes[idx_j] = new Vector2(width, height);
                // m_boxSizes[idx_j] = new Vector2(x, y);
            }
        }
    }
}