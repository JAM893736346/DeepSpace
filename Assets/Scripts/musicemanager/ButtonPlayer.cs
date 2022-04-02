using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPlayer : MonoBehaviour
{
    // Start is called before 
    [SerializeField] AudioData buttonmic;
    public void PlayButton() => AudioManager.Instance.PlayRandomSFX(buttonmic);

}
