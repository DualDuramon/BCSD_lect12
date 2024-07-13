using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    private Vector3 originPos;      //원래 위치 값
    private Vector3 currentPos;     //현재 위치 값

    [SerializeField]
    private Vector3 limitPos;       //흔들림 최대 위치 값
    [SerializeField]
    private Vector3 fineSightLimitPos;  //정조준시 흔들림 최대 위치 값

    [SerializeField]
    private Vector3 smoothSway;     //부드러운 움직임 정도
    
    //필요한 컴포넌트
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

    private void Swaying()  //팔이 마우스 커서에 비해 천천히 따라오게 하는 효과 함수
    {
        float moveX = Input.GetAxisRaw("Mouse X");
        float moveY = Input.GetAxisRaw("Mouse Y");

        if (!theGunController.isFineSightMode)
        {//Lerp에 moveX를 음수로 주는 이유 : 플레이어 팔이 늦게 따라오게 하기 위해 반대로 가게 구현함.
         //그러면 뒤로 후퇴한 다음 다시 앞으로 감으로써 천천히 따라오는 효과를 줌.
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

    private void BackToOriginPos() //팔이 원래 위치로 돌아오게 하는 함수
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
