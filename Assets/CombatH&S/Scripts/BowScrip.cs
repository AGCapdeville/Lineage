using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BowScrip : NetworkBehaviour
{
    [SerializeField] public GameObject Arrow;
    [SerializeField] public float speed = 1000f;
    public bool canShoot = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot == true)
        {
            GameObject ArrowContainer = Instantiate(Arrow, transform.position, transform.rotation);
            Rigidbody arrowRigidbody = ArrowContainer.transform.GetComponent<Rigidbody>();
            arrowRigidbody.AddForce(transform.forward * speed);
        }
    }
    public void setCantShoot()
        {
            canShoot = false;
        }
    public void setCanShoot()
    {
        canShoot = true;
    }
}
