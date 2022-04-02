using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    Toggle toggle1;
    Toggle toggle2;
    // Start is called before the first frame update
    private void Awake()
    {
        toggle1 = transform.GetChild(0).GetComponent<Toggle>();
        toggle2 = transform.GetChild(1).GetComponent<Toggle>();
    }
    private void Start()
    {
        toggle1.onValueChanged.AddListener((bool value) => { OnClicktoggle1(toggle1, value); });
        toggle2.onValueChanged.AddListener((bool value) => { OnClicktoggle2(toggle2, value); });
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (toggle1.isOn)
            {
                toggle1.isOn = false;
                toggle2.isOn = true;
            }
            else
            {
                toggle1.isOn = true;
                toggle2.isOn = false;

            }
        }
    }

    void OnClicktoggle1(Toggle toggle, bool value)
    {
        // Debug.Log("toggle1 change " + (value ? "On" : "Off"));
        if (value)
        {
            PlayerController.style = PlayerController.AttackStyle.melee;
        }
    }
    void OnClicktoggle2(Toggle toggle, bool value)
    {
        //Debug.Log("toggle2 change " + (value ? "On" : "Off"));
        if (value)
        {
            PlayerController.style = PlayerController.AttackStyle.Gun;
        }
    }
}

    