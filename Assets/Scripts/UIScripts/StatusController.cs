using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    //ü��
    [SerializeField]
    private int hp;
    private int currentHp;

    //���¹̳�
    [SerializeField]
    private int sp;
    private int currentSp;
    [SerializeField]
    private int spIncreaseSpeed;        //���¹̳� ������

    [SerializeField]
    private int spRecoveryDelay;        //���¹̳� ȸ�� ������
    private int currentSpRecoveryTime; //���� ���¹̳� ȸ�� Ÿ�̸�

    private bool spUsed;                //���¹̳� ���� ����

    //����
    [SerializeField]
    private int dp;
    private int currentDp;

    //�����
    [SerializeField]
    private int hungry;             //�⺻ ����� ��ġ
    private int currentHungry;      //���� ����� ��ġ

    //����� ���� Ÿ�̸�
    [SerializeField]
    private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    //�񸶸�
    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    //�񸶸� ���� Ÿ�̸�
    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    //������
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    //�������ͽ� �̹���
    [SerializeField]
    private Image[] images_Gauge;

    //�������ͽ� ����
    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;
    
    void Start()
    {
        //�� �������ͽ� �ʱ�ȭ
        currentHp = hp;
        currentDp = dp;
        currentSp = sp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    void Update()
    {
        SpRechargeTime();
        SpRecover();
        Hungry();
        Thirsty();
        GaugeUpdate();
    }

    private void SpRechargeTime() //���¹̳� ȸ�� �ð� ��� �Լ�
    {
        if (spUsed)
        {
            if(currentSpRecoveryTime < spRecoveryDelay)
            {
                currentSpRecoveryTime++;
            }
            else
            {
                spUsed = false;

            }
        }
    }

    private void SpRecover() //���¹̳� ȸ�� �Լ�
    {
        if(!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }

    private void Hungry() //����� ó�� �Լ�
    {
        if (currentHungry > 0)
        {
            if(currentHungryDecreaseTime <= hungryDecreaseTime)     //Ÿ�̸� ������� ����� ���ҽ�Ű��
            {
                currentHungryDecreaseTime++;
            }
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
        }
        else
        {
            Debug.Log("����� ��ġ�� 0�� �Ǿ����ϴ�.");
        }
    }

    private void Thirsty() //�񸶸� ó�� �Լ�
    {
        if(thirsty > 0)
        {
            if(currentThirstyDecreaseTime <= thirstyDecreaseTime)
            {
                currentThirstyDecreaseTime++;
            }
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
        {
            Debug.Log("�񸶸� ��ġ�� 0�� �Ǿ����ϴ�.");
        }
    }

    private void GaugeUpdate() //�������ͽ� UI ������Ʈ �Լ�
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    public void IncreaseHp(int amount)  //Hp ȸ��(����) �Լ�
    {
        if (currentHp + amount < hp) {
            currentHp += amount;
        }
        else {
            currentHp = hp;
        }
    }

    public void DecreaseHp(int amount)  //Hp ���� �Լ�
    {
        if(currentDp > 0) {
            DecreaseDp(amount);
            return;
        }

        currentHp -= amount;

        if (currentHp <= 0) {
            Debug.Log("ĳ������ Hp�� 0�� �Ǿ����ϴ�");
        }
    }

    public void IncreaseDp(int amount)  //Dp ȸ��(����) �Լ�
    {
        if (currentDp + amount < dp)
        {
            currentDp += amount;
        }
        else
        {
            currentDp = dp;
        }
    }

    public void DecreaseDp(int amount)  //Dp ���� �Լ�
    {
        currentDp -= amount;

        if (currentDp <= 0)
        {
            Debug.Log("ĳ������ ������ 0�� �Ǿ����ϴ�");
        }
    }

    public void IncreaseHungry(int amount)  //����� ȸ��(����) �Լ�
    {
        if (currentHungry + amount < hungry)
        {
            currentHungry += amount;
        }
        else
        {
            currentHungry = hungry;
        }
    }

    public void DecreaseHungry(int amount)  //����� ���� �Լ�
    {
        if(currentHungry - amount < 0)
        {
            currentHungry = 0;
        }
        else
        {
            currentHungry -= amount;
        }
    }

    public void IncreaseThirsty(int amount)  //�񸶸� ȸ��(����) �Լ�
    {
        if (currentThirsty + amount < thirsty)
        {
            currentThirsty += amount;
        }
        else
        {
            currentThirsty = hungry;
        }
    }

    public void DecreaseThirsty(int amount)  //�񸶸� ���� �Լ�
    {
        if (currentThirsty - amount < 0)
        {
            currentThirsty = 0;
        }
        else
        {
            currentThirsty -= amount;
        }
    }

    public void DecreaseStamina(int amount) //���¹̳� ���� �Լ�
    {
        spUsed = true;
        currentSpRecoveryTime = 0;

        if (currentSp - amount > 0) {
            currentSp -= amount;
        }
        else {
            currentSp = 0;
        }
    }

    public int GetCurrentSP()
    {
        return currentSp;
    }
}