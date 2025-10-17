using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeverageManager : MonoBehaviour
{
    [SerializeField] private Button createButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private GameObject cupPrefab;


    private void OnEnable()
    {
        createButton.onClick.AddListener(NewBeverage);
        deleteButton.onClick.AddListener(DeleteBeverage);
        createButton.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        createButton.onClick.RemoveListener(NewBeverage);
        deleteButton.onClick.RemoveListener(DeleteBeverage);
    }

    private void NewBeverage()
    {
        if (Beverage.ActiveDrink == null)
        {
            if (CharacterManager.Instance.HasOrdered)
            {
                createButton.gameObject.SetActive(false);
                GameObject newbev = Instantiate(cupPrefab, this.transform);
                newbev.transform.localScale = Vector3.one * 10;
            }
            else { Debug.Log("No Order active"); }
        }
        else { Debug.Log("Drink already active"); }
    }

    private void DeleteBeverage()
    {
        if (Beverage.ActiveDrink != null)
        {
            deleteButton.GetComponent<AudioSource>().Play();
            Beverage.ActiveDrink.TryGetFull();
            Destroy(Beverage.ActiveDrink.gameObject);
            createButton.gameObject.SetActive(true);
        }
    }

    public void CreateButton(bool b)
    {
        createButton.gameObject.SetActive(b);
    }
}
