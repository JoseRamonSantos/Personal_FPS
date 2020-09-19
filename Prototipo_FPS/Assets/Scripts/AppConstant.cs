using UnityEngine;
using System.Collections;

public class AppDevelopFlag
{
	public static readonly bool	DEVELOP_SYSTEM = true;
    public static readonly bool	DEVELOP_USER   = true;
    public static readonly bool	RELEASE        = false;
}

public class AppPaths
{
	public static readonly string  	PERSISTENT_DATA         = Application.persistentDataPath;
	public static readonly string	PATH_RESOURCE_SFX       = "Audio/Sfx/";
    public static readonly string	PATH_RESOURCE_MUSIC     = "Audio/Music/";
}

public class AppScenes
{
	public static readonly string 	MAIN_SCENE      = "00_MainMenu";
	public static readonly string 	LOADING_SCENE   = "01_Loading";
	public static readonly string 	GAME_SCENE      = "02_Game";
	public static readonly string 	WIN_SCENE       = "03_Win";
	public static readonly string 	LOSE_SCENE      = "04_Lose";
}

public class AppPlayerPrefKeys
{
	public static readonly string	MUSIC_VOLUME = "MusicVolume";
	public static readonly string	SFX_VOLUME   = "SfxVolume";
}

public class AppSounds
{
	public static readonly string	MAIN_TITLE_MUSIC            = "MainTitle";
    public static readonly string	BUTTON_SFX                  = "Click";
    public static readonly string	WEAPON_SHOOT                = "WeaponShoot";
    public static readonly string	HANDGUN_03_SHORT_RELOAD     = "HandGun_03_ShortReload";
    public static readonly string	HANDGUN_03_LONG_RELOAD      = "HandGun_03_LongReload";
    public static readonly string	RIFLE_02_SHORT_RELOAD       = "Rifle_02_ShortReload";
    public static readonly string	RIFLE_02_LONG_RELOAD        = "Rifle_02_LongReload";
}



