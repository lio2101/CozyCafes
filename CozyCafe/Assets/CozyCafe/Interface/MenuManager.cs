using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    [Header("Game States")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject introScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject bookScreen;

    [SerializeField] private GameObject scenes;
    [Header("MainMenu")]
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
        StartCoroutine(FinishIntroRoutine());
    }

    private IEnumerator FinishIntroRoutine()
    {
        GameManager.Instance.FadeIntroMusic();
        mainMenu.SetActive(false);
        scenes.SetActive(true);
        yield return new WaitForSeconds(3);
        GameManager.Instance.FadeOut(introScreen.GetComponentInChildren<Image>());
        yield return new WaitForSeconds(2);
        introScreen.SetActive(false);
        GameManager.Instance.StartGame();
    }

    public void NightMenu()
    {

    }

}
