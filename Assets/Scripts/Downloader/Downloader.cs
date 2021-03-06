using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;
using System.Linq;
using CodeStage.AntiCheat.ObscuredTypes;
using Umeng;

public class Downloader : MonoBehaviour
{
    public static Downloader instance;

    public int LatestVersion;
    public int CodeVersion;
    public int DownloaderVersion;

    string AndroidFile = "http://192.168.8.101/HOT/HeartOfTime_Android_xm.apk";
    string iOSFile = "http://192.168.8.101/HOT/HeartOfTime_Android_xm.apk";

    public string DownloadUrl = "";

    public DateTime NowTime;
    public void SetVersion(int SetLatestVersion, int SetCodeVersion, string SetAnnouncement, string timeStamp)
    {
        LatestVersion = SetLatestVersion;
        DownloaderVersion = 1; //每次产apk包要改和server同版号
        CodeVersion = SetCodeVersion;

        ObscuredPrefs.SetInt("CodeVersion", CodeVersion);
        //ObscuredPrefs.SetInt("CodeVersion", 1);

        Debug.Log("Version Checking");
        //////////////////////////////////////////
        GameObject gameVersionChecker = new GameObject("gameVersionChecker");
        gameVersionChecker.AddComponent("VersionChecker");
        gameVersionChecker.transform.parent = transform;
        //////////////////////////////////////////       



        //////////////////////////////////////////
        //gameCharacterRecorder = new GameObject("gameCharacterRecorder");
        //gameCharacterRecorder.AddComponent("CharacterRecorder");
        //gameCharacterRecorder.transform.parent = transform;
        //gameCharacterRecorder.GetComponent<CharacterRecorder>().IsCreate = true;
        //////////////////////////////////////////

        Debug.Log(LatestVersion + " " + DownloaderVersion);
        if (LatestVersion > DownloaderVersion) //下载apk包
        {

        }
        else
        {
            UIManager.instance.OpenPanel("LoadingWindow", true);
            if (PlayerPrefs.GetInt("GuideState") == 0)
            {
                UIManager.instance.NewGuideAnchor(UIManager.AnchorIndex.index_100);
            }
            GameObject.Find("LoadingWindow").GetComponent<LoadingWindow>().IsAuto = false;
            TextTranslator.instance.Announcement = SetAnnouncement;

            //UIManager.instance.OpenPanel("GameAnnouncementWindow", false);
            //GameObject.Find("GameAnnouncementWindow").GetComponent<GameAnnouncementWindow>().SetAnnouncement(SetAnnouncement);
        }
        NowTime = GetTime(timeStamp);
        CancelInvoke();
        InvokeRepeating("UpdateTime", 0, 1.0f);
    }

    public static DateTime GetTime(string timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }

    void UpdateTime()
    {
        NowTime = NowTime.AddSeconds(1);
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        instance = this;
        string Platform = "Android";
#if UNITY_ANDROID
# if XIAOMI
            GA.StartWithAppKeyAndChannelId("5695eedb67e58e75a1000535", "XIAOMI");
#elif HUAWEI
             GA.StartWithAppKeyAndChannelId("5695eedb67e58e75a1000535", "HUAWEI");
#elif QIHOO360
            GA.StartWithAppKeyAndChannelId("5695eedb67e58e75a1000535", "QIHOO360");
#elif OPPO
            GA.StartWithAppKeyAndChannelId("5695eedb67e58e75a1000535", "OPPO");
#elif UC
            GA.StartWithAppKeyAndChannelId("5695eedb67e58e75a1000535", "UC");
#elif VIVO
            GA.StartWithAppKeyAndChannelId("5695eedb67e58e75a1000535", "VIVO");
#elif QQ
            GA.StartWithAppKeyAndChannelId("5695eedb67e58e75a1000535", "QQ");
#else
        GA.StartWithAppKeyAndChannelId("5695eedb67e58e75a1000535", "HOLA");
#endif

#else
        Platform = "iOS";
        GA.StartWithAppKeyAndChannelId("56a5bbbe67e58e9248001122", "iOS");
#endif

#if TestMode
        ObscuredPrefs.SetString("GameHost", "10.120.10.223");
        //ObscuredPrefs.SetString("GameHost", "139.196.14.52");
        ObscuredPrefs.SetString("GamePort", "12000");
#else
        if (ObscuredPrefs.GetString("GameHost") == "")
        {
            int RandomNum = UnityEngine.Random.Range(0, 4);
            switch(RandomNum)
            {
                case 0:
                    ObscuredPrefs.SetString("GameHost", "123.207.146.159"); //安卓 1 13000
                    ObscuredPrefs.SetString("GamePort", "13000");
                    ObscuredPrefs.SetString("DownloadUrl", "http://123.207.146.159/AssetBundle/CN/" + Platform + "/");
                    break;
                case 1:
                    ObscuredPrefs.SetString("GameHost", "123.207.141.241"); //安卓 2 12000
                    ObscuredPrefs.SetString("GamePort", "12000");
                    ObscuredPrefs.SetString("DownloadUrl", "http://123.207.141.241/AssetBundle/CN/" + Platform + "/");
                    break;
                case 2:
                    ObscuredPrefs.SetString("GameHost", "123.206.44.246"); //安卓 3 12000
                    ObscuredPrefs.SetString("GamePort", "12000");
                    ObscuredPrefs.SetString("DownloadUrl", "http://123.206.44.246/AssetBundle/CN/" + Platform + "/");
                    break;
                case 3:
                    ObscuredPrefs.SetString("GameHost", "123.207.167.15"); //安卓 4 12000
                    ObscuredPrefs.SetString("GamePort", "12000");
                    ObscuredPrefs.SetString("DownloadUrl", "http://123.207.167.15/AssetBundle/CN/" + Platform + "/");
                    break;
            }
            //ObscuredPrefs.SetString("GameHost", "139.196.24.42"); //后台
            //ObscuredPrefs.SetString("GameHost", "114.215.99.40"); //海外 holas4 12000
            //ObscuredPrefs.SetString("GameHost", "139.196.148.228"); //ios 1服 ioss1 13000 
            //ObscuredPrefs.SetString("GameHost", "106.75.15.253"); //QA holas2 12000
            //ObscuredPrefs.SetString("GameHost", "123.207.141.241"); //应用宝 androids2 12000
            //ObscuredPrefs.SetString("GameHost", "10.120.10.223");
            //ObscuredPrefs.SetString("GameHost", "121.42.199.237"); //审核
            //ObscuredPrefs.SetString("GameHost", "139.196.14.52"); //bugfree服
            //ObscuredPrefs.SetString("GameHost", "123.207.146.159"); //应用宝 1 13000
            //ObscuredPrefs.SetString("GamePort", "13000");

#if MY_WAR_DEBUG
            ObscuredPrefs.SetString("GameHost", "106.75.15.253"); //QA holas2 12000
            ObscuredPrefs.SetString("GamePort", "12000");
#endif
        }
#endif


        

        if (PlayerPrefs.GetInt("Relogin") != 1)
        {
            //////////////////////////////////////////
            GameObject gameLuaDeliver = new GameObject("gameLuaDeliver");
            gameLuaDeliver.AddComponent("LuaDeliver");
            gameLuaDeliver.transform.parent = transform;
            //////////////////////////////////////////
        }

        //////////////////////////////////////////
        GameObject gameUIManager = new GameObject("gameUIManager");
        gameUIManager.AddComponent("UIManager");
        gameUIManager.transform.parent = transform;
        //////////////////////////////////////////

        //////////////////////////////////////////
        GameObject gameAudioEditer = new GameObject("gameAudioEditer");
        gameAudioEditer.AddComponent("AudioEditer");
        gameAudioEditer.transform.parent = gameObject.transform;
        //////////////////////////////////////////

        //////////////////////////////////////////
        GameObject gameNetworkHandler = new GameObject("gameNetworkHandler");
        gameNetworkHandler.AddComponent("NetworkHandler");
        gameNetworkHandler.transform.parent = transform;
        //////////////////////////////////////////

        //////////////////////////////////////////
        GameObject gameXMLParser = new GameObject("gameXMLParser");
        gameXMLParser.AddComponent("XMLParser");
        gameXMLParser.transform.parent = transform;
        //////////////////////////////////////////

        //////////////////////////////////////////
        GameObject gameResourceLoader = new GameObject("gameResourceLoader");
        gameResourceLoader.AddComponent("ResourceLoader");
        gameResourceLoader.transform.parent = transform;
        //////////////////////////////////////////

        //////////////////////////////////////////
        GameObject gameTextTranslator = new GameObject("gameTextTranslator");
        gameTextTranslator.AddComponent("TextTranslator");
        gameTextTranslator.transform.parent = transform;
        //////////////////////////////////////////

        //////////////////////////////////////////
        GameObject gamePictureCreater = new GameObject("gamePictureCreater");
        gamePictureCreater.AddComponent("PictureCreater");
        gamePictureCreater.transform.parent = transform;
        //////////////////////////////////////////

        //////////////////////////////////////////
        GameObject gameEffectMaker = new GameObject("gameEffectMaker");
        gameEffectMaker.AddComponent("EffectMaker");
        gameEffectMaker.transform.parent = transform;
        //////////////////////////////////////////

        //////////////////////////////////////////
        GameObject gameSceneTransformer = new GameObject("gameSceneTransformer");
        gameSceneTransformer.AddComponent("SceneTransformer");
        gameSceneTransformer.transform.parent = transform;
        //////////////////////////////////////////

        //////////////////////////////////////////
        GameObject gameCharacterRecorder = new GameObject("gameCharacterRecorder");
        gameCharacterRecorder.AddComponent("CharacterRecorder");
        gameCharacterRecorder.transform.parent = transform;
        //////////////////////////////////////////

        


        PlayerPrefs.SetFloat("ElectractySlider", PlayerPrefs.GetFloat("ElectractySlider", 1));
        PlayerPrefs.SetFloat("EffectSlider", PlayerPrefs.GetFloat("EffectSlider", 1));
        PlayerPrefs.SetFloat("BGmusicSlider", PlayerPrefs.GetFloat("BGmusicSlider", 1));
        PlayerPrefs.SetFloat("EffectMusicSlider", PlayerPrefs.GetFloat("EffectMusicSlider", 1));
    }

    void OnApplicationFocus()
    {
        Debug.LogError("ReloginReloginReloginReloginRelogin");
        PlayerPrefs.SetInt("Relogin", 0);
    }
}
