using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun; //현재 소유하고 있는 총

    private float currentFireRate;  //남은 연사 쿨타임
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
            currentFireRate -= Time.deltaTime;  //1초에 1 감소시킴.

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
        currentFireRate = currentGun.fireRate;  //발사 후 연사 쿨타임 초기화
        Shoot();
    }

    private void Shoot()
    {
        currentGun.muzzleFlash.Play();  //총구 섬광 활성화 
        PlaySE(currentGun.fireSound);    //총 발사 소리 재생
        Debug.Log("총알 발사!");

    }

    private void PlaySE(AudioClip clip) 
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
