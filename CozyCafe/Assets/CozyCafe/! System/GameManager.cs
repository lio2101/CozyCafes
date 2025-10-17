using System.ComponentModel;
using UnityEngine;

[DefaultExecutionOrder(-5)]
public class GameManager : MonoBehaviour
{
    //[SerializeField] private static int CHARACTERS_PER_TIMEWINDOW = 1;
    [SerializeField] private bool skipIntro;

    [SerializeField] private string playerName;
    [SerializeField] private AudioClip backGroundMusic;

    [Header("Components")]
    [SerializeField] private MenuManager menuManager;

    private AudioSource audioSource;

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
        audioSource.clip = backGroundMusic;
        audioSource.loop = true;
        audioSource.Play();

        if(skipIntro) { menuManager.SkipIntro(); }
        else { menuManager.StartMainMenu(); }
    }

    public void StartGame()
    {
        NewTimeWindow();
        CharacterManager.Instance.NewCharacter();
    }

    public void FinishDay()
    {
        NextTimeWindowEvent.Invoke(); //to night
        

        Debug.Log("Day finished");

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


    // to do next: 
    // full day cycle
    // sprite outline on mouse enter?
    // sound effects

    // to do at some point:
    //char on sprite change jump
    // book for notes
    // game paused animation
    // trinket for completing story



    // if time
    // fix settings
    // save file when day ends
    // pixel effects?
    // interactable story? multiple choice? happy ending?


    // scripts : ExtraClasses FadeIn() FadeOut()
}
