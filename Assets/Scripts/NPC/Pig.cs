using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pig : WeakAnimal
{
    protected override void ReSet()
    {
        base.ReSet();
        RandomAction();
    }


    private void RandomAction()     //������ �ൿ ���� �Լ�
    {

        int random = Random.Range(0, 4);    //�ִϸ��̼� �ε���(������), ���, Ǯ���, ���, �ȱ�
        RandomSound();                      //�׼� �� ���� ���

        if (random == 0)
        {
            Wait();
        }
        else if (random == 1)
        {
            Eat();
        }
        else if (random == 2)
        {
            Peek();
        }
        else if (random == 3)
        {
            TryWalk();
        }
    }

    private void Wait()     //��ٸ��� �ൿ ���� �Լ�
    {
        currentTime = waitTime;
        Debug.Log("���");
    }
    private void Eat()      //�Ա� �ൿ ���� �Լ�
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("Ǯ ���");

    }
    private void Peek()     //����ϱ� �ൿ ���� �Լ�
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("���");
    }
}
