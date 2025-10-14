using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Sprite buttonNormalSprite;
    [SerializeField] private Sprite buttonPressedSprite;

    private AudioSource buttonSource;
    private Image buttonImage;

    private void Awake()
    {
        buttonImage = GetComponentInChildren<Image>();
        buttonSource = GetComponent<AudioSource>();
        buttonImage.sprite = buttonNormalSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            buttonSource.pitch = 1f;
            buttonSource.Play();
            buttonImage.sprite = buttonPressedSprite;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            buttonSource.pitch = 1.2f;
            buttonSource.Play();
            buttonImage.sprite = buttonNormalSprite;
        }
    }

}
