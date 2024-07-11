using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{

    public void Run(Vector3 targetPos) //targetPos �ݴ�������� �޸����ϴ� �Լ�
    {
        direction = Quaternion.LookRotation(transform.position - targetPos).eulerAngles;    //�ݴ���� �ٶ󺸱�

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        anim.SetBool("Running", isRunning);
    }

    public override void Damage(int dmg, Vector3 targetPos) //�ǰ� ó�� �Լ�
    {
        base.Damage(dmg, targetPos);
        if (!isDead)    //���� �ʾ��� ���
        {
            Run(targetPos);
        }
    }
}
