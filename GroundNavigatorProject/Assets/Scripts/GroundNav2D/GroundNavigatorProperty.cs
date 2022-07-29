using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GroundNavigatorProperty
{
    public float _thickness;

    public List<Vector2> _plainPositions;
    public List<Vector3> _plainEulerAngles;
    public Vector3 _plainScale;

    public List<Vector2> _jointPositions;
    public List<Vector3> _jointEulerAngles;
    public Vector3 _jointScale;

    public List<Vector2> _plainBoxSizes;
    public List<Vector2> _jointBoxSizes;
    public float _jointCircleRadius;

    private Vector3[] temp_jointEulerAngle;

    public GroundNavigatorProperty()
    {
        _plainPositions = new List<Vector2>();
        _plainEulerAngles = new List<Vector3>();
        _plainScale = Vector3.one;

        // jointPositions here.
        _jointEulerAngles = new List<Vector3>();
        _jointScale = Vector3.one;

        _plainBoxSizes = new List<Vector2>();
        _jointBoxSizes = new List<Vector2>();
        _jointCircleRadius = 0.1f;

        temp_jointEulerAngle = new Vector3[4];
        temp_jointEulerAngle[0] = Vector3.zero;
    }

    public void Calculate(List<Vector2> joints, int jointCount, float thickness)
    {
        _thickness = thickness;
        _jointPositions = joints;

        _plainPositions.CheckCapacity(jointCount - 1);
        _plainEulerAngles.CheckCapacity(jointCount - 1);
        _jointEulerAngles.CheckCapacity(jointCount);
        _plainBoxSizes.CheckCapacity(jointCount - 1);
        _jointBoxSizes.CheckCapacity(jointCount);

        for(int i = 0; i < jointCount; i++)
        {
            if(i < jointCount - 1)
            {
                _plainPositions[i] = GetPlainPosition(_jointPositions[i], _jointPositions[i + 1]);
                _plainEulerAngles[i] = GetPlainEulerAngle(_jointPositions[i], _jointPositions[i + 1]);
                _plainBoxSizes[i] = GetPlainBoxSize(_jointPositions[i], _jointPositions[i + 1], _thickness);
            }

            bool hasLeftPlain = i > 0;
            bool hasRightPlain = i < jointCount - 1;
            Vector3 leftEulerAngle = i > 0 ? _plainEulerAngles[i - 1] : Vector3.zero;
            Vector3 rightEulerAngle = i < jointCount - 1 ? _plainEulerAngles[i] : Vector3.zero;

            _jointBoxSizes[i] = GetJointBoxSize(hasLeftPlain, hasRightPlain, leftEulerAngle, rightEulerAngle, thickness);
            _jointEulerAngles[i] = GetJointEulerAngle(hasLeftPlain, hasRightPlain, leftEulerAngle, rightEulerAngle);
        }

        _plainScale = Vector3.one;
        _jointScale = Vector3.one;
        _jointCircleRadius = GetJointCircleRadius(thickness);
    }

    private Vector2 GetPlainPosition(Vector2 start, Vector2 end)
    {
        return (start + end) * 0.5f;
    }

    private Vector3 GetPlainEulerAngle(Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;

        float angleZ = Vector2.Angle(Vector2.right, direction);
        angleZ *= direction.y < 0 ? -1.0f : 1.0f;

        return Vector3.forward * angleZ;
    }

    private Vector3 GetJointEulerAngle(bool hasLeftPlain, bool hasRightPlain, Vector3 leftEulerAngle, Vector3 rightEulerAngle)
    {
        int l = 0;
        int r = 0;

        if(hasLeftPlain) l = 1;
        if(hasRightPlain) r = 2;

        temp_jointEulerAngle[1] = leftEulerAngle;
        temp_jointEulerAngle[2] = rightEulerAngle;
        temp_jointEulerAngle[3] = Vector3.forward * (leftEulerAngle.z + rightEulerAngle.z) * 0.5f;

        return temp_jointEulerAngle[l + r];
    }

    private Vector2 GetPlainBoxSize(Vector2 start, Vector2 end, float thickness)
    {
        float x = Vector2.Distance(start, end);
        float y = thickness;

        return new Vector2(x, y);
    }

    private Vector2 GetJointBoxSize(bool hasLeftPlain, bool hasRightPlain, Vector3 leftEulerAngle, Vector3 rightEulerAngle, float thickness)
    {
        if(hasLeftPlain && hasRightPlain)
        {
            float angle_a = leftEulerAngle.z + 90.0f;
            float angle_b = rightEulerAngle.z + 90.0f;

            while(angle_a < 0.0f) angle_a += 360.0f;
            while(angle_b < 0.0f) angle_b += 360.0f;

            float angle = angle_a - angle_b;

            if(angle < 0.0f) angle = -angle;
            if(angle > 180.0f) angle = 360.0f - angle;

            angle *= Mathf.Deg2Rad;

            float width = thickness * Mathf.Sqrt(2 * (1 - Mathf.Cos(angle))) * 0.5f;
            float height = thickness * Mathf.Cos(angle * 0.5f);

            return new Vector2(width, height);
        }
        else
        {
            return Vector2.one * thickness;
        }
    }

    private float GetJointCircleRadius(float thickness)
    {
        return thickness * 0.5f;
    }
}