using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static bool isActivate = true;   //Ȱ��ȭ ����

    [SerializeField]
    private Hand currentHand;   //���� ������ Hand�� Ÿ�� ����

    //������
    private bool isAttack = false;
    private bool isSwing = false;
    private RaycastHit hitInfo;     //���ݽ� Ray�� ���� �ֵ� ���� �ҷ���.

    private void Update()
    {
        if (isActivate) TryAttack();
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

        yield return new WaitForSeconds(currentHand.attackDelay);   //�� �ֵθ��� ������
        isSwing = true;

        StartCoroutine(HitCoroutine()); //����Ȱ��ȭ ����

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        //��ü delay�� �ش� ��Ǹ� �ð��� ����� �� ������ ������ ��.
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
                isSwing = false; //�����Ѱ� ������ ���̻� �ǰ����� Ȱ��ȭx
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

    public void HandChange(Hand hand)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentHand = hand;
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentHand.anim;

        currentHand.transform.localPosition = Vector3.zero; //Ȥ�� �� hand��ġ �ʱ�ȭ
        currentHand.gameObject.SetActive(true);  //�ش� Hand ������Ʈ Ȱ��ȭ
        isActivate = true;
    }
}
