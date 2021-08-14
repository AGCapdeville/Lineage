using Mirror;
using UnityEngine;
public class BooAI : NetworkBehaviour
{
    public Rigidbody rb;
    public int health = 2;
    public float MoveSpeed = 4f;
    private bool playerNotLooking = false;
    public bool isHit;
    public float recoveryTime = 5f;
    public float dt = 0;

    void Start()
    {
        gameObject.GetComponent<EnemyProperties>().health = health;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        isHit = GetComponent<EnemyProperties>().isHit;
        if (isHit)
        {
            dt = recoveryTime;
            isHit = false;
        }

        dt -= Time.deltaTime;

        if (dt > 0)
        {
            transform.Rotate(transform.forward + new Vector3(Random.value, Random.value, Random.value)* Time.deltaTime);
        }
        else
        {
            rb.velocity = Vector3.zero;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                    transform.LookAt(players[0].transform.position);
                    
                    Vector3 facingVector = players[0].transform.forward + transform.forward;
            
                    if (facingVector.magnitude > 1.4f)
                    {
                        playerNotLooking = true;
                    }
                    else
                    {
                        playerNotLooking = false;
                    }
            
                    if (playerNotLooking)
                    {
                        transform.position += transform.forward * (MoveSpeed * Time.deltaTime);
                    }
        }
        
        

    }
}