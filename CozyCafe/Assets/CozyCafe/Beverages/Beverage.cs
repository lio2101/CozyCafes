using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static BeverageData;

public class Beverage : MonoBehaviour
{
    [SerializeField] private List<Sprite> cupSprites;
    [SerializeField] private List<Sprite> flavorSprites;
    [SerializeField] private Image flavorImg;

    public static Beverage ActiveDrink { get; private set; }

    private Image img;
    private int spriteIndex;
    private BeverageData newDrink;

    public BeverageData BeverageData => newDrink;

    private void Awake()
    {
        spriteIndex = 0;
        ActiveDrink = this;
        newDrink = new BeverageData();
        img = GetComponent<Image>();
        img.sprite = cupSprites[spriteIndex];
        img.color = new Color(1, 1, 1, 1);
        newDrink.Milk = MilkAmount.None;
        newDrink.Flavor = ExtraFlavor.None;

        flavorImg.color = new Color(1, 1, 1, 0);
    }

    private void OnDestroy()
    {
        if (ActiveDrink == this)
            ActiveDrink = null;
        CharacterManager.Instance.ToggleButton(false);
    }

    public void SelectRoast(RoastType roast) { newDrink.Roast = roast; TryGetFull(); }
    public void AddMilk()
    {
        if (newDrink.HasType)
        {
            if (newDrink.Milk < MilkAmount.High)
            {
                Debug.Log("Adding milk");
                newDrink.Milk++;
                TryGetFull();
                ChangeSprite();
            }
            else { Debug.Log("Max amount of Milk reached"); }
        }
        else { Debug.Log("no coffee in cup"); }
    }
    public void AddFlavor(ExtraFlavor flavor)
    {
        if (newDrink.Flavor != ExtraFlavor.None)
        {
            Debug.Log("Flavor already added");
            return;
        }

        if (newDrink.HasType)
        {
            Debug.Log("Adding flavor:" + flavor.ToString());
            newDrink.Flavor = flavor;
            TryGetFull();
            flavorImg.color = new Color(1, 1, 1, 1);
            switch (flavor)
            {
                case ExtraFlavor.Caramel:
                    flavorImg.sprite = flavorSprites[0];
                    break;
                case ExtraFlavor.Cocoa:
                    flavorImg.sprite = flavorSprites[1];
                    break;
                case ExtraFlavor.Vanilla:
                    flavorImg.sprite = flavorSprites[2];
                    break;
            }
        }
        else { Debug.Log("no coffee in cup"); }
    }

    public void TryGetFull()
    {
        if (ActiveDrink.BeverageData.IsFull && CharacterManager.Instance.HasOrdered)
        {
            CharacterManager.Instance.ToggleButton(true);
        }
    }
    public void ChangeSprite()
    {
        spriteIndex++;
        img.sprite = cupSprites[spriteIndex];
    }

}
