using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundScroll : MonoBehaviour
{
    [SerializeField] Vector2 ScrollVirtel;
    Material material;
        // Start is called before the first frame update
    void Start()
    {
        material=GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset+=ScrollVirtel*Time.deltaTime;
    }
}
