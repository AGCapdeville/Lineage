using Mirror;
using UnityEngine;

public class Spawner : NetworkBehaviour
{
    [SerializeField] public GameObject enemy;
    public float spawnRate = 15.0f;

    void Update()
    {
        spawnRate -= Time.deltaTime;

        if (spawnRate <= 0.0f)
        {
            timerEnded();
            
        }
    }

    void timerEnded()
    {
        spawnRate = 15.0f;
        spawn();
    }


    void spawn()
    {
        GameObject enemyInit = Instantiate(enemy, transform.position + transform.forward * 2, transform.rotation);
        NetworkServer.Spawn(enemyInit);
    }
}
