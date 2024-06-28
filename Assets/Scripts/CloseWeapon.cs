using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName;     //근접무기 이름

    //무기 유형 bool변수
    public bool isHand;
    public bool isAxe;
    public bool isPickAxe;

    public float range;         //공격 범위
    public int damage;          //공격력
    public float workSpeed;     //작업 속도
    public float attackDelay;   //공격 딜레이
    public float attackDelayA;  //공격 판정 활성화까지의 딜레이
    public float attackDelayB;  //공격 판정 비활성화까지의 딜레이

    public Animator anim;

}
