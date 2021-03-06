using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Mono.Xml;
using System.Collections.Generic;

public class XMLParser : MonoBehaviour
{
    public static XMLParser instance;

    public List<Monster> ListMonster = new List<Monster>();
    public List<Wave> ListWave = new List<Wave>();

    void Start()
    {
        instance = this;
    }

    public void ParseXMLScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        SP.LoadXml(ParseXMLText);
        System.Security.SecurityElement SE = SP.ToXml();

        ListMonster.Clear();
        ListWave.Clear();
        if (SE.SearchForChildByTag("Weather") != null)
        {
            Hashtable myWeatherMTable = SE.SearchForChildByTag("Weather").Attributes;
            PictureCreater.instance.WeatherID = int.Parse(myWeatherMTable["Type"].ToString());

            //PictureCreater.instance.Weather.transform.position = new Vector3(0, 0, 0);
            //PictureCreater.instance.WeatherLight = int.Parse(myWeatherMTable["Light"].ToString()) / 10f;
            //GameObject.Find("Directionallight").GetComponent<Light>().intensity = PictureCreater.instance.WeatherLight;
        }
        if (SE.SearchForChildByTag("BG") != null)
        {
            System.Security.SecurityElement BGChild = (System.Security.SecurityElement)SE.SearchForChildByTag("BG").Children[0];
            Hashtable myBGMTable = BGChild.Attributes;
            SceneTransformer.instance.NowSceneID = int.Parse(myBGMTable["Name"].ToString());
            PictureCreater.instance.PositionRow = int.Parse(SE.SearchForChildByTag("BG").Attributes["Group"].ToString());

            if (PlayerPrefs.GetFloat("EffectSlider") > 0)
            {
                RenderSettings.fog = false;
                //BattleMap bm = TextTranslator.instance.battleMapDic[SceneTransformer.instance.NowSceneID * 10 + PictureCreater.instance.WeatherID];
                //RenderSettings.fog = true;
                //RenderSettings.fogMode = FogMode.Linear;
                //RenderSettings.fogStartDistance = bm.fogStartDistance;
                //RenderSettings.fogEndDistance = bm.fogEndDistance;
                //RenderSettings.fogDensity = bm.fogDensity;
                //RenderSettings.fogColor = new Color32((byte)bm.fogColorR, (byte)bm.fogColorG, (byte)bm.fogColorB, 255);
                //RenderSettings.ambientLight = new Color32((byte)bm.ambientR, (byte)bm.ambientG, (byte)bm.ambientB, 255);
            }
            else
            {
                RenderSettings.fog = false;
            }
        }
        if (SE.SearchForChildByTag("Waves").Children != null)
        {
            int Wave = 1;
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Waves").Children)
            {
                if (child.Children != null)
                {
                    Wave NewWave = new Wave();
                    foreach (System.Security.SecurityElement subChild in child.Children)
                    {
                        if (subChild.Tag == "StartCamera")
                        {
                            Hashtable myCameraTable = subChild.Attributes;
                            NewWave.StartCameraPosition = new Vector2(float.Parse(myCameraTable["x"].ToString()), float.Parse(myCameraTable["y"].ToString()));
                            NewWave.LMode = int.Parse(myCameraTable["LMode"].ToString());
                            NewWave.RMode = int.Parse(myCameraTable["RMode"].ToString());
                        }
                        else if (subChild.Tag == "Monsters")
                        {
                            foreach (System.Security.SecurityElement finalchild in subChild.Children)
                            {
                                Hashtable myMonsterTable = finalchild.Attributes;
                                Monster NewMonster = new Monster();
                                NewMonster.Wave = Wave;
                                NewMonster.MonsterID = int.Parse(myMonsterTable["MonsterID"].ToString());
                                NewMonster.PositionID = int.Parse(myMonsterTable["PositionID"].ToString());
                                if (PictureCreater.instance.PositionRow == 3)
                                {
                                    if (NewMonster.PositionID % 3 == 1)
                                    {
                                        NewMonster.PositionID = NewMonster.PositionID + 3;
                                    }
                                }
                                else if (PictureCreater.instance.PositionRow == 4)
                                {
                                    if (NewMonster.PositionID % 4 == 1)
                                    {
                                        NewMonster.PositionID = NewMonster.PositionID + 4;
                                    }
                                    if (NewMonster.PositionID % 4 == 0)
                                    {
                                        NewMonster.PositionID = NewMonster.PositionID - 4;
                                    }
                                }
                                else if (PictureCreater.instance.PositionRow == 5)
                                {
                                    if (NewMonster.PositionID % 5 == 1)
                                    {
                                        NewMonster.PositionID = NewMonster.PositionID + 10;
                                    }
                                    if (NewMonster.PositionID % 5 == 2)
                                    {
                                        NewMonster.PositionID = NewMonster.PositionID + 5;
                                    }
                                    if (NewMonster.PositionID % 5 == 3)
                                    {
                                        NewMonster.PositionID = NewMonster.PositionID + 5;
                                    }
                                }
                                ListMonster.Add(NewMonster);
                            }
                        }
                        else if (subChild.Tag == "Terrains")
                        {

                        }
                    }
                    ListWave.Add(NewWave);
                }
                Wave++;
            }
        }
        //if (SE.SearchForChildByTag("BGM") != null)
        //{
        //    Hashtable myBGMTable = SE.SearchForChildByTag("BGM").Attributes;
        //    AudioEditer.instance.SetFightBGM(myBGMTable["Fight1"].ToString(), myBGMTable["Fight2"].ToString(), myBGMTable["Fight3"].ToString());
        //}                
    }

    public class Monster
    {
        public int Wave;
        public int MonsterID;
        public int PositionID;
    }

    public class Wave
    {
        public Vector2 StartCameraPosition;
        public int LMode;
        public int RMode;
        public List<WaveTerrain> ListWaveTerrain = new List<WaveTerrain>();
    }

    public class WaveTerrain
    {
        public int PositionID;
        public int ID;
        public int Destoyed;
        public int EffectTime;
        public int Buff;
        public int Parameter1;
        public int Parameter2;
        public int HP;
        public int Strong;
        public int Agile;
        public int Intell;
        public int PhyDef;
        public int PhyRed;
        public int MagicDef;
        public int MagicRed;
        public int Mv;
        public int Spd;
        public int Count;
        public bool Status;
    }



    public void ParseXMLItemScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////道具(以下)//////////////
            TextTranslator.instance.itemInfoList.Clear();

            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Items").Children)
            {
                Hashtable myXMLItemTable = child.Attributes;
                TextTranslator.instance.AddItem(int.Parse(myXMLItemTable["ItemID"].ToString()),
                    (string)myXMLItemTable["Name"], (string)myXMLItemTable["Des"], int.Parse(myXMLItemTable["Color"].ToString()),
                    0, 0,
                    (string)myXMLItemTable["ToValue"], int.Parse(myXMLItemTable["Icon"].ToString()), int.Parse(myXMLItemTable["SellType"].ToString()),
                    int.Parse(myXMLItemTable["SellPrice"].ToString()), (string)myXMLItemTable["Source1"], (string)myXMLItemTable["Source2"]
                    , (string)myXMLItemTable["Source3"], (string)myXMLItemTable["ToValue"], 0, int.Parse(myXMLItemTable["FuncType"].ToString()), int.Parse(myXMLItemTable["BuyLevel"].ToString()),
                    int.Parse(myXMLItemTable["Stack"].ToString()), int.Parse(myXMLItemTable["RelifeValue"].ToString()), int.Parse(myXMLItemTable["ExchangeGold"].ToString()));
            }
            //////////////道具(以上)//////////////

            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Gate"));
            if (!NetworkHandler.instance.IsCreate)
            {
                UIManager.instance.OpenSinglePanel("PreloadCommon", false);
                NetworkHandler.instance.SendProcess("9401#;");
            }
        }
        catch (Exception ex)
        {
            Debug.Log("XMLItem:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Item");
        }
    }



    public void ParseXMLRoleScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////道具(以下)//////////////
            TextTranslator.instance.heroInfoList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Roles").Children)
            {

                Hashtable myXMLItemTable = child.Attributes;
                TextTranslator.instance.AddHeroInfo(int.Parse(myXMLItemTable["RoleID"].ToString()), int.Parse(myXMLItemTable["Career"].ToString()), (string)myXMLItemTable["CareerShow"],
                    (string)myXMLItemTable["Name"], (string)myXMLItemTable["ClientName"], (string)myXMLItemTable["Des"], "0", int.Parse(myXMLItemTable["Scale"].ToString()),
                    int.Parse(myXMLItemTable["Bio"].ToString()), int.Parse(myXMLItemTable["AtkType"].ToString()), int.Parse(myXMLItemTable["DefType"].ToString()),
                    int.Parse(myXMLItemTable["Race"].ToString()), int.Parse(myXMLItemTable["Rarity"].ToString()),
                    int.Parse(myXMLItemTable["State"].ToString()), 0, 0, int.Parse(myXMLItemTable["Area"].ToString()), 0,
                    0, int.Parse(myXMLItemTable["Hp"].ToString()), int.Parse(myXMLItemTable["Atk"].ToString()),
                    0, 0, int.Parse(myXMLItemTable["Def"].ToString()),
                    0, float.Parse(myXMLItemTable["DmgBonus"].ToString()), float.Parse(myXMLItemTable["DmgReduction"].ToString()), float.Parse(myXMLItemTable["Hit"].ToString()), float.Parse(myXMLItemTable["NoHit"].ToString()),
                    float.Parse(myXMLItemTable["Crit"].ToString()), float.Parse(myXMLItemTable["NoCrit"].ToString()), 0, int.Parse(myXMLItemTable["Speed"].ToString()),
                    int.Parse(myXMLItemTable["Move"].ToString()), int.Parse(myXMLItemTable["Skill1"].ToString()), int.Parse(myXMLItemTable["Skill2"].ToString()),
                    int.Parse(myXMLItemTable["Skill3"].ToString()), int.Parse(myXMLItemTable["Skill4"].ToString()), int.Parse(myXMLItemTable["NeedDebrisNum"].ToString()), (string)myXMLItemTable["PetPhrase"], (string)myXMLItemTable["InForces"],
                    int.Parse(myXMLItemTable["PowerSort"].ToString()), int.Parse(myXMLItemTable["Sex"].ToString()), int.Parse(myXMLItemTable["WeaponID"].ToString()),
                    int.Parse(myXMLItemTable["AtkScore"].ToString()), int.Parse(myXMLItemTable["DefScore"].ToString()), int.Parse(myXMLItemTable["HpScore"].ToString()),
                    int.Parse(myXMLItemTable["SkillScore"].ToString())
                    );
            }
            //////////////道具(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Enemy"));
            if (!NetworkHandler.instance.IsCreate)
            {
                UIManager.instance.OpenSinglePanel("PreloadNormal", false);
            }

        }
        catch (Exception ex)
        {
            Debug.Log("Hero:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Role");
        }
    }

    public void ParseXMLEnemyScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\Enemy.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////怪物(以下)//////////////
            TextTranslator.instance.enemyInfoList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Enemys").Children)
            {
                Hashtable myXMLEnemyTable = child.Attributes;
                //Debug.Log(myXMLEnemyTable["EnemyID"].ToString());
                TextTranslator.instance.AddEnemy(int.Parse(myXMLEnemyTable["EnemyID"].ToString()), int.Parse(myXMLEnemyTable["RoleID"].ToString()), "", 1, 1, int.Parse(myXMLEnemyTable["Level"].ToString()),
                    int.Parse(myXMLEnemyTable["Atk"].ToString()), int.Parse(myXMLEnemyTable["Hp"].ToString()), int.Parse(myXMLEnemyTable["Def"].ToString()), 0.95f, 0, 0, 0, 0, 0, 0);
            }
            //////////////怪物(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Terrain"));
            if (!NetworkHandler.instance.IsCreate)
            {
                UIManager.instance.OpenSinglePanel("PreloadMap", false);
            }

        }
        catch (Exception ex)
        {
            Debug.Log("Hero:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Enemy");
        }
    }

    public void ParseXMLBossAiScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\BossAi.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////怪物(以下)//////////////
            TextTranslator.instance.bossAiList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("BossAis").Children)
            {
                Hashtable myXMLBossAiTable = child.Attributes;
                TextTranslator.instance.AddBossAi(int.Parse(myXMLBossAiTable["BossAiID"].ToString()), int.Parse(myXMLBossAiTable["Stage1"].ToString()), int.Parse(myXMLBossAiTable["Trigger1"].ToString()), int.Parse(myXMLBossAiTable["Parameter01"].ToString()), int.Parse(myXMLBossAiTable["Skill1"].ToString()), int.Parse(myXMLBossAiTable["Action1"].ToString()), myXMLBossAiTable["ActionParameter1"].ToString(), int.Parse(myXMLBossAiTable["Model1"].ToString()), int.Parse(myXMLBossAiTable["Enemy1"].ToString()), int.Parse(myXMLBossAiTable["Size1"].ToString())
                    , int.Parse(myXMLBossAiTable["Stage2"].ToString()), int.Parse(myXMLBossAiTable["Trigger2"].ToString()), int.Parse(myXMLBossAiTable["Parameter02"].ToString()), int.Parse(myXMLBossAiTable["Skill2"].ToString()), int.Parse(myXMLBossAiTable["Action2"].ToString()), myXMLBossAiTable["ActionParameter2"].ToString(), int.Parse(myXMLBossAiTable["Model2"].ToString()), int.Parse(myXMLBossAiTable["Enemy2"].ToString()), int.Parse(myXMLBossAiTable["Size2"].ToString())
                    , int.Parse(myXMLBossAiTable["Stage3"].ToString()), int.Parse(myXMLBossAiTable["Trigger3"].ToString()), int.Parse(myXMLBossAiTable["Parameter03"].ToString()), int.Parse(myXMLBossAiTable["Skill3"].ToString()), int.Parse(myXMLBossAiTable["Action3"].ToString()), myXMLBossAiTable["ActionParameter3"].ToString(), int.Parse(myXMLBossAiTable["Model3"].ToString()), int.Parse(myXMLBossAiTable["Enemy3"].ToString()), int.Parse(myXMLBossAiTable["Size3"].ToString()));
            }
            //////////////怪物(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Terrain"));
            if (!NetworkHandler.instance.IsCreate)
            {
                UIManager.instance.OpenSinglePanel("PreloadSkill", false);
            }

        }
        catch (Exception ex)
        {
            Debug.Log("Hero:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "BossAi");
        }
    }

    public void ParseXMLGateScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\Gate\\Gate.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////关卡(以下)//////////////
            TextTranslator.instance.listGate.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Quests").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddGate(int.Parse(myXMLGateTable["QuestID"].ToString()),
                    (string)myXMLGateTable["Name"], int.Parse(myXMLGateTable["GroupID"].ToString()),
                    int.Parse(myXMLGateTable["ChapterID"].ToString()), int.Parse(myXMLGateTable["NeedLevel"].ToString()),
                    "", (string)myXMLGateTable["Des"], int.Parse(myXMLGateTable["MapID"].ToString()), int.Parse(myXMLGateTable["BattleMapID"].ToString()),
                    int.Parse(myXMLGateTable["ScriptDataID1"].ToString()), int.Parse(myXMLGateTable["ScriptDataID2"].ToString()), int.Parse(myXMLGateTable["ScriptDataID3"].ToString()), 4
                    , int.Parse(myXMLGateTable["ItemID1"].ToString()), int.Parse(myXMLGateTable["ItemID2"].ToString()), int.Parse(myXMLGateTable["ItemID3"].ToString()),
                    int.Parse(myXMLGateTable["ItemID4"].ToString()), int.Parse(myXMLGateTable["ItemID5"].ToString()), int.Parse(myXMLGateTable["ItemID6"].ToString())
                    , int.Parse(myXMLGateTable["BdItemList"].ToString()), int.Parse(myXMLGateTable["BdCnt"].ToString()), int.Parse(myXMLGateTable["GateIcon"].ToString()), int.Parse(myXMLGateTable["FogID"].ToString())
                    , int.Parse(myXMLGateTable["NeedForce"].ToString()), myXMLGateTable["TerrainStore"].ToString(), int.Parse(myXMLGateTable["RoleID"].ToString()), int.Parse(myXMLGateTable["Type"].ToString()), int.Parse(myXMLGateTable["NeedLevel"].ToString()), int.Parse(myXMLGateTable["PlayerExpBonus"].ToString())
                    , (myXMLGateTable["ItemNum1"].ToString()),
                    (myXMLGateTable["ItemNum2"].ToString()),
                    (myXMLGateTable["ItemNum3"].ToString()),
                    (myXMLGateTable["ItemNum4"].ToString()),
                    (myXMLGateTable["ItemNum5"].ToString()),
                    (myXMLGateTable["ItemNum6"].ToString()));
            }
            //////////////关卡(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Role"));
            if (!NetworkHandler.instance.IsCreate)
            {
                UIManager.instance.OpenSinglePanel("PreloadItem", false);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("XMLItem:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Item");
        }
    }


    public void ParseXMLAchievementScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.achievementList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Achieves").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                BetterList<Item> itemList = new BetterList<Item>();
                string[] itemSplit = myXMLGateTable["BonusReward"].ToString().Split('!');
                for (int i = 0; i < itemSplit.Length - 1; i++)
                {
                    string[] item = itemSplit[i].Split('$');
                    itemList.Add(new Item(int.Parse(item[0]), int.Parse(item[1])));
                }
                TextTranslator.instance.AddAchievementList(new Achievement(int.Parse(myXMLGateTable["AchieveID"].ToString()),
                    myXMLGateTable["Name"].ToString(), myXMLGateTable["Des"].ToString(), int.Parse(myXMLGateTable["Icon"].ToString()),
                    int.Parse(myXMLGateTable["PreTaskID"].ToString()), int.Parse(myXMLGateTable["BonusExp"].ToString()), int.Parse(myXMLGateTable["BonusGold"].ToString()),
                    int.Parse(myXMLGateTable["BonusDiamond"].ToString()), itemList, int.Parse(myXMLGateTable["ResetTime"].ToString()), int.Parse(myXMLGateTable["Param"].ToString()),
                    int.Parse(myXMLGateTable["OpenLevel"].ToString()), int.Parse(myXMLGateTable["OpenGate"].ToString()), int.Parse(myXMLGateTable["OpenVip"].ToString()),
                    int.Parse(myXMLGateTable["HappyPoint"].ToString()), int.Parse(myXMLGateTable["Type"].ToString())));
            }
            //////////////成就(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "HappyBox"));
            if (!NetworkHandler.instance.IsCreate)
            {
                UIManager.instance.OpenSinglePanel("PreloadAvatar", false);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("XMLAchievement:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Achievement");
        }
    }

    public void ParseXMLHappyBoxScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.HappyBoxList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("HappyBoxs").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddHappyBoxList(new HappyBox(int.Parse(myXMLGateTable["HappyBoxID"].ToString()), int.Parse(myXMLGateTable["Point"].ToString()),
                                                                     int.Parse(myXMLGateTable["ItemID1"].ToString()), int.Parse(myXMLGateTable["ItemNum1"].ToString()),
                                                                     int.Parse(myXMLGateTable["ItemID2"].ToString()), int.Parse(myXMLGateTable["ItemNum2"].ToString()),
                                                                     int.Parse(myXMLGateTable["ItemID3"].ToString()), int.Parse(myXMLGateTable["ItemNum3"].ToString()),
                                                                     int.Parse(myXMLGateTable["ItemID4"].ToString()), int.Parse(myXMLGateTable["ItemNum4"].ToString()),
                                                                     int.Parse(myXMLGateTable["ItemID5"].ToString()), int.Parse(myXMLGateTable["ItemNum5"].ToString()),
                                                                     int.Parse(myXMLGateTable["ItemID6"].ToString()), int.Parse(myXMLGateTable["ItemNum6"].ToString())));
            }
            //////////////成就(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "WorldBoss"));
            if (!NetworkHandler.instance.IsCreate)
            {
                UIManager.instance.OpenSinglePanel("PreloadCloud", false);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("XMLHappyBox:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "HappyBox");
        }
    }

    public void ParseXMLWorldBossScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.WorldBossList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("WorldBosss").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddWorldBossList(new WorldBoss(int.Parse(myXMLGateTable["MonsterLevel"].ToString()), int.Parse(myXMLGateTable["Icon"].ToString()),
                                                                     float.Parse(myXMLGateTable["Blood"].ToString()), int.Parse(myXMLGateTable["LuckyBoxID"].ToString()),
                                                                     float.Parse(myXMLGateTable["LuckyBoxRank"].ToString())));
            }
            //////////////成就(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "WorldBossReward"));
            if (!NetworkHandler.instance.IsCreate)
            {
                UIManager.instance.OpenSinglePanel("PreloadWood", false);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("XMLWorldBoss:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "WorldBoss");
        }
    }
    public void ParseXMLWorldBossRewardScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.WorldBossRewardList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("WorldBossRewards").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddWorldBossRewardList(new WorldBossReward(int.Parse(myXMLGateTable["WorldBossID"].ToString()), float.Parse(myXMLGateTable["BloodPercent"].ToString()),
                                                                     int.Parse(myXMLGateTable["BloodItem"].ToString()), int.Parse(myXMLGateTable["BloodItemNum"].ToString()), int.Parse(myXMLGateTable["WorldBossRank"].ToString()),
                                                                     int.Parse(myXMLGateTable["RankItem1"].ToString()), int.Parse(myXMLGateTable["RankItemNum1"].ToString()),
                                                                     int.Parse(myXMLGateTable["RankItem2"].ToString()), int.Parse(myXMLGateTable["RankItemNum2"].ToString()),
                                                                     int.Parse(myXMLGateTable["RankItem3"].ToString()), int.Parse(myXMLGateTable["RankItemNum3"].ToString()),
                                                                     int.Parse(myXMLGateTable["RankItem4"].ToString()), int.Parse(myXMLGateTable["RankItemNum4"].ToString())));

            }
            //////////////成就(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "KingRoad"));
            if (!NetworkHandler.instance.IsCreate)
            {
                Texture Preload1 = Resources.Load("MapTecture/ddt") as Texture;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("XMLWorldBossReward:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "WorldBossReward");
        }
    }

    public void ParseXMLKingRoadRewardScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.KingRoadList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("KingRoads").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddKingRoadList(new KingRoad(int.Parse(myXMLGateTable["KingRank"].ToString()), int.Parse(myXMLGateTable["RobotNum"].ToString()),
                                                                     int.Parse(myXMLGateTable["PlayerNum"].ToString()), int.Parse(myXMLGateTable["TounamentCycle"].ToString()),
                                                                     int.Parse(myXMLGateTable["UpRank"].ToString()), int.Parse(myXMLGateTable["DownRank"].ToString()), int.Parse(myXMLGateTable["LeapfrogFightRank"].ToString()),

                                                                     myXMLGateTable["RankScope1"].ToString(), int.Parse(myXMLGateTable["Rank1Bonus1"].ToString()), int.Parse(myXMLGateTable["Rank1BonusNum1"].ToString()), int.Parse(myXMLGateTable["Rank1Bonus2"].ToString()), int.Parse(myXMLGateTable["Rank1BonusNum2"].ToString()),
                                                                     myXMLGateTable["RankScope2"].ToString(), int.Parse(myXMLGateTable["Rank2Bonus1"].ToString()), int.Parse(myXMLGateTable["Rank2BonusNum1"].ToString()), int.Parse(myXMLGateTable["Rank2Bonus2"].ToString()), int.Parse(myXMLGateTable["Rank2BonusNum2"].ToString()),
                                                                     myXMLGateTable["RankScope3"].ToString(), int.Parse(myXMLGateTable["Rank3Bonus1"].ToString()), int.Parse(myXMLGateTable["Rank3BonusNum1"].ToString()), int.Parse(myXMLGateTable["Rank3Bonus2"].ToString()), int.Parse(myXMLGateTable["Rank3BonusNum2"].ToString()),
                                                                     myXMLGateTable["RankScope4"].ToString(), int.Parse(myXMLGateTable["Rank4Bonus1"].ToString()), int.Parse(myXMLGateTable["Rank4BonusNum1"].ToString()), int.Parse(myXMLGateTable["Rank4Bonus2"].ToString()), int.Parse(myXMLGateTable["Rank4BonusNum2"].ToString()),
                                                                     myXMLGateTable["RankScope5"].ToString(), int.Parse(myXMLGateTable["Rank5Bonus1"].ToString()), int.Parse(myXMLGateTable["Rank5BonusNum1"].ToString()), int.Parse(myXMLGateTable["Rank5Bonus2"].ToString()), int.Parse(myXMLGateTable["Rank5BonusNum2"].ToString()),
                                                                     myXMLGateTable["RankScope6"].ToString(), int.Parse(myXMLGateTable["Rank6Bonus1"].ToString()), int.Parse(myXMLGateTable["Rank6BonusNum1"].ToString()), int.Parse(myXMLGateTable["Rank6Bonus2"].ToString()), int.Parse(myXMLGateTable["Rank6BonusNum2"].ToString()),
                                                                     myXMLGateTable["RankScope7"].ToString(), int.Parse(myXMLGateTable["Rank7Bonus1"].ToString()), int.Parse(myXMLGateTable["Rank7BonusNum1"].ToString()), int.Parse(myXMLGateTable["Rank7Bonus2"].ToString()), int.Parse(myXMLGateTable["Rank7BonusNum2"].ToString())));

            }
            //////////////成就(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "LegionCraps"));
            if (!NetworkHandler.instance.IsCreate)
            {
                Texture Preload2 = Resources.Load("Game/saodangTex") as Texture;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("XMLKingRoad:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "KingRoadReward");
        }
    }

    public void ParseXMLLegionCrapScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.LegionCrapList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("LegionCrapss").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddLegionCrapList(new LegionCrap(int.Parse(myXMLGateTable["CrapsType"].ToString()), float.Parse(myXMLGateTable["CrapsRand"].ToString()),
                                                                     int.Parse(myXMLGateTable["HeroNum"].ToString()), float.Parse(myXMLGateTable["HeroRand"].ToString()),
                                                                     int.Parse(myXMLGateTable["ItemID1"].ToString()), int.Parse(myXMLGateTable["ItemNum1"].ToString()),
                                                                     int.Parse(myXMLGateTable["ItemID2"].ToString()), int.Parse(myXMLGateTable["ItemNum2"].ToString()),
                                                                     int.Parse(myXMLGateTable["ItemID3"].ToString()), int.Parse(myXMLGateTable["ItemNum3"].ToString()),
                                                                     int.Parse(myXMLGateTable["ItemID4"].ToString()), int.Parse(myXMLGateTable["ItemNum4"].ToString()),
                                                                     int.Parse(myXMLGateTable["ItemID5"].ToString()), int.Parse(myXMLGateTable["ItemNum5"].ToString()),
                                                                     int.Parse(myXMLGateTable["ItemID6"].ToString()), int.Parse(myXMLGateTable["ItemNum6"].ToString())));

            }
            //////////////成就(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "LegionCity"));
            if (!NetworkHandler.instance.IsCreate)
            {
                Texture Preload3 = Resources.Load("Game/ziyuan_newdi8") as Texture;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("XMLLegionCrap:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "LegionCraps");
        }
    }
    public void ParseXMLLegionCityScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.LegionCityList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("LegionCitys").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddLegionCityList(new LegionCity(int.Parse(myXMLGateTable["CityID"].ToString()), int.Parse(myXMLGateTable["CityType"].ToString()),
                                                                     int.Parse(myXMLGateTable["LegionNeedLevel"].ToString()), int.Parse(myXMLGateTable["ArmyNum"].ToString()),
                                                                     int.Parse(myXMLGateTable["ItemID1"].ToString()), int.Parse(myXMLGateTable["ItemNum1"].ToString()),
                                                                     int.Parse(myXMLGateTable["ItemID2"].ToString()), int.Parse(myXMLGateTable["ItemNum2"].ToString()),
                                                                     int.Parse(myXMLGateTable["ItemID3"].ToString()), int.Parse(myXMLGateTable["ItemNum3"].ToString()),
                                                                     int.Parse(myXMLGateTable["MonsterHp"].ToString()), int.Parse(myXMLGateTable["MonsterAtk"].ToString()),
                                                                     int.Parse(myXMLGateTable["MonsterDef"].ToString()), float.Parse(myXMLGateTable["MonsterHit"].ToString()),
                                                                     float.Parse(myXMLGateTable["MonsterNoHit"].ToString()), float.Parse(myXMLGateTable["MonsterCrit"].ToString()),
                                                                     float.Parse(myXMLGateTable["MonsterNoCrit"].ToString()), float.Parse(myXMLGateTable["MonsterDmgBonus"].ToString()), float.Parse(myXMLGateTable["MonsterDmgReduction"].ToString()), myXMLGateTable["CityName"].ToString()));

            }
            //////////////成就(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ControlGateOpen"));
            if (!NetworkHandler.instance.IsCreate)
            {
                Texture Preload4 = Resources.Load("Game/sj_newdi4") as Texture;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("XMLLegionCity:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "LegionCity");
        }
    }

    public void ParseXMLControlGateOpenScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.ControlGateOpenList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ControlGateOpens").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddControlGateOpenList(new ControlGateOpen(int.Parse(myXMLGateTable["ID"].ToString()), int.Parse(myXMLGateTable["GateID"].ToString()), int.Parse(myXMLGateTable["Features"].ToString())));

            }
            //////////////成就(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "LegionRedBags"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLLegionCity:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ControlGateOpen");
        }
    }

    public void ParseXMLLegionRedBagScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.LegionRedBagList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("LegionRedBagss").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddLegionRedBagList(new LegionRedBag(int.Parse(myXMLGateTable["ID"].ToString()), int.Parse(myXMLGateTable["RedBagType"].ToString()),
                                                                     int.Parse(myXMLGateTable["SendBagType"].ToString()), int.Parse(myXMLGateTable["ItemIcon"].ToString()),
                                                                     myXMLGateTable["ItemName"].ToString(), int.Parse(myXMLGateTable["Level"].ToString()),
                                                                     int.Parse(myXMLGateTable["VipLimit"].ToString()), int.Parse(myXMLGateTable["Diamond"].ToString()),
                                                                     int.Parse(myXMLGateTable["RewardItemId1"].ToString()), int.Parse(myXMLGateTable["RewardItemNum1"].ToString()),
                                                                     int.Parse(myXMLGateTable["RewardItemId2"].ToString()), int.Parse(myXMLGateTable["RewardItemNum2"].ToString()),
                                                                     int.Parse(myXMLGateTable["RewardItemId3"].ToString()), int.Parse(myXMLGateTable["RewardItemNum3"].ToString()),
                                                                     int.Parse(myXMLGateTable["RedBagNum"].ToString()), int.Parse(myXMLGateTable["LegionLevel"].ToString()),
                                                                     int.Parse(myXMLGateTable["GetRewardItemId1"].ToString()), int.Parse(myXMLGateTable["GetRewardItemNum1"].ToString()),
                                                                     int.Parse(myXMLGateTable["GetRewardItemId2"].ToString()), int.Parse(myXMLGateTable["GetRewardItemNum2"].ToString()),
                                                                     int.Parse(myXMLGateTable["GetRewardItemId3"].ToString()), int.Parse(myXMLGateTable["GetRewardItemNum3"].ToString())));
            }
            //////////////成就(以上)//////////////
            //StartCoroutine(ResourceLoader.instance.GetGameResource(false, "TeamGate"));
            //if (!NetworkHandler.instance.IsCreate)
            //{
            //    Texture Preload5 = Resources.Load("Game/sj_newdi2") as Texture;
            //}
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Exp"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLLegionRedBag:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "LegionRedBags");
        }

    }

    /// <summary>
    /// 读取角色的经验值
    /// </summary>
    /// <param name="ParseXMLText"></param>
    public void ParseXMLExpScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////道具(以下)//////////////
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Exps").Children)
            {

                Hashtable myXMLItemTable = child.Attributes;
                TextTranslator.instance.ExpsHeroInfo(Convert.ToInt32(myXMLItemTable["Level"]),
                                                 Convert.ToInt32(myXMLItemTable["PlayerExp"]),
                                                 Convert.ToInt32(myXMLItemTable["RoleExp2"]),
                                                 Convert.ToInt32(myXMLItemTable["StaminaCap"]),
                                                 Convert.ToInt32(myXMLItemTable["StaminaMaxCap"]),
                                                 Convert.ToInt32(myXMLItemTable["EnergyCap"]),
                                                 Convert.ToInt32(myXMLItemTable["EnergyMaxCap"]),
                                                 Convert.ToInt32(myXMLItemTable["RoleExp3"]),
                                                 Convert.ToInt32(myXMLItemTable["RoleExp4"]),
                                                 Convert.ToInt32(myXMLItemTable["RoleExp5"]),
                                                 Convert.ToInt32(myXMLItemTable["RoleExp6"]),
                                                 Convert.ToInt32(myXMLItemTable["BuyGoldNum"]),
                                                 Convert.ToInt32(myXMLItemTable["StaminaBonus"]));
            }
            //////////////道具(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Wish"));
        }
        catch (Exception ex)
        {
            Debug.Log("Exp:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Exp");
        }
    }
    public void ParseXMLWishScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////道具(以下)//////////////
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Wishs").Children)
            {

                Hashtable myXMLItemTable = child.Attributes;
                TextTranslator.instance.WishItemInfo(Convert.ToInt32(myXMLItemTable["WishID"]),
                                                 Convert.ToInt32(myXMLItemTable["ItemID"]),
                                                 Convert.ToInt32(myXMLItemTable["ItemNum"]),
                                                 Convert.ToInt32(myXMLItemTable["MaxCrit"]));
            }
            //////////////道具(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "LegionWeak"));
        }
        catch (Exception ex)
        {
            Debug.Log("Wish:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Wish");
        }
    }
    public void ParseXMLLegionWeakScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////道具(以下)//////////////
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("LegionWeaks").Children)
            {

                Hashtable myXMLItemTable = child.Attributes;
                TextTranslator.instance.LegionWeakInfo(Convert.ToInt32(myXMLItemTable["WinNum"]),
                                                 float.Parse(myXMLItemTable["Decrease"].ToString())
                                                 );
            }
            //////////////道具(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "RoleGrow"));
        }
        catch (Exception ex)
        {
            Debug.Log("LegionWeak:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "LegionWeak");
        }
    }
    public void ParseXMLRoleGrowScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////道具(以下)//////////////
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("RoleGrows").Children)
            {

                Hashtable myXMLItemTable = child.Attributes;
                RoleGrow roleGrow = new RoleGrow(Convert.ToInt32(myXMLItemTable["RoleID"]),
                                                 myXMLItemTable["Name"].ToString(),
                                                 Convert.ToInt32(myXMLItemTable["Rarity"]),
                                                 Convert.ToInt32(myXMLItemTable["Star"]),
                                                 Convert.ToInt32(myXMLItemTable["Hp"]),
                                                 Convert.ToInt32(myXMLItemTable["Atk"]),
                                                 Convert.ToInt32(myXMLItemTable["Def"]),
                                                 Convert.ToInt32(myXMLItemTable["Hit"]),
                                                 Convert.ToInt32(myXMLItemTable["NoHit"]),
                                                 Convert.ToInt32(myXMLItemTable["Crit"]),
                                                 Convert.ToInt32(myXMLItemTable["NoCrit"]),
                                                 Convert.ToInt32(myXMLItemTable["DmgBonus"]),
                                                 Convert.ToInt32(myXMLItemTable["DmgReduction"]));
                TextTranslator.instance.AddRoleGrowList(roleGrow);
            }
            //////////////道具(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "TeamGate"));
        }
        catch (Exception ex)
        {
            Debug.Log("RoleGrow:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "RoleGrow");
        }
    }

    public void ParseXMLTeamGateScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.TeamGateList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("TeamGates").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddTeamGateList(new TeamGate(int.Parse(myXMLGateTable["TeamGateID"].ToString()), int.Parse(myXMLGateTable["GroupID"].ToString()),
                                                                     myXMLGateTable["Name"].ToString(), int.Parse(myXMLGateTable["NeedLevel"].ToString()),
                                                                     int.Parse(myXMLGateTable["Force"].ToString()), int.Parse(myXMLGateTable["BossID"].ToString()),
                                                                     int.Parse(myXMLGateTable["BonusID1"].ToString()), int.Parse(myXMLGateTable["BonusID2"].ToString()),
                                                                     int.Parse(myXMLGateTable["BonusID3"].ToString()), int.Parse(myXMLGateTable["BonusID4"].ToString()),
                                                                     int.Parse(myXMLGateTable["BonusID5"].ToString())));
            }
            //////////////成就(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "GachaPreview"));
            if (!NetworkHandler.instance.IsCreate)
            {
                Texture Preload5 = Resources.Load("Game/sj_newdi2") as Texture;
            }

        }
        catch (Exception ex)
        {
            Debug.Log("XMLTeamGate:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "TeamGate");
        }
    }

    public void ParseXMLGachaPreviewScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.GachaPreviewList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("GachaPreviews").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddGachaPreviewList(new GachaPreview(int.Parse(myXMLGateTable["ID"].ToString()), int.Parse(myXMLGateTable["UIType"].ToString()), int.Parse(myXMLGateTable["ItemID"].ToString()), int.Parse(myXMLGateTable["Level"].ToString())));
            }
            //////////////成就(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Nation"));
            //if (!NetworkHandler.instance.IsCreate)
            //{
            //    Texture Preload5 = Resources.Load("Game/sj_newdi2") as Texture;
            //}

        }
        catch (Exception ex)
        {
            Debug.Log("XMLGachaPreview:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "GachaPreview");
        }
    }


    public void ParseXMLNationScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.NationList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Nations").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddNationList(new Nation(int.Parse(myXMLGateTable["ID"].ToString()), int.Parse(myXMLGateTable["NationType"].ToString()), myXMLGateTable["OfficeName"].ToString(), int.Parse(myXMLGateTable["RobotID"].ToString()),
                                                        int.Parse(myXMLGateTable["Condition"].ToString()), int.Parse(myXMLGateTable["EveryDayCost"].ToString()), int.Parse(myXMLGateTable["ItemID1"].ToString()), int.Parse(myXMLGateTable["ItemNum1"].ToString()),
                                                        int.Parse(myXMLGateTable["ItemID2"].ToString()), int.Parse(myXMLGateTable["ItemNum2"].ToString()), int.Parse(myXMLGateTable["ItemID3"].ToString()), int.Parse(myXMLGateTable["ItemNum3"].ToString()),
                                                        int.Parse(myXMLGateTable["ItemID4"].ToString()), int.Parse(myXMLGateTable["ItemNum4"].ToString()), int.Parse(myXMLGateTable["ItemID5"].ToString()), int.Parse(myXMLGateTable["ItemNum5"].ToString()),
                                                        int.Parse(myXMLGateTable["ItemID6"].ToString()), int.Parse(myXMLGateTable["ItemNum6"].ToString()), float.Parse(myXMLGateTable["BattlefieldDamageBonus"].ToString())));
            }
            //////////////成就(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "BattlefieldPoints"));
            //if (!NetworkHandler.instance.IsCreate)
            //{
            //    Texture Preload5 = Resources.Load("Game/sj_newdi2") as Texture;
            //}

        }
        catch (Exception ex)
        {
            Debug.Log("XMLNation:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Nation");
        }
    }


    public void ParseXMLBattlefieldPointsScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.BattlefieldPointsList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("BattlefieldPointss").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddBattlefieldPointsList(new BattlefieldPoints(int.Parse(myXMLGateTable["ID"].ToString()), int.Parse(myXMLGateTable["Points"].ToString()), int.Parse(myXMLGateTable["ItemID1"].ToString()), int.Parse(myXMLGateTable["ItemNum1"].ToString()),
                                                        int.Parse(myXMLGateTable["ItemID2"].ToString()), int.Parse(myXMLGateTable["ItemNum2"].ToString()), int.Parse(myXMLGateTable["ItemID3"].ToString()), int.Parse(myXMLGateTable["ItemNum3"].ToString()),
                                                        int.Parse(myXMLGateTable["ItemID4"].ToString()), int.Parse(myXMLGateTable["ItemNum4"].ToString()), int.Parse(myXMLGateTable["ItemID5"].ToString()), int.Parse(myXMLGateTable["ItemNum5"].ToString()),
                                                        int.Parse(myXMLGateTable["ItemID6"].ToString()), int.Parse(myXMLGateTable["ItemNum6"].ToString()), int.Parse(myXMLGateTable["ItemID7"].ToString()), int.Parse(myXMLGateTable["ItemNum7"].ToString()),
                                                        int.Parse(myXMLGateTable["ItemID8"].ToString()), int.Parse(myXMLGateTable["ItemNum8"].ToString()), int.Parse(myXMLGateTable["ItemID9"].ToString()), int.Parse(myXMLGateTable["ItemNum9"].ToString()),
                                                        int.Parse(myXMLGateTable["ItemID10"].ToString()), int.Parse(myXMLGateTable["ItemNum10"].ToString())));
            }
            //////////////成就(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "BattlefieldKill"));
            //if (!NetworkHandler.instance.IsCreate)
            //{
            //    Texture Preload5 = Resources.Load("Game/sj_newdi2") as Texture;
            //}

        }
        catch (Exception ex)
        {
            Debug.Log("XMLBattlefieldPoints:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "BattlefieldPoints");
        }
    }



    public void ParseXMLBattlefieldKillScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.BattlefieldKillList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("BattlefieldKills").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddBattlefieldKillList(new BattlefieldKill(int.Parse(myXMLGateTable["ID"].ToString()), int.Parse(myXMLGateTable["Rank"].ToString()), int.Parse(myXMLGateTable["ItemID1"].ToString()), int.Parse(myXMLGateTable["ItemNum1"].ToString()),
                                                        int.Parse(myXMLGateTable["ItemID2"].ToString()), int.Parse(myXMLGateTable["ItemNum2"].ToString()), int.Parse(myXMLGateTable["ItemID3"].ToString()), int.Parse(myXMLGateTable["ItemNum3"].ToString()),
                                                        int.Parse(myXMLGateTable["ItemID4"].ToString()), int.Parse(myXMLGateTable["ItemNum4"].ToString())));
            }
            //////////////成就(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ArmsDealers"));
            //if (!NetworkHandler.instance.IsCreate)
            //{
            //    Texture Preload5 = Resources.Load("Game/sj_newdi2") as Texture;
            //}

        }
        catch (Exception ex)
        {
            Debug.Log("XMLBattlefieldKill:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "BattlefieldKill");
        }
    }

    public void ParseXMLArmsDealersScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.ArmsDealersList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ArmsDealers").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddArmsDealersList(new ArmsDealers(int.Parse(myXMLGateTable["ArmsDealerID"].ToString()), int.Parse(myXMLGateTable["WinPosID"].ToString()), int.Parse(myXMLGateTable["ItemID"].ToString()), int.Parse(myXMLGateTable["ItemNum"].ToString()),
                                                        int.Parse(myXMLGateTable["DiamondsTotalValue"].ToString()), int.Parse(myXMLGateTable["VipBuy"].ToString()), int.Parse(myXMLGateTable["BuyTime"].ToString()), int.Parse(myXMLGateTable["MinimumDiscount"].ToString())));
            }
            //////////////成就(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "SmallGoal"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLArmsDealers:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ArmsDealers");
        }
    }

    public void ParseXMLSmallGoalScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.SmallGoalList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("SmallGoals").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddSmallGoalList(new SmallGoal(int.Parse(myXMLGateTable["SmallGoalID"].ToString()), int.Parse(myXMLGateTable["Type"].ToString()), int.Parse(myXMLGateTable["Quest"].ToString()), int.Parse(myXMLGateTable["ItemID"].ToString()),
                                                        int.Parse(myXMLGateTable["ItemNum"].ToString())));
            }
            //////////////成就(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ResourceTycoon"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLSmallGoal:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "SmallGoal");
        }
    }

    public void ParseXMLResourceTycoonScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////成就(以下)//////////////
            TextTranslator.instance.ResourceTycoonList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ResourceTycoons").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddResourceTycoonList(new ResourceTycoon(int.Parse(myXMLGateTable["ResourceTycoonID"].ToString()), int.Parse(myXMLGateTable["ActivityID"].ToString()), int.Parse(myXMLGateTable["Type"].ToString()), int.Parse(myXMLGateTable["ActivitySheet"].ToString()),
                                                        int.Parse(myXMLGateTable["Sort"].ToString()), int.Parse(myXMLGateTable["CumulativeItemID"].ToString()), int.Parse(myXMLGateTable["CumulativeItemNum"].ToString()), int.Parse(myXMLGateTable["CumulativePoints"].ToString()), 
                                                        int.Parse(myXMLGateTable["ItemID1"].ToString()), int.Parse(myXMLGateTable["ItemNum1"].ToString()),int.Parse(myXMLGateTable["ItemID2"].ToString()),int.Parse(myXMLGateTable["ItemNum2"].ToString()),int.Parse(myXMLGateTable["ItemID3"].ToString()),int.Parse(myXMLGateTable["ItemNum3"].ToString()),int.Parse(myXMLGateTable["ItemID4"].ToString()),int.Parse(myXMLGateTable["ItemNum4"].ToString())));
            }
            //////////////成就(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Sign"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLResourceTycoon:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ResourceTycoon");
        }
    }

    public void ParseXMLSignScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////签到(以下)//////////////
            TextTranslator.instance.signList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Signs").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddSignList(new Sign(int.Parse(myXMLGateTable["SignID"].ToString()),
                    int.Parse(myXMLGateTable["ItemID"].ToString()), int.Parse(myXMLGateTable["ItemNum"].ToString()), int.Parse(myXMLGateTable["VipX2"].ToString())));
            }
            //////////////签到(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "SignExtra"));
            if (!NetworkHandler.instance.IsCreate)
            {
                Texture Preload6 = Resources.Load("Game/di3") as Texture;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("XMLSign:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Sign");
        }
    }

    public void ParseXMLSignExtraScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////签到累计(以下)//////////////
            TextTranslator.instance.signExtraList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("SignExtras").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                BetterList<SignExtraItemData> SignExtraItemList = new BetterList<SignExtraItemData>();
                SignExtraItemList.Add(new SignExtraItemData(int.Parse(myXMLGateTable["ItemID1"].ToString()), int.Parse(myXMLGateTable["ItemNum1"].ToString())));
                SignExtraItemList.Add(new SignExtraItemData(int.Parse(myXMLGateTable["ItemID2"].ToString()), int.Parse(myXMLGateTable["ItemNum2"].ToString())));
                SignExtraItemList.Add(new SignExtraItemData(int.Parse(myXMLGateTable["ItemID3"].ToString()), int.Parse(myXMLGateTable["ItemNum3"].ToString())));
                TextTranslator.instance.AddSignExtraList(new SignExtra(int.Parse(myXMLGateTable["SignExtraID"].ToString()),
                    int.Parse(myXMLGateTable["Day"].ToString()), SignExtraItemList, int.Parse(myXMLGateTable["VipX2"].ToString())));
            }
            //////////////签到累计(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ShopCenter"));
            if (!NetworkHandler.instance.IsCreate)
            {
                Texture Preload7 = Resources.Load("Game/gatebg") as Texture;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("XMLSignExtra:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "SignExtra");
        }
    }

    /// <summary>
    /// 商城信息载入
    /// </summary>
    public void ParseXMLShopCenterScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.ShopCenterDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ShopCenters").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddShopCenterDic(int.Parse(myXMLGateTable["ShopCenterID"].ToString()),
                                          new ShopCenter(int.Parse(myXMLGateTable["ShopCenterID"].ToString()),
                                                            int.Parse(myXMLGateTable["WindowID"].ToString()),
                                                            int.Parse(myXMLGateTable["ItemID"].ToString()),
                                                            int.Parse(myXMLGateTable["ItemNum"].ToString()),
                                                            int.Parse(myXMLGateTable["Vip"].ToString()),
                                                            int.Parse(myXMLGateTable["BuyCount"].ToString()),
                                                            int.Parse(myXMLGateTable["PeculiarID"].ToString())));
            }
            //////////////Talent(以上)//////////////     
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ShopCenterPeculiar"));
            if (!NetworkHandler.instance.IsCreate)
            {
                UIManager.instance.OpenSinglePanel("PreloadSkillText", false);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("XMLVip:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ShopCenter");
        }
    }

    /// <summary>
    /// 商城异价ID组信息载入
    /// </summary>
    public void ParseXMLShopCenterPeculiarScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.ShopCenterPeculiarDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ShopCenterPeculiars").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddShopCenterPeculiarDic(int.Parse(myXMLGateTable["ShopCenterPeculiarID"].ToString()),
                                          new ShopCenterPeculiar(int.Parse(myXMLGateTable["ShopCenterPeculiarID"].ToString()),
                                                            int.Parse(myXMLGateTable["PeculiarID"].ToString()),
                                                            int.Parse(myXMLGateTable["BuyCount"].ToString()),
                                                            int.Parse(myXMLGateTable["PriceDiamond"].ToString())));
            }
            //////////////Talent(以上)//////////////     
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ActivitySevenLogin"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLVip:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ShopCenterPeculiar");
        }
    }

    public void ParseXMLActivitySevenLoginScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////登入签到(以下)//////////////
            TextTranslator.instance.ActivitySevenLoginList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ActivitySevenLogins").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                BetterList<AwardItem> AwardItemList = new BetterList<AwardItem>();
                AwardItemList.Add(new AwardItem(int.Parse(myXMLGateTable["ItemID1"].ToString()), int.Parse(myXMLGateTable["ItemNum1"].ToString())));
                AwardItemList.Add(new AwardItem(int.Parse(myXMLGateTable["ItemID2"].ToString()), int.Parse(myXMLGateTable["ItemNum2"].ToString())));
                AwardItemList.Add(new AwardItem(int.Parse(myXMLGateTable["ItemID3"].ToString()), int.Parse(myXMLGateTable["ItemNum3"].ToString())));
                AwardItemList.Add(new AwardItem(int.Parse(myXMLGateTable["ItemID4"].ToString()), int.Parse(myXMLGateTable["ItemNum4"].ToString())));
                TextTranslator.instance.AddActivitySevenLoginList(new ActivitySevenLogin(int.Parse(myXMLGateTable["DayCount"].ToString()), AwardItemList));
            }
            //////////////登入签到(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "LegionTrain"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLActivitySevenLogin:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ActivitySevenLogin");
        }
    }


    public void ParseXMLLegionTrainScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////训练场签到(以下)//////////////
            TextTranslator.instance.LegionTrainList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("LegionTrains").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddLegionTrainList(new LegionTrain(int.Parse(myXMLGateTable["TrainID"].ToString()), int.Parse(myXMLGateTable["NeedLevel"].ToString()), int.Parse(myXMLGateTable["VipUnLock"].ToString()),
                    int.Parse(myXMLGateTable["DiamondCost"].ToString()), int.Parse(myXMLGateTable["BonusExp"].ToString())));
            }
            //////////////登入签到(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Legion"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLLegionTrain:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "LegionTrain");
        }
    }
    public void ParseXMLLegionScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////军团捐献(以下)//////////////
            TextTranslator.instance.LegionList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Legions").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                BetterList<int> ScheduleValList = new BetterList<int>();
                BetterList<AwardItem> BoxAwardList = new BetterList<AwardItem>();
                ScheduleValList.Add(int.Parse(myXMLGateTable["ScheduleVal1"].ToString()));
                ScheduleValList.Add(int.Parse(myXMLGateTable["ScheduleVal2"].ToString()));
                ScheduleValList.Add(int.Parse(myXMLGateTable["ScheduleVal3"].ToString()));
                ScheduleValList.Add(int.Parse(myXMLGateTable["ScheduleVal4"].ToString()));
                //string[] dataSplit = myXMLGateTable["Box1"].ToString().Split('!');
                //string[] dataSplitTwo = dataSplit[0].Split('$');
                //BoxAwardList.Add(new AwardItem(int.Parse(dataSplitTwo[0]), int.Parse(dataSplitTwo[1])));
                for (int i = 0; i < 4; i++)
                {
                    string[] dataSplit = myXMLGateTable[string.Format("Box{0}", i + 1)].ToString().Split('!');
                    string[] dataSplitTwo = dataSplit[0].Split('$');
                    BoxAwardList.Add(new AwardItem(int.Parse(dataSplitTwo[0]), int.Parse(dataSplitTwo[1])));
                }
                TextTranslator.instance.AddLegionList(new Legion(int.Parse(myXMLGateTable["Level"].ToString()), int.Parse(myXMLGateTable["NeedExp"].ToString()),
                        int.Parse(myXMLGateTable["LimitNum"].ToString()), ScheduleValList, BoxAwardList, int.Parse(myXMLGateTable["CrapsPlayNum"].ToString()), int.Parse(myXMLGateTable["CrapsChangeNum"].ToString()), int.Parse(myXMLGateTable["ViceKing"].ToString()), int.Parse(myXMLGateTable["CreamNum"].ToString())));
            }
            //////////////登入签到(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "LegionTask"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLLegion:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Legion");
        }
    }
    public void ParseXMLLegionTaskScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////军团任务(以下)//////////////
            TextTranslator.instance.LegionTaskList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("LegionTasks").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                BetterList<AwardItem> AwardList = new BetterList<AwardItem>();
                for (int i = 0; i < 4; i++)
                {
                    int BonusID = int.Parse(myXMLGateTable[string.Format("BonusID{0}", i + 1)].ToString());
                    int BonusNum = int.Parse(myXMLGateTable[string.Format("BonusNum{0}", i + 1)].ToString());
                    AwardList.Add(new AwardItem(BonusID, BonusNum));
                }
                TextTranslator.instance.AddLegionTaskList(new LegionTask(int.Parse(myXMLGateTable["LegionTaskID"].ToString()), myXMLGateTable["Name"].ToString(), int.Parse(myXMLGateTable["Color"].ToString()),
                    int.Parse(myXMLGateTable["Rand"].ToString()), int.Parse(myXMLGateTable["TermType"].ToString()), int.Parse(myXMLGateTable["Param1"].ToString()), int.Parse(myXMLGateTable["ParamVal1"].ToString()),
                    int.Parse(myXMLGateTable["ExpBonus"].ToString()), AwardList));
            }
            //////////////登入签到(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "LegionRank"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLLegionTask:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "LegionTask");
        }
    }
    public void ParseXMLLegionRankScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////军团伤害排名奖励(以下)//////////////
            TextTranslator.instance.LegionRankList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("LegionRanks").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                BetterList<AwardItem> BoxAwardList = new BetterList<AwardItem>();
                for (int i = 0; i < 4; i++)
                {
                    BoxAwardList.Add(new AwardItem(int.Parse(myXMLGateTable[string.Format("RankBonusItem{0}", i + 1)].ToString()), int.Parse(myXMLGateTable[string.Format("RankBonusNum{0}", i + 1)].ToString())));
                }
                TextTranslator.instance.AddLegionRankList(new LegionRank(int.Parse(myXMLGateTable["RankID"].ToString()), int.Parse(myXMLGateTable["RankNum"].ToString()), BoxAwardList));
            }

            //////////////军团伤害排名奖励(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "LegionFirstPass"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLLegion:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "LegionRank");
        }
    }

    public void ParseXMLLegionFirstPassScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////军团通关排名奖励(以下)//////////////
            TextTranslator.instance.LegionFirstPassList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("LegionFirstPasss").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                BetterList<AwardItem> BoxAwardList = new BetterList<AwardItem>();
                for (int i = 0; i < 4; i++)
                {
                    BoxAwardList.Add(new AwardItem(int.Parse(myXMLGateTable[string.Format("ItemId{0}", i + 1)].ToString()), int.Parse(myXMLGateTable[string.Format("ItemNum{0}", i + 1)].ToString())));
                }
                TextTranslator.instance.AddLegionFirstPass(new LegionRank(int.Parse(myXMLGateTable["ID"].ToString()), int.Parse(myXMLGateTable["Rnak"].ToString()), BoxAwardList));
            }
            //////////////军团通关排名奖励(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "LegionGate"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLLegion:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "LegionFirstPass");
        }
    }
    public void ParseXMLLegionGateScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////军团副本(以下)//////////////
            TextTranslator.instance.LegionGateList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("LegionGates").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                BetterList<int> BossBloodList = new BetterList<int>();
                BossBloodList.Add(int.Parse(myXMLGateTable["BossBlood1"].ToString()));
                BossBloodList.Add(int.Parse(myXMLGateTable["BossBlood2"].ToString()));
                BossBloodList.Add(int.Parse(myXMLGateTable["BossBlood3"].ToString()));
                BossBloodList.Add(int.Parse(myXMLGateTable["BossBlood4"].ToString()));
                BossBloodList.Add(int.Parse(myXMLGateTable["BossBlood5"].ToString()));
                BossBloodList.Add(int.Parse(myXMLGateTable["BossBlood6"].ToString()));
                BossBloodList.Add(int.Parse(myXMLGateTable["BossBlood7"].ToString()));
                BossBloodList.Add(int.Parse(myXMLGateTable["BossBlood8"].ToString()));
                BossBloodList.Add(int.Parse(myXMLGateTable["BossBlood9"].ToString()));
                BossBloodList.Add(int.Parse(myXMLGateTable["BossBlood10"].ToString()));
                TextTranslator.instance.AddLegionGateList(new LegionGate(int.Parse(myXMLGateTable["LegionGateID"].ToString()), int.Parse(myXMLGateTable["GateGroupID"].ToString()),
                    int.Parse(myXMLGateTable["NextGateID"].ToString()), int.Parse(myXMLGateTable["GateBoxID"].ToString()), BossBloodList));
            }
            //////////////登入签到(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "LegionGateBox"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLLegionGate:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "LegionGate");
        }
    }

    public void ParseXMLLegionGateBoxScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////军团副本(以下)//////////////
            TextTranslator.instance.LegionGateBoxList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("LegionGateBoxs").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;

                TextTranslator.instance.AddLegionGateBoxList(new LegionGateBox(int.Parse(myXMLGateTable["LegionGateBoxID"].ToString()), myXMLGateTable["GateName"].ToString(),
                    int.Parse(myXMLGateTable["GateBoxID"].ToString()), int.Parse(myXMLGateTable["ItemID"].ToString()), int.Parse(myXMLGateTable["ItemNum"].ToString()), int.Parse(myXMLGateTable["CreateBoxNum"].ToString())
                    , int.Parse(myXMLGateTable["PropNum"].ToString())));
            }
            //////////////登入签到(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "LabsLimit"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLLegionGateBox:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "LegionGateBox");
        }
    }

    public void ParseXMLLabsLimitScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////实验室(以下)//////////////
            TextTranslator.instance.LabsLimitList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("LabsLimits").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;

                BetterList<ReformLabItemData> LabItemDataList = new BetterList<ReformLabItemData>();
                int RoleType = int.Parse(myXMLGateTable["RoleType"].ToString());
                LabItemDataList.Add(new ReformLabItemData(RoleType, 1, int.Parse(myXMLGateTable["NeedLevel1"].ToString()), int.Parse(myXMLGateTable["NeedVip1"].ToString()), int.Parse(myXMLGateTable["DiamondCost1"].ToString())));
                LabItemDataList.Add(new ReformLabItemData(RoleType, 2, int.Parse(myXMLGateTable["NeedLevel2"].ToString()), int.Parse(myXMLGateTable["NeedVip2"].ToString()), int.Parse(myXMLGateTable["DiamondCost2"].ToString())));
                LabItemDataList.Add(new ReformLabItemData(RoleType, 3, int.Parse(myXMLGateTable["NeedLevel3"].ToString()), int.Parse(myXMLGateTable["NeedVip3"].ToString()), int.Parse(myXMLGateTable["DiamondCost3"].ToString())));
                LabItemDataList.Add(new ReformLabItemData(RoleType, 4, int.Parse(myXMLGateTable["NeedLevel4"].ToString()), int.Parse(myXMLGateTable["NeedVip4"].ToString()), int.Parse(myXMLGateTable["DiamondCost4"].ToString())));
                LabItemDataList.Add(new ReformLabItemData(RoleType, 5, int.Parse(myXMLGateTable["NeedLevel5"].ToString()), int.Parse(myXMLGateTable["NeedVip5"].ToString()), int.Parse(myXMLGateTable["DiamondCost5"].ToString())));

                TextTranslator.instance.AddLabsLimitList(new LabsLimit(RoleType, LabItemDataList));
            }
            //////////////登入签到(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "LabsPoint"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLLabsLimit:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "LabsLimit");
        }
    }
    public void ParseXMLLabsPointScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////实验室积分(以下)//////////////
            TextTranslator.instance.LabsPointList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("LabsPoints").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;

                TextTranslator.instance.AddLabsPointList(new LabsPoint(int.Parse(myXMLGateTable["LabsPointType"].ToString()), int.Parse(myXMLGateTable["RankPoint"].ToString())));
            }
            //////////////登入签到(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ActivitySevenDay"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLLabsPoint:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "LabsPoint");
        }
    }
    public void ParseXMLQuestionScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////问卷(以下)//////////////
            TextTranslator.instance.QuestionList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Questions").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                BetterList<string> SelectionList = new BetterList<string>();
                SelectionList.Add(myXMLGateTable["SelectA"].ToString());
                SelectionList.Add(myXMLGateTable["SelectB"].ToString());
                SelectionList.Add(myXMLGateTable["SelectC"].ToString());
                SelectionList.Add(myXMLGateTable["SelectD"].ToString());
                SelectionList.Add(myXMLGateTable["SelectE"].ToString());
                SelectionList.Add(myXMLGateTable["SelectF"].ToString());
                SelectionList.Add(myXMLGateTable["SelectG"].ToString());
                SelectionList.Add(myXMLGateTable["SelectH"].ToString());
                SelectionList.Add(myXMLGateTable["SelectI"].ToString());
                TextTranslator.instance.AddQuestionList(new Question(int.Parse(myXMLGateTable["QuestionID"].ToString()), int.Parse(myXMLGateTable["Type"].ToString()), int.Parse(myXMLGateTable["Option"].ToString()),
                    myXMLGateTable["Name"].ToString(), SelectionList));
            }
            //////////////登入签到(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ActivitySevenDay"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLQuestion:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Question");
        }
    }
    public void ParseXMLActivitySevenDayScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////七日活动(以下)//////////////
            TextTranslator.instance.ActivitySevenDayDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ActivitySevenDays").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddActivitySevenDay(int.Parse(myXMLGateTable["ActivitySevenDayID"].ToString()),
                    new ActivitySevenDay(int.Parse(myXMLGateTable["ActivitySevenDayID"].ToString()), int.Parse(myXMLGateTable["DayCount"].ToString()),
                                         int.Parse(myXMLGateTable["BonusGroupID"].ToString()), int.Parse(myXMLGateTable["ItemID1"].ToString()),
                                         int.Parse(myXMLGateTable["ItemNum1"].ToString()), int.Parse(myXMLGateTable["ItemID2"].ToString()),
                                         int.Parse(myXMLGateTable["ItemNum2"].ToString()), int.Parse(myXMLGateTable["ItemID3"].ToString()),
                                         int.Parse(myXMLGateTable["ItemNum3"].ToString()), int.Parse(myXMLGateTable["ItemID4"].ToString()),
                                         int.Parse(myXMLGateTable["ItemNum4"].ToString()), int.Parse(myXMLGateTable["LimitType"].ToString()),
                                         myXMLGateTable["LimitTerm"].ToString()));//int.Parse(myXMLGateTable["LimitTerm"].ToString())
            }
            //////////////七日活动(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ActivitySevenRank"));
            if (!NetworkHandler.instance.IsCreate)
            {
                NetworkHandler.instance.SendProcess("9121#;"); //取得嘉年华列表
            }
        }
        catch (Exception ex)
        {
            Debug.Log("XMLActivitySevenDay:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ActivitySevenDay");
        }
    }

    public void ParseXMLActivitySevenRankScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////七日活动(以下)//////////////
            TextTranslator.instance.ActivitySevenRankList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ActivitySevenRanks").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                BetterList<AwardItem> mAwardList = new BetterList<AwardItem>();
                mAwardList.Add(new AwardItem(int.Parse(myXMLGateTable["BonusID1"].ToString()), int.Parse(myXMLGateTable["BonusNum1"].ToString())));
                mAwardList.Add(new AwardItem(int.Parse(myXMLGateTable["BonusID2"].ToString()), int.Parse(myXMLGateTable["BonusNum2"].ToString())));
                mAwardList.Add(new AwardItem(int.Parse(myXMLGateTable["BonusID3"].ToString()), int.Parse(myXMLGateTable["BonusNum3"].ToString())));
                TextTranslator.instance.AddActivitySevenRank(
                    new ActivitySevenRank(int.Parse(myXMLGateTable["ActivitySevenRankID"].ToString()), int.Parse(myXMLGateTable["Force"].ToString()),
                                         mAwardList));
            }
            //////////////七日活动(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ActivitySevenHero"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLActivitySevenRank:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ActivitySevenRank");
        }
    }

    public void ParseXMLActivitySevenHeroScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////七日活动(以下)//////////////
            TextTranslator.instance.ActivitySevenHeroList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ActivitySevenHeros").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                BetterList<AwardItem> mAwardList = new BetterList<AwardItem>();
                mAwardList.Add(new AwardItem(int.Parse(myXMLGateTable["BonusID1"].ToString()), int.Parse(myXMLGateTable["BonusNum1"].ToString())));
                mAwardList.Add(new AwardItem(int.Parse(myXMLGateTable["BonusID2"].ToString()), int.Parse(myXMLGateTable["BonusNum2"].ToString())));
                mAwardList.Add(new AwardItem(int.Parse(myXMLGateTable["BonusID3"].ToString()), int.Parse(myXMLGateTable["BonusNum3"].ToString())));
                TextTranslator.instance.AddActivitySevenHero(
                    new ActivitySevenHero(int.Parse(myXMLGateTable["ActivitySevenHeroID"].ToString()), int.Parse(myXMLGateTable["RankID"].ToString()),
                                         mAwardList));
            }
            //////////////七日活动(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ActivityHalfLimitBuy"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLActivitySevenHero:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ActivitySevenHero");
        }
    }
    public void ParseXMLActivityHalfLimitBuyScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////七日折扣(以下)//////////////
            TextTranslator.instance.ActivityHalfLimitBuyDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ActivityHalfLimitBuys").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddActivityHalfLimitBuy(int.Parse(myXMLGateTable["ActivityHalfLimitBuyID"].ToString()), new ActivityHalfLimitBuy(int.Parse(myXMLGateTable["ActivityHalfLimitBuyID"].ToString()),
                    int.Parse(myXMLGateTable["OpenDayCount"].ToString()), int.Parse(myXMLGateTable["ItemID"].ToString()), int.Parse(myXMLGateTable["ItemNum"].ToString()),
                    int.Parse(myXMLGateTable["Price"].ToString()), int.Parse(myXMLGateTable["NowPrice"].ToString()), int.Parse(myXMLGateTable["ReBate"].ToString()), int.Parse(myXMLGateTable["LimitBuyNum"].ToString()),
                    int.Parse(myXMLGateTable["LimitVip"].ToString())));
                //Debug.LogError("aaaaaaaaaaaaa" + TextTranslator.instance.ActivityHalfLimitBuyDic[1].Price);
            }
            //////////////七日折扣(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ItemSort"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLActivitySevenDay:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ActivityHalfLimitBuy");
        }
    }
    public void ParseXMLItemSortScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////道具(以下)//////////////
            TextTranslator.instance.itemSortList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ItemSorts").Children)
            {

                Hashtable myXMLItemTable = child.Attributes;
                TextTranslator.instance.AddItemSort(new ItemSort(int.Parse(myXMLItemTable["ItemSortID"].ToString()), int.Parse(myXMLItemTable["ItemID"].ToString())));
            }
            //////////////道具(以上)//////////////

            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "NewGuide"));

        }
        catch (Exception ex)
        {
            Debug.Log("XMLItemSort:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ItemSort");
        }
    }

    public void ParseXMLNewGuideScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////等级开启NewGuide(以下)//////////////
            TextTranslator.instance.newGuideList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("NewGuides").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;

                TextTranslator.instance.AddNewGuideList(new NewGuide(int.Parse(myXMLGateTable["NewGuideID"].ToString()), int.Parse(myXMLGateTable["Level"].ToString()), myXMLGateTable["Name"].ToString(),
                    myXMLGateTable["Des"].ToString(), myXMLGateTable["MainIcon"].ToString(), myXMLGateTable["LevelUpTipIcon"].ToString(), myXMLGateTable["MapDes"].ToString()));
            }
            //////////////等级开启NewGuide(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Skill"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLNewGuide:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "NewGuide");
        }
    }

    public void ParseXMLSkillScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\Skill.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////技能(以下)//////////////
            TextTranslator.instance.skilllist.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Skills").Children)
            {
                Hashtable myXMLSkillTable = child.Attributes;
                //string[] chainSplit = myXMLSkillTable["Chain"].ToString().Split('$');
                List<int> chain = new List<int>();
                //for (int i = 0; i < chainSplit.Length - 1; i++)
                //{
                //    if (chainSplit[i] != "0")
                //    {
                //        chain.Add(int.Parse(chainSplit[i]));
                //    }
                //}
                TextTranslator.instance.AddSkillList(new Skill(int.Parse(myXMLSkillTable["SkillID"].ToString()), myXMLSkillTable["Name"].ToString(),
                    int.Parse(myXMLSkillTable["Type"].ToString()), int.Parse(myXMLSkillTable["Level"].ToString()), myXMLSkillTable["Des"].ToString(), 0, 0,
                    0, 0, 0, "0", 0, 0, 0, 0, int.Parse(myXMLSkillTable["Area"].ToString()), 0, int.Parse(myXMLSkillTable["Color"].ToString()), chain, 0,
                    int.Parse(myXMLSkillTable["Type1"].ToString()), int.Parse(myXMLSkillTable["Val1"].ToString()), int.Parse(myXMLSkillTable["Duration1"].ToString()),
                    int.Parse(myXMLSkillTable["Type2"].ToString()), int.Parse(myXMLSkillTable["Val2"].ToString()), int.Parse(myXMLSkillTable["Duration2"].ToString()),
                    int.Parse(myXMLSkillTable["Type3"].ToString()), int.Parse(myXMLSkillTable["Val3"].ToString()), int.Parse(myXMLSkillTable["Duration3"].ToString()), int.Parse(myXMLSkillTable["Buff"].ToString())));
            }
            //////////////技能(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ManualSkill"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLSkill:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Skill");
        }
    }


    public void ParseXMLManualSkillScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\ManualSkill.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////技能(以下)//////////////
            TextTranslator.instance.ManualSkillList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ManualSkills").Children)
            {
                Hashtable myXMLSkillTable = child.Attributes;
                TextTranslator.instance.AddManualSkillList(new ManualSkill(int.Parse(myXMLSkillTable["Type"].ToString()), int.Parse(myXMLSkillTable["ManualSkillID"].ToString()), myXMLSkillTable["Name"].ToString(), myXMLSkillTable["Des"].ToString(),
                    int.Parse(myXMLSkillTable["CdTime"].ToString()), int.Parse(myXMLSkillTable["OpenLevel"].ToString()), int.Parse(myXMLSkillTable["RoundNum"].ToString()), int.Parse(myXMLSkillTable["ToObj"].ToString()),
                    float.Parse(myXMLSkillTable["Param1"].ToString()), float.Parse(myXMLSkillTable["Param2"].ToString()), float.Parse(myXMLSkillTable["Param3"].ToString())
                    , float.Parse(myXMLSkillTable["Param4"].ToString()), float.Parse(myXMLSkillTable["Param5"].ToString()), int.Parse(myXMLSkillTable["Buff"].ToString()), int.Parse(myXMLSkillTable["Camp"].ToString())));
            }
            //////////////技能(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Buff"));
        }
        catch (Exception ex)
        {
            Debug.Log("ManualSkill:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ManualSkill");
        }
    }

    public void ParseXMLBuffScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\Buff.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////技能(以下)//////////////
            TextTranslator.instance.buffList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Buffs").Children)
            {
                Hashtable myXMLSkillTable = child.Attributes;
                TextTranslator.instance.AddBuffList(new Buff(int.Parse(myXMLSkillTable["BuffID"].ToString()), int.Parse(myXMLSkillTable["ActionType"].ToString()), int.Parse(myXMLSkillTable["Type"].ToString()),
                    int.Parse(myXMLSkillTable["IsStack"].ToString()), int.Parse(myXMLSkillTable["IsOnRound"].ToString()), int.Parse(myXMLSkillTable["ToObj"].ToString()), myXMLSkillTable["EffectName"].ToString(), int.Parse(myXMLSkillTable["OnRoundCount"].ToString()), float.Parse(myXMLSkillTable["EffectVal1"].ToString()),
                    float.Parse(myXMLSkillTable["EffectVal2"].ToString()), int.Parse(myXMLSkillTable["Icon"].ToString()), myXMLSkillTable["SpecialEffect"].ToString(), myXMLSkillTable["EffectInit"].ToString(), myXMLSkillTable["Des"].ToString()));
            }
            //////////////技能(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "NpcTactics"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLBuff:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Buff");
        }
    }


    public void ParseXMLNPCTacticsScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\NpcTactics.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////技能(以下)//////////////
            TextTranslator.instance.NPCTacticsList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("NpcTacticss").Children)
            {
                Hashtable myXMLSkillTable = child.Attributes;
                TextTranslator.instance.AddNPCTacticsList(new NPCTactics(int.Parse(myXMLSkillTable["NpcTacticsID"].ToString()), int.Parse(myXMLSkillTable["GroupID"].ToString()), int.Parse(myXMLSkillTable["GoNum"].ToString()),
                    int.Parse(myXMLSkillTable["ManualSkillID"].ToString()), int.Parse(myXMLSkillTable["ManualSkillNum"].ToString())));
            }
            //////////////技能(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "EquipRefine"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLBuff:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "NPCTactics");
        }
    }


    /// <summary>
    /// 装备信息载入
    /// </summary>
    /// <param name="ParseXMLText"></param>
    public void ParseXMLEquipRefineScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////装备(以下)//////////////
            TextTranslator.instance.equipInfoDic.Clear();

            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("EquipRefines").Children)
            {
                Hashtable myXMLEquipTable = child.Attributes;
                TextTranslator.instance.AddEquipInfoList(new EquipDetailInfo(int.Parse(myXMLEquipTable["LvMax"].ToString()),
                    float.Parse(myXMLEquipTable["Hp"].ToString()), float.Parse(myXMLEquipTable["Atk"].ToString()), int.Parse(myXMLEquipTable["Def"].ToString()),
                    float.Parse(myXMLEquipTable["Hit"].ToString()), float.Parse(myXMLEquipTable["NoHit"].ToString()), float.Parse(myXMLEquipTable["Crit"].ToString()), float.Parse(myXMLEquipTable["NoCrit"].ToString()),
                    float.Parse(myXMLEquipTable["DmgBonus"].ToString()), float.Parse(myXMLEquipTable["DmgReduction"].ToString()), int.Parse(myXMLEquipTable["EquipID"].ToString())));
            }
            //////////////装备(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "EquipRefineCost"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLEquip:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "EquipRefine");
        }
    }


    /// <summary>
    /// 装备信息载入
    /// </summary>
    /// <param name="ParseXMLText"></param>
    public void ParseXMLEquipRefineCostScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////装备(以下)//////////////
            TextTranslator.instance.equipRefineCostDic.Clear();

            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("EquipRefineCosts").Children)
            {
                Hashtable myXMLEquipTable = child.Attributes;
                TextTranslator.instance.AddEquipRefineCost(new EquipRefineCost(int.Parse(myXMLEquipTable["EquipRefineLevel"].ToString()), int.Parse(myXMLEquipTable["NeedLevel"].ToString()),
                    int.Parse(myXMLEquipTable["Exp"].ToString()), int.Parse(myXMLEquipTable["DecoStoneNum"].ToString()), int.Parse(myXMLEquipTable["DecoNum"].ToString()), int.Parse(myXMLEquipTable["DecoGold"].ToString())));
            }
            //////////////装备(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Vip"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLEquip:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "EquipRefineCost");
        }
    }

    /// <summary>
    /// VIP信息载入
    /// </summary>
    /// <param name="ParseXMLText"></param>
    public void ParseXMLVipScript(string ParseXMLText)
    {
        //SP.LoadXml(ParseXMLText);
        //System.Security.SecurityElement SE = SP.ToXml();

        ////////////////VIP(以下)//////////////
        //TextTranslator.instance.vipInfoArray.Clear();
        //foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Vips").Children)
        //{
        //    Hashtable myXMLGateTable = child.Attributes;
        //    TextTranslator.instance.AddVipUse(new VipInfo(int.Parse(myXMLGateTable["Vip"].ToString()),
        //        int.Parse(myXMLGateTable["VipNeedLunaGem"].ToString()), myXMLGateTable["Description"].ToString()));
        //}
        ////////////////VIP(以上)//////////////

        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.VipDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Vips").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddVipDic(int.Parse(myXMLGateTable["VipID"].ToString()), new Vip(int.Parse(myXMLGateTable["VipID"].ToString()),
                    int.Parse(myXMLGateTable["TradingDiamondCount"].ToString()),
                    int.Parse(myXMLGateTable["GateRushState10"].ToString()),
                    int.Parse(myXMLGateTable["DayCoinTreeCount"].ToString()),
                    int.Parse(myXMLGateTable["CoinTreeBooRate"].ToString()),
                    int.Parse(myXMLGateTable["ResetMasterGateCount"].ToString()),
                    //int.Parse(myXMLGateTable["OnceIndianaState"].ToString()),
                    //int.Parse(myXMLGateTable["TowerFightSkipState"].ToString()),
                    int.Parse(myXMLGateTable["Indiana5State"].ToString()),
                    //int.Parse(myXMLGateTable["ClubMoneyState"].ToString()),
                    int.Parse(myXMLGateTable["RoleWashState"].ToString()),
                    int.Parse(myXMLGateTable["PvpCdTime"].ToString()),
                    myXMLGateTable["NewTaskID"].ToString(),
                    float.Parse(myXMLGateTable["TowerPointAddRate"].ToString()),
                    int.Parse(myXMLGateTable["PvpFightSkipState"].ToString()),
                    int.Parse(myXMLGateTable["GateActivityGoldCdTime"].ToString()),
                    int.Parse(myXMLGateTable["GateActivityRwdCdTime"].ToString()),
                    myXMLGateTable["VipGift"].ToString(),
                    int.Parse(myXMLGateTable["BuyDiamondPrice"].ToString()),
                    int.Parse(myXMLGateTable["BuyDiamondSale"].ToString()),
                    myXMLGateTable["Des"].ToString(),
                    float.Parse(myXMLGateTable["GoldBooRateX1"].ToString()),
                    float.Parse(myXMLGateTable["GoldBooRateX2"].ToString()),
                    float.Parse(myXMLGateTable["GoldBooRateX3"].ToString()),
                    float.Parse(myXMLGateTable["GoldBooRateX4"].ToString()),
                    float.Parse(myXMLGateTable["GoldBooRateX5"].ToString()),
                    float.Parse(myXMLGateTable["GoldBooRateX10"].ToString())));
            }
            //////////////Talent(以上)//////////////     
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "FightMotion"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLVip:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Vip");
        }
    }


    public void ParseXMLTerrainScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\Terrain.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////地形(以下)//////////////
            TextTranslator.instance.terrainInfoList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Terrains").Children)
            {
                Hashtable myXMLTerrainTable = child.Attributes;
                TextTranslator.instance.AddTerrain(int.Parse(myXMLTerrainTable["TerrainID"].ToString()), myXMLTerrainTable["Name"].ToString(), myXMLTerrainTable["Des"].ToString(), int.Parse(myXMLTerrainTable["BuffID"].ToString()), int.Parse(myXMLTerrainTable["Icon"].ToString()));
            }
            //////////////地形(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Achievement"));
        }
        catch (Exception ex)
        {
            Debug.Log("Hero:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Terrain");
        }
    }

    /// <summary>
    /// FightMotion载入
    /// </summary>
    /// <param name="ParseXMLText"></param>
    public void ParseXMLFightMotionScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\FightMotion.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////FightMotion(以下)//////////////
            TextTranslator.instance.fightMotionDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Motions").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddFightMotion(int.Parse(myXMLGateTable["MotionID"].ToString()), new FightMotion(
                    int.Parse(myXMLGateTable["MotionID"].ToString()), int.Parse(myXMLGateTable["Id"].ToString()),
                    int.Parse(myXMLGateTable["CardID"].ToString()), myXMLGateTable["Remark"].ToString(),
                    myXMLGateTable["EffectList"].ToString(), myXMLGateTable["EffectTimeList"].ToString(),
                    myXMLGateTable["CameraList"].ToString(), myXMLGateTable["CameraTimeList"].ToString()));
            }
            //////////////FightMotion(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "FightEffect"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLFightMotion:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "FightMotion");
        }
    }

    /// <summary>
    /// FightEffect载入
    /// </summary>
    /// <param name="ParseXMLText"></param>
    public void ParseXMLFightEffectScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\FightEffect.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////FightEffect(以下)//////////////
            TextTranslator.instance.fightEffectDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Effects").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddFightEffect(int.Parse(myXMLGateTable["EffectID"].ToString()), new FightEffect(
                    int.Parse(myXMLGateTable["EffectID"].ToString()), myXMLGateTable["Remark"].ToString(),
                    myXMLGateTable["Sourd"].ToString(), myXMLGateTable["EffectParent"].ToString(), myXMLGateTable["Effect"].ToString(),
                    int.Parse(myXMLGateTable["EffectPosX"].ToString()), int.Parse(myXMLGateTable["EffectPosY"].ToString()),
                    int.Parse(myXMLGateTable["EffectRatio"].ToString()), int.Parse(myXMLGateTable["Projectile"].ToString()),
                    int.Parse(myXMLGateTable["Shake"].ToString()), int.Parse(myXMLGateTable["ShakeRange"].ToString()),
                    int.Parse(myXMLGateTable["ShakeTime"].ToString()), int.Parse(myXMLGateTable["SoundDelay"].ToString()), int.Parse(myXMLGateTable["ShakeDelay"].ToString())));
            }
            //////////////FightEffect(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "FightProjectile"));
        }
        catch (Exception ex)
        {
            Debug.Log("XMLFightEffect:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "FightEffect");
        }
    }

    /// <summary>
    /// FightProjectile载入
    /// </summary>
    /// <param name="ParseXMLText"></param>
    public void ParseXMLFightProjectileScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\FightProjectile.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////FightProjectile(以下)//////////////
            TextTranslator.instance.fightProjectileDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Projectiles").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                //Debug.Log(myXMLGateTable["ProjectileID"].ToString());
                TextTranslator.instance.AddFightProjectile(int.Parse(myXMLGateTable["ProjectileID"].ToString()), new FightProjectile(
                    int.Parse(myXMLGateTable["ProjectileID"].ToString()), myXMLGateTable["Ramark"].ToString(),
                    myXMLGateTable["Sound"].ToString(), myXMLGateTable["ProjectileEffect"].ToString(),
                    int.Parse(myXMLGateTable["ProjectilePosX"].ToString()), int.Parse(myXMLGateTable["ProjectilePosY"].ToString()), int.Parse(myXMLGateTable["ProjectilePosZ"].ToString()),
                    int.Parse(myXMLGateTable["EffectRatio"].ToString()), int.Parse(myXMLGateTable["Destroy"].ToString()), int.Parse(myXMLGateTable["TouchMode"].ToString()),
                    myXMLGateTable["TouchEffect"].ToString(), myXMLGateTable["TouchEffect2"].ToString(), int.Parse(myXMLGateTable["TouchEffectPosX"].ToString()),
                    int.Parse(myXMLGateTable["TouchEffectPosY"].ToString()), int.Parse(myXMLGateTable["ProjectileSpeed"].ToString()),
                    int.Parse(myXMLGateTable["EffectDelay"].ToString()), int.Parse(myXMLGateTable["SummonFlag"].ToString()), int.Parse(myXMLGateTable["AllyFlag"].ToString())));
            }
            //////////////FightProjectile(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "FightCamera"));

            if (!NetworkHandler.instance.IsCreate)
            {
                CharacterRecorder.instance.ownedHeroList.Clear();
                NetworkHandler.instance.SendProcess("1004#0;");
                NetworkHandler.instance.SendProcess("1005#0;");
                SceneTransformer.instance.ShowMainScene(false);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("FightProjectile:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "FightProjectile");
        }
    }

    /// <summary>
    /// FightCamera载入
    /// </summary>
    /// <param name="ParseXMLText"></param>
    public void ParseXMLFightCameraScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\FightCamera.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////FightCamera(以下)//////////////
            TextTranslator.instance.fightCameraDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Cameras").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddFightCamera(int.Parse(myXMLGateTable["CameraID"].ToString()), new FightCamera(
                    int.Parse(myXMLGateTable["CameraID"].ToString()), myXMLGateTable["Remark"].ToString(),
                    int.Parse(myXMLGateTable["TargetRule"].ToString()), int.Parse(myXMLGateTable["CameraSpeed"].ToString()),
                    int.Parse(myXMLGateTable["ZoomAction"].ToString())));
            }
            //////////////FightCamera(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "BattleMap"));
        }
        catch (Exception ex)
        {
            Debug.Log("FightCamera:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "FightCamera");
        }
    }

    public void ParseXMLBattleMapScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////BattleMaps(以下)//////////////
            TextTranslator.instance.battleMapDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("BattleMaps").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddBattleMap(int.Parse(myXMLGateTable["BattleMapID"].ToString()), new BattleMap(
                    int.Parse(myXMLGateTable["BattleMapID"].ToString()), myXMLGateTable["PrefabName"].ToString(), int.Parse(myXMLGateTable["FogStartDistance"].ToString()),
                    int.Parse(myXMLGateTable["FogEndDistance"].ToString()), int.Parse(myXMLGateTable["FogDensity"].ToString()), int.Parse(myXMLGateTable["FogColorR"].ToString()),
                    int.Parse(myXMLGateTable["FogColorG"].ToString()), int.Parse(myXMLGateTable["FogColorB"].ToString()), int.Parse(myXMLGateTable["Sound"].ToString()),
                    int.Parse(myXMLGateTable["AmbientR"].ToString()), int.Parse(myXMLGateTable["AmbientG"].ToString()), int.Parse(myXMLGateTable["AmbientB"].ToString()),
                    int.Parse(myXMLGateTable["DirectLight"].ToString()), int.Parse(myXMLGateTable["DirectLightR"].ToString()), int.Parse(myXMLGateTable["DirectLightG"].ToString()), int.Parse(myXMLGateTable["DirectLightB"].ToString()),
                    int.Parse(myXMLGateTable["DarkLight"].ToString()), int.Parse(myXMLGateTable["DarkLightR"].ToString()), int.Parse(myXMLGateTable["DarkLightG"].ToString()), int.Parse(myXMLGateTable["DarkLightB"].ToString()),
                    int.Parse(myXMLGateTable["BackLight"].ToString()), int.Parse(myXMLGateTable["BackLightR"].ToString()), int.Parse(myXMLGateTable["BackLightG"].ToString()), int.Parse(myXMLGateTable["BackLightB"].ToString()),

                    int.Parse(myXMLGateTable["LightMapNum"].ToString())));
            }
            //////////////BattleMaps(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Chapter"));
        }
        catch (Exception ex)
        {
            Debug.Log("FightCamera:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "BattleMap");
        }
    }

    public void ParseXMLChapterScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////FightCamera(以下)//////////////
            TextTranslator.instance.listChapter.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Chapters").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddChapter(new GateChapter(int.Parse(myXMLGateTable["ChapterID"].ToString()), myXMLGateTable["Name"].ToString(), int.Parse(myXMLGateTable["Level"].ToString())));
            }
            //////////////FightCamera(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "GateCompleteBox"));
        }
        catch (Exception ex)
        {
            Debug.Log("FightCamera:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Chapter");
        }
    }
    public void ParseXMLGateCompleteBoxScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////FightCamera(以下)//////////////
            //TextTranslator.instance.fightCameraDic.Clear();
            TextTranslator.instance.listGateCompleteBox.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("GateCompleteBoxs").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                //TextTranslator.instance.AddChapter(new GateChapter(int.Parse(myXMLGateTable["ChapterID"].ToString()), myXMLGateTable["Name"].ToString(), int.Parse(myXMLGateTable["Level"].ToString())));
                TextTranslator.instance.AddGateCompleteBox(new GateCompleteBox(int.Parse(myXMLGateTable["GateCompleteBoxID"].ToString()), int.Parse(myXMLGateTable["GroupID"].ToString()), int.Parse(myXMLGateTable["ItemID1"].ToString()), int.Parse(myXMLGateTable["ItemNum1"].ToString()), int.Parse(myXMLGateTable["ItemID2"].ToString()), int.Parse(myXMLGateTable["ItemNum2"].ToString()), int.Parse(myXMLGateTable["ItemID3"].ToString()), int.Parse(myXMLGateTable["ItemNum3"].ToString()), int.Parse(myXMLGateTable["ItemID4"].ToString()), int.Parse(myXMLGateTable["ItemNum4"].ToString()), int.Parse(myXMLGateTable["ItemID5"].ToString()), int.Parse(myXMLGateTable["ItemNum5"].ToString()), int.Parse(myXMLGateTable["ItemID6"].ToString()), int.Parse(myXMLGateTable["ItemNum6"].ToString())));
            }
            //////////////FightCamera(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "GateRankBox"));
        }
        catch (Exception ex)
        {
            Debug.Log("FightCamera:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "GateCompleteBox");
        }
    }
    public void ParseXMLGateRankBoxScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////FightCamera(以下)//////////////
            //TextTranslator.instance.fightCameraDic.Clear();
            TextTranslator.instance.listGateRankBox.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("GateRankBoxs").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                //TextTranslator.instance.AddChapter(new GateChapter(int.Parse(myXMLGateTable["ChapterID"].ToString()), myXMLGateTable["Name"].ToString(), int.Parse(myXMLGateTable["Level"].ToString())));
                TextTranslator.instance.AddGateRankBox(new GateRankBox(int.Parse(myXMLGateTable["GateRankBoxID"].ToString()), int.Parse(myXMLGateTable["GateType"].ToString()), int.Parse(myXMLGateTable["GroupID"].ToString()), int.Parse(myXMLGateTable["StarNum"].ToString()), int.Parse(myXMLGateTable["ItemID1"].ToString()), int.Parse(myXMLGateTable["ItemNum1"].ToString()), int.Parse(myXMLGateTable["ItemID2"].ToString()), int.Parse(myXMLGateTable["ItemNum2"].ToString()), int.Parse(myXMLGateTable["ItemID3"].ToString()), int.Parse(myXMLGateTable["ItemNum3"].ToString()), int.Parse(myXMLGateTable["ItemID4"].ToString()), int.Parse(myXMLGateTable["ItemNum4"].ToString()), int.Parse(myXMLGateTable["ItemID5"].ToString()), int.Parse(myXMLGateTable["ItemNum5"].ToString()), int.Parse(myXMLGateTable["ItemID6"].ToString()), int.Parse(myXMLGateTable["ItemNum6"].ToString())));
            }
            //////////////FightCamera(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "GateLimit"));
        }
        catch (Exception ex)
        {
            Debug.Log("FightCamera:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "GateRankBox");
        }
    }
    public void ParseXMLGateLimitScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\GateLimit.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////FightCamera(以下)//////////////
            TextTranslator.instance.GateLimitDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("GateLimits").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddGateLimitDic(int.Parse(myXMLGateTable["GateLimitID"].ToString()), new GateLimit(int.Parse(myXMLGateTable["GateLimitID"].ToString()), int.Parse(myXMLGateTable["LimitTerm"].ToString()),
                    int.Parse(myXMLGateTable["LimitParam1"].ToString()), int.Parse(myXMLGateTable["LimitParam2"].ToString()), int.Parse(myXMLGateTable["WinTerm"].ToString()), int.Parse(myXMLGateTable["WinParam1"].ToString())
                    , int.Parse(myXMLGateTable["WinParam2"].ToString()), int.Parse(myXMLGateTable["Star2Term"].ToString()), int.Parse(myXMLGateTable["Star2Param1"].ToString()), int.Parse(myXMLGateTable["Star2Param2"].ToString()),
                    int.Parse(myXMLGateTable["Star3Term"].ToString()), int.Parse(myXMLGateTable["Star3Param1"].ToString()), int.Parse(myXMLGateTable["Star3Param2"].ToString()), int.Parse(myXMLGateTable["EffectKaZaType"].ToString())));

            }
            //////////////FightCamera(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "RoleRankNeed"));
        }
        catch (Exception ex)
        {
            Debug.Log("GateLimit:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "GateLimit");
        }
    }
    public void ParseXMLRoleRankNeedScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////RoleRankNeed(以下)//////////////
            TextTranslator.instance.listRoleRankNeed.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("RoleRankNeeds").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddRoleRankNeed(new RoleRankNeedInfo(int.Parse(myXMLGateTable["Rank"].ToString()), int.Parse(myXMLGateTable["ExNeedPiece"].ToString()), int.Parse(myXMLGateTable["ExNeedCoin"].ToString()),
                    int.Parse(myXMLGateTable["UpNeedPiece"].ToString()), int.Parse(myXMLGateTable["UpNeedCoin"].ToString()), int.Parse(myXMLGateTable["ConvertPiece"].ToString())));
            }
            //////////////RoleRankNeed(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Innate"));
        }
        catch (Exception ex)
        {
            Debug.Log("FightCamera:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "RoleRankNeed");
        }
    }

    public void ParseXMLCareerScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Career(以下)//////////////
            TextTranslator.instance.heroCareerInfoList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Careers").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddCareerInfoList(new CareerInfo(int.Parse(myXMLGateTable["CareerId"].ToString()), myXMLGateTable["Name"].ToString()));
            }
            //////////////Career(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Innate"));
        }
        catch (Exception ex)
        {
            Debug.Log("FightCamera:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Career");
        }
    }
    public void ParseXMLInnatesScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {

            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Career(以下)//////////////
            TextTranslator.instance.InnatesList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Innates").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddInnate(new Innates(int.Parse(myXMLGateTable["TalentID"].ToString()),
                    int.Parse(myXMLGateTable["TalentType"].ToString()), int.Parse(myXMLGateTable["Seat"].ToString()),
                    int.Parse(myXMLGateTable["UnlockCondition"].ToString()), int.Parse(myXMLGateTable["EffectType"].ToString()),
                    int.Parse(myXMLGateTable["ValueType1"].ToString()), int.Parse(myXMLGateTable["Value1"].ToString()), int.Parse(myXMLGateTable["ValueType2"].ToString()),
                    int.Parse(myXMLGateTable["Value2"].ToString()), myXMLGateTable["Name"].ToString(), int.Parse(myXMLGateTable["BackendReading"].ToString()), myXMLGateTable["Des"].ToString(),
                    int.Parse(myXMLGateTable["CostType"].ToString()), int.Parse(myXMLGateTable["CostValue"].ToString()), int.Parse(myXMLGateTable["TalentCost"].ToString())));
            }
            //////////////Career(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "InnateExchange"));
        }
        catch (Exception ex)
        {
            Debug.Log("FightCamera:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Innate");
        }
    }
    /// <summary>
    /// 天赋消耗
    /// </summary>
    /// <param name="ParseXMLText"></param>
    public void ParseXMLInnateExchangesScript(string ParseXMLText)
    {
       
        SecurityParser SP = new SecurityParser();
        try
        {

            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Career(以下)//////////////
            TextTranslator.instance.innateExchange.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("InnateExchanges").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddInnateExchangeList(new InnateExchange(int.Parse(myXMLGateTable["TalentExchangeID"].ToString()),
                    int.Parse(myXMLGateTable["TalentPoint"].ToString()), int.Parse(myXMLGateTable["NeedPlayLevel"].ToString()),
                    int.Parse(myXMLGateTable["ConsumeHeroesFragment"].ToString()), int.Parse(myXMLGateTable["ConsumeResources1"].ToString()),
                    int.Parse(myXMLGateTable["ResourcesNum1"].ToString()), int.Parse(myXMLGateTable["ConsumeResources2"].ToString()), int.Parse(myXMLGateTable["ResourcesNum2"].ToString())));
            }
            //////////////Career(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Resource"));
        }
        catch (Exception ex)
        {
            Debug.Log("FightCamera:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "InnateExchange");
        }
    }
    public void ParseXMLResourceScript(string ParseXMLText)
    {

        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.ResourceDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Resources").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddResource(int.Parse(myXMLGateTable["ResourceID"].ToString()), new Resource(
                    int.Parse(myXMLGateTable["ResourceID"].ToString()), int.Parse(myXMLGateTable["MapID"].ToString()), int.Parse(myXMLGateTable["ChapterID"].ToString()), int.Parse(myXMLGateTable["EnergyCost"].ToString()),
                    int.Parse(myXMLGateTable["ResTime"].ToString()), myXMLGateTable["RoleDebrisNum"].ToString(), myXMLGateTable["DiaBonus"].ToString(), myXMLGateTable["DiaRand"].ToString(),
                    myXMLGateTable["GoldBonus"].ToString(), myXMLGateTable["GoldRand"].ToString(), int.Parse(myXMLGateTable["DebrisID1"].ToString()), myXMLGateTable["DebrisNum1"].ToString(),
                    int.Parse(myXMLGateTable["DebrisRand1"].ToString()), int.Parse(myXMLGateTable["DebrisID2"].ToString()), myXMLGateTable["DebrisNum2"].ToString(), int.Parse(myXMLGateTable["DebrisRand2"].ToString()),
                    int.Parse(myXMLGateTable["DebrisID3"].ToString()), myXMLGateTable["DebrisNum3"].ToString(), int.Parse(myXMLGateTable["DebrisRand3"].ToString()),
                    int.Parse(myXMLGateTable["DebrisID4"].ToString()), myXMLGateTable["DebrisNum4"].ToString(), int.Parse(myXMLGateTable["DebrisRand4"].ToString()),
                    int.Parse(myXMLGateTable["ItemID1"].ToString()), myXMLGateTable["ItemNum1"].ToString(), int.Parse(myXMLGateTable["ItemRand1"].ToString()),
                    int.Parse(myXMLGateTable["ItemID2"].ToString()), myXMLGateTable["ItemNum2"].ToString(), int.Parse(myXMLGateTable["ItemRand2"].ToString()),
                    int.Parse(myXMLGateTable["ItemID3"].ToString()), myXMLGateTable["ItemNum3"].ToString(), int.Parse(myXMLGateTable["ItemRand3"].ToString()),
                    int.Parse(myXMLGateTable["ItemID4"].ToString()), myXMLGateTable["ItemNum4"].ToString(), int.Parse(myXMLGateTable["ItemRand4"].ToString()),
                    int.Parse(myXMLGateTable["ItemID5"].ToString()), myXMLGateTable["ItemNum5"].ToString(), int.Parse(myXMLGateTable["ItemRand5"].ToString())));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "RoleClassUp"));
        }
        catch (Exception ex)
        {
            Debug.LogError("Resource:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Resource");
        }
    }
    public void ParseXMLRoleClassUpScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\RoleClassUp.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////RoleClassUp(以下)//////////////
            TextTranslator.instance.RoleClassUpList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("RoleAdvances").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddRoleClassUp(new RoleClassUp(int.Parse(myXMLGateTable["RoleID"].ToString()), myXMLGateTable["Name"].ToString(), int.Parse(myXMLGateTable["Level"].ToString()),
                    int.Parse(myXMLGateTable["Gold"].ToString()), int.Parse(myXMLGateTable["Color"].ToString()), int.Parse(myXMLGateTable["ItemID1"].ToString()), int.Parse(myXMLGateTable["ItemNum1"].ToString()), int.Parse(myXMLGateTable["ItemID2"].ToString())
                    , int.Parse(myXMLGateTable["ItemNum2"].ToString()), int.Parse(myXMLGateTable["ItemID3"].ToString()), int.Parse(myXMLGateTable["ItemNum3"].ToString()),
                    int.Parse(myXMLGateTable["ItemID4"].ToString()), int.Parse(myXMLGateTable["ItemNum4"].ToString())));
            }
            //////////////RoleClassUp(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "EquipClassUp"));
        }
        catch (Exception ex)
        {
            Debug.Log("FightCamera:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "RoleClassUp");
        }
    }

    public void ParseXMLEquipClassUpScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\EquipClassUp.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////EquipClassUp(以下)//////////////
            TextTranslator.instance.EquipClassUpList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("EquipClassUps").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                //Debug.Log(myXMLGateTable["EquipID"].ToString());
                TextTranslator.instance.AddEquipClassUp(new EquipClassUp(int.Parse(myXMLGateTable["EquipId"].ToString()), myXMLGateTable["Name"].ToString(), int.Parse(myXMLGateTable["NowClass"].ToString()),
                    int.Parse(myXMLGateTable["NeedItem1"].ToString()), int.Parse(myXMLGateTable["ItemNum1"].ToString()), int.Parse(myXMLGateTable["ItemClass1"].ToString()), int.Parse(myXMLGateTable["NeedCoin"].ToString()),
                    int.Parse(myXMLGateTable["Strong"].ToString()), int.Parse(myXMLGateTable["Intell"].ToString()), int.Parse(myXMLGateTable["PhyDef"].ToString()),
                    int.Parse(myXMLGateTable["MagicDef"].ToString()), int.Parse(myXMLGateTable["HP"].ToString()), int.Parse(myXMLGateTable["StrongGrowth"].ToString()), int.Parse(myXMLGateTable["IntellGrowth"].ToString()), int.Parse(myXMLGateTable["PhyDefGrowth"].ToString())
                    , int.Parse(myXMLGateTable["MagicDefGrowth"].ToString()), int.Parse(myXMLGateTable["HPGrowth"].ToString())));
            }
            //////////////EquipClassUp(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "EquipStrong"));
        }
        catch (Exception ex)
        {
            Debug.Log("EquipClassUp:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "EquipClassUp");
        }
    }


    public void ParseXMLEquipStrongScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\EquipStrong.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////EquipClassUp(以下)//////////////
            TextTranslator.instance.EquipStrongList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("EquipStrongs").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                //Debug.Log(myXMLGateTable["EquipID"].ToString());
                TextTranslator.instance.AddEquipStrong(new EquipStrong(int.Parse(myXMLGateTable["EquipID"].ToString()), myXMLGateTable["Name"].ToString(), int.Parse(myXMLGateTable["Race"].ToString()),
                    int.Parse(myXMLGateTable["Part"].ToString()), int.Parse(myXMLGateTable["Color"].ToString()), int.Parse(myXMLGateTable["Icon"].ToString()), int.Parse(myXMLGateTable["LvMax"].ToString()),
                    int.Parse(myXMLGateTable["Hp"].ToString()), int.Parse(myXMLGateTable["Atk"].ToString()), int.Parse(myXMLGateTable["Def"].ToString()),
                    float.Parse(myXMLGateTable["Hit"].ToString()), float.Parse(myXMLGateTable["NoHit"].ToString()), float.Parse(myXMLGateTable["Crit"].ToString()), float.Parse(myXMLGateTable["NoCrit"].ToString()), float.Parse(myXMLGateTable["DmgBonus"].ToString())
                    , float.Parse(myXMLGateTable["DmgReduction"].ToString())));
            }
            //////////////EquipClassUp(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "StrengthenMaster"));
        }
        catch (Exception ex)
        {
            Debug.Log("EquipClassUp:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "EquipStrong");
        }
    }

    public void ParseXMLStrengthenMasterScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////EquipClassUp(以下)//////////////
            TextTranslator.instance.strengthenMasterList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("StrengthenMasters").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                //Debug.Log(myXMLGateTable["EquipID"].ToString());
                TextTranslator.instance.AddStrengthenMaster(myXMLGateTable);
            }
            //////////////EquipClassUp(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "RoleTalent"));
        }
        catch (Exception ex)
        {
            Debug.Log("StrengthenMaster:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "StrengthenMaster");
        }
    }

    public void ParseXMLRoleTalentScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\RoleTalent.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.talentDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("RoleTalents").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddTalent(int.Parse(myXMLGateTable["RoleTalentID"].ToString()), new RoleTalent(
                    int.Parse(myXMLGateTable["RoleTalentID"].ToString()), int.Parse(myXMLGateTable["RoleID"].ToString()), myXMLGateTable["Name"].ToString(), myXMLGateTable["Des"].ToString(), int.Parse(myXMLGateTable["Star"].ToString()),
                    int.Parse(myXMLGateTable["ValType"].ToString()), int.Parse(myXMLGateTable["Area"].ToString()), int.Parse(myXMLGateTable["Angry"].ToString()),
                    float.Parse(myXMLGateTable["Hp"].ToString()), float.Parse(myXMLGateTable["Atk"].ToString()), float.Parse(myXMLGateTable["Def"].ToString()),
                    float.Parse(myXMLGateTable["Hit"].ToString()), float.Parse(myXMLGateTable["NoHit"].ToString()), float.Parse(myXMLGateTable["Crit"].ToString()),
                    float.Parse(myXMLGateTable["NoCrit"].ToString()), float.Parse(myXMLGateTable["DmgBonus"].ToString()), float.Parse(myXMLGateTable["DmgReduction"].ToString())));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "EquipExp"));
        }
        catch (Exception ex)
        {
            Debug.Log("RoleTalents:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "RoleTalent");
        }
    }

    public void ParseXMLEquipExpScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.EquipExpDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("EquipExps").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddEquipExp(int.Parse(myXMLGateTable["Lv"].ToString()), int.Parse(myXMLGateTable["ExpType1"].ToString()));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "EquipStrongQuality"));
        }
        catch (Exception ex)
        {
            Debug.Log("EquipExp:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "EquipExp");
        }
    }

    public void ParseXMLEquipStrongQualityScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.EquipStrongQualityList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("EquipStrongQualitys").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                BetterList<AwardItem> materialItemList = new BetterList<AwardItem>();
                if (myXMLGateTable["ItemID1"].ToString() != "0")
                {
                    materialItemList.Add(new AwardItem(int.Parse(myXMLGateTable["ItemID1"].ToString()), int.Parse(myXMLGateTable["ItemNum1"].ToString())));
                }
                if (myXMLGateTable["ItemID2"].ToString() != "0")
                {
                    materialItemList.Add(new AwardItem(int.Parse(myXMLGateTable["ItemID2"].ToString()), int.Parse(myXMLGateTable["ItemNum2"].ToString())));
                }
                if (myXMLGateTable["ItemID3"].ToString() != "0")
                {
                    materialItemList.Add(new AwardItem(int.Parse(myXMLGateTable["ItemID3"].ToString()), int.Parse(myXMLGateTable["ItemNum3"].ToString())));
                }
                if (myXMLGateTable["ItemID4"].ToString() != "0")
                {
                    materialItemList.Add(new AwardItem(int.Parse(myXMLGateTable["ItemID4"].ToString()), int.Parse(myXMLGateTable["ItemNum4"].ToString())));
                }
                TextTranslator.instance.AddEquipStrongQuality(new EquipStrongQuality(int.Parse(myXMLGateTable["EquipID"].ToString()), int.Parse(myXMLGateTable["Race"].ToString()),
                    int.Parse(myXMLGateTable["Part"].ToString()), int.Parse(myXMLGateTable["Color"].ToString()), int.Parse(myXMLGateTable["Cash"].ToString()), int.Parse(myXMLGateTable["NeedLevel"].ToString()), materialItemList));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "RoleWash"));
        }
        catch (Exception ex)
        {
            Debug.Log("EquipStrongQuality:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "EquipStrongQuality");
        }
    }

    public void ParseXMLRoleWashScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\RoleWash.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.RoleWashList.Clear();
            int i = 0;
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("RoleWashs").Children)
            {
                i++;
                //Debug.Log(i);
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddRoleWash(new RoleWash(int.Parse(myXMLGateTable["RoleID"].ToString()), int.Parse(myXMLGateTable["HpMax"].ToString()),
                    int.Parse(myXMLGateTable["AtkMax"].ToString()), int.Parse(myXMLGateTable["DefMax"].ToString()), float.Parse(myXMLGateTable["CritMax"].ToString()),
                    float.Parse(myXMLGateTable["NoCritMax"].ToString())));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "RoleFate"));
        }
        catch (Exception ex)
        {
            Debug.Log("RoleWash:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "RoleWash");
        }
    }
    public void ParseXMLRoleFateScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\RoleFate.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.RoleFateList.Clear();
            int i = 0;
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("RoleFates").Children)
            {
                i++;
                Hashtable myXMLGateTable = child.Attributes;
                List<int> FateIDList = new List<int>();
                if (myXMLGateTable["FateID1"].ToString() != "0")
                {
                    FateIDList.Add(int.Parse(myXMLGateTable["FateID1"].ToString()));
                }
                if (myXMLGateTable["FateID2"].ToString() != "0")
                {
                    FateIDList.Add(int.Parse(myXMLGateTable["FateID2"].ToString()));
                }
                if (myXMLGateTable["FateID3"].ToString() != "0")
                {
                    FateIDList.Add(int.Parse(myXMLGateTable["FateID3"].ToString()));
                }
                if (myXMLGateTable["FateID4"].ToString() != "0")
                {
                    FateIDList.Add(int.Parse(myXMLGateTable["FateID4"].ToString()));
                }
                if (myXMLGateTable["FateID5"].ToString() != "0")
                {
                    FateIDList.Add(int.Parse(myXMLGateTable["FateID5"].ToString()));
                }
                if (myXMLGateTable["FateID6"].ToString() != "0")
                {
                    FateIDList.Add(int.Parse(myXMLGateTable["FateID6"].ToString()));
                }
                if (myXMLGateTable["FateID7"].ToString() != "0")
                {
                    FateIDList.Add(int.Parse(myXMLGateTable["FateID7"].ToString()));
                }
                if (myXMLGateTable["FateID8"].ToString() != "0")
                {
                    FateIDList.Add(int.Parse(myXMLGateTable["FateID8"].ToString()));
                }
                TextTranslator.instance.AddRoleFate(new RoleFate(int.Parse(myXMLGateTable["RoleFateID"].ToString()), int.Parse(myXMLGateTable["RoleID"].ToString()), myXMLGateTable["RoleName"].ToString(), myXMLGateTable["Name"].ToString(),
                    int.Parse(myXMLGateTable["FateType"].ToString()), myXMLGateTable["Des"].ToString(), float.Parse(myXMLGateTable["Hp"].ToString()), float.Parse(myXMLGateTable["Atk"].ToString()),
                      FateIDList));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "RoleBreach"));
        }
        catch (Exception ex)
        {
            Debug.Log("RoleFate:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "RoleFate");
        }
    }
    public void ParseXMLRoleBreachScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\RoleBreach.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.RoleBreachList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("RoleBreachs").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddRoleBreach(new RoleBreach(int.Parse(myXMLGateTable["RoleID"].ToString()), int.Parse(myXMLGateTable["Star"].ToString()),
                    int.Parse(myXMLGateTable["NeedLevel"].ToString()), int.Parse(myXMLGateTable["StoneNum"].ToString()), int.Parse(myXMLGateTable["DebrisNum"].ToString()),
                    int.Parse(myXMLGateTable["Gold"].ToString())));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "WorldEvent"));
        }
        catch (Exception ex)
        {
            Debug.Log("RoleBreach:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "RoleBreach");
        }
    }

    public void ParseXMLWorldEventScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.WorldEventDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("WorldEvents").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddWorldEvent(int.Parse(myXMLGateTable["WorldEventID"].ToString()),
                    new WorldEvent(int.Parse(myXMLGateTable["WorldEventID"].ToString()), myXMLGateTable["Name"].ToString(), int.Parse(myXMLGateTable["ToGate"].ToString()), int.Parse(myXMLGateTable["GateID"].ToString()), myXMLGateTable["ResultDes"].ToString()));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Map"));
        }
        catch (Exception ex)
        {
            Debug.Log("WorldEvent:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "WorldEvent");
        }
    }


    public void ParseXMLMapScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {

#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\Map.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif

            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.MapDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Maps").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddGateMap(int.Parse(myXMLGateTable["MapID"].ToString()), new GateMap(int.Parse(myXMLGateTable["MapID"].ToString()), "", int.Parse(myXMLGateTable["QuestID"].ToString()), int.Parse(myXMLGateTable["MonsterID"].ToString()), int.Parse(myXMLGateTable["ZoomRate"].ToString()),
                    int.Parse(myXMLGateTable["MonsterLevel"].ToString()), int.Parse(myXMLGateTable["MonsterRarity"].ToString()),
                    int.Parse(myXMLGateTable["SkillLevel"].ToString()), int.Parse(myXMLGateTable["InitialRage"].ToString()), int.Parse(myXMLGateTable["HpRate"].ToString()), int.Parse(myXMLGateTable["AtkRate"].ToString()), int.Parse(myXMLGateTable["DefRate"].ToString()), 0, 0, 0, 0, 0, 0, int.Parse(myXMLGateTable["PosID"].ToString())));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "RoleDestiny"));
        }
        catch (Exception ex)
        {
            Debug.Log("WorldEvent:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Map");
        }
    }

    public void ParseXMLRoleDestinyScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\RoleDestiny.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.RoleDestinyList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("RoleDestinys").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddRoleDestiny(int.Parse(myXMLGateTable["RoleID"].ToString()), int.Parse(myXMLGateTable["Level"].ToString()), int.Parse(myXMLGateTable["NeedLevel"].ToString()),
                    int.Parse(myXMLGateTable["Exp"].ToString()), float.Parse(myXMLGateTable["Hp"].ToString()), float.Parse(myXMLGateTable["Atk"].ToString()),
                    float.Parse(myXMLGateTable["Def"].ToString()));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "RoleDestinyCost"));
        }
        catch (Exception ex)
        {
            Debug.Log("RoleDestiny:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "RoleDestiny");
        }
    }

    public void ParseXMLRoleDestinyCostScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.RoleDestinyCostDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("RoleDestinyCosts").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                BetterList<int> percentList = new BetterList<int>();
                string[] itemSplit = myXMLGateTable["CountList"].ToString().Split('$');
                percentList.Clear();
                for (int i = 0; i < itemSplit.Length; i++)
                {
                    percentList.Add(int.Parse(itemSplit[i]));
                }
                TextTranslator.instance.AddRoleDestinyCost(int.Parse(myXMLGateTable["RoleDestinyLevel"].ToString()), new RoleDestinyCost(int.Parse(myXMLGateTable["RoleDestinyLevel"].ToString()),
                   int.Parse(myXMLGateTable["StoneMax"].ToString()), int.Parse(myXMLGateTable["StoneCost"].ToString()), percentList));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "EquipStrongCost"));
        }
        catch (Exception ex)
        {
            Debug.Log("RoleDestinyCost:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "RoleDestinyCost");
        }
    }

    public void ParseXMLEquipStrongCostScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.EquipStrongCostList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("EquipStrongCosts").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                BetterList<int> CostList = new BetterList<int>();
                CostList.Add(int.Parse(myXMLGateTable["Gold1"].ToString()));
                CostList.Add(int.Parse(myXMLGateTable["Gold2"].ToString()));
                CostList.Add(int.Parse(myXMLGateTable["Gold3"].ToString()));
                CostList.Add(int.Parse(myXMLGateTable["Gold4"].ToString()));
                CostList.Add(int.Parse(myXMLGateTable["Gold5"].ToString()));
                CostList.Add(int.Parse(myXMLGateTable["Gold6"].ToString()));
                TextTranslator.instance.AddEquipStrongCost(new EquipStrongCost(int.Parse(myXMLGateTable["EquipLevel"].ToString()), int.Parse(myXMLGateTable["RoleRarity"].ToString()), int.Parse(myXMLGateTable["NeedExp"].ToString()), CostList));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "RareTreasureOpen"));
        }
        catch (Exception ex)
        {
            Debug.Log("EquipStrongCost:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "EquipStrongCost");
        }
    }
    public void ParseXMLRareTreasureOpenScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////秘宝开启(以下)//////////////
            TextTranslator.instance.RareTreasureOpenDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("RareTreasureOpens").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddRareTreasureOpen(int.Parse(myXMLGateTable["RareTreasureOpenID"].ToString()), new RareTreasureOpen(int.Parse(myXMLGateTable["RareTreasureOpenID"].ToString()), int.Parse(myXMLGateTable["NeedLevel"].ToString())));
            }
            //////////////秘宝开启(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "RareTreasureAttr"));
        }
        catch (Exception ex)
        {
            Debug.Log("RareTreasureOpen:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "RareTreasureOpen");
        }
    }
    public void ParseXMLRareTreasureAttrScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////秘宝属性(以下)//////////////
            TextTranslator.instance.RareTreasureAttrList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("RareTreasureAttrs").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddRareTreasureAttr(new RareTreasureAttr(int.Parse(myXMLGateTable["RareTreasureID"].ToString()), int.Parse(myXMLGateTable["Color"].ToString()), int.Parse(myXMLGateTable["Hp"].ToString()),
                  int.Parse(myXMLGateTable["Atk"].ToString()), int.Parse(myXMLGateTable["Def"].ToString()), float.Parse(myXMLGateTable["Hit"].ToString()), float.Parse(myXMLGateTable["NoHit"].ToString()),
                  float.Parse(myXMLGateTable["Crit"].ToString()), float.Parse(myXMLGateTable["NoCrit"].ToString()), float.Parse(myXMLGateTable["DmgBonus"].ToString()), float.Parse(myXMLGateTable["DmgReduction"].ToString())));
            }
            //////////////秘宝属性(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "RareTreasureExp"));
        }
        catch (Exception ex)
        {
            Debug.Log("RareTreasureAttr:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "RareTreasureAttr");
        }
    }
    public void ParseXMLRareTreasureExpScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////秘宝经验(以下)//////////////
            TextTranslator.instance.RareTreasureExpList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("RareTreasureExps").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddRareTreasureExp(new RareTreasureExp(int.Parse(myXMLGateTable["RareTreasureExpID"].ToString()), int.Parse(myXMLGateTable["Color"].ToString()), int.Parse(myXMLGateTable["Level"].ToString()),
                  int.Parse(myXMLGateTable["NeedExp"].ToString()), int.Parse(myXMLGateTable["NeedLevel"].ToString())));
            }
            //////////////秘宝经验(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "EverydayActivity"));
        }
        catch (Exception ex)
        {
            Debug.Log("RareTreasureAttr:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "RareTreasureExp");
        }
    }
    //
    public void ParseXMLEverydayActivityScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////Talent(以下)//////////////
            TextTranslator.instance.EverydayActivityDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("GateActivitys").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddEverydayActivityDic(int.Parse(myXMLGateTable["GateActivityID"].ToString()), new EverydayActivity(
                int.Parse(myXMLGateTable["GateActivityID"].ToString()), int.Parse(myXMLGateTable["Type"].ToString()),
                int.Parse(myXMLGateTable["Hard"].ToString()), int.Parse(myXMLGateTable["GateID"].ToString()),
                myXMLGateTable["Des"].ToString(), int.Parse(myXMLGateTable["TimeCD"].ToString())
                , int.Parse(myXMLGateTable["NeedLevel"].ToString()), myXMLGateTable["OpenWeek"].ToString(),
                myXMLGateTable["OpenDate"].ToString(), int.Parse(myXMLGateTable["LmtFightCount"].ToString()),
                int.Parse(myXMLGateTable["PosUI"].ToString()), int.Parse(myXMLGateTable["RewardGroupID"].ToString()),
                int.Parse(myXMLGateTable["ShowBonusID1"].ToString()), int.Parse(myXMLGateTable["ShowBonusNum1"].ToString()),
                int.Parse(myXMLGateTable["ShowBonusID2"].ToString()), int.Parse(myXMLGateTable["ShowBonusNum2"].ToString()),
                int.Parse(myXMLGateTable["ShowBonusID3"].ToString()), int.Parse(myXMLGateTable["ShowBonusNum3"].ToString()),
                int.Parse(myXMLGateTable["ShowBonusID4"].ToString()), int.Parse(myXMLGateTable["ShowBonusNum4"].ToString())));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "EverydayActivityAward"));
        }
        catch (Exception ex)
        {
            Debug.Log("EverydayActivity:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "EverydayActivity");
        }
    }

    public void ParseXMLEveryActivityRewardScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////Talent(以下)//////////////
            TextTranslator.instance.EverydayActivityAwardDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("GateActivityRewards").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddEverydayActivityAwardDic(int.Parse(myXMLGateTable["GateActivityRewardID"].ToString()), new EverydayActivityAward(
                    int.Parse(myXMLGateTable["GateActivityRewardID"].ToString()), int.Parse(myXMLGateTable["GroupID"].ToString()), int.Parse(myXMLGateTable["Depth"].ToString()),
                    int.Parse(myXMLGateTable["ActionTerm"].ToString()), float.Parse(myXMLGateTable["ActionVal"].ToString()), int.Parse(myXMLGateTable["ItemID1"].ToString()),
                    int.Parse(myXMLGateTable["ItemNum1"].ToString()), int.Parse(myXMLGateTable["ItemID2"].ToString()), int.Parse(myXMLGateTable["ItemNum2"].ToString()),
                    int.Parse(myXMLGateTable["ItemID3"].ToString()), int.Parse(myXMLGateTable["ItemNum3"].ToString()), int.Parse(myXMLGateTable["ItemID4"].ToString()),
                    int.Parse(myXMLGateTable["ItemNum4"].ToString())));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Talk"));
        }
        catch (Exception ex)
        {
            Debug.Log("EveryActivityReward:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "EverydayActivityAward");
        }
    }

    public void ParseXMLTalkScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////Talent(以下)//////////////
            TextTranslator.instance.TalkList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Talks").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddTalkDic(int.Parse(myXMLGateTable["TalkID"].ToString()), int.Parse(myXMLGateTable["InTurnID"].ToString()),
                    int.Parse(myXMLGateTable["LeftType"].ToString()), myXMLGateTable["LeftDialog"].ToString(), int.Parse(myXMLGateTable["RightType"].ToString()), myXMLGateTable["RightDialog"].ToString());
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "TechTree"));
        }
        catch (Exception ex)
        {
            Debug.Log("Talk:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Talk");
        }
    }

    public void ParseXMLTechTreeScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.TechTreeList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("TechTrees").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddTechTreeDic(int.Parse(myXMLGateTable["TechTreeID"].ToString()), int.Parse(myXMLGateTable["Depth"].ToString()),
                int.Parse(myXMLGateTable["EffectType"].ToString()), float.Parse(myXMLGateTable["EffectVal1"].ToString()), float.Parse(myXMLGateTable["EffectVal2"].ToString()), int.Parse(myXMLGateTable["Level"].ToString()),
                int.Parse(myXMLGateTable["UnLockPreID"].ToString()), int.Parse(myXMLGateTable["UnLockNeedLevel"].ToString()), int.Parse(myXMLGateTable["UnLockPreDepthPoint"].ToString()), int.Parse(myXMLGateTable["LevelUpNeedPoint"].ToString()),
                myXMLGateTable["Name"].ToString(), int.Parse(myXMLGateTable["Icon"].ToString()), myXMLGateTable["Des"].ToString(), int.Parse(myXMLGateTable["GoldCost"].ToString()), myXMLGateTable["NoOpenDescription"].ToString());
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Tower"));
        }
        catch (Exception ex)
        {
            Debug.Log("TechTree:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "TechTree");
        }
    }
    public void ParseXMLTowerScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.TowerDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Towers").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddTowerDic(int.Parse(myXMLGateTable["FloorID"].ToString()), new TowerData(int.Parse(myXMLGateTable["FloorID"].ToString()), int.Parse(myXMLGateTable["Type"].ToString()),
                    int.Parse(myXMLGateTable["GatePoint1"].ToString()), int.Parse(myXMLGateTable["GatePoint2"].ToString()), int.Parse(myXMLGateTable["GatePoint3"].ToString()),
                    float.Parse(myXMLGateTable["GateStarRate1"].ToString()), float.Parse(myXMLGateTable["GateStarRate2"].ToString()), float.Parse(myXMLGateTable["GateStarRate3"].ToString()),
                    int.Parse(myXMLGateTable["GateStarCost1"].ToString()), int.Parse(myXMLGateTable["GateStarCost2"].ToString()), int.Parse(myXMLGateTable["GateStarCost3"].ToString())));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "TowerBoxCost"));
        }
        catch (Exception ex)
        {
            Debug.Log("TowerS:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Tower");
        }
    }
    public void ParseXMLTowerBoxCostScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.TowerBoxCostDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("TowerBoxCosts").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddTowerBoxCostDic(int.Parse(myXMLGateTable["TowerBoxID"].ToString()), new TowerBoxCostData(int.Parse(myXMLGateTable["TowerBoxID"].ToString()), int.Parse(myXMLGateTable["Diamond"].ToString())
                   ));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Market"));
        }
        catch (Exception ex)
        {
            Debug.Log("TowerBoxCost:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "TowerBoxCost");
        }
    }
    public void ParseXMLMarketScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////Talent(以下)//////////////
            TextTranslator.instance.MarketDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Markets").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                //<Market MarketID="1" BuyCount="1" StaminaNum="120" StaminaPrice="50" EnergyNum="60" EnergyPrice="50" GoldNum="2000$110" 
                //GoldBooRateX10="0.10" GoldBooRateX6="0.15" GoldBooRateX4="0.20" GoldBooRateX2="0.25" GoldPrice="50" />
                TextTranslator.instance.AddMarketDic(int.Parse(myXMLGateTable["MarketID"].ToString()), new Market(int.Parse(myXMLGateTable["MarketID"].ToString()),
                    int.Parse(myXMLGateTable["BuyCount"].ToString()), int.Parse(myXMLGateTable["StaminaNum"].ToString()),
                    int.Parse(myXMLGateTable["StaminaPrice"].ToString()), int.Parse(myXMLGateTable["EnergyNum"].ToString()),
                    int.Parse(myXMLGateTable["EnergyPrice"].ToString()), int.Parse(myXMLGateTable["GoldPrice"].ToString()),
                    int.Parse(myXMLGateTable["ResetMstGateDiamondCost"].ToString()),
                    int.Parse(myXMLGateTable["PvpBuyCountDiamondCost"].ToString()),
                    int.Parse(myXMLGateTable["PvpRefreshDiamondCost"].ToString()),
                    int.Parse(myXMLGateTable["ShopRefreshDiamondCost"].ToString()),
                    int.Parse(myXMLGateTable["WorldEventRefreshCost"].ToString()),
                    int.Parse(myXMLGateTable["SmuggleRefreshDaimondCost"].ToString()),
                    int.Parse(myXMLGateTable["WorldBossDiamondCost"].ToString()),
                    int.Parse(myXMLGateTable["WorldBossGoldCost"].ToString()),
                    int.Parse(myXMLGateTable["WorldBossClearCDDiamondCost"].ToString()),
                    int.Parse(myXMLGateTable["LegionGateBuyPrice"].ToString()),
                    int.Parse(myXMLGateTable["KingRoadFightDiamond"].ToString()),
                    int.Parse(myXMLGateTable["LegionTaskRefreshDiamond"].ToString()),
                    int.Parse(myXMLGateTable["CrapsChangePrice"].ToString())));
                //////////////Talent(以上)//////////////                
            }
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "PvpReward"));
        }
        catch (Exception ex)
        {
            Debug.Log("Market:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Market");
        }
    }

    public void ParseXMLPVPRewardScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////Talent(以下)//////////////
            TextTranslator.instance.PVPRewardDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("PvpRewards").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddPVPRewardDic(int.Parse(myXMLGateTable["PvpRewardID"].ToString()),
                    new PVPReward(int.Parse(myXMLGateTable["PvpRewardID"].ToString()), int.Parse(myXMLGateTable["RankID"].ToString()),
                        int.Parse(myXMLGateTable["DiamondBonus"].ToString()), int.Parse(myXMLGateTable["GoldBonus"].ToString()),
                        int.Parse(myXMLGateTable["RYCoinBonus"].ToString()), int.Parse(myXMLGateTable["ItemID"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum"].ToString())));
                //////////////Talent(以上)//////////////
            }
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "PvpPointShop"));
        }
        catch (Exception ex)
        {
            Debug.Log("PVPReward:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "PvpReward");
        }
    }

    public void ParseXMLPvpPointShopScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////Talent(以下)//////////////
            TextTranslator.instance.PvpPointShopDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("PvpPointShops").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddPvpPointShopDic(int.Parse(myXMLGateTable["PvpPointShopID"].ToString()),
                                          new PvpPointShop(int.Parse(myXMLGateTable["PvpPointShopID"].ToString()),
                                                           int.Parse(myXMLGateTable["Point"].ToString()),
                                                           int.Parse(myXMLGateTable["RYCoin"].ToString()),
                                                           int.Parse(myXMLGateTable["Gold"].ToString()),
                                                           int.Parse(myXMLGateTable["ItemID"].ToString()),
                                                           int.Parse(myXMLGateTable["ItemNum"].ToString())));
                //////////////Talent(以上)//////////////
            }
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "PvpOnceReward"));
        }
        catch (Exception ex)
        {
            Debug.Log("PvpPointShop:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "PvpPointShop");
        }
    }
    public void ParseXMLPvpOnceRewardScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////Talent(以下)//////////////
            TextTranslator.instance.PvpOnceRewardDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("PvpOnceRewards").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddPvpOnceRewardDic(int.Parse(myXMLGateTable["PvpOnceRewardID"].ToString()),
                    new PvpOnceReward(int.Parse(myXMLGateTable["PvpOnceRewardID"].ToString()), int.Parse(myXMLGateTable["PvpRankID"].ToString()),
                        int.Parse(myXMLGateTable["Diamond"].ToString()), int.Parse(myXMLGateTable["Gold"].ToString()),
                        int.Parse(myXMLGateTable["ItemID1"].ToString()), int.Parse(myXMLGateTable["ItemNum1"].ToString()),
                        int.Parse(myXMLGateTable["ItemID2"].ToString()), int.Parse(myXMLGateTable["ItemNum2"].ToString())));
                //////////////Talent(以上)//////////////
            }
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "SmuggleCar"));
        }
        catch (Exception ex)
        {
            Debug.Log("PVPReward:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "PvpOnceReward");
        }

    }

    public void ParseXMLSmuggleCarScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////Talent(以下)//////////////
            TextTranslator.instance.SmuggleCarDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("SmuggleCars").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddSmuggleCarDic(int.Parse(myXMLGateTable["SmuggleCarID"].ToString()),
                    new SmuggleCar(int.Parse(myXMLGateTable["SmuggleCarID"].ToString()), int.Parse(myXMLGateTable["Speed"].ToString()),
                        float.Parse(myXMLGateTable["RobbedBonusRatio"].ToString()), float.Parse(myXMLGateTable["Rand"].ToString()),
                        int.Parse(myXMLGateTable["Bonus1"].ToString()), int.Parse(myXMLGateTable["BonusNum1"].ToString()),
                        int.Parse(myXMLGateTable["Bonus2"].ToString()), int.Parse(myXMLGateTable["BonusNum2"].ToString()),
                         int.Parse(myXMLGateTable["Bonus3"].ToString()), int.Parse(myXMLGateTable["Bonus3"].ToString())));
                //////////////Talent(以上)//////////////
            }
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "SuperCar"));
        }
        catch (Exception ex)
        {
            Debug.Log("SmuggleCar:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "SmuggleCar");
        }

    }
    public void ParseXMLSuperCarScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////Talent(以下)//////////////
            TextTranslator.instance.SuperCarDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("SuperCars").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddSuperCarDic(int.Parse(myXMLGateTable["SuperCarID"].ToString()),
                    new SuperCar(int.Parse(myXMLGateTable["SuperCarID"].ToString()), int.Parse(myXMLGateTable["Color"].ToString()),
                        int.Parse(myXMLGateTable["NeedLevel"].ToString()), int.Parse(myXMLGateTable["NeedDebris"].ToString()),
                        int.Parse(myXMLGateTable["Speed"].ToString()), int.Parse(myXMLGateTable["Hp"].ToString()),
                        int.Parse(myXMLGateTable["Atk"].ToString()), int.Parse(myXMLGateTable["Def"].ToString()), myXMLGateTable["Name"].ToString()));
                //////////////Talent(以上)//////////////
            }
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "WeaponUpStar"));
        }
        catch (Exception ex)
        {
            Debug.Log("SuperCar:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "SuperCar");
        }

    }

    public void ParseXMLWeaponUpStarScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////Talent(以下)//////////////
            TextTranslator.instance.WeaponUpStarDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("WeaponUpStars").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddWeaponUpStarDic(int.Parse(myXMLGateTable["ID"].ToString()), int.Parse(myXMLGateTable["WeaponID"].ToString()), int.Parse(myXMLGateTable["Color"].ToString()),
                        int.Parse(myXMLGateTable["Star"].ToString()), float.Parse(myXMLGateTable["Hp"].ToString()),
                        float.Parse(myXMLGateTable["Atk"].ToString()), float.Parse(myXMLGateTable["Def"].ToString()), float.Parse(myXMLGateTable["UpStarRand"].ToString()),
                        int.Parse(myXMLGateTable["NeedItemID1"].ToString()), int.Parse(myXMLGateTable["NeedItemNum1"].ToString()));
                //////////////Talent(以上)//////////////
            }
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "WeaponUpClass"));
        }
        catch (Exception ex)
        {
            Debug.Log("WeaponUpStar:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "WeaponUpStar");
        }

    }
    public void ParseXMLWeaponUpClassScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////Talent(以下)//////////////
            TextTranslator.instance.WeaponUpClassDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("WeaponUpClasss").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddWeaponUpClassDic(int.Parse(myXMLGateTable["ID"].ToString()), int.Parse(myXMLGateTable["WeaponID"].ToString()),
                        int.Parse(myXMLGateTable["UpClassType"].ToString()), int.Parse(myXMLGateTable["Hp"].ToString()),
                        int.Parse(myXMLGateTable["Atk"].ToString()), int.Parse(myXMLGateTable["Def"].ToString()), int.Parse(myXMLGateTable["NeedGold"].ToString()),
                        int.Parse(myXMLGateTable["StoneNeedNum"].ToString()), int.Parse(myXMLGateTable["RoleDebrisNum"].ToString()));
                //////////////Talent(以上)//////////////
            }
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "WeaponMaterial"));
        }
        catch (Exception ex)
        {
            Debug.Log("WeaponUpClass:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "WeaponUpClass");
        }

    }
    public void ParseXMLWeaponMaterialScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////Talent(以下)//////////////
            TextTranslator.instance.WeaponMaterialDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("WeaponMaterials").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddWeaponMaterialDic(int.Parse(myXMLGateTable["WeaponID"].ToString()),
                    new WeaponMaterial(int.Parse(myXMLGateTable["WeaponID"].ToString()), int.Parse(myXMLGateTable["NeedDebris"].ToString())));
                //////////////Talent(以上)//////////////
            }
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "WeaponWheel"));
        }
        catch (Exception ex)
        {
            Debug.Log("WeaponMaterial:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "WeaponMaterial");
        }

    }
    public void ParseXMLWeaponWheelScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////Talent(以下)//////////////
            TextTranslator.instance.WeaponWheelDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("WeaponWheels").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddWeaponWheelDic(int.Parse(myXMLGateTable["ID"].ToString()),
                    new WeaponWheel(int.Parse(myXMLGateTable["ID"].ToString()), int.Parse(myXMLGateTable["ItemID"].ToString()), int.Parse(myXMLGateTable["ItemNum"].ToString()), int.Parse(myXMLGateTable["Rand"].ToString())));
                //////////////Talent(以上)//////////////
            }
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "FightTalk"));

            //if (!NetworkHandler.instance.IsCreate)
            //{
            //    int rangNum = PlayerPrefs.GetInt("MainScenceNum");
            //    if (rangNum == 0 || rangNum == 1 || rangNum == 6)//沙漠
            //    {
            //        GameObject go = Resources.Load("Prefab/Scene/zhujiemian_desert") as GameObject;
            //    }
            //    else if (rangNum == 2 || rangNum == 7)//海港
            //    {
            //        GameObject go = Resources.Load("Prefab/Scene/zhujiemian_harbor") as GameObject;
            //    }
            //    else if (rangNum == 3 || rangNum == 8) //工厂
            //    {
            //        GameObject go = Resources.Load("Prefab/Scene/zhujiemian") as GameObject;
            //    }
            //    else if (rangNum == 4 || rangNum == 9)//丛林
            //    {
            //        GameObject go = Resources.Load("Prefab/Scene/zhujiemian_forest") as GameObject;
            //    }
            //    else if (rangNum == 5 || rangNum == 10)//城市
            //    {
            //        GameObject go = Resources.Load("Prefab/Scene/zhujiemian_city") as GameObject;
            //    }
            //    else
            //    {
            //        GameObject go = Resources.Load("Prefab/Scene/zhujiemian_city") as GameObject;
            //    }
            //}
        }
        catch (Exception ex)
        {
            Debug.Log("WeaponWheel:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "WeaponWheel");
        }

    }
    public void ParseXMLFightTalkScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\FightTalk.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();
            //////////////Talent(以下)//////////////
            TextTranslator.instance.ListFightTalk.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("FightTalks").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddFightTalk(new FightTalk(int.Parse(myXMLGateTable["FightTalkID"].ToString()), int.Parse(myXMLGateTable["GateID"].ToString()),
                        int.Parse(myXMLGateTable["RoleID"].ToString()), int.Parse(myXMLGateTable["Type"].ToString()),
                        int.Parse(myXMLGateTable["Kind"].ToString()), int.Parse(myXMLGateTable["Round"].ToString()), myXMLGateTable["Talk"].ToString()));
                //////////////Talent(以上)//////////////
            }
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "TowerRankReward"));
        }
        catch (Exception ex)
        {
            Debug.Log("SmuggleCar:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "FightTalk");
        }

    }

    public void ParseXMLTowerRankRewardScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.TowerRankRewardDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("TowerRankRewards").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddTowerRankRewardDic(int.Parse(myXMLGateTable["TowerRankRewardID"].ToString()), new TowerRankReward(int.Parse(myXMLGateTable["TowerRankRewardID"].ToString()),
                    int.Parse(myXMLGateTable["RankID"].ToString()), int.Parse(myXMLGateTable["Gold"].ToString()), int.Parse(myXMLGateTable["SLCoin"].ToString())));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "TowerShop"));

        }
        catch (Exception ex)
        {
            Debug.Log("TowerRankReward:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "TowerRankReward");
        }
    }
    public void ParseXMLTowerShopScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.TowerShopDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("TowerShops").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddTowerShopDic(int.Parse(myXMLGateTable["TowerShopID"].ToString()), new TowerShop(int.Parse(myXMLGateTable["TowerShopID"].ToString()), int.Parse(myXMLGateTable["Point"].ToString()),
                    int.Parse(myXMLGateTable["ColorUI"].ToString()), int.Parse(myXMLGateTable["ItemID1"].ToString()), int.Parse(myXMLGateTable["ItemNum1"].ToString()),
                    int.Parse(myXMLGateTable["ItemID2"].ToString()), int.Parse(myXMLGateTable["ItemNum2"].ToString()),
                    int.Parse(myXMLGateTable["ItemID3"].ToString()), int.Parse(myXMLGateTable["ItemNum3"].ToString()),
                    int.Parse(myXMLGateTable["ItemID4"].ToString()), int.Parse(myXMLGateTable["ItemNum4"].ToString())
                   ));
            }
            //////////////Talent(以上)//////////////;
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Barrage"));


        }
        catch (Exception ex)
        {
            Debug.Log("ActionEvent:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "TowerShop");
        }
    }

    public void ParseXMLBarrageEventScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
#if TestMode
            FileStream FS = new FileStream("D:\\MyWar\\Barrage.xml", FileMode.Open, FileAccess.Read);
            StreamReader SR = new StreamReader(FS);
            SP.LoadXml(SR.ReadToEnd());
#else
            SP.LoadXml(ParseXMLText);
#endif
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.BarrageList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Barrages").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddBarrageDic(int.Parse(myXMLGateTable["ID"].ToString()), int.Parse(myXMLGateTable["GateID"].ToString()),
                  (myXMLGateTable["Des"].ToString()), int.Parse(myXMLGateTable["OutTime"].ToString()), int.Parse(myXMLGateTable["Position"].ToString()), int.Parse(myXMLGateTable["Color"].ToString()));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Fortress"));


        }
        catch (Exception ex)
        {
            Debug.Log("Barrage:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Barrages");
        }
    }
    public void ParseXMLFortressEventScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.FortressList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Fortresss").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddFortressDic(int.Parse(myXMLGateTable["ID"].ToString()), int.Parse(myXMLGateTable["Type"].ToString()), int.Parse(myXMLGateTable["Level"].ToString()), int.Parse(myXMLGateTable["BuildLevel"].ToString()),
                  int.Parse(myXMLGateTable["NeedItemId"].ToString()), int.Parse(myXMLGateTable["NeedItemNum"].ToString()), int.Parse(myXMLGateTable["BonusItemId"].ToString()), float.Parse(myXMLGateTable["BonusItemNum"].ToString()),
                  float.Parse(myXMLGateTable["FriendBase"].ToString()), float.Parse(myXMLGateTable["FriendGrow"].ToString()));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "CombinSkill"));
        }
        catch (Exception ex)
        {
            Debug.Log("Fortress:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Fortress");
        }
    }
    public void ParseXMLCombinSkillScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.CombinSkillDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("CombinSkills").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddCombinSkillDic(int.Parse(myXMLGateTable["HeroId1"].ToString()),
                    new CombinSkill(int.Parse(myXMLGateTable["CombinSkillID"].ToString()),
                        (myXMLGateTable["SkillName"].ToString()),
                        (myXMLGateTable["SkillDes"].ToString()),
                        int.Parse(myXMLGateTable["HeroId1"].ToString()),
                        int.Parse(myXMLGateTable["HeroId2"].ToString()),
                        int.Parse(myXMLGateTable["HeroId3"].ToString()),
                        int.Parse(myXMLGateTable["HeroId4"].ToString()),
                        int.Parse(myXMLGateTable["HeroId5"].ToString()),
                        int.Parse(myXMLGateTable["HeroId6"].ToString()),
                        int.Parse(myXMLGateTable["BuffId1"].ToString()),
                        float.Parse(myXMLGateTable["BuffRand1"].ToString()),
                        float.Parse(myXMLGateTable["Param1"].ToString()),
                        int.Parse(myXMLGateTable["BuffId2"].ToString()),
                        float.Parse(myXMLGateTable["BuffRand2"].ToString()),
                          float.Parse(myXMLGateTable["Param2"].ToString()),
                        int.Parse(myXMLGateTable["BuffId3"].ToString()),
                        float.Parse(myXMLGateTable["BuffRand3"].ToString()),
                          float.Parse(myXMLGateTable["Param3"].ToString()),
                        int.Parse(myXMLGateTable["BuffId4"].ToString()),
                        float.Parse(myXMLGateTable["BuffRand4"].ToString()),
                          float.Parse(myXMLGateTable["Param4"].ToString())
                        ));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ActivitiesCenter"));

            //if (NetworkHandler.instance.IsCreate)
            //{
            //    GameObject go1 = Resources.Load("Prefab/Effect/jinx_skill") as GameObject;
            //    GameObject go2 = Resources.Load("Prefab/Effect/jinx_skill_02") as GameObject;
            //    GameObject go3 = Resources.Load("Prefab/Effect/jinx_skill_02_bf") as GameObject;

            //    GameObject go4 = Resources.Load("Prefab/Effect/Trench_skill") as GameObject;
            //    GameObject go5 = Resources.Load("Prefab/Effect/Trench_skill_pre") as GameObject;
            //}
        }
        catch (Exception ex)
        {
            Debug.Log("CombinSkill:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "CombinSkill");
        }
    }
    /// <summary>
    /// 每日副本活动
    /// </summary>
    public void ParseXMLActivitiesCenterScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.ActivitiesCenterDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ActivitiesOpenCenters").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddActivitiesCenterDic(
                    new ActivitiesCenter(int.Parse(myXMLGateTable["ActivitiesOpenCenterID"].ToString()),
                        int.Parse(myXMLGateTable["ActivityID"].ToString()),
                        (myXMLGateTable["Name"].ToString()),
                        int.Parse(myXMLGateTable["Type"].ToString()),
                        int.Parse(myXMLGateTable["TimeType"].ToString()),
                        int.Parse(myXMLGateTable["DayCount"].ToString()),
                        int.Parse(myXMLGateTable["TimeCycle"].ToString()),
                        (myXMLGateTable["StartDate"].ToString()),
                        (myXMLGateTable["EndDate"].ToString()),
                        (myXMLGateTable["StartTime"].ToString()),
                        (myXMLGateTable["EndTime"].ToString()),
                        (myXMLGateTable["SpacialTime"].ToString()),
                        (myXMLGateTable["RewardEndTime"].ToString()),
                        int.Parse(myXMLGateTable["IsPicture"].ToString()),
                        int.Parse(myXMLGateTable["ShowTimeType"].ToString()),
                        (myXMLGateTable["Title"].ToString()),
                        (myXMLGateTable["Des"].ToString()),
                        int.Parse(myXMLGateTable["NewServiceActivites"].ToString())
                        ));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ActivityDayExchange"));
        }
        catch (Exception ex)
        {
            Debug.Log("ActivitiesOpenCenter:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ActivitiesCenter");
        }
    }
    public void ParseXMLActivityDayExchangeScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.ActivityDayExchangeDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ActivityDayExchanges").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddActivityDayExchangeDic(int.Parse(myXMLGateTable["DayExchangeID"].ToString()),
                    new ActivityDayExchange(int.Parse(myXMLGateTable["DayExchangeID"].ToString()),
                        int.Parse(myXMLGateTable["ActivityID"].ToString()),
                        (myXMLGateTable["Des"].ToString()),
                        int.Parse(myXMLGateTable["RewardType"].ToString()),
                        int.Parse(myXMLGateTable["Turnover"].ToString()),
                        int.Parse(myXMLGateTable["Point"].ToString()),
                        int.Parse(myXMLGateTable["ItemID1"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum1"].ToString()),
                        int.Parse(myXMLGateTable["ItemID2"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum2"].ToString()),
                        int.Parse(myXMLGateTable["ItemID3"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum3"].ToString()),
                        int.Parse(myXMLGateTable["ItemID4"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum4"].ToString())
                        )
                        );

            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ActivityExchange"));
        }
        catch (Exception ex)
        {
            Debug.Log("ActivityDayExchange:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ActivityDayExchange");
        }
    }
    public void ParseXMLActivityExchangeScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.ActivityExchangeDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ActivityExchanges").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddActivityExchangeDic(int.Parse(myXMLGateTable["ExchangeID"].ToString()),
                        int.Parse(myXMLGateTable["ActivityID"].ToString()),
                        int.Parse(myXMLGateTable["GroupID"].ToString()),
                        (myXMLGateTable["Des"].ToString()),
                        int.Parse(myXMLGateTable["Type"].ToString()),
                        int.Parse(myXMLGateTable["Condition"].ToString()),
                        int.Parse(myXMLGateTable["Point"].ToString()),
                        int.Parse(myXMLGateTable["ItemID1"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum1"].ToString()),
                        int.Parse(myXMLGateTable["ItemID2"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum2"].ToString()),
                        int.Parse(myXMLGateTable["ItemID3"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum3"].ToString()),
                        int.Parse(myXMLGateTable["ItemID4"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum4"].ToString())
                        );

            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ActivityGrowthFund"));
        }
        catch (Exception ex)
        {
            Debug.Log("ActivityExchange:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ActivityExchange");
        }
    }
    public void ParseXMLActivityGrowthFundScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.ActivityGrowthFundDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ActivityGrowthFunds").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddActivityGrowthFundDic(int.Parse(myXMLGateTable["GrowthFundID"].ToString()),
                    new ActivityGrowthFund(
                        int.Parse(myXMLGateTable["GrowthFundID"].ToString()),
                        int.Parse(myXMLGateTable["Type"].ToString()),
                        int.Parse(myXMLGateTable["Condition"].ToString()),
                        (myXMLGateTable["Des"].ToString()),
                        int.Parse(myXMLGateTable["ItemID"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum"].ToString())
                        )
                        );

            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ActivityBonusVip"));
        }
        catch (Exception ex)
        {
            Debug.Log("ActivityGrowthFund:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ActivityGrowthFund");
        }
    }
    public void ParseXMLActivityBonusVipScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.ActivityBonusVipDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ActivityBonusVips").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddActivityBonusVipDic(int.Parse(myXMLGateTable["BonusVipID"].ToString()),
                    new ActivityBonusVip(
                        int.Parse(myXMLGateTable["BonusVipID"].ToString()),
                        int.Parse(myXMLGateTable["ActivityID"].ToString()),
                        int.Parse(myXMLGateTable["ConvertType"].ToString()),
                        int.Parse(myXMLGateTable["Param1"].ToString()),
                        int.Parse(myXMLGateTable["Param2"].ToString()),
                        (myXMLGateTable["Des"].ToString()),
                        int.Parse(myXMLGateTable["ConvertCount"].ToString()),
                        int.Parse(myXMLGateTable["ItemID1"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum1"].ToString()),
                        int.Parse(myXMLGateTable["ItemID2"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum2"].ToString()),
                        int.Parse(myXMLGateTable["ItemID3"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum3"].ToString()),
                        int.Parse(myXMLGateTable["ItemID4"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum4"].ToString())
                        )
                        );

            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ActivityItemFixed"));
        }
        catch (Exception ex)
        {
            Debug.Log("ActivityBonusVip:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ActivityBonusVip");
        }
    }
    public void ParseXMLActivityItemFixedScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.ActivityItemFixedDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ActivityItemFixeds").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddActivityItemFixedDic(
                        int.Parse(myXMLGateTable["ItemFixedID"].ToString()),
                        int.Parse(myXMLGateTable["ActivityID"].ToString()),
                        int.Parse(myXMLGateTable["Round"].ToString()),
                        int.Parse(myXMLGateTable["Type"].ToString()),
                        int.Parse(myXMLGateTable["ConvertType"].ToString()),
                        int.Parse(myXMLGateTable["Param1"].ToString()),
                        int.Parse(myXMLGateTable["Param2"].ToString()),
                        (myXMLGateTable["Des"].ToString()),
                        int.Parse(myXMLGateTable["ConvertCount"].ToString()),
                        int.Parse(myXMLGateTable["ItemID1"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum1"].ToString()),
                        int.Parse(myXMLGateTable["ItemID2"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum2"].ToString()),
                        int.Parse(myXMLGateTable["ItemID3"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum3"].ToString()),
                        int.Parse(myXMLGateTable["ItemID4"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum4"].ToString())
                        );

            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ActivityGachaHeroPoint"));
        }
        catch (Exception ex)
        {
            Debug.Log("ActivityItemFixed:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ActivityItemFixed");
        }
    }
    public void ParseXMLActivityGachaHeroPointScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.ActivityGachaHeroPointDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ActivityGachaHeroPoints").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddActivityGachaHeroPointDic(
                        new ActivityGachaHeroPoint(
                        int.Parse(myXMLGateTable["ID"].ToString()),
                        int.Parse(myXMLGateTable["ActivityID"].ToString()),
                        int.Parse(myXMLGateTable["Sort"].ToString()),
                        int.Parse(myXMLGateTable["Point"].ToString()),
                        int.Parse(myXMLGateTable["BonusID1"].ToString()),
                        int.Parse(myXMLGateTable["BonusNum1"].ToString()),
                        int.Parse(myXMLGateTable["BonusID2"].ToString()),
                        int.Parse(myXMLGateTable["BonusNum2"].ToString()),
                        int.Parse(myXMLGateTable["BonusID3"].ToString()),
                        int.Parse(myXMLGateTable["BonusNum3"].ToString()),
                        int.Parse(myXMLGateTable["BonusID4"].ToString()),
                        int.Parse(myXMLGateTable["BonusNum4"].ToString()),
                        int.Parse(myXMLGateTable["BonusID5"].ToString()),
                        int.Parse(myXMLGateTable["BonusNum5"].ToString()),
                        int.Parse(myXMLGateTable["BonusID6"].ToString()),
                        int.Parse(myXMLGateTable["BonusNum6"].ToString()),
                        int.Parse(myXMLGateTable["BonusID7"].ToString()),
                        int.Parse(myXMLGateTable["BonusNum7"].ToString()),
                        int.Parse(myXMLGateTable["BonusID8"].ToString()),
                        int.Parse(myXMLGateTable["BonusNum8"].ToString()))
                        );

            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ActivityGachaHeroRank"));
        }
        catch (Exception ex)
        {
            Debug.Log("ActivityGachaHeroPoint:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ActivityGachaHeroPoint");
        }
    }
    public void ParseXMLActivityGachaHeroRankScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.ActivityGachaHeroRankDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ActivityGachaHeroRanks").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddActivityGachaHeroRankDic(
                        new ActivityGachaHeroRank(
                        int.Parse(myXMLGateTable["ID"].ToString()),
                        int.Parse(myXMLGateTable["ActivityID"].ToString()),
                        int.Parse(myXMLGateTable["Rank"].ToString()),
                        int.Parse(myXMLGateTable["BonusID1"].ToString()),
                        int.Parse(myXMLGateTable["BonusNum1"].ToString()),
                        int.Parse(myXMLGateTable["BonusID2"].ToString()),
                        int.Parse(myXMLGateTable["BonusNum2"].ToString()),
                        int.Parse(myXMLGateTable["BonusID3"].ToString()),
                        int.Parse(myXMLGateTable["BonusNum3"].ToString()),
                        int.Parse(myXMLGateTable["BonusID4"].ToString()),
                        int.Parse(myXMLGateTable["BonusNum4"].ToString()),
                        int.Parse(myXMLGateTable["BonusID5"].ToString()),
                        int.Parse(myXMLGateTable["BonusNum5"].ToString()),
                        int.Parse(myXMLGateTable["BonusID6"].ToString()),
                        int.Parse(myXMLGateTable["BonusNum6"].ToString()),
                        int.Parse(myXMLGateTable["BonusID7"].ToString()),
                        int.Parse(myXMLGateTable["BonusNum7"].ToString()),
                        int.Parse(myXMLGateTable["BonusID8"].ToString()),
                        int.Parse(myXMLGateTable["BonusNum8"].ToString()))
                        );

            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "IndianaPoint"));
        }
        catch (Exception ex)
        {
            Debug.Log("ActivityGachaHeroRank:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ActivityGachaHeroRank");
        }
    }
    public void ParseXMLIndianaPointScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.IndianaPointDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("IndianaPoints").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddIndianaPointDic(int.Parse(myXMLGateTable["ID"].ToString()),
                    new IndianaPoint(int.Parse(myXMLGateTable["ID"].ToString()),
                        int.Parse(myXMLGateTable["Point"].ToString()),
                        int.Parse(myXMLGateTable["ItemID1"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum1"].ToString()),
                        int.Parse(myXMLGateTable["ItemID2"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum2"].ToString()),
                        int.Parse(myXMLGateTable["ItemID3"].ToString()),
                        int.Parse(myXMLGateTable["ItemNum3"].ToString())
                         )
                        );

            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Chip"));
        }
        catch (Exception ex)
        {
            Debug.Log("IndianaPoints:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "IndianaPoint");
        }
    }

    public void ParseXMLChipScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.chargeItemList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Chips").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddChipList(int.Parse(myXMLGateTable["ChipID"].ToString()),
                        int.Parse(myXMLGateTable["Color"].ToString()),
                        int.Parse(myXMLGateTable["SlotNum"].ToString()),
                        int.Parse(myXMLGateTable["EffectType1"].ToString()),
                        float.Parse(myXMLGateTable["EffectVal1"].ToString()),
                        int.Parse(myXMLGateTable["EffectType2"].ToString()),
                        float.Parse(myXMLGateTable["EffectVal2"].ToString()),
                        int.Parse(myXMLGateTable["EffectType3"].ToString()),
                        float.Parse(myXMLGateTable["EffectVal3"].ToString()),
                        (myXMLGateTable["Des"].ToString())
                        );
                
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "Exchange"));
        }
        catch (Exception ex)
        {
            Debug.Log("IndianaPoints:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Chip");
        }
    }
    public void ParseXMLExchargeScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.chargeItemList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("Exchanges").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                //Debug.Log(myXMLGateTable["EquipID"].ToString());
                TextTranslator.instance.AddExchangeItem(myXMLGateTable);
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "NuclearPowerPlan"));
        }
        catch (Exception ex)
        {
            Debug.Log("IndianaPoints:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "Exchange");
        }
    }


    public void ParseXMLNuclearPowerPlanScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.NuclearPowerPlanList.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("NuclearPowerPlans").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                //Debug.Log(myXMLGateTable["EquipID"].ToString());
                TextTranslator.instance.AddNuclearPowerPlanList(int.Parse(myXMLGateTable["NuclearPowerPlanID"].ToString()),
                    int.Parse(myXMLGateTable["DiamondsValue"].ToString()), int.Parse(myXMLGateTable["AddForce"].ToString()), int.Parse(myXMLGateTable["AddOneHeroAtk"].ToString()), int.Parse(myXMLGateTable["AddOneHeroHp"].ToString()));
            }
            //////////////Talent(以上)//////////////
            StartCoroutine(ResourceLoader.instance.GetGameResource(false, "ActionEvent"));
        }
        catch (Exception ex)
        {
            Debug.Log("IndianaPoints:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "NuclearPowerPlan");
        }
    }

    public void ParseXMLActionEventScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();
        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.ActionEventDic.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ActionEvents").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddActionEventDic(int.Parse(myXMLGateTable["ActionEventID"].ToString()), new ActionEvent(int.Parse(myXMLGateTable["ActionEventID"].ToString()), int.Parse(myXMLGateTable["GroupID"].ToString()),
                    int.Parse(myXMLGateTable["GateID"].ToString()), myXMLGateTable["GateName"].ToString(), int.Parse(myXMLGateTable["NeedLevel"].ToString()),
                    int.Parse(myXMLGateTable["EnableDays"].ToString()), int.Parse(myXMLGateTable["EnergyCost"].ToString()),
                    int.Parse(myXMLGateTable["TalkID"].ToString()), int.Parse(myXMLGateTable["BonusID1"].ToString()),
                    int.Parse(myXMLGateTable["BonusNum1"].ToString()), int.Parse(myXMLGateTable["BonusID2"].ToString()), int.Parse(myXMLGateTable["BonusNum2"].ToString()), int.Parse(myXMLGateTable["BonusID3"].ToString()), int.Parse(myXMLGateTable["BonusNum3"].ToString())
                    , int.Parse(myXMLGateTable["BonusID4"].ToString()), int.Parse(myXMLGateTable["BonusNum4"].ToString()), int.Parse(myXMLGateTable["ForGateID"].ToString())
                   ));
            }
            //////////////Talent(以上)//////////////;
            if (NetworkHandler.instance.IsCreate)
            {
                //Debug.LogError("新手引导剧情画面1");
                //StartCoroutine(PictureCreater.instance.Newbie());
                //UIManager.instance.OpenPanel("GuideLegionWar", true);

                AudioEditer.instance.PlayLoop("Weijia");
                GameObject go = (GameObject)Instantiate(Resources.Load("Prefab/Effect/GuideLegionWar"));
                go.name = "GuideLegionWar";
                Transform t = go.transform;
                t.parent = GameObject.Find("UIRoot").transform;
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                StartCoroutine(StartStory());
            }
            else
            {
                StartCoroutine(CharacterRecorder.instance.SyncHeroListFormServer());
            }
        }
        catch (Exception ex)
        {
            Debug.Log("ActionEvent:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ActionEvent");
        }
    }

    public void ShowWeiJia()
    {   
        StartCoroutine(StartStory());
    }

    public IEnumerator StartStory()
    {
        yield return new WaitForSeconds(1f);
        AudioEditer.instance.PlayLoop("Weijia");
        GameObject go1 = (GameObject)Instantiate(Resources.Load("Prefab/Effect/GuideLegionWar"));
        go1.name = "GuideLegionWar";
        Transform t1 = go1.transform;
        t1.parent = GameObject.Find("UIRoot").transform;
        t1.localPosition = Vector3.zero;
        t1.localRotation = Quaternion.identity;
        t1.localScale = Vector3.one;
        //PictureCreater.instance.CreateRole(60023, "Enemy", 17, Color.cyan, 50000, 50000, 1000, 108.0f, false, false, 1, 1.5f, 0, "", 0, 6933, 0, 2000, 0, 0, 0, 1025, 2028, 0, 1, 0, 1, 2, 1000);
        //yield return new WaitForSeconds(0.1f);
        //PictureCreater.instance.CreateRole(60029, "Enemy", 18, Color.cyan, 50000, 50000, 1000, 109.8f, false, false, 1, 1.5f, 0, "", 0, 6987, 0, 2000, 0, 0, 0, 1036, 2028, 0, 1, 0, 1, 2, 1000);
        //yield return new WaitForSeconds(0.1f);
        //PictureCreater.instance.CreateRole(60027, "Enemy", 14, Color.cyan, 50000, 50000, 1000, 105.8f, false, false, 1, 1.5f, 0, "", 0, 6254, 0, 2000, 0, 0, 0, 1035, 2028, 0, 1, 0, 1, 2, 1000);
        //yield return new WaitForSeconds(0.1f);
        //PictureCreater.instance.CreateRole(60028, "Enemy", 21, Color.cyan, 45000, 45000, 1000, 105.5f, false, false, 1, 1.5f, 0, "", 0, 6741, 0, 2000, 0, 0, 0, 1029, 2028, 0, 1, 0, 1022, 2, 1000);
        //yield return new WaitForSeconds(0.1f);
        //PictureCreater.instance.CreateRole(60026, "Enemy", 15, Color.cyan, 50000, 50000, 1000, 106.3f, false, false, 1, 1.5f, 0, "", 0, 6344, 0, 2000, 0, 0, 0, 1034, 2028, 0, 1, 0, 1, 2, 1000);
        //yield return new WaitForSeconds(0.1f);
        //PictureCreater.instance.CreateRole(60018, "Enemy", 33, Color.red, 50000, 50000, 1000, 107.1f, true, false, 1, 1.5f, 0, "", 0, 6854, 0, 0, 0, 0, 0, 1030, 2028, 0, 1, 0, 2, 2, 1000);
        //yield return new WaitForSeconds(0.1f);
        //PictureCreater.instance.CreateRole(65304, "Enemy", 38, Color.red, 98447, 98447, 1000, 100.0f, true, false, 1, 1.5f, 0, "", 0, 99999, 0, 0, 0, 0, 0, 1004, 2028, 0, 1, 0, 2, 2, 1000);
        //yield return new WaitForSeconds(0.1f);
        //PictureCreater.instance.CreateRole(60012, "Enemy", 32, Color.red, 120000, 120000, 1000, 108.9f, true, false, 1, 1.5f, 0, "", 0, 6454, 0, 0, 0, 0, 0, 1010, 2028, 0, 1, 0, 2, 2, 1000);
        //yield return new WaitForSeconds(0.1f);
        //PictureCreater.instance.CreateRole(60020, "Enemy", 29, Color.red, 60000, 60000, 1000, 106.9f, true, false, 1, 1.5f, 0, "", 0, 6088, 0, 0, 0, 0, 0, 1021, 2028, 0, 1, 0, 2, 2, 1000);
        //yield return new WaitForSeconds(0.1f);
        //PictureCreater.instance.CreateRole(65404, "Enemy", 41, Color.red, 60000, 60000, 1000, 6.9f, true, false, 1, 1.5f, 0, "", 0, 6088, 0, 0, 0, 0, 0, 1021, 2028, 0, 1, 0, 2, 2, 1000);
        //yield return new WaitForSeconds(0.1f);
        //PictureCreater.instance.CreateRole(65400, "Enemy", 34, Color.red, 60000, 60000, 1000, 6.9f, true, false, 1, 1.5f, 0, "", 0, 6088, 0, 0, 0, 0, 0, 1021, 2028, 0, 1, 0, 2, 2, 1000);
        //yield return new WaitForSeconds(0.1f);
        //PictureCreater.instance.CreateRole(65501, "Enemy", 34, Color.red, 60000, 60000, 1000, 6.9f, true, false, 1, 1.5f, 0, "", 0, 6088, 0, 0, 0, 0, 0, 1021, 2028, 0, 1, 0, 2, 2, 1000);
        //yield return new WaitForSeconds(0.1f);
        //PictureCreater.instance.CreateRole(65500, "Enemy", 37, Color.red, 60000, 60000, 1000, 6.9f, true, false, 1, 1.5f, 0, "", 0, 6088, 0, 0, 0, 0, 0, 1021, 2028, 0, 1, 0, 2, 2, 1000);
        //yield return new WaitForSeconds(0.1f);
        //PictureCreater.instance.DestroyAllComponent();
        yield return new WaitForSeconds(1f);
        DestroyImmediate(GameObject.Find("LoadingWindow"));
        DestroyImmediate(GameObject.Find("StartWindow"));
        yield return new WaitForSeconds(5.5f);
        UIManager.instance.OpenPanel("MaskWindow", true);
        yield return new WaitForSeconds(0.5f);
        AudioEditer.instance.PlayOneShot("intro_flash");
        GameObject go = (GameObject)Instantiate(Resources.Load("Prefab/Effect/KaiChang_WeiJia_eff"));
        go.name = "KaiChang_WeiJia_eff";
        Transform t = go.transform;
        t.parent = GameObject.Find("GuideLegionWar").transform;
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
        yield return new WaitForSeconds(5f);
        LuaDeliver.instance.ResetGuide();
        Destroy(GameObject.Find("GuideLegionWar"));
        UIManager.instance.OpenPanel("StartNameWindow", true);
    }

    /// <summary>
    /// 服务器信息载入
    /// </summary>
    /// <param name="ParseXMLText"></param>
    public void ParseXMLServerListsScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.ServerLists.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("ServerLists").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                Debug.LogError(myXMLGateTable["Name"].ToString());
                TextTranslator.instance.AddServerLists(new ServerList(int.Parse(myXMLGateTable["ServerID"].ToString()), myXMLGateTable["ServerTag"].ToString(), myXMLGateTable["GroupID"].ToString(), myXMLGateTable["Name"].ToString(),
                                        myXMLGateTable["OpenDate"].ToString(), myXMLGateTable["CloseDate"].ToString(),
                                        int.Parse(myXMLGateTable["Type"].ToString()), int.Parse(myXMLGateTable["State"].ToString()),
                                        myXMLGateTable["LuaScriptID"].ToString(), myXMLGateTable["ServerIP"].ToString(),
                                        int.Parse(myXMLGateTable["ServerPort"].ToString())));
            }
            //////////////Talent(以上)//////////////     


        }
        catch (Exception ex)
        {
            Debug.Log("XMLServerList:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "ServerList");
        }
    }


    public void ParseXMLDownloadListsScript(string ParseXMLText)
    {
        SecurityParser SP = new SecurityParser();

        try
        {
            SP.LoadXml(ParseXMLText);
            System.Security.SecurityElement SE = SP.ToXml();

            //////////////Talent(以下)//////////////
            TextTranslator.instance.DownloadLists.Clear();
            foreach (System.Security.SecurityElement child in SE.SearchForChildByTag("DownloadLists").Children)
            {
                Hashtable myXMLGateTable = child.Attributes;
                TextTranslator.instance.AddDownloadLists(new DownloadList(myXMLGateTable["Platform"].ToString(), myXMLGateTable["Url"].ToString()));
            }
            //////////////Talent(以上)//////////////     


        }
        catch (Exception ex)
        {
            Debug.Log("XMLServerList:" + ex.ToString());
            ResourceLoader.instance.GetGameResource(true, "DownloadList");
        }
    }
}
