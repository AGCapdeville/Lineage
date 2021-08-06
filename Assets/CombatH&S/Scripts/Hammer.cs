using Mirror;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemyCube"))
        {
            other.gameObject.SetActive(false);
        }

    }
}
