using System;
using System.Collections;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
    }
    private void OnDisable()
    {
        //GameManager.Instance.FadeOut(img);
    }

}
