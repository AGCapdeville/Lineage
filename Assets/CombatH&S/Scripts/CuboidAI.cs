using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuboidAI : MonoBehaviour
{
    public int health = 4;
    public float MoveSpeed = 3f;
    private bool playerNotLooking = false;
    private GameObject targetPlayer;
    private GameObject[] players = null;
    private Animator[] animators;

    private void Awake()
    {
        animators = GetComponentsInChildren<Animator>();
    }
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        targetPlayer = players[0];
        transform.LookAt(targetPlayer.transform.position);


        float dist = Vector3.Distance(targetPlayer.transform.position, transform.position);
        if (dist >= 5f)
        {
            transform.position += transform.forward * (MoveSpeed * Time.deltaTime);
        }
        else
        {
            //attack swing
            animators[1].SetBool("Attacking", attacking);
        }

        Debug.Log("Distance between player and Cuboid = " + dist);
}
}
