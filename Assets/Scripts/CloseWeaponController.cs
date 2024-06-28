using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour
{

    [SerializeField]
    protected CloseWeapon currentCloseWeapon;   //현재 장착된 Hand형 타입 무기

    //공격중
    protected bool isAttack = false;
    protected bool isSwing = false;
    protected RaycastHit hitInfo;     //공격시 Ray에 닿은 애들 정보 불러옴.


    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);   //팔 휘두르기 딜레이
        isSwing = true;

        StartCoroutine(HitCoroutine()); //공격활성화 시점

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        //전체 delay중 해당 모션만 시간만 고려한 후 나머지 딜레이 값.
        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;

        yield return null;
    }

    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
        {
            return true;
        }
        return false;
    }

    //가상함수
    public virtual void CloseWeaponChange(CloseWeapon closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentCloseWeapon = closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero; //혹시 모를 hand위치 초기화
        currentCloseWeapon.gameObject.SetActive(true);  //해당 Hand 오브젝트 활성화
    }
}
