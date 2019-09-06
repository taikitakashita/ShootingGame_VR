using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [SerializeField] private UnityEngine.UI.Text LogLabel;

    public void OnClick()
    {
        LogLabel.text = "onClick!";
    }

    public void OnCheckd(bool checkd)
    {
        Debug.Log("Takashita1 " + checkd);
        LogLabel.text = "chekd = " + checkd;
    }
    public void OnValueChenged(float value)
    {
        Debug.Log("Takashita2 " + value);
        LogLabel.text = "value = " + value.ToString();
    }
}