using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(CompositeCollider2D))]
public class GroundNavigator : MonoBehaviour
{
    [Header("Navigator Settings")]
    [Range(0.01f, 3.0f)]
    public float _thickness = 0.1f;
    public Transform _plains;
    public Transform _joints;
    public bool BakeGroundOnEditor;

    private List<Vector2> m_jointPositions;
    private GroundNavigatorProperty m_calculator;
    private Rigidbody2D m_rigidbody;

    private void OnEnable()
    {
        EditorApplication.hierarchyChanged += OnUpdate;
    }

    private void OnDisable()
    {
        EditorApplication.hierarchyChanged -= OnUpdate;
    }

    private void OnValidate()
    {
        SetFields();
        OnUpdate();
    }

    private void Start()
    {
        SetFields();
        OnUpdate();
    }

    private void SetFields()
    {
        if(m_jointPositions == null)
            m_jointPositions = new List<Vector2>();

        if(m_calculator == null)
            m_calculator = new GroundNavigatorProperty();

        if(m_rigidbody == null)
            m_rigidbody = GetComponent<Rigidbody2D>();

        m_rigidbody.bodyType = RigidbodyType2D.Static;

        BakeGroundOnEditor = false;
    }

    public void OnUpdate()
    {
        int jointCount = _joints.childCount;

        m_jointPositions.CheckCapacity(jointCount);

        Vector2[] v = new Vector2[jointCount];
        List<Vector2> vlist = new List<Vector2>(v);

        for(int i = 0; i < jointCount; i++)
        {
            v[i] = _joints.GetChild(i).position;
        }

        m_calculator.Calculate(vlist, jointCount, _thickness);

        CheckGameObjects(_joints, _plains);

        for(int i = 0; i < jointCount; i++)
        {
            UpdateJoint(_joints.GetChild(i).gameObject, m_calculator, i);

            if(i > 0)
                UpdatePlain(_plains.GetChild(i - 1).gameObject, m_calculator, i - 1);
        }
    }

    private void CheckGameObjects(Transform joints, Transform plains)
    {
        int jointCount = joints.childCount;
        int plainCount = plains.childCount;

        for(int i = plainCount; i < jointCount - 1; i++)
        {
            GameObject newObj = new GameObject();
            newObj.transform.SetParent(plains);
        }

        for(int i = 0; i < plainCount; i++)
            plains.GetChild(i).gameObject.SetActive(i < plainCount);
    }

    private GroundPlain UpdatePlain(GameObject plainObj, GroundNavigatorProperty calculator, int index)
    {
        GroundPlain plain = plainObj.ValidateComponent<GroundPlain>();
        Transform ptransform = plain.transform;
        BoxCollider2D box = plain.box;

        ptransform.position = calculator._plainPositions[index];
        ptransform.eulerAngles = calculator._plainEulerAngles[index];
        ptransform.localScale = calculator._plainScale;
        box.size = calculator._plainBoxSizes[index];

        return plain;
    }

    private GroundJoint UpdateJoint(GameObject jointObj, GroundNavigatorProperty calculator, int index)
    {
        GroundJoint joint = jointObj.ValidateComponent<GroundJoint>();
        Transform jtransform = joint.transform;
        BoxCollider2D box = joint.box;
        CircleCollider2D circle = joint.circle;

        // joint.jointPosition = calculator._jointPositions[index];
        jtransform.eulerAngles = joint.syncBySlopeAngle ? calculator._jointEulerAngles[index] : Vector3.zero;
        jtransform.localScale = calculator._jointScale;

        box.size = joint.syncBySlopeAngle ? calculator._jointBoxSizes[index] : Vector2.one * calculator._thickness;
        circle.radius = calculator._jointCircleRadius;

        return joint;
    }
}