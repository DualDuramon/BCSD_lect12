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
    [SerializeField] private float runSpeed;        //달리기속도
    private float applySpeed;                       //현재 적용되는 속도 

    private Vector3 direction;  //진행방향 벡터

    //상태변수
    private bool isAction;      //현재 행동중 여부
    private bool isWalking;     //걷기여부
    private bool isRunning;     //달리는 중 여부
    private bool isDead;        //사망 여부


    [SerializeField] private float walkTime;     //걷기시간
    [SerializeField] private float waitTime;     //대기시간
    [SerializeField] private float runTime;      //달리기시간
    private float currentTime;                  //현재 상태진행시간

    //필요한 컴포넌트
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider boxCol;

    //소리 관련
    private AudioSource theAudio;
    [SerializeField] private AudioClip[] pigNormalSounds;     //일상 Pig 사운드
    [SerializeField] private AudioClip pigHurtSounds; //피격 pig 사운드
    [SerializeField] private AudioClip pigDeadSounds; //사망 pig 사운드




    private void Start()
    {
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;

    }

    private void Update()
    {
        if (!isDead)
        {
            Move();
            Rotation();
            ElapseTime();
        }
    }

    private void Move()
    {
        if (isWalking || isRunning)
        {
            rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
        }
    }

    private void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 rot = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.01f);
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
        isWalking = false; isAction = true; isRunning = false;
        applySpeed = walkSpeed;
        anim.SetBool("Walking", isWalking); anim.SetBool("Running", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
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
    private void TryWalk()  //걷기 행동 실행 함수.
    {
        isWalking = true; 
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        applySpeed = walkSpeed;
        Debug.Log("걷기");

    }

    public void Run(Vector3 targetPos) //targetPos 반대방향으로 달리게하는 함수
    {
        direction = Quaternion.LookRotation(transform.position - targetPos).eulerAngles;    //반대방향 바라보기

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        anim.SetBool("Running", isRunning);

    }

    public void Damage(int dmg, Vector3 targetPos)  //피격처리 함수
    {
        if (!isDead)
        {
            hp -= dmg;
            if (hp <= 0)
            {
                //Debug.Log("체력 0 이하");
                Dead();
                return;
            }

            PlaySE(pigHurtSounds);
            anim.SetTrigger("Hurt");
            Run(targetPos);
        }
    }

    private void Dead()
    {
        PlaySE(pigDeadSounds);
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");

    }

    private void RandomSound()      //일상 사운드 랜덤 재생 함수
    {
        int random = Random.Range(0, 3);    //3개의 일상 사운드
        PlaySE(pigNormalSounds[random]);
    }

    private void PlaySE(AudioClip clip)     //사운드 재생 함수
    {
        theAudio.clip = clip;
        theAudio.PlayOneShot(clip);

    }
}
