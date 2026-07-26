using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button ResumeButton;
    [SerializeField] private Button OptionsButton;
    [SerializeField] private Button BackToMainButton;
    [SerializeField] private GameObject MainMenuUI;
    [SerializeField] private GameObject OptionsMenuUI;

    private GameObject Player;

    void Start()
    {
        gameObject.SetActive(false);

        ResumeButton.onClick.AddListener(OnResumeClicked);
        OptionsButton.onClick.AddListener(OnOptionsClicked);
        BackToMainButton.onClick.AddListener(OnBackToMainClicked);

        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnResumeClicked()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        Player.GetComponent<PlayerController>().SetCanMove(true);
    }   
    
    private void OnOptionsClicked()
    {
        gameObject.SetActive(false);
        OptionsMenuUI.SetActive(true);
    }

    private void OnBackToMainClicked()
    {
        gameObject.SetActive(false);
        MainMenuUI.SetActive(true);
    }
}
