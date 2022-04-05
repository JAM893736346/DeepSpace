using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlaneDown : MonoBehaviour
{
    Transform Victory;
    Transform Gameover;
    Canvas VictoryCanvas;
    Canvas GameoverCanvas;
    Transform game;
    Animator animator;
    Animator GameOverani;
    // Start is called before the first frame update
    private void OnEnable()
    {

        GameOverManager.Gameoverevent += PlayerAnimation;
        GameWinManager.GameWinevent += PlayerVictoryAniamtion;
    }
    private void OnDisable()
    {
        GameOverManager.Gameoverevent -= PlayerAnimation;
        GameWinManager.GameWinevent -= PlayerVictoryAniamtion;

    }
    void Start()
    {
        Victory = GameObject.Find("Victory").transform;
        Gameover = GameObject.Find("GameOver").transform;
        VictoryCanvas = Victory.GetComponent<Canvas>();
        GameoverCanvas = Gameover.GetComponent<Canvas>();
        animator = GameObject.Find("e").transform.GetComponent<Animator>();
        GameOverani = GameObject.Find("w").transform.GetComponent<Animator>();
    }

    void PlayerAnimation()
    {
        GameOverani.SetBool("IsMove", true);
    }
    void PlayerVictoryAniamtion()
    {
        animator.Play("VictoryDown");
    }


}
