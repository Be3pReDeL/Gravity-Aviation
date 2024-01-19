using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
using Unity.Notifications;
#endif

public class GetBetaFeatures : MonoBehaviour {
    public static int SceneIndex { get; private set; } = 2;
    public static string URLToShow { get; private set; }

    public string URL;

    private const string STOPMESSAGE = "no beta features for this region";

    private void Start() {
#if UNITY_IOS
        RequestIosPermissions();
#endif
        StartCoroutine(RequestNotificationPermission());
        StartCoroutine(GetAPIAnswer());
    }

#if UNITY_IOS
    private void RequestIosPermissions() {
        ATTrackingStatusBinding.RequestAuthorizationTracking();
        var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
        if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED) {
            PlayerPrefs.SetInt("Got Ads ID?", 1);
        }
    }
#endif

    private IEnumerator GetAPIAnswer() {
        using (UnityWebRequest www = UnityWebRequest.Get(URL)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError(www.error);
            } else {
                ProcessApiResponse(www.downloadHandler.text);
            }
        }
    }

    private void ProcessApiResponse(string responseText) {
        string processedText = responseText.Replace("\"", "");


        if (processedText == STOPMESSAGE) {
            SceneIndex = 2;
        } else {
            SceneIndex = 1;
            
            if(PlayerPrefs.GetString("URL", string.Empty) == string.Empty)
                URLToShow = processedText;
            else
                URLToShow = PlayerPrefs.GetString("URL");
        }

        LoadingBar.LoadComplete.Invoke();
    }

    private IEnumerator RequestNotificationPermission() {
#if UNITY_IOS
        var request = NotificationCenter.RequestPermission();
        yield return request;
#endif
    }
}
