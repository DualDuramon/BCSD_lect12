using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField]
    private float sec_Per_RealTimeSec;  //시간-현실시간 비율

    private bool isNight = false;

    //안개 밀도 관련
    [SerializeField] private float fogDensityCalc;      //증감량 비율
    [SerializeField] private float nightFogDensity;     //밤 안개 밀도
    private float dayFogDensity;                        //낮 안개 밀도
    private float currentFogDensity;                    //현재 안개 밀도

    // Start is called before the first frame update
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * sec_Per_RealTimeSec * Time.deltaTime);

        if (transform.eulerAngles.x >= 170) //태양의 rotation의 x값이 170 이상
        {
            isNight = true;
        }
        else if (transform.eulerAngles.x >= 340)
        {
            isNight = false;
        }

        if (isNight)
        {
            if(currentFogDensity <= nightFogDensity)
            {
                //밤일경우 일정한 속도로 안개밀도 증가
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if (currentFogDensity >= dayFogDensity)
            {
                //밤일경우 일정한 속도로 안개밀도 증가
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}
