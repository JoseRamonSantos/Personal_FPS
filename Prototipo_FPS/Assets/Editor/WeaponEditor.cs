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

    private Vector2 m_scrollView = Vector2.zero;

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = "Assets/Resources/Weapons/WeaponList.asset";
            inventoryItemList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(WeaponList)) as WeaponList;
        }

        if (inventoryItemList == null)
        {
            viewIndex = 1;

            WeaponList asset = ScriptableObject.CreateInstance<WeaponList>();
            AssetDatabase.CreateAsset(asset, "Assets/Resources/Weapons/WeaponList.asset");
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
        GUILayout.BeginVertical();

        WeaponItem wItem = inventoryItemList.weaponList[viewIndex - 1];

        wItem.m_name = EditorGUILayout.TextField("Name", wItem.m_name as string);
        wItem.m_wType = (E_WEAPON_TYPE)EditorGUILayout.EnumPopup("Weapon type", wItem.m_wType);
        
        GUILayout.Space(10);
        m_scrollView = EditorGUILayout.BeginScrollView(m_scrollView);

        EditorGUILayout.LabelField("Attributes", EditorStyles.boldLabel);
        wItem.m_damage = EditorGUILayout.IntField("Damage", wItem.m_damage);
        wItem.m_fireRate = EditorGUILayout.IntField("Fire rate", wItem.m_fireRate);
        wItem.m_totalAmmo = EditorGUILayout.IntField("Init Ammo", wItem.m_totalAmmo);
        wItem.m_clipAmmo = EditorGUILayout.IntField("Clip Ammo", wItem.m_clipAmmo);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Accuracy", EditorStyles.boldLabel);
        wItem.m_accuracy.m_accuracyDropPerShot = EditorGUILayout.FloatField("Accuracy drop per shoot", wItem.m_accuracy.m_accuracyDropPerShot);
        wItem.m_accuracy.m_accuracyRecoverPerSecond = EditorGUILayout.FloatField("Accuracy recover per second", wItem.m_accuracy.m_accuracyRecoverPerSecond);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);
        if (wItem.m_wType == E_WEAPON_TYPE.SHOTGUN)
        {
            wItem.m_options.m_bulletsPerShoot = EditorGUILayout.IntField("Bullets per shoot", wItem.m_options.m_bulletsPerShoot);
        }
        GUILayout.Space(5);
        wItem.m_options.m_distanceDamageReduction = EditorGUILayout.CurveField("Damage reduction per distance", wItem.m_options.m_distanceDamageReduction);
        GUILayout.Space(5);
        wItem.m_options.m_hasChamber = EditorGUILayout.Toggle("Has chamber", wItem.m_options.m_hasChamber);
        wItem.m_options.m_hasInstantReload = EditorGUILayout.Toggle("Has instant reload", wItem.m_options.m_hasInstantReload);
        GUILayout.Space(5);
        wItem.m_options.m_projectile = EditorGUILayout.Toggle("Instantiate projectile", wItem.m_options.m_projectile);
        if (wItem.m_options.m_projectile)
        {
            wItem.m_options.m_pfProjectile = (GameObject)EditorGUILayout.ObjectField("Projectile prefab", wItem.m_options.m_pfProjectile, typeof(GameObject), true);
        }
        GUILayout.Space(5);
        wItem.m_options.m_bulletShell = EditorGUILayout.Toggle("Instantiate bullet shell", wItem.m_options.m_bulletShell);
        if (wItem.m_options.m_bulletShell)
        {
            wItem.m_options.m_pfBulletShell = (GameObject)EditorGUILayout.ObjectField("Bullet shell prefab", wItem.m_options.m_pfBulletShell, typeof(GameObject), true);
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Recoil", EditorStyles.boldLabel);
        wItem.m_recoilSettings.m_positionDampTime = EditorGUILayout.FloatField("Position damp time", wItem.m_recoilSettings.m_positionDampTime);
        wItem.m_recoilSettings.m_rotationDampTime = EditorGUILayout.FloatField("Rotation damp time", wItem.m_recoilSettings.m_rotationDampTime);
        GUILayout.Space(5);
        wItem.m_recoilSettings.m_recoil1 = EditorGUILayout.FloatField("Recoil 1", wItem.m_recoilSettings.m_recoil1);
        wItem.m_recoilSettings.m_recoil2 = EditorGUILayout.FloatField("Recoil 2", wItem.m_recoilSettings.m_recoil2);
        wItem.m_recoilSettings.m_recoil3 = EditorGUILayout.FloatField("Recoil 3", wItem.m_recoilSettings.m_recoil3);
        wItem.m_recoilSettings.m_recoil4 = EditorGUILayout.FloatField("Recoil 4", wItem.m_recoilSettings.m_recoil4);
        GUILayout.Space(5);
        wItem.m_recoilSettings.m_recoilRotation= EditorGUILayout.Vector3Field("Recoil rotation", wItem.m_recoilSettings.m_recoilRotation);
        wItem.m_recoilSettings.m_recoilKickBack= EditorGUILayout.Vector3Field("Recoil kick back", wItem.m_recoilSettings.m_recoilKickBack);
        //wItem.m_recoilSettings.m_recoilRotation_Aim= EditorGUILayout.Vector3Field("Recoil rotation (Aim)", wItem.m_recoilSettings.m_recoilRotation_Aim);
        //wItem.m_recoilSettings.m_recoilKickBack_Aim= EditorGUILayout.Vector3Field("Recoil kick back (Aim)", wItem.m_recoilSettings.m_recoilKickBack_Aim);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Sound clips", EditorStyles.boldLabel);
        wItem.m_soundClips.m_fireSound = (AudioClip)EditorGUILayout.ObjectField("Fire sound", wItem.m_soundClips.m_fireSound, typeof(AudioClip), true);
        wItem.m_soundClips.m_triggerSound = (AudioClip)EditorGUILayout.ObjectField("Trigger sound", wItem.m_soundClips.m_triggerSound, typeof(AudioClip), true);
        if (wItem.m_options.m_bulletShell)
        {
            wItem.m_soundClips.m_bulletShellSound = (AudioClip)EditorGUILayout.ObjectField("Bullet shell sound", wItem.m_soundClips.m_bulletShellSound, typeof(AudioClip), true);
        }
        GUILayout.Space(5);
        wItem.m_soundClips.m_holsterSound = (AudioClip)EditorGUILayout.ObjectField("Fire sound", wItem.m_soundClips.m_holsterSound, typeof(AudioClip), true);
        wItem.m_soundClips.m_drawSound = (AudioClip)EditorGUILayout.ObjectField("Fire sound", wItem.m_soundClips.m_drawSound, typeof(AudioClip), true);
        GUILayout.Space(5);
        if (wItem.m_options.m_hasChamber && wItem.m_options.m_hasInstantReload)
        {
            wItem.m_soundClips.m_reloadAmmoLeftSound = (AudioClip)EditorGUILayout.ObjectField("Reload (Ammo left)", wItem.m_soundClips.m_reloadAmmoLeftSound, typeof(AudioClip), true);
            wItem.m_soundClips.m_reloadOutOfAmmoSound = (AudioClip)EditorGUILayout.ObjectField("Reload (Out of ammo)", wItem.m_soundClips.m_reloadOutOfAmmoSound, typeof(AudioClip), true);
        }
        else if (wItem.m_options.m_hasInstantReload && !wItem.m_options.m_hasChamber)
        {
            wItem.m_soundClips.m_reload = (AudioClip)EditorGUILayout.ObjectField("Reload", wItem.m_soundClips.m_reload, typeof(AudioClip), true);
        }
        else if (!wItem.m_options.m_hasInstantReload)
        {
            wItem.m_soundClips.m_reload = (AudioClip)EditorGUILayout.ObjectField("Reload (Open)", wItem.m_soundClips.m_reload, typeof(AudioClip), true);
            wItem.m_soundClips.m_reloadInsert= (AudioClip)EditorGUILayout.ObjectField("Reload (Insert)", wItem.m_soundClips.m_reloadInsert, typeof(AudioClip), true);
            wItem.m_soundClips.m_reloadClose = (AudioClip)EditorGUILayout.ObjectField("Reload (Close)", wItem.m_soundClips.m_reloadClose, typeof(AudioClip), true);

        }

        GUILayout.Space(20);

        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();

        //inventoryItemList.weaponList[viewIndex-1].mode     = (ModeSkillType)EditorGUILayout.EnumPopup  ("Mode",        inventoryItemList.skillList[viewIndex-1].mode);

        //GUILayout.Label("Icon");
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
        //newItem.m_name = "New Weapon";
        inventoryItemList.weaponList.Add(newItem);
        viewIndex = inventoryItemList.weaponList.Count;
    }

    void DeleteItem(int index)
    {
        inventoryItemList.weaponList.RemoveAt(index);
    }
}
