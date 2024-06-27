using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    
    [SerializeField]
    private Hand currentHand;   //���� ������ Hand�� Ÿ�� ����

    //������
    private bool isAttack = false;
    private bool isSwing = false;
    private RaycastHit hitInfo;     //���ݽ� Ray�� ���� �ֵ� ���� �ҷ���.

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
}
