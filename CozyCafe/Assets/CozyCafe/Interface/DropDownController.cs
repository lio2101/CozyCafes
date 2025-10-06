using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropDownController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private List<string> options = new();

    private TMP_Dropdown dropdown;
    private int selectedIndex = 0;

    public delegate void ValueChangedEvent(int optionIndex);
    public event ValueChangedEvent ValueChanged;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    private void OnEnable()
    {
        dropdown.onValueChanged.AddListener(SetValue);
    }

    private void OnDisable()
    {
        dropdown.onValueChanged.RemoveListener(SetValue);
    }

    public void SetOptions(IEnumerable<string> newOptions)
    {
        options.Clear();
        selectedIndex = 0;
        options.AddRange(newOptions);
    }

    public void SetValue(int index)
    {
        if (options.Count == 0)
            return;

        selectedIndex = Mathf.Clamp(index, 0, options.Count - 1);
        label.text = options[selectedIndex].ToString();

        ValueChanged.Invoke(selectedIndex);
    }

}
