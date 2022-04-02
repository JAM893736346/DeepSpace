using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //游戏结束事件
    public static System.Action onGameOver;
    public static System.Action onPause;
    public static System.Action onunPause;
    
    public static GameState GameState { get => Instance.gameState; set => Instance.gameState = value; }
    
    [SerializeField] GameState gameState = GameState.Playing;
}

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Scoring
}