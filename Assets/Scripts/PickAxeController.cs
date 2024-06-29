using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAxeController : CloseWeaponController
{
    public static bool isActivate = true;   //Ȱ��ȭ ����

    void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

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
                if (hitInfo.transform.tag == "Rock")
                {
                    hitInfo.transform.GetComponent<Rock>().Mining();
                }
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
