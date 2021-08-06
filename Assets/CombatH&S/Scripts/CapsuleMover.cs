using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;


public class CapsuleMover : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position + transform.right * 5.0f, transform.up, 100 * Time.deltaTime);
    }
}
