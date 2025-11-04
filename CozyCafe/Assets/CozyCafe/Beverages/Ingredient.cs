using System;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// Enums
[System.Serializable]
public enum IngredientType
{
    LightRoast,
    MediumRoast,
    MediumDarkRoast,
    DarkRoast,
    Milk,
    Caramel,
    Cocoa,
    Vanilla
}

public class Ingredient : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] IngredientType ingredientType;
    [SerializeField] AudioClip pickupSound;
    [SerializeField] AudioClip ingredientSound;
    [SerializeField] private bool isPickupable;
    [SerializeField] private int pickupRange = 20;

    private Button interactButton;
    private HoverText hover;
    private AudioSource audioSource;
    private Image img;

    void Awake()
    {
        interactButton = GetComponentInChildren<Button>();
        hover = GetComponentInChildren<HoverText>();
        audioSource = GetComponent<AudioSource>();
        img = GetComponentInChildren<Image>();
    }

    private void OnEnable()
    {
        interactButton.onClick.AddListener(OnOptionSelected);
    }

    private void OnDisable()
    {
        interactButton.onClick.RemoveListener(OnOptionSelected);
    }

    private void OnOptionSelected()
    {
        if (Beverage.ActiveDrink == null) return; // no active cup

        audioSource.clip = ingredientSound;
        audioSource.Play();

        switch (ingredientType)
        {
            case IngredientType.LightRoast:

                break;
            case IngredientType.MediumRoast:

                break;
            case IngredientType.MediumDarkRoast:

                break;
            case IngredientType.DarkRoast:
                break;
            case IngredientType.Milk:
                Beverage.ActiveDrink.AddMilk();
                break;
            case IngredientType.Caramel:
                Beverage.ActiveDrink.AddFlavor(ExtraFlavor.Caramel);
                break;
            case IngredientType.Cocoa:
                Beverage.ActiveDrink.AddFlavor(ExtraFlavor.Cocoa);
                break;
            case IngredientType.Vanilla:
                Beverage.ActiveDrink.AddFlavor(ExtraFlavor.Vanilla);
                break;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //pick up
        img.GetComponent<RectTransform>().offsetMin = new Vector2(0, pickupRange);

        audioSource.clip = pickupSound;
        audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        img.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        //set down
    }
}
