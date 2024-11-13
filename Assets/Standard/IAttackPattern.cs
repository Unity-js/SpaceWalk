using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IAttackPattern

    {
        void Attack(GameObject projectile, Vector3 origin, Vector3 target); // 투사체 , 시작지점, 목표위치
    }

// 전략패턴을 활용하여 다양한 원거리 무기 공격 패턴을 만들어보세요.