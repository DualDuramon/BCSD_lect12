using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName;     //동물이름
    [SerializeField] private int hp;                //동물체력
    [SerializeField] private float walkSpeed;       //걷기속도

    private Vector3 direction;

    //상태변수
    private bool isAction;      //현재 행동중 여부
    private bool isWalking;     //걷기여부
    
    [SerializeField]private float walkTime;     //걷기시간
    [SerializeField]private float waitTime;     //대기시간
    private float currentTime;                  //현재 상태진행시간

    //필요한 컴포넌트
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider boxCol;


    private void Start()
    {
        currentTime = waitTime;
        isAction = true;

    }

    private void Update()
    {
        Move();
        Rotation();
        ElapseTime();
    }

    private void Move()
    {
        if (isWalking)
        {
            rigid.MovePosition(transform.position + transform.forward * walkSpeed * Time.deltaTime);
        }
    }

    private void Rotation()
    {
        if (isWalking)
        {
            Vector3 rot = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(rot));
        }
    }

    private void ElapseTime()   //시간 경과 함수
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                //초기화 후 다음 랜덤 행동 개시
                ReSet();
            }
        }

    }

    private void ReSet()
    {
        isWalking = false;
        isAction = true;
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        anim.SetBool("Walking", isWalking);
        RandomAction();
    }

    private void RandomAction()     //무작위 행동 실행 함수
    {
        int random = Random.Range(0, 4);    //애니메이션 인덱스(랜덤값), 대기, 풀뜯기, 경계, 걷기

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

    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("대기");
    }
    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("풀 뜯기");

    }
    private void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("경계");

    }
    private void TryWalk()
    {
        currentTime = walkTime;
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        Debug.Log("걷기");

    }
}
