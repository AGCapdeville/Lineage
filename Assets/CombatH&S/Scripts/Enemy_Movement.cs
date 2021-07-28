using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Basic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_Movement : NetworkBehaviour
{
    int MoveSpeed = 4;
    int MaxDist = 25;
    int MinDist = 5;

    public int health = 2;

    public Rigidbody rb;
    [SerializeField] public float jumpForce = 6.0f;
    [SerializeField] public float jumpImpulse = .5f;
    [SerializeField] public float gravity = -9.8f;
    [SerializeField] private float speedVert;
    [SerializeField] private bool isGrounded;

    void Start()
    {
        speedVert = 0.0f; //enemy starts at rest
        isGrounded = true;
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Enemy on Collison with: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "ground")
        {
            isGrounded = true;
            Debug.Log("grounded");
        }
    }

    void Update()
    {

        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");

        if (Vector3.Distance(transform.position, gos[0].transform.position) >= MinDist && Vector3.Distance(transform.position, gos[0].transform.position) < MaxDist)
        {
            transform.LookAt(gos[0].transform.position);
            
            if (isGrounded && Random.value >= 0.999) //jump
            {
                Debug.Log("jumping");
                rb.velocity = Vector3.up * jumpForce;
                transform.position += Vector3.up * jumpImpulse;
                isGrounded = false;
            }
            else
            {
                Debug.Log(("falling"));
                transform.position += transform.forward * (MoveSpeed * Time.deltaTime);
            }

        }
        if (Vector3.Distance(transform.position, gos[0].transform.position) >= MaxDist)
        {
            transform.Rotate(transform.forward + new Vector3(Random.value, Random.value, Random.value)* Time.deltaTime);
        }
    }
}
