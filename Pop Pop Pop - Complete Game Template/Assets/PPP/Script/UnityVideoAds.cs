using UnityEngine;
using UnityEngine.Advertisements;

public class UnityVideoAds : MonoBehaviour
{
	public void ShowAdNormal()
	{
		#if UNITY_ADS
		if (Advertisement.IsReady()) 
			Advertisement.Show();
		#else
		Debug.LogWarning("Please turn on the Unity Ads in 'Windows/Services' to show the video ads");
		Debug.LogWarning("Please note that Unity Ads only show up when you are in ANDROID or IOS platform");
		#endif
	}
}
