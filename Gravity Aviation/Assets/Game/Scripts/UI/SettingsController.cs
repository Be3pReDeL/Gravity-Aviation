using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.iOS;
using UnityEngine.UI;

[OPS.Obfuscator.Attribute.DoNotObfuscateClass]
public class SettingsController : UIController {
    [SerializeField] private string _contactUsURL;

    [SerializeField] private Button _musicButton, _soundButton;
    [SerializeField] private Sprite _toogleOn, _toogleOff;

    [SerializeField] private AudioMixer audioMixer;

    [OPS.Obfuscator.Attribute.DoNotRename]

    public override void Start(){
        base.Start();

        bool isMusicOn = Convert.ToBoolean(PlayerPrefs.GetInt("Music", 1));
        _musicButton.image.sprite = isMusicOn ? _toogleOn : _toogleOff;
        bool isSoundOn = Convert.ToBoolean(PlayerPrefs.GetInt("SFX", 1));
        _soundButton.image.sprite = isSoundOn ? _toogleOn : _toogleOff;
    }
    [OPS.Obfuscator.Attribute.DoNotRename]
    public void ToggleMusic() 
    {
        bool isMusicOn = Convert.ToBoolean(PlayerPrefs.GetInt("Music", 1));
        PlayerPrefs.SetInt("Music", Convert.ToInt32(!isMusicOn));

        _musicButton.image.sprite = PlayerPrefs.GetInt("Music", 1) == 1 ? _toogleOn : _toogleOff;
        
        audioMixer.SetFloat("Soundtrack", isMusicOn ? -80f : 0f);
        audioMixer.SetFloat("Ambience", isMusicOn ? -80f : 0f);
    }

    [OPS.Obfuscator.Attribute.DoNotRename]
    public void ToggleSound() 
    {
        bool isSoundOn = Convert.ToBoolean(PlayerPrefs.GetInt("SFX", 1));
        PlayerPrefs.SetInt("SFX", Convert.ToInt32(!isSoundOn));

        _soundButton.image.sprite = PlayerPrefs.GetInt("SFX", 1) == 1 ? _toogleOn : _toogleOff;
        
        audioMixer.SetFloat("SFX", isSoundOn ? -80f : 0f);
    }

    [OPS.Obfuscator.Attribute.DoNotRename]
    public void ContactUs()
    {
        string contactUrl = _contactUsURL; // URL вашей страницы контактов
        Application.OpenURL(contactUrl);
    }


    [OPS.Obfuscator.Attribute.DoNotRename]
    public void RateUs(){
        Device.RequestStoreReview();
    }
}

