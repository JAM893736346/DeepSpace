using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyBar : PlayerHealth_BAr
{
    // Start is called before the first frame update
    protected override void SetPercentText()
    {
       PersentText.text = "Energy:"+Mathf.RoundToInt(targetFillAmount * 100f) + "%";
    }
    // Update is called once per frame
    void Update()
    {

    }
}
