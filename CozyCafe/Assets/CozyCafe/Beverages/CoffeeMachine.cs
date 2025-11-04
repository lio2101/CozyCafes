using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CoffeeMachine : MonoBehaviour
{
    [SerializeField] private float roastTime = 5.0f;
    [SerializeField] private List<Sprite> statesprites;
    [SerializeField] private Button[] beans;

    [Header("Audio")]

    [SerializeField] private AudioClip fillBeans;
    [SerializeField] private AudioClip startMachine;
    [SerializeField] private AudioClip tryStartMachine;
    [SerializeField] private AudioClip machineActive;
    [SerializeField] private AudioClip machineDone;



    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button removebeansButton;

    private int index;
    private bool hasBeans;

    private Image thisImage;
    private AudioSource audioSource;

    private RoastType roastType;
    private Coroutine machineRoutine;

    private void Start()
    {
        thisImage = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();

        index = 0;
        thisImage.sprite = statesprites[index];

        for (int i = 0; i < beans.Length; i++)
        {
            int index = i;
            beans[i].onClick.AddListener(() => SetBeans((RoastType)index));
        }
    }

    private void OnEnable()
    {
        startButton.onClick.AddListener(OnStart);
        removebeansButton.onClick.AddListener(RemoveBeans);
    }
    private void OnDisable()
    {
        startButton.onClick.RemoveListener(OnStart);
        removebeansButton.onClick.RemoveListener(RemoveBeans);
    }

    public void SetBeans(RoastType type)
    {
        if (hasBeans)
        {
            return;
        }
        roastType = type;
        hasBeans = true;
        index++;
        thisImage.sprite = statesprites[index];
        Debug.Log("Adding RoastType : " + type.ToString());
    }

    private void OnStart()
    {
        if (hasBeans)
        {
            if (Beverage.ActiveDrink != null)
            {
                //make Sound
                audioSource.clip = startMachine;
                audioSource.Play();
                index++;
                thisImage.sprite = statesprites[index];
                machineRoutine = StartCoroutine(MachineRoutine());
            }
            else { Debug.Log("No drink found"); }

        }
        else
        {
            //make empty sound
            audioSource.clip = tryStartMachine;
            audioSource.Play();
            Debug.Log("No beans in machine");
        }
    }

    private void RemoveBeans()
    {
        if (hasBeans)
        {
            audioSource.clip = fillBeans;
            audioSource.Play();
            index = 0;
            thisImage.sprite = statesprites[index];
            hasBeans = false;
        }
        else { Debug.Log("no beans to remove here"); }
    }

    private IEnumerator MachineRoutine()
    {
        audioSource.clip = machineActive;
        audioSource.Play();

        startButton.interactable = false;
        removebeansButton.interactable = false;

        yield return new WaitForSeconds(roastTime / 4);

        //play sound

        index++;
        thisImage.sprite = statesprites[index];

        yield return new WaitForSeconds(roastTime / 4);

        index++;
        thisImage.sprite = statesprites[index];

        yield return new WaitForSeconds(roastTime / 4);

        index++;
        thisImage.sprite = statesprites[index];

        startButton.interactable = true;
        removebeansButton.interactable = true;
        hasBeans = false;

        audioSource.clip = machineDone;
        audioSource.Play();

        yield return new WaitForSeconds(roastTime / 4);


        index = 0;
        thisImage.sprite = statesprites[index];

        Beverage.ActiveDrink.SelectRoast(roastType);
        Beverage.ActiveDrink.ChangeSprite();

        machineRoutine = null;
    }
}
