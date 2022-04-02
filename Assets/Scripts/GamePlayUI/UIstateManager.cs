using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIstateManager : MonoBehaviour
{
    // Start is called before the first frame update
    Canvas canvas;
    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    protected virtual void OnEnable()
    {
        GameManager.onPause += stateCanvesHid;
        GameManager.onunPause += stateCanvesturn;
    }
    protected virtual void OnDestroy()
    {
        GameManager.onPause -= stateCanvesHid;
        GameManager.onunPause -= stateCanvesturn;
    }
   protected void stateCanvesHid()
    {
        canvas.enabled = false;
    }
    protected void stateCanvesturn()
    {
        canvas.enabled = true;
    }

}
