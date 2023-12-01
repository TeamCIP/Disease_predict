using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChange : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    [SerializeField] Image toggleBackground;
    [SerializeField] Color normalColor;
    [SerializeField] Color selectedColor;

    // Update is called once per frame
    void Update()
    {
        if(toggle.isOn)
        {
            toggleBackground.color = selectedColor;
        }
        else if(!toggle.isOn)
        {
            toggleBackground.color = normalColor;
        }
    }
}
