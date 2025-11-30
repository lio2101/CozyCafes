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
    [SerializeField] private BeverageManager beverageManager;

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

    public void NewGame()
    {
        foreach (CharacterData c in characters)
        {
            c.StoryProgress = 0;
            c.VisitAmount = 0;
            c.CorrectAmount = 0;
        }
        returningCharacters.Clear();
        beverageManager.CreateButton(false);
        ResetDay();
    }


    // Public 
    public void ResetDay()
    {
        characterCount = 0;
        availableCharacters.Clear();
        availableCharacters.AddRange(characters);
        visitedCharacters.Clear();
        activeCharacter = null;
    }

    public void NewCharacter()
    {
        if (activeCharacter != null)
        {
            Destroy(activeCharacterObj);
        }
        Debug.Log("new character");
        //initiate prefab
        activeCharacterObj = Instantiate(characterPrefab);
        activeCharacterObj.transform.SetParent(characterParent.transform, false);

        activeCharacter = activeCharacterObj.GetComponent<Character>();

        int index = Random.Range(0, availableCharacters.Count);

        activeCharacter.InitCharacter(availableCharacters[index]);

        visitedCharacters.Add(availableCharacters[index]);
        returningCharacters.Add(availableCharacters[index]);
        availableCharacters.RemoveAt(index);


        activeCharacterObj.name = activeCharacter.Data.Name;

        characterCount++;

        hasOrdered = false;
    }

    public void DeleteCharacter()
    {
        bool isLast = characterCount == CHARACTERSPERDAY;

        activeCharacter.DestroyCharacter(isLast);

        if (isLast)
        {
            //Save progress
            ResetDay();
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
            dialogueField.Setname(activeCharacter.Data.Name);
            hasOrdered = true;
            beverageManager.CreateButton(true);
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
                    dialogueField.Setname(activeCharacter.Data.Name);

                    hasOrdered = false;
                    beverageManager.CreateButton(false);
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

    public void Jump()
    {
        activeCharacter.Jump();
    }

    // Private

    private void SetTalking(bool b)
    {
        activeCharacter.ChangeExpression(b ? ExpressionType.Talking : ExpressionType.Neutral);
    }

}
