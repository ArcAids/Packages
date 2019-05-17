using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < 2)
            GetComponent<Rigidbody>().AddForce(Vector3.up * 40);
    }
}
