using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName;     //�����̸�
    [SerializeField] private int hp;                //����ü��
    [SerializeField] private float walkSpeed;       //�ȱ�ӵ�
    [SerializeField] private float runSpeed;        //�޸���ӵ�
    private float applySpeed;                       //���� ����Ǵ� �ӵ� 

    private Vector3 direction;  //������� ����

    //���º���
    private bool isAction;      //���� �ൿ�� ����
    private bool isWalking;     //�ȱ⿩��
    private bool isRunning;     //�޸��� �� ����
    private bool isDead;        //��� ����


    [SerializeField] private float walkTime;     //�ȱ�ð�
    [SerializeField] private float waitTime;     //���ð�
    [SerializeField] private float runTime;      //�޸���ð�
    private float currentTime;                  //���� ��������ð�

    //�ʿ��� ������Ʈ
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider boxCol;

    //�Ҹ� ����
    private AudioSource theAudio;
    [SerializeField] private AudioClip[] pigNormalSounds;     //�ϻ� Pig ����
    [SerializeField] private AudioClip pigHurtSounds; //�ǰ� pig ����
    [SerializeField] private AudioClip pigDeadSounds; //��� pig ����




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

    private void ElapseTime()   //�ð� ��� �Լ�
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

    private void ReSet()
    {
        isWalking = false; isAction = true; isRunning = false;
        applySpeed = walkSpeed;
        anim.SetBool("Walking", isWalking); anim.SetBool("Running", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
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
    private void TryWalk()  //�ȱ� �ൿ ���� �Լ�.
    {
        isWalking = true; 
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        applySpeed = walkSpeed;
        Debug.Log("�ȱ�");

    }

    public void Run(Vector3 targetPos) //targetPos �ݴ�������� �޸����ϴ� �Լ�
    {
        direction = Quaternion.LookRotation(transform.position - targetPos).eulerAngles;    //�ݴ���� �ٶ󺸱�

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        anim.SetBool("Running", isRunning);

    }

    public void Damage(int dmg, Vector3 targetPos)  //�ǰ�ó�� �Լ�
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

    private void RandomSound()      //�ϻ� ���� ���� ��� �Լ�
    {
        int random = Random.Range(0, 3);    //3���� �ϻ� ����
        PlaySE(pigNormalSounds[random]);
    }

    private void PlaySE(AudioClip clip)     //���� ��� �Լ�
    {
        theAudio.clip = clip;
        theAudio.PlayOneShot(clip);

    }
}
