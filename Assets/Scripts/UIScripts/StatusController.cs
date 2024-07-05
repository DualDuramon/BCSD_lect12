using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    //체력
    [SerializeField]
    private int hp;
    private int currentHp;

    //스태미너
    [SerializeField]
    private int sp;
    private int currentSp;
    [SerializeField]
    private int spIncreaseSpeed;        //스태미너 증가량

    [SerializeField]
    private int spRecoveryDelay;        //스태미너 회복 딜레이
    private int currentSpRecoveryTime; //현재 스태미너 회복 타이머

    private bool spUsed;                //스태미너 감소 여부

    //방어력
    [SerializeField]
    private int dp;
    private int currentDp;

    //배고픔
    [SerializeField]
    private int hungry;
    private int currentHungry;

    //배고픔 감소 타이머
    [SerializeField]
    private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    //목마름
    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    //목마름 감소 타이머
    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    //만족감
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    //스테이터스 이미지
    [SerializeField]
    private Image[] images_Gauge;

    //스테이터스 순서
    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;
    
    void Start()
    {
        //각 스테이터스 초기화
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

    private void SpRechargeTime() //스태미너 회복 시간 계산 함수
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

    private void SpRecover() //스태미너 회복 함수
    {
        if(!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }

    private void Hungry() //배고픔 처리 함수
    {
        if (currentHungry > 0)
        {
            if(currentHungryDecreaseTime <= hungryDecreaseTime)     //타이머 방식으로 배고픔 감소시키기
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
            Debug.Log("배고픔 수치가 0이 되었습니다.");
        }
    }

    private void Thirsty() //목마름 처리 함수
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
            Debug.Log("목마름 수치가 0이 되었습니다.");
        }
    }

    private void GaugeUpdate() //스테이터스 UI 업데이트 함수
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    public void DecreaseStamina(int amount) //스태미너 감소 함수
    {
        spUsed = true;
        currentSpRecoveryTime = 0;

        if (currentSp - amount > 0)
        {
            currentSp -= amount;
        }
        else
        {
            currentSp = 0;
        }
    }
}
