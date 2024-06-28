using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : CloseWeaponController
{
    public static bool isActivate = false;   //Ȱ��ȭ ����

    void Update()
    {
        if (isActivate) TryAttack();
    }

    protected override IEnumerator HitCoroutine()
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

    public override void CloseWeaponChange(CloseWeapon closeWeapon)
    {
        isActivate = true;
        base.CloseWeaponChange(closeWeapon);
    }
}
