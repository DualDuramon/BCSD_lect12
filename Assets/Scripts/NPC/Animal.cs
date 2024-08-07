using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField] protected string animalName;     //동물이름
    [SerializeField] protected int hp;                //동물체력
    [SerializeField] protected float walkSpeed;       //걷기속도
    [SerializeField] protected float runSpeed;        //달리기속도

    protected Vector3 destination;  //목적지 방향 벡터

    //상태변수
    protected bool isAction;      //현재 행동중 여부
    protected bool isWalking;     //걷기여부
    protected bool isRunning;     //달리는 중 여부
    protected bool isDead;        //사망 여부


    [SerializeField] protected float walkTime;          //걷기시간
    [SerializeField] protected float waitTime;          //대기시간
    [SerializeField] protected float runTime;           //달리기시간
    protected float currentTime;                        //현재 상태진행시간

    //필요한 컴포넌트
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider boxCol;

    //소리 관련
    protected AudioSource theAudio;
    [SerializeField] protected AudioClip[] normalSounds;     //일상 Animal 사운드
    [SerializeField] protected AudioClip hurtSounds; //피격 Animal 사운드
    [SerializeField] protected AudioClip deadSounds; //사망 Animal 사운드

    //NavMeshAgent관련
    protected NavMeshAgent nav;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    private void Update()
    {
        if (!isDead)
        {
            Move();
            ElapseTime();
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
        {
            //rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
            nav.SetDestination(transform.position + destination * 5f);  //5배 거리되는 지점으로 도망 
        }
    }

    protected void ElapseTime()   //시간 경과 함수
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

    protected virtual void ReSet()
    {
        isWalking = false; isAction = true; isRunning = false;
        nav.speed = walkSpeed;
        nav.ResetPath();
        anim.SetBool("Walking", isWalking); anim.SetBool("Running", isRunning);
        destination.Set(Random.Range(-0.2f, 0.2f), 0f,  Random.Range(0.5f, 1f));
    }

    protected void TryWalk()  //걷기 행동 실행 함수.
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        nav.speed = walkSpeed;
        Debug.Log("걷기");

    }

    public virtual void Damage(int dmg, Vector3 targetPos)  //피격처리 함수
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

            PlaySE(hurtSounds);
            anim.SetTrigger("Hurt");
        }
    }

    protected void Dead()
    {
        PlaySE(deadSounds);
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");

    }

    protected void RandomSound()      //일상 사운드 랜덤 재생 함수
    {
        int random = Random.Range(0, 3);    //3개의 일상 사운드
        PlaySE(normalSounds[random]);
    }

    protected void PlaySE(AudioClip clip)     //사운드 재생 함수
    {
        theAudio.clip = clip;
        theAudio.PlayOneShot(clip);

    }
}
