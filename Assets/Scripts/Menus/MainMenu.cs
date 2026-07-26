using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button StartButton;
    [SerializeField] private Button CreditsButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private GameObject CreditsMenuUI;

    [SerializeField] public AudioMixer mixer;


    private GameObject playerController;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player");
        playerController.GetComponent<PlayerController>().SetCanMove(false);

        StartButton.onClick.AddListener(OnStartClicked);
        CreditsButton.onClick.AddListener(OnCreditsClicked);
        QuitButton.onClick.AddListener(OnQuitClicked);

        ApplyVolume(PlayerPrefs.GetFloat("Master", 0f), PlayerPrefs.GetFloat("SFX", 0f), PlayerPrefs.GetFloat("Music", 0f));
    }

    void Update()
    {
        
    }

    private void OnStartClicked()
    {
        playerController.GetComponent<PlayerController>().SetCanMove(true);
        gameObject.SetActive(false);
    }

    private void OnCreditsClicked()
    {
        gameObject.SetActive(false);
        CreditsMenuUI.SetActive(true);
    }

    void ApplyVolume(float master, float sfx, float music)
    {
        mixer.SetFloat("MasterVolume", master);
        mixer.SetFloat("SFXVolume", sfx);
        mixer.SetFloat("MusicVolume", music);
    }

    private void OnQuitClicked()
    {
        Application.Quit();
    }
}
