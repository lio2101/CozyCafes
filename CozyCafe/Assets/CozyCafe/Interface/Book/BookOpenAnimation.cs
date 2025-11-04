using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookOpenAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject page;
    [SerializeField] private float animationDuration;

    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
        this.gameObject.SetActive(false);
        page.SetActive(false);
    }
    private void OnEnable()
    {
        StartCoroutine(OpenRoutine());
    }

    private void OnDisable()
    {
        img.sprite = sprites[0];
        page.SetActive(false);
    }

    private IEnumerator OpenRoutine()
    {
        page.SetActive(false);
        int index = 0;
        while (index < sprites.Length)
        {
            img.sprite = sprites[index];
            index++;
            yield return new WaitForSeconds(animationDuration/sprites.Length);
        }
        img.sprite = sprites[sprites.Length-1];
        this.gameObject.SetActive(false);
        page.SetActive(true);
    }
}
