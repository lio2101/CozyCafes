using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    [SerializeField] private Image[] backGrounds;
    [SerializeField] private float animDuration = 3.0f;


    int currentIndex;

    private void Start()
    {
        currentIndex = 0;
    }

    private void OnEnable()
    {
        GameManager.Instance.NextTimeWindowEvent += ChangeBackground;
    }
    private void OnDisable()
    {
        GameManager.Instance.NextTimeWindowEvent -= ChangeBackground;
    }

    private void ChangeBackground()
    {
        //animation here, test it
        Debug.Log("setting bg to " + currentIndex);

        foreach (var bg in backGrounds)
        {
            bg.enabled = false;
        }
        if (currentIndex < backGrounds.Length)
        {
            backGrounds[currentIndex].enabled = true;
            backGrounds[currentIndex + 1].enabled = true;
            StartCoroutine(ChangeBackgroundAnimation(backGrounds[currentIndex], backGrounds[currentIndex + 1]));
            currentIndex = (currentIndex < backGrounds.Length ? currentIndex + 1 : 0);
        }
        else
        {
            Debug.Log("day over");
            backGrounds[currentIndex].enabled = true;
            backGrounds[0].enabled = true;
            StartCoroutine(ChangeBackgroundAnimation(backGrounds[currentIndex], backGrounds[0]));
            currentIndex = 0;
        }

    }

    private IEnumerator ChangeBackgroundAnimation(Image cur, Image next)
    {


        float elapsedTime = 0;
        Color currentColorA = cur.color;
        currentColorA.a = 1f;
        Color currentColorB = currentColorA;
        currentColorB.a = 0f;

        Color nextColorA = next.color;
        currentColorA.a = 0f;
        Color nextColorB = currentColorA;
        currentColorB.a = 1f;

        cur.color = currentColorA;
        next.color = nextColorB;



        //Fade in
        while (elapsedTime < animDuration)
        {
            float t = elapsedTime / animDuration;
            cur.color = Color.Lerp(currentColorA, currentColorB, t);
            next.color = Color.Lerp(nextColorA, nextColorB, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cur.color = currentColorB;
        next.color = nextColorB;
    }

}
