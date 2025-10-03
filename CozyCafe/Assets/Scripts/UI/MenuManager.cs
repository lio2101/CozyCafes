using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.ComponentModel.Design;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject saveScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject exitScreen;


    [SerializeField] private Button pauseButton;

    [Header("Menu Elements")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    [Header("Exit Questions")]
    [SerializeField] private Button yesButton;




    private bool isPaused;


    private void OnEnable()
    {
        pauseScreen.SetActive(false);
        menuScreen.SetActive(false);
        settingsScreen.SetActive(false);
        saveScreen.SetActive(false);
        exitScreen.SetActive(false);
        exitScreen.SetActive(false);

        pauseButton.onClick.AddListener(OnPause);
        continueButton.onClick.AddListener(OnPause);
        saveButton.onClick.AddListener(OnSave);
        settingsButton.onClick.AddListener(OnSettings);
        exitButton.onClick.AddListener(OnExit);
        yesButton.onClick.AddListener(OnExitConfirmed);
    }

    private void OnDisable()
    {
        pauseButton.onClick.RemoveListener(OnPause);
        continueButton.onClick.RemoveListener(OnPause);
        saveButton.onClick.RemoveListener(OnSave);
        settingsButton.onClick.RemoveListener(OnSettings);
        exitButton.onClick.RemoveListener(OnExit);
        yesButton.onClick.RemoveListener(OnExitConfirmed);
    }

    private void OnPause()
    {
        if (!isPaused)
        {
            pauseScreen.SetActive(true);
            menuScreen.SetActive(true);
            pauseButton.interactable = false;
        }
        else
        {
            pauseScreen.SetActive(false);
            menuScreen.SetActive(false);
            pauseButton.interactable = true;

        }
        isPaused = !isPaused;
    }

    private void OnSave()
    {
        menuScreen.SetActive(false);
        saveScreen.SetActive(true);
    }

    private void OnSettings()
    {
        menuScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    private void OnExit()
    {
        exitScreen.SetActive(true);
    }

    private void OnExitConfirmed()
    {
        Debug.Log("Appl Quit");
        //GameManager.Exit
        Application.Quit();
    }
}
