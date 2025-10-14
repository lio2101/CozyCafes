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
    [SerializeField] private IngredientData info;
    [SerializeField] private Vector2 offset;

    private TMP_Text text;
    private RectTransform canvasRect;
    private int beanAmount;
    private bool isHovering = false;
    private RectTransform hoverRect;
    private GameObject currentHoverObject;
    private static Canvas hoverCanvas;
    private List<Image> beans;

    public int BeanAmount { get { return beanAmount; } set { beanAmount = value; } }

    private void Awake()
    {
        if (hoverCanvas == null)
            hoverCanvas = GameObject.Find("HoverCanvas")?.GetComponent<Canvas>();
        canvasRect = hoverCanvas.GetComponent<RectTransform>();
        text = this.GetComponentInChildren<TMP_Text>();
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
        

        //set text
        string thistext = text.text.Replace(" ", "");

        foreach (RoastInfo i in info.RoastInfos)
        {
            if (i.Type.ToString() == thistext)
            {
                SetHover(i.Info, i.BeanAmount);
            }
        }
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

    private void SetHover(string text, int beans)
    {
        currentHoverObject.GetComponentInChildren<TMP_Text>().text = text;
        int count = 0;
        foreach (Image img in currentHoverObject.GetComponentsInChildren<Image>())
        {
            img.gameObject.SetActive(false);
            if (count <= beans)
            {
                img.gameObject.SetActive(true);
                count++;
            }
        }

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
