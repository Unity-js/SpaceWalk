using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : IAttackPattern
{
    public float magicspeed = 10.0f;
    public void Attack(GameObject projectile, Vector3 origin, Vector3 target)
    {
        int numprojectiles = 10; // 투사체 개수 
        float checkangle = 360.0f / numprojectiles; // 투사체끼리의 각도

        for (int i = 0; i < numprojectiles; i++) // 투사체 개수만큼 
        {
            float angle = checkangle * i; // 각도 계산 

            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad),0, Mathf.Sin(angle * Mathf.Deg2Rad));  // 각도에 따라서 원형으로 퍼져야함, y축은 건드릴 필요 x 
            // 원의 원주를 따라 배치해야하고, 삼각함수를 이용했으므로 Radian으로 변환 (Deg2Rad)

            GameObject magicProjectile = GameObject.Instantiate(projectile, origin, Quaternion.identity); // 투사체 origin 위치에서 생성
            Rigidbody rb = magicProjectile.GetComponent<Rigidbody>();

            rb.velocity = dir * magicspeed; // 지정속도대로 퍼져나감


        }

        
    }
}

//    - 원 모양으로 퍼쳐나가는 마법 공격
