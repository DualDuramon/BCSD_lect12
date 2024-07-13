using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true;        //플레이어의 움직임 제어
    public static bool isOpenInventory = false;     //인벤토리 활성화 여부
    public static bool isOpenCraftManual = false;   //건축메뉴창 활성화 여부

    public static bool isNight = false;     //밤낮 여부
    public static bool isWater = false;     //물속 여부

    public static bool isPause = false;     //일시정지

    private WeaponManager theWM;    //웨폰매니저
    private bool flag = false;      //웨폰매니저의 코루틴 실행 여부

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        theWM = FindObjectOfType<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpenInventory || isOpenCraftManual || isPause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canPlayerMove = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canPlayerMove = true;
        }

        if (isWater)
        {
            if (!flag)
            {
                StopAllCoroutines();
                theWM.StartCoroutine(theWM.WeaponInCoroutine());
                flag = true;
            }
        }
        else
        {
            if (flag)
            {
                flag = false;
                theWM.WeaponOut();

            }
        }
    }
}
