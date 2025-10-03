using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropDownController : Slider
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private List<string> options = new();
    private int selectedIndex = 0;

    public delegate void ValueChangedEvent(int optionIndex);
    public event ValueChangedEvent ValueChanged;

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
    }

}
