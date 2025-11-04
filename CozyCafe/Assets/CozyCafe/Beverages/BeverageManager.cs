using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeverageManager : MonoBehaviour
{
    [SerializeField] private GameObject createUI;

    [SerializeField] private Button createButton;
    [SerializeField] private GameObject cupPrefab;


    private void OnEnable()
    {
        createButton.onClick.AddListener(NewBeverage);
        createButton.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        createButton.onClick.RemoveListener(NewBeverage);
    }

    private void NewBeverage()
    {
        if (Beverage.ActiveDrink == null)
        {
            if (CharacterManager.Instance.HasOrdered)
            {
                CreateButton(false);
                GameObject newbev = Instantiate(cupPrefab, this.transform);
                newbev.transform.localScale = Vector3.one * 10;
            }
            else { Debug.Log("No Order active"); }
        }
        else { Debug.Log("Drink already active"); }
    }

    public void DeleteBeverage()
    {
        if (Beverage.ActiveDrink != null)
        {
            Beverage.ActiveDrink.TryGetFull();
            Destroy(Beverage.ActiveDrink.gameObject);
            CreateButton(true);
        }
    }

    public void CreateButton(bool b)
    {
        createButton.gameObject.SetActive(b);
    }
}
