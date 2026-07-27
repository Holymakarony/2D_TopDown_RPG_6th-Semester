using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer Gruppen")]
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup sfxGroup;

    private AudioSource musicSource;
    private AudioSource ambienceSource;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }

        // Erstellt automatisch AudioSources für dauerhafte Sounds
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.loop = true;

        ambienceSource = gameObject.AddComponent<AudioSource>();
        ambienceSource.outputAudioMixerGroup = musicGroup;
        ambienceSource.loop = true;
    }

    // Musik abspielen (wechselt den Track)
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.Play();
    }

    // Hintergrundgeräusche abspielen
    public void PlayAmbience(AudioClip clip)
    {
        if (ambienceSource.clip == clip) return;
        ambienceSource.clip = clip;
        ambienceSource.Play();
    }

    // Universelle Methode für einmalige Soundeffekte (Explosionen, Klicks, Angriffe)
    public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;
        // Erstellt einen temporären Sound in der 2D/3D Welt, der sich nach dem Abspielen selbst löscht
        GameObject sfxObj = new GameObject("TempSFX");
        sfxObj.transform.position = position;
        
        AudioSource source = sfxObj.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.outputAudioMixerGroup = sfxGroup;
        
        source.Play();
        Destroy(sfxObj, clip.length);
    }
}
