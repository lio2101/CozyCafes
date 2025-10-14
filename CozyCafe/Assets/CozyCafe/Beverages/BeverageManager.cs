using UnityEngine;
using UnityEngine.UI;

public class BeverageManager : MonoBehaviour
{
    [SerializeField] private Button[] createButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private GameObject cupPrefab;

    private void OnEnable()
    {
        foreach (var button in createButton)
        {
            button.onClick.AddListener(NewBeverage);
        }
        deleteButton.onClick.AddListener(DeleteBeverage);
    }

    private void OnDisable()
    {
        foreach (var button in createButton)
        {
            button.onClick.RemoveListener(NewBeverage);
        }
        deleteButton.onClick.RemoveListener(DeleteBeverage);
    }

    private void NewBeverage()
    {
        if (Beverage.ActiveDrink == null)
        {
            GameObject newbev = Instantiate(cupPrefab, this.transform);
            newbev.transform.localScale = Vector3.one * 10;
        }
        else { Debug.Log("Drink already active"); }
    }

    private void DeleteBeverage()
    {
        if (Beverage.ActiveDrink != null)
        {
            Destroy(Beverage.ActiveDrink.gameObject);
        }
    }
}
