using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : IAttackPattern
{
    public float bulletspeed = 10.0f;
    public void Attack(GameObject projectile, Vector3 origin, Vector3 target)
    {
        GameObject bullet = GameObject.Instantiate(projectile, origin, Quaternion.identity); // 투사체를 origin 위치에서 생성

        Rigidbody rb = bullet.GetComponent<Rigidbody>(); // Rigidbody 컴포넌트로 물리적이동

        Vector3 dir = (target - origin).normalized; // target 위치로 벡터3 계산 > 정규화

        rb.velocity = dir * bulletspeed; // 지정된 속도로 직선이동
    }
}

// 직선으로 진행하는 총알