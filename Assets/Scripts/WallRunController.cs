using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WallRunController : MonoBehaviour
{
    [SerializeField]
    CharacterController controller;
    [SerializeField]
    Transform camTransform;

    UnityStandardAssets.Characters.FirstPerson.MouseLook mouseLook = new UnityStandardAssets.Characters.FirstPerson.MouseLook();

    [SerializeField]
    float speed=1;
    [SerializeField]
    float jumpForce;
    [SerializeField]
    float gravityMultiplier=1;

    Vector3 directionInput;
    float gravity;
    float verticalVelocity;
    float lowGravity;

    bool canJump=false;

    // Start is called before the first frame update
    void Start()
    {
        mouseLook.Init(transform,camTransform);
        lowGravity = gravityMultiplier / 10;
        gravity = gravityMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        directionInput = camTransform.right* Input.GetAxis("Horizontal") + camTransform.forward* Input.GetAxis("Vertical");
        directionInput = Vector3.ProjectOnPlane(directionInput,Vector3.up).normalized;
        directionInput.x *= speed * Time.deltaTime;
        directionInput.z *= speed * Time.deltaTime;
        
        verticalVelocity -= gravityMultiplier * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            verticalVelocity += jumpForce;
        }
        directionInput.y = verticalVelocity * Time.deltaTime;

        controller.Move( directionInput);
        mouseLook.LookRotation(transform,camTransform);

        CollisionFlags flags= controller.collisionFlags;

        if(flags == CollisionFlags.Below)
        {
            canJump = true;
            verticalVelocity = gravityMultiplier * Time.deltaTime;
            gravityMultiplier = gravity;
        }else if (flags == CollisionFlags.Sides)
        {
            gravityMultiplier = lowGravity;
        }
        else
        {
            canJump = false;
            gravityMultiplier = gravity;
        }

    }

}
