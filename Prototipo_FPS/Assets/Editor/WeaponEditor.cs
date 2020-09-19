using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponEditor : EditorWindow
{
    [MenuItem("Window/Weapon Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(WeaponEditor));
    }

    public WeaponList inventoryItemList;
    private int viewIndex = 1;

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = "Assets/Resources/WeaponList.asset";
            inventoryItemList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(WeaponList)) as WeaponList;
        }

        if (inventoryItemList == null)
        {
            viewIndex = 1;

            WeaponList asset = ScriptableObject.CreateInstance<WeaponList>();
            AssetDatabase.CreateAsset(asset, "Assets/Resources/WeaponList.asset");
            AssetDatabase.SaveAssets();

            inventoryItemList = asset;

            if (inventoryItemList)
            {
                inventoryItemList.weaponList = new List<WeaponItem>();
                string relPath = AssetDatabase.GetAssetPath(inventoryItemList);
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void WeaponListMenu()
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Weapon", viewIndex, GUILayout.ExpandWidth(false)), 1, inventoryItemList.weaponList.Count);
        EditorGUILayout.LabelField("of   " + inventoryItemList.weaponList.Count.ToString() + "  weapon", "", GUILayout.ExpandWidth(false));
        GUILayout.EndHorizontal();
        string[] _choices = new string[inventoryItemList.weaponList.Count];
        for (int i = 0; i < inventoryItemList.weaponList.Count; i++)
        {
            _choices[i] = inventoryItemList.weaponList[i].m_name;
        }
        int _choiceIndex = viewIndex - 1;
        viewIndex = EditorGUILayout.Popup(_choiceIndex, _choices) + 1;

        //EditorUtility.SetDirty(someClass);
        GUILayout.Space(10);
        inventoryItemList.weaponList[viewIndex - 1].m_name = EditorGUILayout.TextField("Name", inventoryItemList.weaponList[viewIndex - 1].m_name as string);
        inventoryItemList.weaponList[viewIndex - 1].m_damage = EditorGUILayout.FloatField("Damage", inventoryItemList.weaponList[viewIndex - 1].m_damage);
        inventoryItemList.weaponList[viewIndex - 1].m_fireRate = EditorGUILayout.FloatField("Fire rate", inventoryItemList.weaponList[viewIndex - 1].m_fireRate);
        inventoryItemList.weaponList[viewIndex - 1].m_range = EditorGUILayout.FloatField("Range", inventoryItemList.weaponList[viewIndex - 1].m_range);
        inventoryItemList.weaponList[viewIndex - 1].m_accuracyDropPerShot = EditorGUILayout.FloatField("Accuracy drop per shoot", inventoryItemList.weaponList[viewIndex - 1].m_accuracyDropPerShot);
        inventoryItemList.weaponList[viewIndex - 1].m_hasChamber = (bool)EditorGUILayout.Toggle("Has Chamber", inventoryItemList.weaponList[viewIndex - 1].m_hasChamber);
        inventoryItemList.weaponList[viewIndex - 1].m_initAmmo = EditorGUILayout.IntField("Init Ammo", inventoryItemList.weaponList[viewIndex - 1].m_initAmmo);
        inventoryItemList.weaponList[viewIndex - 1].m_clipAmmo = EditorGUILayout.IntField("Clip Ammo", inventoryItemList.weaponList[viewIndex - 1].m_clipAmmo);
        //inventoryItemList.weaponList[viewIndex-1].mode     = (ModeSkillType)EditorGUILayout.EnumPopup  ("Mode",        inventoryItemList.skillList[viewIndex-1].mode);

        GUILayout.Label("Icon");
        //inventoryItemList.skillList[viewIndex-1].icon             = (Sprite)EditorGUILayout.ObjectField (inventoryItemList.skillList[viewIndex-1].icon, typeof(Sprite), false);
        //inventoryItemList.weaponList[viewIndex - 1].m_isAMachineGun = (bool)EditorGUILayout.Toggle("Body", inventoryItemList.weaponList[viewIndex - 1].m_isAMachineGun, GUILayout.ExpandWidth(false));

    }

    void PrintTopMenu()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        if (GUILayout.Button("<- Prev", GUILayout.ExpandWidth(false)))
        {
            if (viewIndex > 1)
                viewIndex--;
            else
                viewIndex = inventoryItemList.weaponList.Count;
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Next ->", GUILayout.ExpandWidth(false)))
        {
            if (viewIndex < inventoryItemList.weaponList.Count)
                viewIndex++;
            else
                viewIndex = 0;
        }
        GUILayout.Space(60);
        if (GUILayout.Button("+ Add Weapon", GUILayout.ExpandWidth(false)))
        {
            AddItem();
        }
        GUILayout.Space(5);
        if (GUILayout.Button("- Delete Weapon", GUILayout.ExpandWidth(false)))
        {
            DeleteItem(viewIndex - 1);
        }
        GUILayout.EndHorizontal();

        if (inventoryItemList.weaponList.Count > 0)
        {
            WeaponListMenu();
        }
        else
        {
            GUILayout.Space(10);
            GUILayout.Label("This Weapon List is Empty.");
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Weapon Editor", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (inventoryItemList != null)
        {
            PrintTopMenu();
        }
        else
        {
            GUILayout.Space(10);
            GUILayout.Label("Can't load weapon list.");
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(inventoryItemList);
        }
    }



    void AddItem()
    {
        WeaponItem newItem = new WeaponItem();
        newItem.m_name = "New Weapon";
        inventoryItemList.weaponList.Add(newItem);
        viewIndex = inventoryItemList.weaponList.Count;
    }

    void DeleteItem(int index)
    {
        inventoryItemList.weaponList.RemoveAt(index);
    }
}
