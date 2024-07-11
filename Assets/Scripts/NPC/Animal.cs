using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] protected string animalName;     //�����̸�
    [SerializeField] protected int hp;                //����ü��
    [SerializeField] protected float walkSpeed;       //�ȱ�ӵ�
    [SerializeField] protected float runSpeed;        //�޸���ӵ�
    protected float applySpeed;                       //���� ����Ǵ� �ӵ� 

    protected Vector3 direction;  //������� ����

    //���º���
    protected bool isAction;      //���� �ൿ�� ����
    protected bool isWalking;     //�ȱ⿩��
    protected bool isRunning;     //�޸��� �� ����
    protected bool isDead;        //��� ����


    [SerializeField] protected float walkTime;          //�ȱ�ð�
    [SerializeField] protected float waitTime;          //���ð�
    [SerializeField] protected float runTime;           //�޸���ð�
    [SerializeField] protected float turnningSpeed;     //ȸ���ӵ�
    protected float currentTime;                        //���� ��������ð�

    //�ʿ��� ������Ʈ
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider boxCol;

    //�Ҹ� ����
    protected AudioSource theAudio;
    [SerializeField] protected AudioClip[] normalSounds;     //�ϻ� Animal ����
    [SerializeField] protected AudioClip hurtSounds; //�ǰ� Animal ����
    [SerializeField] protected AudioClip deadSounds; //��� Animal ����


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

    protected void Move()
    {
        if (isWalking || isRunning)
        {
            rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
        }
    }

    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 rot = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turnningSpeed);
            rigid.MoveRotation(Quaternion.Euler(rot));
        }
    }

    protected void ElapseTime()   //�ð� ��� �Լ�
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                //�ʱ�ȭ �� ���� ���� �ൿ ����
                ReSet();
            }
        }

    }

    protected virtual void ReSet()
    {
        isWalking = false; isAction = true; isRunning = false;
        applySpeed = walkSpeed;
        anim.SetBool("Walking", isWalking); anim.SetBool("Running", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
    }

    protected void TryWalk()  //�ȱ� �ൿ ���� �Լ�.
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        applySpeed = walkSpeed;
        Debug.Log("�ȱ�");

    }

    public virtual void Damage(int dmg, Vector3 targetPos)  //�ǰ�ó�� �Լ�
    {
        if (!isDead)
        {
            hp -= dmg;
            if (hp <= 0)
            {
                //Debug.Log("ü�� 0 ����");
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

    protected void RandomSound()      //�ϻ� ���� ���� ��� �Լ�
    {
        int random = Random.Range(0, 3);    //3���� �ϻ� ����
        PlaySE(normalSounds[random]);
    }

    protected void PlaySE(AudioClip clip)     //���� ��� �Լ�
    {
        theAudio.clip = clip;
        theAudio.PlayOneShot(clip);

    }
}
