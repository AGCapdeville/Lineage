using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class innerTargetDirection : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Vector3.forward;
        Debug.DrawRay(this.transform.position, dir * 10, Color.blue);
    }
}
