using System;
using UnityEngine;

[Serializable]
public enum RoastType
{
    Light, //light flavor, citric, high caffeine
    Medium, //balanced flavor, fruity, sweet
    MediumDark, //rich flavor, bitter-sweet, nutty
    Dark //bitter flavor, smoky, chocolaty
}
[Serializable]
public enum MilkAmount
{
    None,
    Low,
    Medium,
    High
}
[Serializable]
public enum ExtraFlavor
{
    None,
    Caramel,
    Cocoa,
    Vanilla
}
[Serializable]
public class BeverageData
{

    // Fields

    [SerializeField] private RoastType type;
    [SerializeField] private MilkAmount amount;
    [SerializeField] private ExtraFlavor flavor;

    private bool hasType;
    private bool hasMilk;
    private bool hasFlavor;

    //Properties

    public RoastType Roast { get { return type; } set { type = value; hasType = true; } }
    public MilkAmount Milk { get { return amount; } set { amount = value; hasMilk = true; } }
    public ExtraFlavor Flavor { get { return flavor; } set { flavor = value; hasFlavor = true; } }

    public bool HasType => hasType;
    public bool HasMilk => hasMilk;
    public bool HasFlavor => hasFlavor;
    public bool IsFull => hasType && hasMilk && hasFlavor;

    public void ResetData()
    {
        hasType = hasMilk = hasFlavor = false;
    }
}
