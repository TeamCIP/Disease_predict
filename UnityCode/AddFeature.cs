using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddFeature : MonoBehaviour
{
    [SerializeField] private FeatureSO Feature;
    [SerializeField] private TextMeshProUGUI featureText;
    [SerializeField] private Toggle featureToggle;

    private void Start()
    {
        featureText.text = Feature.featureName;
        featureToggle.isOn = false;
    }


    void Update()
    {
        if(featureToggle.isOn)
        {
            Feature.featureTrue = true;
        }
        if(!featureToggle.isOn)
        {
            Feature.featureTrue = false;
        }
    }
}
