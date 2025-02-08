using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;


public class GUIManager : MonoBehaviour {
	[Header("StartMenu")]
	public GameObject StartMenu;
	public TextMeshProUGUI Level;
	public GameObject NextButton;
	public GameObject PreButton;
	public Image soundImg;
	public Sprite soundOn;
	public Sprite soundOff;
	public string storeLink = "your store link";
	public string facebookLink = "your facebook link";

	
	[Header("UI")]
	public GameObject UI;
	public Text pathLeft;
	public GameObject tapToPlay;
	public GameObject AddStaminaPanel;
	

	public static GUIManager instance;
	// Use this for initialization
	void Start () {
		instance = this;
		StartMenu.SetActive (true);
		UI.SetActive (false);

		if (!GlobalValue.isRestart) {
			GlobalValue.levelPlaying = GameManager.HighLevel;
		}
		init ();	//init start menu
		
		if (GlobalValue.isSound) 
			soundImg.sprite = soundOn;
		else
			soundImg.sprite = soundOff;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (GameManager.CurrentState == GameManager.GameState.Playing)
				GameManager.instance.Restart ();
			else
				Application.Quit ();
		}
		pathLeft.text = GlobalValue.levelPlayingPathLeft + "";
	}



	//called by Button

	public void Play(){
		if(StaminaTimer.currentStamina>=1)
		{
            GameManager.instance.Play();
            StartMenu.SetActive(false);
			tapToPlay.SetActive(false);
            UI.SetActive(true);
			StaminaTimer.subtract(1);
        }
		else
		{
			AddStaminaPanel.SetActive(true);
			Debug.Log("体力不够");
		}
		
	}

	//Button Next and Pre
	private void init(){
		
		Level.text = "关卡 " + GlobalValue.levelPlaying;
		if (GameManager.HighLevel == 1) {
			NextButton.SetActive (false);
			PreButton.SetActive (false);
		} else if (GlobalValue.levelPlaying == 1) {
			NextButton.SetActive (true);
			PreButton.SetActive (false);
		} else if (GlobalValue.levelPlaying == GameManager.HighLevel) {
			NextButton.SetActive (false);
			PreButton.SetActive (true);
		} else {
			NextButton.SetActive (true);
			PreButton.SetActive (true);
		}
	}

	public void Next(){
		SoundManager.PlaySfx ("Click");
		GlobalValue.levelPlaying++;
		init ();
	}

	public void Pre(){
		SoundManager.PlaySfx ("Click");
		GlobalValue.levelPlaying--;
		init ();
	}
	
	public void Sound(){
		GlobalValue.isSound = !GlobalValue.isSound;
		GlobalValue.isMusic = !GlobalValue.isMusic;
		if (GlobalValue.isSound) {
			soundImg.sprite = soundOn;
			SoundManager.SoundVolume = 1f;
			SoundManager.MusicVolume = 1f;
			SoundManager.PlaySfx ("Click");
		} else {
			soundImg.sprite = soundOff;
			SoundManager.SoundVolume = 0f;
			SoundManager.MusicVolume = 0f;
		}
	}

	public void OpenFacebook(){
		Application.OpenURL (facebookLink);
	}

	public void OpenStoreLink(){
		Application.OpenURL (storeLink);
	}

}
