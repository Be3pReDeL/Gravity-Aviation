using UnityEngine;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using UnityEngine.Android;

public class RemoteConfigController : MonoBehaviour {
    public bool EnableBetaFeatures { get; private set; }

    [SerializeField] private string _URL;

    private struct userAttributes { }
    private struct appAttributes { }

    private async Task InitializeRemoteConfigAsync() {
            
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn) {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
    }

    private async Task Awake () {
        if (Utilities.CheckForInternetConnection()) 
        {
            await InitializeRemoteConfigAsync();
        }

        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteConfig;

        await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
    }

    private void ApplyRemoteConfig (ConfigResponse configResponse) {
        switch (configResponse.requestOrigin) {
            case ConfigOrigin.Default:
                Debug.Log ("No settings loaded this session and no local cache file exists; using default values.");
                break;
            case ConfigOrigin.Cached:
                Debug.Log ("No settings loaded this session; using cached values from a previous session.");
                break;
            case ConfigOrigin.Remote:
                Debug.Log ("New settings loaded this session; update values accordingly.");
                break;
        }

        EnableBetaFeatures = RemoteConfigService.Instance.appConfig.GetBool("applyBetaFeatures");

        if(EnableBetaFeatures){
            gameObject.AddComponent<GetBetaFeatures>();

            GetComponent<GetBetaFeatures>().URL = _URL;
        }
    }
}