using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Schema;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using static BeverageData;


// Enums
[System.Serializable]
public enum IngredientType
{
    Roast,
    Milk,
    Flavor
}

public class Ingredient : MonoBehaviour
{
    [SerializeField] IngredientType ingredientType;
    [SerializeField] IngredientData ingredientinfos;
    [SerializeField] private GameObject submenuPrefab;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform backViewParent;

    private Button interactButton;
    private GameObject submenuInstance;

    private RectTransform submenuRect;
    private RectTransform ingredientRect;
    private Canvas rootCanvas;


    void Awake()
    {
        interactButton = GetComponentInChildren<Button>();
        ingredientRect = GetComponent<RectTransform>();
        rootCanvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        CreateSubmenu();
        submenuInstance.SetActive(false); // hide it initially

    }

    private void OnEnable()
    {
        interactButton.onClick.AddListener(ToggleSubmenu);
    }

    private void OnDisable()
    {
        interactButton.onClick.RemoveListener(ToggleSubmenu);
    }

    private void ToggleSubmenu()
    {
        submenuInstance.SetActive(!submenuInstance.activeSelf);
    }

    private void CreateSubmenu()
    {
        submenuInstance = Instantiate(submenuPrefab, this.gameObject.transform);
        submenuInstance.name = $"{ingredientType} Submenu";

        // Reposition?
        //submenuRect = submenuInstance.GetComponent<RectTransform>();
        //RectTransform thisRect = this.GetComponent<RectTransform>();
        //float rectHeight = thisRect.rect.height;
        //Vector3 newpos = new Vector3(thisRect.rect.position.x, (thisRect.rect.position.y + rectHeight), 0);
        //submenuRect.transform.localPosition = new Vector3(newpos.x, 0, 0);


        Type enumType = ingredientType switch
        {
            IngredientType.Roast => typeof(RoastType),
            IngredientType.Milk => typeof(MilkAmount),
            IngredientType.Flavor => typeof(ExtraFlavor),
            _ => null
        };

        foreach (var value in Enum.GetValues(enumType))
        {
            GameObject btnObj = Instantiate(buttonPrefab, submenuInstance.transform);
            btnObj.name = value.ToString();

            TMP_Text label = btnObj.GetComponentInChildren<TMP_Text>();
            string formattedText = Regex.Replace(value.ToString(), "(?<!^)([A-Z])", " $1");
            label.text = formattedText;

            Button btn = btnObj.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => OnOptionSelected(value));
        }

        submenuInstance.SetActive(false);
    }

    private void OnOptionSelected(object value)
    {
        if (Beverage.ActiveDrink == null) return; // no active cup

        switch (ingredientType)
        {
            case IngredientType.Roast:
                if (!Beverage.ActiveDrink.BeverageData.HasType)
                {
                    Beverage.ActiveDrink.SelectRoast((RoastType)value);
                    //Debug.Log("Added Roasttype: " + Enum.GetName(typeof(RoastType), value));
                    ToggleSubmenu();
                }
                else { Debug.Log("Already added Roast"); }
                break;
            case IngredientType.Milk:
                if (!Beverage.ActiveDrink.BeverageData.HasMilk)
                {
                    Beverage.ActiveDrink.SelectMilk((MilkAmount)value);
                    //Debug.Log("Added Milk: " + Enum.GetName(typeof(MilkAmount), value));
                    ToggleSubmenu();
                }
                else { Debug.Log("Already added Milk"); }

                break;
            case IngredientType.Flavor:
                if (!Beverage.ActiveDrink.BeverageData.HasFlavor)
                {
                    Beverage.ActiveDrink.SelectFlavor((ExtraFlavor)value);
                    //Debug.Log("Added Flavor: " + Enum.GetName(typeof(ExtraFlavor), value));
                    ToggleSubmenu();
                }
                else { Debug.Log("Already added Flavor"); }

                break;
        }
    }

}
