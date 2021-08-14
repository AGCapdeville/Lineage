using Mirror;
using UnityEngine;

public class Hammer : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {
            Debug.Log("hammer on Collison with: " + other.gameObject.tag);
            //other.gameObject.SetActive(false);
            other.GetComponent<EnemyProperties>().isHit = true;
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.velocity = other.transform.forward * -10.0f;
            other.gameObject.GetComponent<EnemyProperties>().health -= 1;
        }

    }
}
