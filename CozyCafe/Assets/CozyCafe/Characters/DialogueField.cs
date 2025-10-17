using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class DialogueField : MonoBehaviour
{
    [TextArea(5, 10)][SerializeField] private string presetText;
    [SerializeField] private bool startOnEnable;
    [SerializeField] private AudioClip babbleAudio;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button overButton;


    [Header("Settings")]
    [SerializeField] private float letterSpeed = 1.0f;
    //[SerializeField] private float soundSpeed = 1.0f;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.8f, 1.2f);

    private AudioSource audioSource;
    private TMP_Text textField;
    private Coroutine animationCoroutine;

    private List<string> currentDialogue;
    private string currentLine;
    private int lineIndex;
    private bool isConvo;

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

        if (startOnEnable)
        {
            List<string> parts = presetText.Split(new string[] { "/p" }, StringSplitOptions.None).ToList();
            SetDialogue(parts);

        }
    }

    private void OnDisable()
    {
        continueButton.onClick.RemoveListener(OnButton);
    }

    public void SetDialogue(List<string> dialogue, bool convo = false)
    {
        isConvo = convo;
        List<string> updatedList = new List<string>();

        foreach (string text in dialogue)
        {
            string[] parts = text.Split(new string[] { "/p" }, System.StringSplitOptions.None);

            foreach (string part in parts)
            {
                string trimmed = part.Trim();
                trimmed.Replace("[player]", GameManager.Instance.PlayerName1);
                if (!string.IsNullOrEmpty(trimmed))
                    updatedList.Add(trimmed);
            }
        }
        currentDialogue = updatedList;
        lineIndex = 0;
        currentLine = currentDialogue[lineIndex];
        NextLine(currentLine);
    }

    // Text Sound

    private void NextLine(string line)
    {
        if(line == null) { Debug.Log("no dialogue found "); return; }
        animationCoroutine = StartCoroutine(TextAnimation(line));
    }

    private void SkipAnimation()
    {
        if(isConvo)
            OnDialogue.Invoke(false);
        StopCoroutine(animationCoroutine);
        animationCoroutine = null;
        textField.text = currentLine;
    }

    private IEnumerator TextAnimation(string str)
    {
        textField.text = "";
        if(isConvo)
            OnDialogue.Invoke(true);
        for (int i = 0; i <= str.Length - 1; i++)
        {
            audioSource.pitch = UnityEngine.Random.Range(pitchRange.x, pitchRange.y);
            audioSource.Play();

            textField.text += str[i];
            yield return new WaitForSeconds(0.1f / letterSpeed);
        }
        if(isConvo)
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
            if (isConvo) { CharacterManager.Instance.EndConversation(); }
            else { overButton.gameObject.SetActive(true); }
        }
    }
}
