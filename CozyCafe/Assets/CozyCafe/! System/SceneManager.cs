using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.ComponentModel.Design;
using Unity.VisualScripting;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private Button changeToBack;
    [SerializeField] private Button changeToFront;
    [SerializeField] private GameObject frontScene;
    [SerializeField] private GameObject backScene;
    [SerializeField] private float buttonCooldown = 2.0f;
    [SerializeField] private float animationDuration = 1.0f;

    private float screenWidth;
    private bool isFront;


    private void Awake()
    {
        screenWidth = Screen.width;
        isFront = true;
        frontScene.SetActive(true);

        Vector3 distance = backScene.transform.position + Vector3.right * screenWidth;
        backScene.transform.position = backScene.transform.position + distance;
    }

    private void OnEnable()
    {
        changeToBack.onClick.AddListener(() => ChangeScene(0));
        changeToFront.onClick.AddListener(() => ChangeScene(1));
    }

    private void OnDisable()
    {
        changeToBack.onClick.RemoveListener(() => ChangeScene(0));
        changeToFront.onClick.AddListener(() => ChangeScene(1));
    }

    public void Togglebutton(bool b)
    {
        changeToBack.gameObject.SetActive(b);
    }

    private void ChangeScene(int index)
    {
        if (!CharacterManager.Instance.IsConversationActive)
        {
            StartCoroutine(ButtonCooldownRoutine());
            StartCoroutine(SlideshowRoutine(index));
        }

        //game manager active scene
    }



    private IEnumerator ButtonCooldownRoutine()
    {
        changeToBack.interactable = false;
        changeToFront.interactable = false;
        yield return new WaitForSeconds(buttonCooldown);
        changeToBack.interactable = true;
        changeToFront.interactable = true;
    }


    private IEnumerator SlideshowRoutine(int index)
    {
        frontScene.SetActive(true);
        backScene.SetActive(true);

        RectTransform frontRT = frontScene.GetComponent<RectTransform>();
        RectTransform backRT = backScene.GetComponent<RectTransform>();

        float screenWidth = ((RectTransform)frontRT.parent).rect.width;

        Vector2 startPos = Vector2.zero;
        Vector2 distance = Vector2.right * screenWidth;

        int dir = (index == 1) ? 1 : -1;

        Vector2 startPosA = startPos;
        Vector2 targetPosA = startPos + distance * dir;

        Vector2 startPosB = startPos - distance * dir;
        Vector2 targetPosB = startPos;

        frontRT.anchoredPosition = startPosA;
        backRT.anchoredPosition = startPosB;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);

            if (index == 0)
            {
                frontRT.anchoredPosition = Vector2.Lerp(startPosA, targetPosA, t);
                backRT.anchoredPosition = Vector2.Lerp(startPosB, targetPosB, t);
            }
            else
            {
                backRT.anchoredPosition = Vector2.Lerp(startPosA, targetPosA, t);
                frontRT.anchoredPosition = Vector2.Lerp(startPosB, targetPosB, t);
            }

            yield return null;
        }

        isFront = !isFront;
    }

}
