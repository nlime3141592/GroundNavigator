using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ground : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        OnUpdate();
    }

    protected virtual void OnValidate()
    {
        OnUpdate();
    }

    protected virtual void Start()
    {
        OnUpdate();
    }

    private void OnUpdate()
    {
        if(this.gameObject.activeSelf)
        {
            SetFields();
            SetOtherSettings();

            OnPropertyChange();
        }
    }

    private void OnPropertyChange()
    {
        GroundNavigator navigator = GetComponentInParent<GroundNavigator>();
        navigator.OnUpdate();
    }

    protected void SetGroundObjectInfo(string name, string tag, string layerName)
    {
        GameObject obj = this.gameObject;

        if(name != null) obj.name = name;
        if(tag != null) obj.tag = tag;
        if(layerName != null) obj.layer = LayerMask.NameToLayer(layerName);
    }

    protected abstract void SetFields();
    protected abstract void SetOtherSettings();
}