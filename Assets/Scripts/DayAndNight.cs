using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField]
    private float sec_Per_RealTimeSec;  //�ð�-���ǽð� ����

    //�Ȱ� �е� ����
    [SerializeField] private float fogDensityCalc;      //������ ����
    [SerializeField] private float nightFogDensity;     //�� �Ȱ� �е�
    [SerializeField]private float dayFogDensity;                        //�� �Ȱ� �е�
    [SerializeField] private float currentFogDensity;                    //���� �Ȱ� �е�

    // Start is called before the first frame update
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * sec_Per_RealTimeSec * Time.deltaTime);
        
        if (transform.eulerAngles.x >= 340)
        {
            GameManager.isNight = false;
        }
        else if (transform.eulerAngles.x >= 170) //�¾��� rotation�� x���� 170 �̻�
        {
            GameManager.isNight = true;
        }

        if (GameManager.isNight)
        {
            if(currentFogDensity <= nightFogDensity)
            {
                //���ϰ�� ������ �ӵ��� �Ȱ��е� ����
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if (currentFogDensity >= dayFogDensity)
            {
                //���ϰ�� ������ �ӵ��� �Ȱ��е� ����
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}
