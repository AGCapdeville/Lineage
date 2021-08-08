using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BowScrip : NetworkBehaviour
{
    [SerializeField] public GameObject Arrow;
    [SerializeField] public float speed = 1000f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject ArrowContainer = Instantiate(Arrow, transform.position - transform.right, transform.rotation*Quaternion.Euler(0, -90, 10));
            Rigidbody arrowRigidbody = ArrowContainer.transform.GetComponent<Rigidbody>();
            arrowRigidbody.AddForce(-transform.right * speed);
        }
    }
}
