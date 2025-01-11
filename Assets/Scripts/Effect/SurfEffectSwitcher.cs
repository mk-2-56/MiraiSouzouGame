using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEngine.VFX;
using System;

//[CustomEditor(typeof(SurfEffectSwitcher))]
//public class TestInspector : UnityEditor.Editor
//{
//    string _assetName = "";

//    public override void OnInspectorGUI()
//    { 
//        DrawDefaultInspector();
        
//        GUILayout.Label("Asset Name");
//        _assetName = GUILayout.TextField(_assetName);

//        if(GUILayout.Button(new GUIContent("Generate Asset")))
//        { 
//            if(_assetName == "")
//                return;
//            target.GetType().GetMethod("Initialize").Invoke(target, null);
//            target.GetType().GetMethod("CreatAssetFromSetting").Invoke(target, new[]{_assetName });
//        }
//    }
//}

public class SurfEffectSwitcher : MonoBehaviour
{
    public SurfingEffect _effectPreset;

    public void Initialize()
    {
        _vfx = GetComponent<VisualEffect>();
    }
    public void CreatAssetFromSetting()
    {
        SurfingEffect asset = new();

        asset._vfxAsset = _vfx.visualEffectAsset;

        List<VFXExposedProperty> _properties = new();
        _vfx.visualEffectAsset.GetExposedProperties(_properties);

        var GetProp = new Dictionary<Type, GetVFXProp> 
        {
            { typeof(float), ( string name) => {  return _vfx.GetFloat(name); } },
            { typeof(Gradient), ( string name) => {  return _vfx.GetGradient(name); } },
            { typeof(int), ( string name) => {  return _vfx.GetInt(name); } },
            { typeof(Texture), ( string name) => {  return _vfx.GetTexture(name); } },
            { typeof(Vector3), ( string name) => {  return _vfx.GetVector3(name); } }
        };

        foreach (var entry in _properties)
        {
            Type type = entry.type;
            asset._properties.Add(entry, GetProp[type](entry.name));
        }

        _effectPreset = asset;
    }

    delegate void   SetVFXProp(object prop, string propName);
    delegate object GetVFXProp(string propName);

    public void ApplyAssetToTarget(VisualEffect other)
    {

        var SetProp = new Dictionary<Type, SetVFXProp> 
        {
            { typeof(float), (object prop, string name) => {  other.SetFloat(name, (float)prop); } }, 
            { typeof(Gradient), (object prop, string name) => {  other.SetGradient(name, (Gradient)prop); } },
            { typeof(int), (object prop, string name) => {  other.SetInt(name, (int)prop); } },
            { typeof(Texture), (object prop, string name) => {  other.SetTexture(name, (Texture)prop); } },
            { typeof(Vector3), (object prop, string name) => {  other.SetVector3(name, (Vector3)prop); } }
        };

        foreach (var entry in _effectPreset._properties)
        {
            string propName = entry.Key.name;
            Type type = entry.Key.type;
            SetProp[type](entry.Value, propName);
        }
        other.visualEffectAsset = _effectPreset._vfxAsset;
    }

    VisualEffect _vfx;
    bool _editor;

    void Start()
    {
        Initialize();
        CreatAssetFromSetting();
    }

    private void OnTriggerEnter(Collider other)
    {
        VisualEffect otherVfx = other.transform.Find("Facing/Cog/EffectDispatcher/TrailEffect").GetComponent<VisualEffect>();
        ApplyAssetToTarget(otherVfx);
    }
}

public class SurfingEffect
{
    public VisualEffectAsset _vfxAsset;
    public Dictionary<VFXExposedProperty, object> _properties = new();
}