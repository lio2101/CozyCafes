using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private DialogueField dialogueField;
    [SerializeField] private Button interactButton;
    [SerializeField] private AudioClip doorSound;

    [Header("Character Settings")]
    [SerializeField] private static int CHARACTERSPERDAY = 3;
    [SerializeField] private List<CharacterData> characters;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject characterParent;
    [SerializeField] private float timeBeforeAppear = 2f;
    [SerializeField] private float appearAnimationDuration = 0.5f;

    public static CharacterManager Instance { get; private set; }

    private List<CharacterData> availableCharacters = new List<CharacterData>();

    private List<CharacterData> visitedCharacters = new List<CharacterData>();

    private List<CharacterData> returningCharacters = new List<CharacterData>();

    private GameObject activeCharacterObj;
    private Character activeCharacter;
    private AudioSource characterAudioSource;
    private bool isConversationActive;
    private bool hasOrdered;
    private int characterCount;

    public float TimeBeforeAppear => timeBeforeAppear;
    public float AppearAnimationDuration => appearAnimationDuration;
    public bool IsConversationActive => isConversationActive;
    public bool HasOrdered => hasOrdered;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        characterCount = 0;
        characterAudioSource = gameObject.GetComponent<AudioSource>();
        ToggleButton(false);
        ToggleDialogueWindow(false);
        isConversationActive = false;

        returningCharacters.Clear();
        ResetDay();
    }

    void OnEnable()
    {
        interactButton.onClick.AddListener(StartConversation);
        dialogueField.OnDialogue += SetTalking;
    }

    void OnDisable()
    {
        interactButton.onClick.RemoveListener(StartConversation);
        dialogueField.OnDialogue -= SetTalking;
    }


    // Public 
    public void ResetDay()
    {
        availableCharacters = characters;
        visitedCharacters.Clear();
        activeCharacter = null;
    }

    public void NewCharacter()
    {
        if(availableCharacters.Count == visitedCharacters.Count)
        {
            Debug.Log("All characters visited");
        }

        if (activeCharacter != null)
        {
            Destroy(activeCharacterObj);
        }
        // Door Sound
        if (characters != null)
        {
            //initiate prefab
            activeCharacterObj = Instantiate(characterPrefab);
            activeCharacterObj.transform.SetParent(characterParent.transform, false);

            activeCharacter = activeCharacterObj.GetComponent<Character>();

            int index = Random.Range(0, availableCharacters.Count);

            activeCharacter.InitCharacter(availableCharacters[index]);


            visitedCharacters.Add(availableCharacters[index]);
            returningCharacters.Add(availableCharacters[index]);

            availableCharacters.RemoveAt(index);

            // if character is here for the first time, reset story
            if (!returningCharacters.FirstOrDefault(data => activeCharacter.Data == data))
            {
                activeCharacter.Data.StoryProgress = 0;
                activeCharacter.Data.IsReturning = false;
            }
            activeCharacterObj.name = activeCharacter.Data.Name;

            characterCount++;
        }
        hasOrdered = false;
    }

    public void DeleteCharacter()
    {
        //add fade out animation here
        bool isLast = characterCount == CHARACTERSPERDAY;

        activeCharacter.DestroyCharacter(isLast);

        if (isLast)
        {
            //Save progress
            GameManager.Instance.FinishDay();
        }
    }

    public void EndConversation()
    {
        isConversationActive = false;

        if (hasOrdered)
        {
            ToggleButton(false);
            ToggleDialogueWindow(false);
        }
        if (activeCharacter.DrinkReceived)
        {
            ToggleButton(false);
            ToggleDialogueWindow(false);
            DeleteCharacter();
        }
    }

    public void ToggleButton(bool b)
    {
        interactButton.gameObject.SetActive(b);
    }

    public void ToggleDialogueWindow(bool b)
    {
        dialogueField.gameObject.SetActive(b);
    }

    public void StartConversation()
    {
        bool drinkExists = Beverage.ActiveDrink != null;
        isConversationActive = true;

        if (!hasOrdered)
        {
            //Greeting and Order
            //Debug.Log("greet");

            ToggleDialogueWindow(true);
            ToggleButton(false);
            dialogueField.SetDialogue(activeCharacter.GetConversation(hasOrdered), true);
            hasOrdered = true;
        }

        else
        {
            if (drinkExists)
            {
                bool drinkIsReady = Beverage.ActiveDrink.BeverageData.IsFull;
                if (drinkIsReady)
                {
                    //Destroy Cup
                    Destroy(Beverage.ActiveDrink.gameObject);
                    //Feedback and Goodbye
                    //Debug.Log("feedback");

                    ToggleDialogueWindow(true);
                    ToggleButton(false);
                    dialogueField.SetDialogue(activeCharacter.GetConversation(hasOrdered), true);

                    hasOrdered = false;
                }
                else { Debug.Log("Drink not ready"); }
            }
            else { Debug.Log("Drink not made"); }
        }
    }

    public void PlaySound()
    {
        characterAudioSource.clip = doorSound;
        characterAudioSource.Play();
    }

    // Private

    private void SetTalking(bool b)
    {
        activeCharacter.ChangeExpression(b ? ExpressionType.Talking : ExpressionType.Neutral);
    }

}
