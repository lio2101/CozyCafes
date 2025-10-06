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
    [SerializeField] private List<CharacterData> characters;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject characterParent;
    [SerializeField] private float timeBeforeAppear = 2f;
    [SerializeField] private float appearAnimationDuration = 0.5f;

    public static CharacterManager Instance { get; private set; }

    private List<CharacterData> availableCharacters = new List<CharacterData>();

    private List<CharacterData> visitedCharacters = new List<CharacterData>();

    private List<CharacterData> returningCharacters = new List<CharacterData>();

    private GameObject currentObject;
    private Character currentCharacter;
    private bool isConversationActive;
    private AudioSource characterAudioSource;

    public float TimeBeforeAppear => timeBeforeAppear;
    public float AppearAnimationDuration => appearAnimationDuration;
    public bool IsConversationActive => isConversationActive;

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

    void Start()
    {
        characterAudioSource = gameObject.GetComponent<AudioSource>();
        dialogueField.gameObject.SetActive(false);
        interactButton.gameObject.SetActive(false);
        isConversationActive = false;

        returningCharacters.Clear();
        ResetDay();

        NewCharacter();
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
        currentCharacter = null;
    }

    public void NewCharacter()
    {
        // Door Sound
        if (characters != null)
        {
            //initiate prefab
            currentObject = Instantiate(characterPrefab);
            currentObject.transform.SetParent(characterParent.transform, false);

            currentCharacter = currentObject.GetComponent<Character>();

            int index = Random.Range(0, availableCharacters.Count);

            currentCharacter.InitCharacter(availableCharacters[index]);


            visitedCharacters.Add(availableCharacters[index]);
            returningCharacters.Add(availableCharacters[index]);

            availableCharacters.RemoveAt(index);

            // if character is here for the first time, reset story
            if (!returningCharacters.FirstOrDefault(data => currentCharacter.Data == data))
            {
                currentCharacter.Data.StoryProgress = 0;
            }
            currentObject.name = currentCharacter.Data.Name;
        }
    }

    public void ToggleConversationUI(bool isactive)
    {
        Debug.Log("conversation active: " + IsConversationActive);
        isConversationActive = isactive;

        dialogueField.gameObject.SetActive(isConversationActive);
        interactButton.gameObject.SetActive(!isConversationActive);

    }

    public void StartConversation()
    {
        ToggleConversationUI(true);
        dialogueField.SetDialogue(currentCharacter.GetConversation());
    }

    public void PlaySound()
    {
        characterAudioSource.clip = doorSound;
        characterAudioSource.Play();
    }

    // Private

    private void SetTalking(bool b)
    {
        currentCharacter.ChangeExpression(b ? ExpressionType.Talking : ExpressionType.Neutral);
    }

}
