using UnityEngine;
using OneSignalSDK;

[OPS.Obfuscator.Attribute.DoNotObfuscateClass]
public class OneSignalIntegration : MonoBehaviour
{
    [SerializeField] private string _OneSignalKey;

    private void Start(){
        OneSignal.Initialize(_OneSignalKey);
    }
}
