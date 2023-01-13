using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonSelect : MonoBehaviour
{
    public Selectable primaryButton;
    void OnEnable()
    {
        primaryButton.Select();
    }

    void Update()
    {
        
    }
}
