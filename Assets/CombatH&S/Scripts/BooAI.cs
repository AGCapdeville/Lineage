using UnityEngine;
public class BooAI : MonoBehaviour
{
    public int health = 2;
    public float MoveSpeed = 4f;
    private bool playerNotLooing = false;

    void Start()
    {

    }

    void Update()
    {

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        transform.LookAt(players[0].transform.position);


        Debug.DrawLine(transform.position, players[0].transform.position, Color.red);

        Vector3 facingVector = players[0].transform.forward + transform.forward;

        if (facingVector.magnitude > 1.4f)
        {
            playerNotLooing = true;
        }
        else
        {
            playerNotLooing = false;
        }

        if (playerNotLooing)
        {
            transform.position += transform.forward * (MoveSpeed * Time.deltaTime);
        }

    }
}