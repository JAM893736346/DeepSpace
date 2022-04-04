using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth_BAr : dynamichealth
{
    [SerializeField] protected Text PersentText;
    protected virtual void SetPercentText()
    {
        PersentText.text = "Blood:"+Mathf.RoundToInt(targetFillAmount * 100f) + "%";
    }
    public override void Initialize(float currentValue, float maxValue)
    {
        base.Initialize(currentValue, maxValue);
        SetPercentText();
    }
    protected override IEnumerator BuffFillingCoroutine(Image image)
    {
        SetPercentText();
        return base.BuffFillingCoroutine(image);
    }
}
