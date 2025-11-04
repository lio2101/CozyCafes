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

    [Header("Hover Info")]
    [SerializeField] string title;
    [TextArea(2, 10)][SerializeField] string info;
    [TextArea(2, 10)][SerializeField] string usage;

    private Vector2 offset = new Vector2(175, 50);
    private RectTransform canvasRect;
    private bool isHovering = false;
    private RectTransform hoverRect;
    private GameObject currentHoverObject;
    private static Canvas hoverCanvas;

    private void Awake()
    {
        if (hoverCanvas == null)
            hoverCanvas = GameObject.Find("HoverCanvas")?.GetComponent<Canvas>();
        canvasRect = hoverCanvas.GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentHoverObject != null)
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
        texts[0].text = title;
        texts[1].text = info;
        texts[2].text = usage;

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
