using System;
using System.Collections.Generic;
using UnityEngine;

namespace JlMetroidvaniaProject.MapManagement
{
    [ExecuteInEditMode]
    public class __Ground : MonoBehaviour
    {
        protected __GroundNavigator navigator => m_navigator;
        protected __GroundNavigatorProperty properties => m_properties;

        private __GroundNavigator m_navigator;
        private __GroundNavigatorProperty m_properties;

        private void Start()
        {
            if(IsPlaying())
                return;

            OnInitialize();
        }

        private void Reset()
        {
            if(IsPlaying())
                return;

            OnInitialize();
        }

        private void OnValidate()
        {
            if(IsPlaying())
                return;

            if(transform.hasChanged)
            {
                OnTransformChange();
            }

            OnLogicUpdate();
        }

        private void Update()
        {
            if(IsPlaying())
                return;

            if(transform.hasChanged)
            {
                OnInitialize();
                OnTransformChange();
            }

            OnLogicUpdate();
        }

        private void OnDrawGizmos()
        {
            OnDrawGizmo();
        }

        protected virtual void OnInitialize()
        {
            __GroundNavigator nav = GetComponentInParent<__GroundNavigator>();

            if(nav != null)
            {
                if(m_navigator == null)
                    m_navigator = nav;
                else if(nav != m_navigator)
                    m_navigator = nav;
            }

            m_properties = m_navigator.GetProperties();
        }

        protected virtual void OnTransformChange()
        {

        }

        protected virtual void OnLogicUpdate()
        {

        }

        protected virtual void OnDrawGizmo()
        {

        }

        private bool IsPlaying()
        {
            return Application.IsPlaying(this);
        }

        protected virtual bool CanNavigate()
        {
            return m_navigator != null;
        }
    }
}