using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;  //총의 이름
    public float range;     //사정거리
    public float accuracy;  //총의 정확도
    public float fireRate;  //연사속도
    public float reloadTime;    //재장전 속도

    public int damage;          //총의 데미지

    public int reloadBulletCount;   //탄창 총알 개수
    public int currentBulletCount;  //현재 탄창에 남아있는 총알 개수
    public int maxBulletCount;      //최대 소유 가능 총알 개수
    public int carryBulletCount;    //현재 소유하고 있는 총알 개수

    public float retroActionForce;             //반동 세기
    public float retroActionFineSightForce;    //정조준시 반동 세기

    public Vector3 FineSightOriginPos;          //정조준 시 위치

    public Animator anim;
    public ParticleSystem muzzleFlash;  //총구 섬광 이펙트

    public AudioClip fireSound;         //총성 사운드



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
