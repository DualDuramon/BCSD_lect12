using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    private float gunAccuracy;  //ũ�ν��� ���� ���� ��Ȯ��.

    [SerializeField]
    private GameObject go_CrossHairHud; //ũ�ν���� ��Ȱ��ȭ�� ���� �θ�ü


    

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
