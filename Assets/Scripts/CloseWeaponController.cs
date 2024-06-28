using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour
{

    [SerializeField]
    protected CloseWeapon currentCloseWeapon;   //���� ������ Hand�� Ÿ�� ����

    //������
    protected bool isAttack = false;
    protected bool isSwing = false;
    protected RaycastHit hitInfo;     //���ݽ� Ray�� ���� �ֵ� ���� �ҷ���.


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

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);   //�� �ֵθ��� ������
        isSwing = true;

        StartCoroutine(HitCoroutine()); //����Ȱ��ȭ ����

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        //��ü delay�� �ش� ��Ǹ� �ð��� ����� �� ������ ������ ��.
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

    //�����Լ�
    public virtual void CloseWeaponChange(CloseWeapon closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentCloseWeapon = closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero; //Ȥ�� �� hand��ġ �ʱ�ȭ
        currentCloseWeapon.gameObject.SetActive(true);  //�ش� Hand ������Ʈ Ȱ��ȭ
    }
}
