using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public UiMananger uiMananger;

    void OnEnable()
    {
        uiMananger.role += HidTitle;
        uiMananger.back += trunTitle;
    }
    private void OnDsetory()
    {
        uiMananger.role -= HidTitle;
        uiMananger.back -= trunTitle;

    }
    void HidTitle()
    {
        gameObject.SetActive(false);
    }
    void trunTitle()
    {
        gameObject.SetActive(true);
    }
}
