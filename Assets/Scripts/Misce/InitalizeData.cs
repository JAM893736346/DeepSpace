using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitalizeData : MonoBehaviour
{
    [SerializeField] GameObject Player1;
    [SerializeField] GameObject Player2;
    private void Awake()
    {
        print(ScenesLoader.PlayerNumber);
        Player1.SetActive(false);
        Player2.SetActive(false);
    }
    private void OnEnable()
    {
        switch (ScenesLoader.PlayerNumber)
        {
            case 1:
                Player1.SetActive(true);
                Player2.SetActive(false);
                break;

            case 2:
                Player1.SetActive(false);
                Player2.SetActive(true);
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
