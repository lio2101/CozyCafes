using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float pageCooldown = 1f;
    [Header("Audio")]
    [SerializeField] private AudioClip openAudio;
    [SerializeField] private AudioClip pageAudio;

    [Header("Buttons")]
    [SerializeField] private Button bookButton;
    [SerializeField] private Button nextPage;
    [SerializeField] private Button beforePage;
    [Header("Components")]
    [SerializeField] private LineGenrator[] pages;
    [SerializeField] private BookOpenAnimation animationObject;
    [SerializeField] private Image img;
    [SerializeField] private Sprite[] animationPages;

    private AudioSource source;
    private int pageIndex;

    private Coroutine isTurning;
    private bool isOpen = false;

    public int PageIndex => pageIndex;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        bookButton.onClick.AddListener(OpenBook);
        nextPage.onClick.AddListener(() => Page(true));
        beforePage.onClick.AddListener(() => Page(false));
        this.gameObject.SetActive(false);
        pageIndex = 0;

        foreach (var page in pages)
        {
            page.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        isOpen = false;
        ToggleButtons(false);

    }

    private void OpenBook()
    {
        isOpen = !isOpen;
        this.gameObject.SetActive(isOpen);
        animationObject.gameObject.SetActive(isOpen);
        pages[pageIndex].gameObject.SetActive(true);


        if (!isOpen)
        {
            pages[pageIndex].ResetPen();
            pages[pageIndex].gameObject.SetActive(false);
            ToggleButtons(true);

        }
        else
        {
            source.clip = openAudio;
            source.Play();
        }
    }

    private void Page(bool right)
    {
        if (isTurning != null)
            return;
        if (right && pageIndex == pages.Length - 1)
            return;
        if (!right && pageIndex == 0)
            return;

        isTurning = StartCoroutine(TurnPage(right));
        source.clip = pageAudio;
        source.Play();
    }

    private IEnumerator TurnPage(bool right)
    {
        pages[pageIndex].gameObject.SetActive(false);

        beforePage.interactable = false;
        nextPage.interactable = false;
        int index = right ? 0 : animationPages.Length - 1;
        int i = 0;
        while (i < animationPages.Length)
        {
            img.sprite = animationPages[index];
            index = right ? index += 1 : index -= 1;
            yield return new WaitForSeconds(animationDuration / animationPages.Length);
            i++;
        }

        pageIndex = right ? pageIndex += 1 : pageIndex -= 1;
        pages[pageIndex].gameObject.SetActive(true);
        Debug.Log("Currently on page " + pageIndex);

        yield return new WaitForSeconds(pageCooldown);
        beforePage.interactable = true;
        nextPage.interactable = true;
        isTurning = null;
    }

    public void ToggleButtons(bool b)
    {
        nextPage.interactable = b;
        nextPage.GetComponent<Image>().raycastTarget = b;
        beforePage.interactable = b;
        beforePage.GetComponent<Image>().raycastTarget = b;
    }
}
