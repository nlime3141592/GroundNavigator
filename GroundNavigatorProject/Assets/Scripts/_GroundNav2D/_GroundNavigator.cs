using System;
using System.Collections.Generic;
using UnityEngine;

namespace JlMetroidvaniaProject.MapManagement
{
    public class _GroundNavigator : MonoBehaviour
    {
        [Range(0.01f, 3.0f)]
        public float thickness = 0.1f;

        public Transform _plains;
        public Transform _joints;

        public _GroundNavigatorProperty properties;

        public void Reset()
        {
            _Validate();
        }

        public void OnValidate()
        {
            _Validate();
        }

        public void _Validate()
        {
            CheckField();
            properties.Calculate(_joints, thickness);

            _Ground[] grounds = GetComponentsInChildren<_Ground>(includeInactive: true);

            foreach(_Ground g in grounds)
            {
                g._Validate();
            }
        }

        private void CheckField()
        {
            if(properties == null)
                properties = new _GroundNavigatorProperty();
        }
    }
}