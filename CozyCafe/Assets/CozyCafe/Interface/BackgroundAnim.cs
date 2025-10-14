using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundAnim : MonoBehaviour
{
    [SerializeField] private List<Image> images;
    [SerializeField] private float fadeDuration = 5.0f;
    private Coroutine animRoutine;

    private void OnEnable()
    {
        foreach (Image image in images) { image.color = new Color(1, 1, 1, 0); }
        images[0].color = new Color(1, 1, 1, 1);
        animRoutine = StartCoroutine(AnimationRoutine());
    }

    private void OnDisable()
    {
        StopCoroutine(animRoutine);
        animRoutine = null;
    }

    private IEnumerator AnimationRoutine()
    {
        int index = 0;

        while (true)
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
}
