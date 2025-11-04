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
        screenWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
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

        Vector3 startPos = Vector3.zero;
        frontScene.transform.position = startPos;
        backScene.transform.position = startPos;


        Vector3 distance = startPos + Vector3.right * screenWidth;

        // direction = +1 (right) or -1 (left)
        int dir = (index == 1) ? 1 : -1;

        Vector3 startPosA = startPos;
        Vector3 targetPosA = startPosA + distance * dir;

        Vector3 startPosB = backScene.transform.position - distance * dir;
        Vector3 targetPosB = startPos;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);

            if (index == 0)
            {
                frontScene.transform.position = Vector3.Lerp(startPosA, targetPosA, t);
                backScene.transform.position = Vector3.Lerp(startPosB, targetPosB, t);
            }
            else
            {
                backScene.transform.position = Vector3.Lerp(startPosA, targetPosA, t);
                frontScene.transform.position = Vector3.Lerp(startPosB, targetPosB, t);
            }
            yield return null;
        }
        isFront = !isFront;
    }

}
