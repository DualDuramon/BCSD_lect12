using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    private Vector3 originPos;      //���� ��ġ ��
    private Vector3 currentPos;     //���� ��ġ ��

    [SerializeField]
    private Vector3 limitPos;       //��鸲 �ִ� ��ġ ��
    [SerializeField]
    private Vector3 fineSightLimitPos;  //�����ؽ� ��鸲 �ִ� ��ġ ��

    [SerializeField]
    private Vector3 smoothSway;     //�ε巯�� ������ ����
    
    //�ʿ��� ������Ʈ
    [SerializeField]
    private GunController theGunController;


    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.canPlayerMove)
        {
            TrySway();
        }
    }

    private void TrySway()
    {
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
        {
            Swaying();
        }
        else
        {
            BackToOriginPos();
        }
    }

    private void Swaying()  //���� ���콺 Ŀ���� ���� õõ�� ������� �ϴ� ȿ�� �Լ�
    {
        float moveX = Input.GetAxisRaw("Mouse X");
        float moveY = Input.GetAxisRaw("Mouse Y");

        if (!theGunController.isFineSightMode)
        {//Lerp�� moveX�� ������ �ִ� ���� : �÷��̾� ���� �ʰ� ������� �ϱ� ���� �ݴ�� ���� ������.
         //�׷��� �ڷ� ������ ���� �ٽ� ������ �����ν� õõ�� ������� ȿ���� ��.
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -moveX, smoothSway.x), -limitPos.x, limitPos.x),
                           Mathf.Clamp(Mathf.Lerp(currentPos.y, -moveY, smoothSway.x), -limitPos.y, limitPos.y),
                           originPos.z);
        }
        else
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
                           Mathf.Clamp(Mathf.Lerp(currentPos.y, -moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                           originPos.z);
        }

        transform.localPosition = currentPos;
    }

    private void BackToOriginPos() //���� ���� ��ġ�� ���ƿ��� �ϴ� �Լ�
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
