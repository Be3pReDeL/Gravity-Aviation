using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] musicTracks; // Массив музыкальных треков
    [SerializeField] private AudioSource audioSource; // Источник звука для воспроизведения музыки

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayCurrentMusic();
    }

    [OPS.Obfuscator.Attribute.DoNotRename]
    public void PlayCurrentMusic()
    {
        string currentMusic = PlayerPrefs.GetString("Music_current", "defaultMusic");
        AudioClip selectedTrack = FindMusicByName(currentMusic);
        if (selectedTrack != null)
        {
            audioSource.clip = selectedTrack;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Music track not found: " + currentMusic);
        }
    }

    private AudioClip FindMusicByName(string name)
    {
        foreach (var track in musicTracks)
        {
            if (track.name == name)
                return track;
        }
        return null; // Возвращает null, если трек не найден
    }
}
