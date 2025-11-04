using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlinkEffect : MonoBehaviour
{
    [SerializeField] private float blinkEvery = 1f;

    private TMP_Text text;
    private Color startColor;
    private Color endColor;
    Coroutine blinker;

    private void Awake()
    {
        text = this.GetComponentInChildren<TMP_Text>();
        startColor = text.color;
        endColor = Color.white;
        endColor.a = 0;

    }

    private void OnEnable()
    {
        StartCoroutine(BlinkRoutine());
    }

    private void OnDisable()
    {
        StopCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            float timer = 0f;
            while (timer < blinkEvery /2)
            {
                text.color = Color.Lerp(startColor, endColor, timer / (blinkEvery /2));
                timer += Time.deltaTime;
                yield return null;
            }
            timer = 0f;
            while (timer < blinkEvery / 2)
            {
                text.color = Color.Lerp(endColor, startColor, timer / (blinkEvery / 2));
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}
