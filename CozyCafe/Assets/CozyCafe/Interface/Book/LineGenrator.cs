using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LineGenrator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Color Buttons")]
    [SerializeField] private Button[] colorButtons;
    [SerializeField] private Button eraser;

    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject eraserPrefab;


    private Draw activeLine;
    private bool isInArea;
    private bool penActive;
    private bool eraserActive;

    private int currentColor;
    private int sortingOrder = 0;
    private Book book;

    private void Start()
    {
        book = GetComponentInParent<Book>();
        currentColor = -1;
        int i = 0;
        foreach (var button in colorButtons)
        {
            int index = i;
            button.onClick.AddListener(() => ChangeColor(index));
            i++;
        }
        eraser.onClick.AddListener(ToggleEraser);
    }

    private void OnEnable()
    {
        this.GetComponent<Image>().raycastTarget = true;
    }

    public void ResetPen()
    {
        foreach (Button b in colorButtons)
        {
            b.GetComponent<RectTransform>().localScale = Vector3.one;
            penActive = false;
            currentColor = -1;
        }
        eraser.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    private void Update()
    {
        if (penActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isInArea)
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    GameObject newLine = Instantiate(prefabs[currentColor], this.gameObject.transform);
                    activeLine = newLine.GetComponent<Draw>();

                    LineRenderer lr = newLine.GetComponent<LineRenderer>();
                    lr.sortingOrder = sortingOrder++;

                    activeLine.UpdateLine(mousePos);

                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                activeLine = null;
            }
            if (activeLine != null)
            {
                if (isInArea)
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    activeLine.UpdateLine(mousePos);
                }
            }

        }

        if (eraserActive && Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("col hit");
                Draw hitLine = hit.collider.GetComponentInParent<Draw>();
                if (hitLine != null && hitLine.transform.IsChildOf(transform))
                {
                    Debug.Log("col hit, erasing");
                    Destroy(hitLine.gameObject);
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isInArea = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isInArea = false;
    }

    private void ChangeColor(int index)
    {
        if(eraserActive)
            ToggleEraser();
        if (currentColor != index)
        {
            if (currentColor != -1)
                colorButtons[currentColor].GetComponent<RectTransform>().localScale = Vector3.one;
            currentColor = index;
            penActive = true;
            colorButtons[currentColor].GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
        else
        {
            colorButtons[currentColor].GetComponent<RectTransform>().localScale = Vector3.one;
            penActive = false;
            currentColor = -1;
        }
        book.ToggleButtons(!penActive);
        activeLine = null;
    }

    private void ToggleEraser()
    {
        if (eraserActive)
        {
            eraser.GetComponent<RectTransform>().localScale = Vector3.one;
            this.GetComponent<Image>().raycastTarget = true;
        }
        else
        {
            eraser.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
            penActive = false;
            if (currentColor != -1)
                colorButtons[currentColor].GetComponent<RectTransform>().localScale = Vector3.one;
            currentColor = -1;
            this.GetComponent<Image>().raycastTarget = false;

        }
        eraserActive = !eraserActive;
        book.ToggleButtons(!eraserActive);
        Debug.Log("erasing " + eraserActive);
    }
}
