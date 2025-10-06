using UnityEngine;

[CreateAssetMenu(fileName = "BeverageData", menuName = "Scriptable Objects/BeverageData")]
public class BeverageData : MonoBehaviour
{
    public enum RoastType
    {
        Light, //light flavor, citric, high caffeine
        Medium, //balanced flavor, fruity, sweet
        MediumDark, //rich flavor, bitter-sweet, nutty
        Dark //bitter flavor, smoky, chocolaty
    }

    public enum MilkAmount
    {
        None,
        Low,
        Medium,
        High
    }

    public enum ExtraFlavor
    {
        None,
        Caramel,
        Cocoa,
        CondensedMilk
    }

    // Fields

    [SerializeField] private RoastType type;
    [SerializeField] private MilkAmount amount;
    [SerializeField] private ExtraFlavor flavor;

    //Properties

    public RoastType Type => type;
    public MilkAmount Amount => amount;
    public ExtraFlavor Flavor => flavor;
}
