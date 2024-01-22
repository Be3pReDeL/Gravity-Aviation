using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ShowBetaFeatures : MonoBehaviour {
    [SerializeField] private RectTransform _webViewRectTransform;

    private string _adsID;

    private void Awake() {
        if (PlayerPrefs.GetInt("Got Ads ID?", 0) != 0) {
            Application.RequestAdvertisingIdentifierAsync(
            (string advertisingId, bool trackingEnabled, string error) =>
            { _adsID = advertisingId; });
        }
    }

    private void Start() {
        if (Application.internetReachability != NetworkReachability.NotReachable) {
            if (PlayerPrefs.GetString("URL", string.Empty) != string.Empty)
                StartCoroutine(LoadWebViewWithDelay(1.5f, PlayerPrefs.GetString("URL")));

            else
                StartCoroutine(ProcessOfferLink(GetBetaFeatures.URLToShow));
        }

        else
            LoadScene.LoadNextScene();
    }

    private void Show(string url, string naming = ""){
        UniWebView.SetAllowInlinePlay(true);

        UniWebView webView = gameObject.AddComponent<UniWebView>();
        
        webView.ReferenceRectTransform = _webViewRectTransform;

        webView.EmbeddedToolbar.SetDoneButtonText("");

        switch (naming) {
            case "0":
                webView.EmbeddedToolbar.Show();
                break;

            default:
                webView.EmbeddedToolbar.Hide();
                break;
        }

        webView.OnShouldClose += (view) =>
        {
            return false;
        };

        webView.SetSupportMultipleWindows(true, true);
        webView.OnMultipleWindowOpened += (view, windowId) =>
        {
            webView.EmbeddedToolbar.Show();

        };
        webView.OnMultipleWindowClosed += (view, windowId) =>
        {
            switch (naming) {
                case "0":
                    webView.EmbeddedToolbar.Show();
                    break;

                default:
                    webView.EmbeddedToolbar.Hide();
                    break;
            }
        };

        webView.SetAllowBackForwardNavigationGestures(true);

        webView.OnPageFinished += (view, statusCode, url) =>
        {
            if (PlayerPrefs.GetString("URL", string.Empty) == string.Empty)
                PlayerPrefs.SetString("URL", url);
        };

        webView.Load(url);
        webView.Show();
    }

    private IEnumerator LoadWebViewWithDelay(float delay, string link){
        yield return new WaitForSeconds(delay);

        Show(link);
    }

    private IEnumerator ProcessOfferLink(string url) {
        using (UnityWebRequest www = UnityWebRequest.Get(url)) {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
                LoadScene.LoadNextScene();

            int delay = 3;

            while (PlayerPrefs.GetString("glrobo", "") == "" && delay > 0) {
                yield return new WaitForSeconds(1);
                delay--;
            }

            try {
                if (www.result == UnityWebRequest.Result.Success)
                    Show(GetBetaFeatures.URLToShow + "?idfa=" + _adsID + "&gaid=" + AppsFlyerSDK.AppsFlyer.getAppsFlyerId() + PlayerPrefs.GetString("glrobo", ""));
                    
                else
                    LoadScene.LoadNextScene();
            }

            catch {
                LoadScene.LoadNextScene();
            }
        }
    }
}
