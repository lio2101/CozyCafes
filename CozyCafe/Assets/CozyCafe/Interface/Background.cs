using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    [SerializeField] private List<Image> images;
    [SerializeField] private float fadeDuration = 3.0f;


    int index;

    private void Start()
    {
        index = 0;
        foreach (Image image in images) { image.color = new Color(1, 1, 1, 0); }
        images[0].color = new Color(1, 1, 1, 1);
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
        StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {

        Image cur = images[index];
        Image next = images[(index + 1) % images.Count];

        // Ensure next starts invisible but above current
        next.color = new Color(1, 1, 1, 0);
        next.transform.SetAsLastSibling(); // move next on top

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            next.color = new Color(1, 1, 1, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        next.color = new Color(1, 1, 1, 1);
        cur.color = new Color(1, 1, 1, 0);

        index = (index + 1) % images.Count;
    }

}
