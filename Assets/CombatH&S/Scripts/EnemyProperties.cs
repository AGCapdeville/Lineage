using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EnemyProperties : NetworkBehaviour
{
    public int health = 0;
    public bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        isHit = false;
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
