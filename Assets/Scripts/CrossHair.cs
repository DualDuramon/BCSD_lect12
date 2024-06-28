using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [SerializeField]
    private Animator anim;      //크로스헤어의 애니메이션

    private float gunAccuracy;  //크로스헤어 상태에 따른 총의 정확도.

    [SerializeField]
    private GameObject go_CrossHairHud; //크로스헤어 비활성화를 위한 부모객체

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
        if (anim.GetBool("Walking")) gunAccuracy = 0.06f;   //걸을때
        else if (anim.GetBool("Crouching")) gunAccuracy = 0.015f;   //앉아있을때
        else if(theGunController.GetFineSightMode()) gunAccuracy = 0.001f;  //정조준 시
        else gunAccuracy = 0.035f;  //서있을 때

        return gunAccuracy;
    }
}
