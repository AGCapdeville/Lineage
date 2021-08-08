using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ArrowScrip : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemyCube"))
        {
            other.GetComponent<Enemy_Movement>().health -= 1;
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.velocity = other.transform.forward * -5.0f;
            if (other.GetComponent<Enemy_Movement>().health <= 0)
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}
