using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{

    public void Run(Vector3 targetPos) //targetPos �ݴ�������� �޸����ϴ� �Լ�
    {
        destination 
            = new Vector3(transform.position.x - targetPos.x, 0f, transform.position.z - targetPos.z).normalized;

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        nav.speed = runSpeed;
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
