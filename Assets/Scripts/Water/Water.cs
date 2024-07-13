using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{
    [SerializeField] private float waterDrag;   //���ӿ����� �߷�
    private float originDrag;                   //���� �߷�

    [SerializeField] private Color waterColor;          //������ ����
    [SerializeField] private float waterFogDensity;     //���� Ź�� ����

    private Color originColor;                          //���ٱ����� �������� ���� �ʱ�ȭ
    private float originFogDensity;                     //���ٱ����� �������� �Ȱ��е� �ʱ�ȭ

    //���϶��� �Ȱ� ����
    [SerializeField] private Color waterNightColor;     //���϶��� ���� ����
    [SerializeField] private float waterNightFogDensity; //���϶��� ���� Ź�� ����
    [SerializeField] private Color originNightColor;
    [SerializeField] private float originNightFogDensity;

    //���� ����
    [SerializeField] private string sound_WaterOut;
    [SerializeField] private string sound_WaterIn;
    [SerializeField] private string Sound_WaterBreath;

    //ȣ�� ����
    [SerializeField] private float breathTime;  //�ִ� ȣ�� Ÿ�̸�
    private float currentBreathTime = 0f;       //���� ȣ�� Ÿ�̸�
    [SerializeField] private float totalOxygen; //�ִ� ȣ��
    private float currentOxygen;                //���� ���� ȣ��
    private float temp;

    [SerializeField] private GameObject go_BaseUI;
    [SerializeField] private Text text_totalOxygen;
    [SerializeField] private Text text_currentOxygen;
    [SerializeField] private Image image_OxygenGuage;

    //�ʿ� ������Ʈ
    private StatusController thePlayerStat;

    // Start is called before the first frame update
    void Start()
    {
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;
        originDrag = 0;

        thePlayerStat = FindObjectOfType<StatusController>();
        currentOxygen = totalOxygen;
        text_totalOxygen.text = totalOxygen.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isWater)
        {
            currentBreathTime += Time.deltaTime;

            if(currentBreathTime >= breathTime)
            {
                SoundManager.instance.PlaySE(Sound_WaterBreath);
                currentBreathTime = 0f;
            }
            DecreaseOxygen();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetWater(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }

    private void GetWater(Collider player)
    {
        SoundManager.instance.PlaySE(sound_WaterIn);

        go_BaseUI.SetActive(true);
        GameManager.isWater = true;
        player.transform.GetComponent<Rigidbody>().drag = waterDrag;

        if (!GameManager.isNight)
        {
            RenderSettings.fogColor = waterColor;
            RenderSettings.fogDensity = waterFogDensity;
        }
        else
        {
            RenderSettings.fogColor = waterNightColor;
            RenderSettings.fogDensity = waterNightFogDensity;
        }
    }

    private void GetOutWater(Collider player)
    {
        if (GameManager.isWater)
        {
            go_BaseUI.SetActive(false);
            SoundManager.instance.PlaySE(sound_WaterOut);

            currentOxygen = totalOxygen;
            GameManager.isWater = false;
            player.transform.GetComponent<Rigidbody>().drag = originDrag;
            player.transform.GetComponent<PlayerController>().ResetSpeed();
                
            if (!GameManager.isNight)
            {
                RenderSettings.fogColor = originColor;
                RenderSettings.fogDensity = originFogDensity;
            }
            else
            {
                RenderSettings.fogColor = originNightColor;
                RenderSettings.fogDensity = originNightFogDensity;
            }
        }
        
    }

    private void DecreaseOxygen()
    {
        if (GameManager.isWater)
        {
            currentOxygen -= Time.deltaTime;
            text_currentOxygen.text = Mathf.RoundToInt(currentOxygen).ToString();
            image_OxygenGuage.fillAmount = currentOxygen / totalOxygen;

            if(currentOxygen <= 0)
            {
                temp += Time.deltaTime;
                if (temp >= 1)
                {
                    thePlayerStat.DecreaseHp(1);
                    temp = 0;
                }
                currentOxygen = 0;
            }
        }
    }
}
