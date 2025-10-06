using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class DialogueField : MonoBehaviour
{
    [SerializeField] private AudioClip babbleAudio;
    [SerializeField] private Button continueButton;

    [Header("Settings")]
    [SerializeField] private float letterSpeed = 1.0f;
    [SerializeField] private float soundSpeed = 1.0f;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.8f, 1.2f);

    private AudioSource audioSource;
    private TMP_Text textField;
    private Coroutine animationCoroutine;

    private List<string> currentDialogue;
    private string currentLine;
    private int lineIndex;

    public delegate void TextAnimationDelegate(bool isActive);
    public TextAnimationDelegate OnDialogue;

    private void Awake()
    {
        textField = GetComponentInChildren<TMP_Text>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = babbleAudio;
    }

    private void OnEnable()
    {
        continueButton.onClick.AddListener(OnButton);
    }

    private void OnDisable()
    {
        continueButton.onClick.RemoveListener(OnButton);
    }

    public void SetDialogue(List<string> dialogue)
    {
        currentDialogue = dialogue;
        lineIndex = 0;
        currentLine = currentDialogue[lineIndex];
        NextLine(currentLine);
    }

    // Text Sound

    private void NextLine(string line)
    {
        animationCoroutine = StartCoroutine(TextAnimation(line));
    }

    private void SkipAnimation()
    {
        OnDialogue.Invoke(false);
        StopCoroutine(animationCoroutine);
        animationCoroutine = null;
        textField.text = currentLine;
    }

    private IEnumerator TextAnimation(string str)
    {
        textField.text = "";
        OnDialogue.Invoke(true);
        for (int i = 0; i <= str.Length - 1; i++)
        {
            audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
            audioSource.Play();

            textField.text += str[i];
            yield return new WaitForSeconds(0.1f / letterSpeed);
        }
        OnDialogue.Invoke(false);
        animationCoroutine = null;
    }

    private void OnButton()
    {
        if (animationCoroutine != null)
        {
            SkipAnimation();
        }
        else if (lineIndex < currentDialogue.Count - 1)
        {
            lineIndex++;
            currentLine = currentDialogue[lineIndex];
            NextLine(currentLine);
        }
        else
        {
            CharacterManager.Instance.ToggleConversationUI(false);
        }
    }
}
