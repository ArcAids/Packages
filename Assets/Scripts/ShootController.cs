using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    RaycastHit hit;
    GameObject hitObject;
    [SerializeField]
    LayerMask layerMask;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(Physics.Raycast(new Ray(cam.transform.position,cam.transform.forward),out hit,50,layerMask))
            {
                if(hitObject!=null)
                {
                    hitObject.transform.position = hit.point + hit.normal;
                    hitObject.SetActive(true);
                    hitObject = null;
                }
                else if (hit.collider.CompareTag("Movable"))
                {
                    hitObject = hit.collider.gameObject;
                    hitObject.SetActive(false);
                }
            }
        }
    }
}
