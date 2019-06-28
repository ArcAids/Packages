using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatorinator : MonoBehaviour
{
    [SerializeField]
    Vector3 rotation;
    private void Awake()
    {
        GetComponent<Rigidbody>().angularVelocity=rotation ;
    }
}
