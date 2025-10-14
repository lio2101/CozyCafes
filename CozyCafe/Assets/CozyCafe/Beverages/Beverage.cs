using UnityEngine;
using static BeverageData;

public class Beverage : MonoBehaviour
{
    public static Beverage ActiveDrink { get; private set; }
    private BeverageData newDrink;

    public BeverageData BeverageData => newDrink;

    private void Awake()
    {
        ActiveDrink = this;
        newDrink = new BeverageData();
    }

    private void OnDestroy()
    {
        if (ActiveDrink == this)
            ActiveDrink = null;
    }

    public void SelectRoast(RoastType roast) { newDrink.Roast = roast; TryGetFull(); }
    public void SelectMilk(MilkAmount milk) { newDrink.Milk = milk; TryGetFull(); }
    public void SelectFlavor(ExtraFlavor flavor) { newDrink.Flavor = flavor; TryGetFull(); }

    public void TryGetFull()
    {
        if(ActiveDrink.BeverageData.IsFull && CharacterManager.Instance.HasOrdered)
        {
            CharacterManager.Instance.ToggleButton(true);
        }
    }

}
