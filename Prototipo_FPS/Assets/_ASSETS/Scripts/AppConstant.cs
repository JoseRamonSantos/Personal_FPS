using UnityEngine;
using System.Collections;

public class AppPaths
{
	public static readonly string PERSISTENT_DATA = Application.persistentDataPath;
	public static readonly string PATH_RESOURCE_SFX = "Audio/Sfx/";
	public static readonly string PATH_RESOURCE_MUSIC = "Audio/Music/";

	public static readonly string PATH_RESOURCE_WEAPONS = "Weapons/WeaponList";

	public static readonly string PATH_B_IMPACT_SURFACE_GENERIC = "VFX/BulletImpact/bI_Surface_Generic";
	public static readonly string PATH_B_IMPACT_CHAR_GENERIC = "VFX/BulletImpact/bI_Char_Generic";
	public static readonly string PATH_B_IMPACT_CHAR_ROBOT = "VFX/BulletImpact/bI_Char_Robot";
}



public class AppPlayerPrefKeys
{
	public static readonly string MUSIC_VOLUME = "MusicVolume";
	public static readonly string SFX_VOLUME = "SfxVolume";
}

public class AppSounds
{
	//WEAPONS - AK47
	public static readonly string WEAPON_AK47_SHOOT = "WeaponShoot";
	public static readonly string WEAPON_AK47_SHORT_RELOAD = "WeaponShoot";
	public static readonly string WEAPON_AK47_LONG_RELOAD = "WeaponShoot";
	public static readonly string WEAPON_AK47_DRAW = "WeaponShoot";
	public static readonly string WEAPON_AK47_HOLSTER = "WeaponShoot";

	//WEAPONS - GLOCK
	public static readonly string WEAPON_GLOCK_SHOOT = "WeaponShoot";
	public static readonly string HANDGUN_03_SHORT_RELOAD = "HandGun_03_ShortReload";
	public static readonly string HANDGUN_03_LONG_RELOAD = "HandGun_03_LongReload";
	public static readonly string RIFLE_02_SHORT_RELOAD = "Rifle_02_ShortReload";
	public static readonly string RIFLE_02_LONG_RELOAD = "Rifle_02_LongReload";
}

