using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    LayerMask targetables;
    [SerializeField]
    GameObject followThis;
    [SerializeField]
    Camera cam;
    PlayerMovement playerMovement;

    RaycastHit hit;
    IPlayerMovementInput input;

    private void Start()
    {
        input = new PlayerInput();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        input.GetPlayerInput();

        playerMovement.Move(input.Vertical, input.Horizontal, input.Dash);
        if(followThis)
            playerMovement.LookToward(followThis.transform.position -transform.position);
        else
        {
            if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit,80,targetables))
                playerMovement.LookToward(hit.point- transform.position);
            else
                playerMovement.LookToward(transform.forward);

        }

    }
}
