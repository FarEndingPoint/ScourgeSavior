using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class GameData : AbstractModel
{
    public bool isLevel1Unlocked;
    public bool isLevel2Unlocked;
    public bool isLevel3Unlocked;
    public bool isLevel4Unlocked;
    public bool isLevel5Unlocked;
    public bool isLevel6Unlocked;

    public int music, sound;

    public string musicPath = "resources://Music/";
    public string[] musicName = { "Escape", "Intro", "MegaloBox", "Nevada" };

    public string soundPath = "resources://Sound/";
    public string[] soundName = { "Jump" };

    protected override void OnInit()
    {
        //isLevel1Unlocked = true;
        //isLevel2Unlocked = false;
        //isLevel3Unlocked = false;
        //isLevel4Unlocked = false;
        //isLevel5Unlocked = false;
        //isLevel6Unlocked = false;

        isLevel1Unlocked = true;
        isLevel2Unlocked = PlayerPrefs.GetInt("isLevel2Unlocked", 0) == 1 ? true : false;
        isLevel3Unlocked = PlayerPrefs.GetInt("isLevel3Unlocked", 0) == 1 ? true : false;
        isLevel4Unlocked = PlayerPrefs.GetInt("isLevel4Unlocked", 0) == 1 ? true : false;
        isLevel5Unlocked = PlayerPrefs.GetInt("isLevel5Unlocked", 0) == 1 ? true : false;
        isLevel6Unlocked = PlayerPrefs.GetInt("isLevel6Unlocked", 0) == 1 ? true : false;
        PlayerPrefs.SetInt("isLevel2Unlocked", isLevel2Unlocked ? 1 : 0);
        PlayerPrefs.SetInt("isLevel3Unlocked", isLevel3Unlocked ? 1 : 0);
        PlayerPrefs.SetInt("isLevel4Unlocked", isLevel4Unlocked ? 1 : 0);
        PlayerPrefs.SetInt("isLevel5Unlocked", isLevel5Unlocked ? 1 : 0);
        PlayerPrefs.SetInt("isLevel6Unlocked", isLevel6Unlocked ? 1 : 0);

        music = PlayerPrefs.GetInt("music", 50);
        sound = PlayerPrefs.GetInt("sound", 50);
        PlayerPrefs.SetInt("music", music);
        PlayerPrefs.SetInt("sound", sound);
    }
}
