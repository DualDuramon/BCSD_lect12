using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{

    public void Run(Vector3 targetPos) //targetPos 반대방향으로 달리게하는 함수
    {
        direction = Quaternion.LookRotation(transform.position - targetPos).eulerAngles;    //반대방향 바라보기

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        anim.SetBool("Running", isRunning);
    }

    public override void Damage(int dmg, Vector3 targetPos) //피격 처리 함수
    {
        base.Damage(dmg, targetPos);
        if (!isDead)    //죽지 않았을 경우
        {
            Run(targetPos);
        }
    }
}
