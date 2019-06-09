using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownShooter : FirstPersonShooter
{
    [SerializeField]
    bool ignoreHeight;

    Plane playerShootingPlane;
    private new void Awake()
    {
        base.Awake();
        playerShootingPlane = new Plane(Vector3.up,gunHolder.position);
    }

    protected override void PointGunAtTarget()
    {
        Ray ray;

        ray = cam.ScreenPointToRay(Input.mousePosition);

        if (playerShootingPlane.Raycast(ray,out float point))
        {
            //gunHolder.LookAt(hit.point,Vector3.up);

            Vector3 gunPointDirection = ray.GetPoint(point);
            if (ignoreHeight)
                gunPointDirection.y = gunHolder.position.y;
            gunPointDirection = (gunPointDirection - gunHolder.position).normalized;
            gunHolder.rotation = Quaternion.Lerp(gunHolder.rotation, Quaternion.LookRotation(gunPointDirection, Vector3.up), Time.deltaTime * 6);
        }
    }
}
