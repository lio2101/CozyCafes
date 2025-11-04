using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-5)]
public class GameManager : MonoBehaviour
{
    //[SerializeField] private static int CHARACTERS_PER_TIMEWINDOW = 1;
    [SerializeField] private bool skipIntro;

    [SerializeField] private string playerName;
    [SerializeField] private AudioClip backGroundMusic;
    [SerializeField] private AudioClip nightMusic;
    [SerializeField] private AudioClip menuMusic;


    [SerializeField] private SceneManager sm;
    [SerializeField] private Button newDaybutton;


    [Header("Components")]
    [SerializeField] private MenuManager menuManager;

    private AudioSource audioSource;
    private float musicFadeTime = 3.0f;

    //Properties
    public static GameManager Instance { get; private set; }
    public string PlayerName1 { get => playerName; set => playerName = value; }

    public delegate void NextTimeWindow();
    public NextTimeWindow NextTimeWindowEvent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = menuMusic;
        audioSource.loop = true;
        audioSource.Play();

        newDaybutton.onClick.AddListener(StartNewDay);
        newDaybutton.gameObject.SetActive(false);

        if (skipIntro) { menuManager.SkipIntro(); }
        else { menuManager.StartMainMenu(); }
    }

    public void StartGame()
    {
        newDaybutton.gameObject.SetActive(false);
        sm.Togglebutton(true);
        NewTimeWindow();
        CharacterManager.Instance.NewGame();
        CharacterManager.Instance.NewCharacter();
    }

    public void StartNewDay()
    {
        newDaybutton.gameObject.SetActive(false);
        CharacterManager.Instance.ResetDay();
        StartCoroutine(DayRoutine());
    }

    public void FinishDay()
    {
        Debug.Log("Day finished");
        NextTimeWindowEvent.Invoke(); //to night
        StartCoroutine(NightRoutine());
        //Change
    }

    public void NewTimeWindow()
    {
        NextTimeWindowEvent.Invoke();
    }



    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetName(string name)
    {
        Debug.Log("Set player name to " + name);
        playerName = name;
    }

    public void FadeIntroMusic()
    {
        StartCoroutine(FadeRoutine(backGroundMusic));
    }


    private IEnumerator NightRoutine()
    {
        yield return StartCoroutine(FadeRoutine(nightMusic));
        // do after
        sm.Togglebutton(false);
        newDaybutton.gameObject.SetActive(true);
    }

    private IEnumerator DayRoutine()
    {
        yield return StartCoroutine(FadeRoutine(backGroundMusic));
        // do after
        NewTimeWindow();
        sm.Togglebutton(true);
        CharacterManager.Instance.ResetDay();
        CharacterManager.Instance.NewCharacter();
    }

    private IEnumerator FadeRoutine(AudioClip to)
    {
        Debug.Log("lowering music");
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / musicFadeTime;
            yield return null;
        }

        audioSource.Stop();
        Debug.Log("stopping music");

        yield return new WaitForSeconds(0.5f);
        audioSource.clip = to;
        audioSource.Play();

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / musicFadeTime;
            yield return null;
        }

        audioSource.volume = startVolume;
        Debug.Log("finished music");

    }

    // scripts : ExtraClasses FadeIn() FadeOut()

    public void FadeOut(Image img)
    {
        StartCoroutine(FadeOutRoutine(img));
    }

    public IEnumerator FadeOutRoutine(Image img, float fadeDuration = 1f)
    {
        Color startColor = img.color;
        Color endColor = startColor;
        endColor.a = 0;

        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            img.color = Color.Lerp(startColor, endColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        img.color = new Color(1, 1, 1, 0);
    }
}
