using Mirror;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemyCube"))
        {
            Debug.Log("hammer on Collison with: " + other.gameObject.tag);
            //other.gameObject.SetActive(false);
            other.GetComponent<Enemy_Mover>().health -= 1;
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.velocity = other.transform.forward * -50.0f;
            if (other.GetComponent<Enemy_Mover>().health <= 0)
            {
                other.gameObject.SetActive(false);
            }
        }

    }
}
