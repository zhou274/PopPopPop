/*
 * This script control almost the game: State, UI, Ads
*/
using UnityEngine;
using System.Collections;
using TTSDK.UNBridgeLib.LitJson;
using TTSDK;
using StarkSDKSpace;
using System.Collections.Generic;

#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif
public class GameManager : MonoBehaviour {
	public static GameManager instance;
    public string clickid;
    private StarkAdManager starkAdManager;
    public enum GameState{
		Menu,
		Playing,
		Pause,
		Dead
	};

	private GameState state;
	private int score = 0;

	public static GameState CurrentState{
		get{ return instance.state; }
		set{ instance.state = value; }
	}

	public static int Score{
		get{ return instance.score; }
		set{ instance.score = value; }
	}

	public static int HighLevel{
		get{ return PlayerPrefs.GetInt(GlobalValue.levelHighest,1); }
		set{ PlayerPrefs.SetInt(GlobalValue.levelHighest, value); }
	}

	// Use this for initialization
	void Start () {
		instance = this;
		state = GameState.Menu;

	}

	public void Play(){
		state = GameState.Playing;
		GlobalValue.levelPlayingPathLeft = GlobalValue.levelPlaying;	//for PlayerController
		GlobalValue.levelPathLeft = GlobalValue.levelPlaying;		//for CreatePlatform

//		AdsController.HideAds ();

	}
	public void Skip()
	{
        ShowVideoAd("192if3b93qo6991ed0",
            (bol) => {
                if (bol)
                {

                    GameSuccess();


                    clickid = "";
                    getClickid();
                    apiSend("game_addiction", clickid);
                    apiSend("lt_roi", clickid);


                }
                else
                {
                    StarkSDKSpace.AndroidUIManager.ShowToast("观看完整视频才能获取奖励哦！");
                }
            },
            (it, str) => {
                Debug.LogError("Error->" + str);
                //AndroidUIManager.ShowToast("广告加载异常，请重新看广告！");
            });
        
	}
	public void GameSuccess(){

		GlobalValue.levelPlaying++;

		if (GlobalValue.levelPlaying >= HighLevel)
			HighLevel++;		//save playerPref
		
		StartCoroutine (WaitForRestart (1.5f));
        ShowInterstitialAd("1lcaf5895d5l1293dc",
                () => {
                    Debug.LogError("--插屏广告完成--");

                },
                (it, str) => {
                    Debug.LogError("Error->" + str);
                });
        //		AdsController.ShowAds ();

    }

	public void GameOver(){
		state = GameState.Dead;
		StartCoroutine (WaitForRestart (1f));
        ShowInterstitialAd("1lcaf5895d5l1293dc",
            () => {
                Debug.LogError("--插屏广告完成--");

            },
            (it, str) => {
                Debug.LogError("Error->" + str);
            });
        //		AdsController.ShowAds ();

    }
    /// <summary>
    /// 播放插屏广告
    /// </summary>
    /// <param name="adId"></param>
    /// <param name="errorCallBack"></param>
    /// <param name="closeCallBack"></param>
    public void ShowInterstitialAd(string adId, System.Action closeCallBack, System.Action<int, string> errorCallBack)
    {
        starkAdManager = StarkSDK.API.GetStarkAdManager();
        if (starkAdManager != null)
        {
            var mInterstitialAd = starkAdManager.CreateInterstitialAd(adId, errorCallBack, closeCallBack);
            mInterstitialAd.Load();
            mInterstitialAd.Show();
        }
    }
    public void Restart(){
		StartCoroutine (WaitForRestart (0f));
	}

	IEnumerator WaitForRestart(float time){
		yield return new WaitForSeconds (time);
		GlobalValue.isRestart = true;

		#if UNITY_5_3
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		#else
		Application.LoadLevel (Application.loadedLevel);
		#endif
	}
    public void getClickid()
    {
        var launchOpt = StarkSDK.API.GetLaunchOptionsSync();
        if (launchOpt.Query != null)
        {
            foreach (KeyValuePair<string, string> kv in launchOpt.Query)
                if (kv.Value != null)
                {
                    Debug.Log(kv.Key + "<-参数-> " + kv.Value);
                    if (kv.Key.ToString() == "clickid")
                    {
                        clickid = kv.Value.ToString();
                    }
                }
                else
                {
                    Debug.Log(kv.Key + "<-参数-> " + "null ");
                }
        }
    }

    public void apiSend(string eventname, string clickid)
    {
        TTRequest.InnerOptions options = new TTRequest.InnerOptions();
        options.Header["content-type"] = "application/json";
        options.Method = "POST";

        JsonData data1 = new JsonData();

        data1["event_type"] = eventname;
        data1["context"] = new JsonData();
        data1["context"]["ad"] = new JsonData();
        data1["context"]["ad"]["callback"] = clickid;

        Debug.Log("<-data1-> " + data1.ToJson());

        options.Data = data1.ToJson();

        TT.Request("https://analytics.oceanengine.com/api/v2/conversion", options,
           response => { Debug.Log(response); },
           response => { Debug.Log(response); });
    }


    /// <summary>
    /// </summary>
    /// <param name="adId"></param>
    /// <param name="closeCallBack"></param>
    /// <param name="errorCallBack"></param>
    public void ShowVideoAd(string adId, System.Action<bool> closeCallBack, System.Action<int, string> errorCallBack)
    {
        starkAdManager = StarkSDK.API.GetStarkAdManager();
        if (starkAdManager != null)
        {
            starkAdManager.ShowVideoAdWithId(adId, closeCallBack, errorCallBack);
        }
    }
}
