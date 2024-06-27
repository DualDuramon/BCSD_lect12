using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    
    [SerializeField]
    private Hand currentHand;   //현재 장착된 Hand형 타입 무기

    //공격중
    private bool isAttack = false;
    private bool isSwing = false;
    private RaycastHit hitInfo;     //공격시 Ray에 닿은 애들 정보 불러옴.

    private void Update()
    {
        
        TryAttack();
    }

    private void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackDelay);   //팔 휘두르기 딜레이
        isSwing = true;

        StartCoroutine(HitCoroutine()); //공격활성화 시점

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        //전체 delay중 해당 모션만 시간만 고려한 후 나머지 딜레이 값.
        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        isAttack = false;

        yield return null;
    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false; //적중한게 있으면 더이상 피격판정 활성화x
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    private bool CheckObject()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range)){
            return true;
        }
        return false;
    }
}
