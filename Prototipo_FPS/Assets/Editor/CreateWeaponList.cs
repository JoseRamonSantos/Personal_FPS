using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateWeaponList : MonoBehaviour
{
    [MenuItem("Assets/Create/Weapon List")]
    public static WeaponList Create()
    {
        WeaponList asset = ScriptableObject.CreateInstance<WeaponList>();
        AssetDatabase.CreateAsset(asset, "Assets/Resources/WeaponList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
