using System.ComponentModel;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    [SerializeField] private string playerName;
    [SerializeField] private AudioClip backGroundMusic;

    private AudioSource audioSource;

    //Properties
    public static GameManager Instance { get; private set; }
    public string PlayerName1 { get => playerName; set => playerName = value; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = backGroundMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void StartGame()
    {

    }


    // to do next: 
    // ingredients pop up select menu, sprite outline on mouse enter, 
    // ingredient with popup, scruct of roasttype and description, 
        //on click -> send over to drinkmaker
    // drink maker -> ingredients refs, del drink button
    // drink SOs
    // full day cycle
    // 3 characters
    // 3 characters
    // sound effects

    // to do at some point:
    // book for notes
    // fix settings
    // save file when day ends



    // if time
    // make drink random for character?
    // pixel effects?
    // trinket for completing story
    // interactable story? multiple choice? happy ending?
    // SOs of drinks that are actually drinks, if chosen, popup: youve made an "..." !


    // scripts : ExtraClasses FadeIn() FadeOut()
}
