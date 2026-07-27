using UnityEngine;

public class SceneAudioTrigger : MonoBehaviour
{
    public AudioClip backgroundMusic;
    public AudioClip ambientSound;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            if (backgroundMusic != null) AudioManager.Instance.PlayMusic(backgroundMusic);
            if (ambientSound != null) AudioManager.Instance.PlayAmbience(ambientSound);
        }
    }
}
