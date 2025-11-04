using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeleteDrinkButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Yes No")]
    [SerializeField] private Button yesb;
    [SerializeField] private Button nob;
    [Header("Components")]
    [SerializeField] private GameObject confirmMenu;
    [SerializeField] private Button[] toggleButtons;
    [SerializeField] private TMP_Text deleteText;
    [SerializeField] private BeverageManager bm;


    private Button button;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
        confirmMenu.SetActive(false);
        button.onClick.AddListener(ShowConfirm);
        nob.onClick.AddListener(HideConfirm);
        yesb.onClick.AddListener(OnDelete);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        deleteText.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        deleteText.color = Color.white;
    }

    private void ShowConfirm()
    {
        if (Beverage.ActiveDrink == null)
            return;
        confirmMenu.SetActive(true);

        foreach (var button in toggleButtons)
        {
            button.interactable = false;
        }

    }

    private void HideConfirm()
    {
        confirmMenu.SetActive(false);

        foreach (var button in toggleButtons)
        {
            button.interactable = true;
        }
    }

    private void OnDelete()
    {
        bm.DeleteBeverage();
        HideConfirm();
    }
}
