using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    private float gunAccuracy;  //크로스헤어에 따른 총의 정확도.

    [SerializeField]
    private GameObject go_CrossHairHud; //크로스헤어 비활성화를 위한 부모객체


    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WalkingAnimation(bool flag)
    {
        anim.SetBool("Walking", flag);
    }

    public void RunningAnimation(bool flag)
    {
        anim.SetBool("Running", flag);
    }

    public void CrouchingAnimation(bool flag)
    {
        anim.SetBool("Crouching", flag);
    }

    public void FireAnimation()
    {
        if (anim.GetBool("Walking")) anim.SetTrigger("WalkFire");
        else if (anim.GetBool("Crouching")) anim.SetTrigger("CrouchFire");
        else anim.SetTrigger("IdleFire");
    }
}
