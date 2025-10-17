using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class HoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject hoverPrefab;
    [SerializeField] private Vector2 offset;

    private RectTransform canvasRect;
    private bool isHovering = false;
    private RectTransform hoverRect;
    private GameObject currentHoverObject;
    private static Canvas hoverCanvas;

    private string thisTitle;
    private string thisInfo;

    private void Awake()
    {
        if (hoverCanvas == null)
            hoverCanvas = GameObject.Find("HoverCanvas")?.GetComponent<Canvas>();
        canvasRect = hoverCanvas.GetComponent<RectTransform>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(currentHoverObject != null)
        {
            Destroy(currentHoverObject.gameObject);
            currentHoverObject = null;
        }

        isHovering = true;
        currentHoverObject = Instantiate(hoverPrefab, hoverCanvas.transform);
        hoverRect = currentHoverObject.GetComponent<RectTransform>();
        foreach (var g in currentHoverObject.GetComponentsInChildren<Graphic>())
            g.raycastTarget = false;

        TMP_Text[] texts = currentHoverObject.GetComponentsInChildren<TMP_Text>();
        texts[0].text = thisTitle;
        texts[1].text = thisInfo;

        StartCoroutine(FollowMouse());

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(currentHoverObject);
        currentHoverObject = null;
        isHovering = false;
    }

    private void OnDisable()
    {
        Destroy(currentHoverObject);
        currentHoverObject = null;
        isHovering = false;
    }

    public void SetHover(string title, string text)
    {
        thisTitle = title;
        thisInfo = text;
    }

    private IEnumerator FollowMouse()
    {
        while (isHovering)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                hoverRect.parent as RectTransform,
                Input.mousePosition,
                null,
                out pos);
            hoverRect.anchoredPosition = (pos * canvasRect.localScale.x + offset); // offset from cursor
            yield return null;
        }
    }
}
