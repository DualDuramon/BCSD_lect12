using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun; //���� �����ϰ� �ִ� ��

    private float currentFireRate;  //���� ���� ��Ÿ��
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;  //1�ʿ� 1 ���ҽ�Ŵ.

        }
    }

    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }

    private void Fire()
    {
        currentFireRate = currentGun.fireRate;  //�߻� �� ���� ��Ÿ�� �ʱ�ȭ
        Shoot();
    }

    private void Shoot()
    {
        currentGun.muzzleFlash.Play();  //�ѱ� ���� Ȱ��ȭ 
        PlaySE(currentGun.fireSound);    //�� �߻� �Ҹ� ���
        Debug.Log("�Ѿ� �߻�!");

    }

    private void PlaySE(AudioClip clip) 
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
