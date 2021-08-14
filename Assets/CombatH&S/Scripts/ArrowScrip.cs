using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ArrowScrip : NetworkBehaviour
{
    [SerializeField] public float expirationTime = 10f;
    private float elapsedTime = 0f;

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > expirationTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {
            other.GetComponent<EnemyProperties>().health -= 1;
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.velocity = other.transform.forward * -5.0f;
            gameObject.SetActive(false);
            other.GetComponent<EnemyProperties>().isHit = true;
        }
    }
}
