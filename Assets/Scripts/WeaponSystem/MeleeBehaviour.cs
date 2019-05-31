﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class MeleeBehaviour : WeaponBehaviour, IAttack
{
    [SerializeField]
    float damage;
    [SerializeField]
    float attackDelay;
    TrailRenderer trail;
    Animator animator;

    bool isAttacking=false;

    readonly int baseSwingHash = Animator.StringToHash("Swing");

    private void Start()
    {
        trail = GetComponent<TrailRenderer>();
        trail.emitting = false;
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        if(!isAttacking)
        {
            animator.SetTrigger(baseSwingHash);
            StartCoroutine(AttackWait());
        }
    }

    IEnumerator AttackWait()
    {
        isAttacking = true;
        trail.emitting = true;
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
        trail.emitting = false;
    }

    public override void Equip()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isAttacking)
        {
            ITakeDamage health=other.gameObject.GetComponent<ITakeDamage>();
            if (health != null)
                health.OnDamageTaken(damage);
        }
    }
}




