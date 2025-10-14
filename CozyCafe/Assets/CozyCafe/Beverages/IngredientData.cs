using System;
using UnityEditor;
using UnityEngine;

// Structs

[Serializable]
public struct RoastInfo
{
    [SerializeField] private RoastType type;
    [SerializeField] private int beanAmount;
    [TextArea(3, 10)][SerializeField] private string info;

    public RoastType Type => type;
    public int BeanAmount => beanAmount;
    public string Info => info;
}

[Serializable]
public struct MilkInfo
{
    [SerializeField] private MilkAmount type;
    [TextArea(3, 10)][SerializeField] private string info;

    public MilkAmount Type => type;
    public string Info => info;
}

[Serializable]
public struct FlavorInfo
{
    [SerializeField] private ExtraFlavor type;
    [TextArea(3, 10)][SerializeField] private string info;

    public ExtraFlavor Type => type;
    public string Info => info;
}

[CreateAssetMenu(fileName = "IngredientData", menuName = "Scriptable Objects/IngredientData")]
public class IngredientData : ScriptableObject
{
    [SerializeField] private RoastInfo[] roastinfos = new RoastInfo[Enum.GetValues(typeof(RoastType)).Length];
    //[SerializeField] private MilkInfo[] milkInfos = new MilkInfo[Enum.GetValues(typeof(MilkAmount)).Length];
    //[SerializeField] private FlavorInfo[] flavorInfo = new FlavorInfo[Enum.GetValues(typeof(ExtraFlavor)).Length];

    public RoastInfo[] RoastInfos => roastinfos;
    //public MilkInfo[] MilkInfos => milkInfos;
    //public FlavorInfo[] FlavorInfo => flavorInfo;
}
