using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private float waterDrag;   //물속에서의 중력
    private float originDrag;                   //원래 중력

    [SerializeField] private Color waterColor;          //물속의 색깔
    [SerializeField] private float waterFogDensity;     //물속 탁한 정도

    private Color originColor;                          //물바깥으로 나왔을때 색상 초기화
    private float originFogDensity;                     //물바깥으로 나왔을때 안개밀도 초기화

    //밤일때의 안개 설정
    [SerializeField] private Color waterNightColor;     //밤일때의 물속 색깔
    [SerializeField] private float waterNightFogDensity; //밤일때의 물속 탁한 정도
    [SerializeField] private Color originNightColor;
    [SerializeField] private float originNightFogDensity;

    //사운드 관련
    [SerializeField] private string sound_WaterOut;
    [SerializeField] private string sound_WaterIn;
    [SerializeField] private string Sound_WaterBreath;

    [SerializeField] private float breathTime;
    private float currentBreathTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = 0;

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
            SoundManager.instance.PlaySE(sound_WaterOut);

            GameManager.isWater = false;
            player.transform.GetComponent<Rigidbody>().drag = originDrag;

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
}
