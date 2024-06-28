using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [SerializeField]
    private Animator anim;      //ũ�ν������ �ִϸ��̼�

    private float gunAccuracy;  //ũ�ν���� ���¿� ���� ���� ��Ȯ��.

    [SerializeField]
    private GameObject go_CrossHairHud; //ũ�ν���� ��Ȱ��ȭ�� ���� �θ�ü

    [SerializeField]
    private GunController theGunController;
    
    public void WalkingAnimation(bool flag)
    {
        WeaponManager.currentWeaponAnim.SetBool("Walk", flag);
        anim.SetBool("Walking", flag);
    }

    public void RunningAnimation(bool flag)
    {
        WeaponManager.currentWeaponAnim.SetBool("Run", flag);
        anim.SetBool("Running", flag);
    }

    public void JumpingAnimation(bool flag)
    {
        anim.SetBool("Running", flag);
    }

    public void CrouchingAnimation(bool flag)
    {
        anim.SetBool("Crouching", flag);
    }

    public void FineSightAnimation(bool flag)
    {
        anim.SetBool("FineSight", flag);
    }

    public void FireAnimation()
    {
        if (anim.GetBool("Walking")) anim.SetTrigger("WalkFire");
        else if (anim.GetBool("Crouching")) anim.SetTrigger("CrouchFire");
        else anim.SetTrigger("IdleFire");
    }

    public float GetAccuracy()
    {
        if (anim.GetBool("Walking")) gunAccuracy = 0.06f;   //������
        else if (anim.GetBool("Crouching")) gunAccuracy = 0.015f;   //�ɾ�������
        else if(theGunController.GetFineSightMode()) gunAccuracy = 0.001f;  //������ ��
        else gunAccuracy = 0.035f;  //������ ��

        return gunAccuracy;
    }
}
