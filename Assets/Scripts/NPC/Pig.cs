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


    private void RandomAction()     //무작위 행동 실행 함수
    {

        int random = Random.Range(0, 4);    //애니메이션 인덱스(랜덤값), 대기, 풀뜯기, 경계, 걷기
        RandomSound();                      //액션 시 사운드 재생

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

    private void Wait()     //기다리기 행동 실행 함수
    {
        currentTime = waitTime;
        Debug.Log("대기");
    }
    private void Eat()      //먹기 행동 실행 함수
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("풀 뜯기");

    }
    private void Peek()     //경계하기 행동 실행 함수
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("경계");
    }
}
