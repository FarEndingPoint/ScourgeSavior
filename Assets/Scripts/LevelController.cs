using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using ns;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class LevelController : MonoSingleton<LevelController>
{
    [SerializeField] int _level;
    public int level {  get { return _level; } }
    [SerializeField] Enemy[] enemies;
    [SerializeField] Transform[] points;
    [SerializeField] Image backgroundColor;
    [SerializeField] Image levelName;
    int fadeInduration;
    bool isBegan;
    [SerializeField] int totalEnemyNum;
    int enemyNumCount;
    int curEnemyNum;
    int killCount;
    [SerializeField] int minCurEnemyNum;
    [SerializeField] int maxCurEnemyNum;
    [SerializeField] float generateGapTime;
    float curGenerateGapTime;
    bool isOpenSettingsPanel;
    bool hasPass;

    private void Awake()
    {
        fadeInduration = 2;
        isBegan = false;
        enemyNumCount = 0;
        curEnemyNum = 0;
        killCount = 0;
        curGenerateGapTime = 0;
        isOpenSettingsPanel = false;
        hasPass = false;
    }

    private void Start()//TODO:²âÊÔ×¢ÊÍ´úÂë
    {
        levelName.DOFade(1, fadeInduration);
        Main.Interface.GetSystem<InputSystem>().inputController.Disable();
        UIKit.OpenPanel<SettingsPanel>();
        UIKit.HidePanel<SettingsPanel>();
        ActionKit.Delay(fadeInduration, () =>
        {
            backgroundColor.gameObject.SetActive(false);
            levelName.gameObject.SetActive(false);
            UIKit.OpenPanel<ReadyPanel>();
        }).Start(this);

        //Main.Interface.GetSystem<InputSystem>().inputController.Enable();//É¾³ý
    }

    private void Update()
    {
        GenerateLogic();
        SwitchSettingsPanel();
    }

    public void BeginPlay()
    {
        isBegan = true;
    }

    void GenerateLogic()
    {
        if (!isBegan || curEnemyNum >= maxCurEnemyNum || enemyNumCount >= totalEnemyNum) return;
        if (curEnemyNum < minCurEnemyNum)
        {
            int t = minCurEnemyNum - curEnemyNum;
            curEnemyNum += t;
            enemyNumCount += t;
            GenerateEnemy(t);
            curGenerateGapTime = generateGapTime;
        }
        else if (curGenerateGapTime > 0) curGenerateGapTime -= Time.deltaTime;
        else
        {
            curEnemyNum++;
            enemyNumCount++;
            GenerateEnemy(1);
            curGenerateGapTime = generateGapTime;
        }
    }

    void GenerateEnemy(int num)
    {
        for (int i = 0; i < num; i++)
        {
            int enemy = Random.Range(0, enemies.Length);
            int point = Random.Range(0, points.Length);
            Instantiate(enemies[enemy], points[point].position, Quaternion.identity);
        }
    }

    void SwitchSettingsPanel()
    {
        if (!hasPass && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpenSettingsPanel)
            {
                UIKit.HidePanel<SettingsPanel>();
                Time.timeScale = 1;
                if(isBegan) Main.Interface.GetSystem<InputSystem>().inputController.Enable();
            }
            else
            {
                UIKit.ShowPanel<SettingsPanel>();
                Time.timeScale = 0;
                Main.Interface.GetSystem<InputSystem>().inputController.Disable();
            }
            isOpenSettingsPanel = !isOpenSettingsPanel;
        }
    }

    public void KillEnemy()
    {
        killCount++;
        curEnemyNum--;
        if (killCount >= totalEnemyNum && curEnemyNum == 0)
        {
            switch(level)
            {
                case 1: 
                    Main.Interface.GetModel<GameData>().isLevel2Unlocked = true;
                    PlayerPrefs.SetInt("isLevel2Unlocked", 1);
                    break;
                case 2: 
                    Main.Interface.GetModel<GameData>().isLevel3Unlocked = true;
                    PlayerPrefs.SetInt("isLevel3Unlocked", 1);
                    break;
                case 3:
                    Main.Interface.GetModel<GameData>().isLevel4Unlocked = true;
                    PlayerPrefs.SetInt("isLevel4Unlocked", 1);
                    break;
                case 4: 
                    Main.Interface.GetModel<GameData>().isLevel5Unlocked = true;
                    PlayerPrefs.SetInt("isLevel5Unlocked", 1);
                    break;
                case 5:
                    Main.Interface.GetModel<GameData>().isLevel6Unlocked = true;
                    PlayerPrefs.SetInt("isLevel6Unlocked", 1);
                    break;
            }
            UIKit.ShowPanel<SettingsPanel>();
            hasPass = true;
            Time.timeScale = 0;
            Main.Interface.GetSystem<InputSystem>().inputController.Disable();
            _level = 0;
        }
    }

    public void Lose()
    {
        UIKit.ShowPanel<SettingsPanel>();
        hasPass = true;
        Time.timeScale = 0;
        Main.Interface.GetSystem<InputSystem>().inputController.Disable();
        _level = 0;
    }
}
