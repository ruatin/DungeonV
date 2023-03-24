using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    //적이 안으로 진입하면
    //작동해서 애니메이션 발동 및 적 공격
    //한번 작동후 일정시간 쿨타임

    public float attackCoolTime=3f;
    private float _attackTime;
    public float damage = 5;
    public bool onAttack = false;

    private bool _isAttackAble;

    private void Update()
    {
        if (_attackTime <= attackCoolTime)
        {
            _attackTime -= attackCoolTime;
            _isAttackAble = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && _isAttackAble)
        {
            _isAttackAble = false;
            other.GetComponent<Enemy>().Damaged(damage);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && _isAttackAble)
        {
            _isAttackAble = false;
            other.GetComponent<Enemy>().Damaged(damage);
        }
    }
    
    //적이 접촉하면 onAttack true 전환 되며 적이 나가면 onAttack false 전환
    //
}
