using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : NetworkBehaviour
{
    [SerializeField] int waveNumber;
    [SerializeField] List<GameObject> players;

    public void Start()
    {
        players = new List<GameObject>();
        waveNumber = 0;
    }

    public void playerJoined(GameObject player)
    {
        players.Add(player);
    }
    

    public void waveComplete()
    {
        waveNumber++;
        // start timer for next wave... 10 sec
        // update wave number for all player HUDs
    }

}
