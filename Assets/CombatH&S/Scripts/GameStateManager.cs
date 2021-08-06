using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    
    public int time;

    public void Awake()
    {
        if (Instance == null){
            Instance = this;
        }
    }
}
