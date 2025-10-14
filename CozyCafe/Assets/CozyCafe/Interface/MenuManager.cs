using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Game States")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject introScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject scenes;



    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject exitScreen;


    private void OnEnable()
    {
        pauseScreen.SetActive(false);
        menuScreen.SetActive(false);
        settingsScreen.SetActive(false);
        exitScreen.SetActive(false);
    }
    public void StartMainMenu()
    {
        mainMenu.SetActive(true);
        pauseScreen.SetActive(false);
        introScreen.SetActive(false);
        scenes.SetActive(false);
    }

    public void SkipIntro()
    {
        mainMenu.SetActive(false);
        introScreen.SetActive(false);
        scenes.SetActive(true);
        GameManager.Instance.SetName("Player");
        GameManager.Instance.StartGame();
    }
    public void FinishIntro()
    {
        mainMenu.SetActive(false);
        introScreen.SetActive(false);
        scenes.SetActive(true);
        GameManager.Instance.StartGame();
    }

}
