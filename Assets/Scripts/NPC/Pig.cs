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

    private Vector3 direction;

    //���º���
    private bool isAction;      //���� �ൿ�� ����
    private bool isWalking;     //�ȱ⿩��
    
    [SerializeField]private float walkTime;     //�ȱ�ð�
    [SerializeField]private float waitTime;     //���ð�
    private float currentTime;                  //���� ��������ð�

    //�ʿ��� ������Ʈ
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
        isWalking = false;
        isAction = true;
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        anim.SetBool("Walking", isWalking);
        RandomAction();
    }

    private void RandomAction()     //������ �ൿ ���� �Լ�
    {
        int random = Random.Range(0, 4);    //�ִϸ��̼� �ε���(������), ���, Ǯ���, ���, �ȱ�

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
        Debug.Log("���");
    }
    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("Ǯ ���");

    }
    private void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("���");

    }
    private void TryWalk()
    {
        currentTime = walkTime;
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        Debug.Log("�ȱ�");

    }
}
