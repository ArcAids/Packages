using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    float damage;

    public void Init(float damage, float force)
    {
        this.damage = damage;
        GetComponent<Rigidbody>().velocity=transform.forward *force;
        Destroy(gameObject,2);
    }

    private void OnTriggerEnter(Collider other)
    {
        ITakeDamage target = other.gameObject.GetComponent<ITakeDamage>();
        if (target!=null)
        {
            target.OnDamageTaken(damage);
            Destroy(gameObject);
        }
    }
}
