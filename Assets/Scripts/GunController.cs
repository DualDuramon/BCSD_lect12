using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun; //현재 소유하고 있는 총

    private float currentFireRate;  //남은 연사 쿨타임
    private bool isReload = false;
    private bool isFineSightMode = false;  //정조준모드 여부

    [SerializeField]
    private Vector3 originPos;  //정조준 전 총기 원래 위치

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight(); //정조준 시도
    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;  //1초에 1 감소시킴.

        }
    }

    private void TryFire()  //사격시도 함수
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0) 
            {
                Shoot();
            }
            else
            {
                CancleFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancleFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()   //재장전 코루틴 함수
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount; //탄창 비우기
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if(currentGun.carryBulletCount >= currentGun.reloadBulletCount) //현재 들고있는 탄약이 재장전할 탄약보다 많을 때
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            Debug.Log("소유한 탄이 더이상 없습니다.");
        }
    }

    private void Shoot()
    {
        currentGun.currentBulletCount--;        //남은 총알 감소
        currentFireRate = currentGun.fireRate;  //발사 후 연사 쿨타임 초기화
        currentGun.muzzleFlash.Play();          //총구 섬광 활성화 
        PlaySE(currentGun.fireSound);           //총 발사 소리 재생

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine()); //반동 코루틴 실행
        Debug.Log("총알 발사!");

    }

    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    public void CancleFineSight()
    {
        if (isFineSightMode)
        {
            FineSight();
        }
    }

    private void FineSight()    //정조준함수
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);

        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else //정조준 아닐 때
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeActivateCoroutine());
        }
    }

    IEnumerator FineSightActivateCoroutine()
    {
        while (currentGun.transform.localPosition != currentGun.FineSightOriginPos)
        {   //currentGun이 정조준 위치로 올 때 까지 반복.
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.FineSightOriginPos, 0.2f);
            yield return null;  //1프레임 대기
        }
    }
    IEnumerator FineSightDeActivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {   //currentGun이 원래 위치로 올 때 까지 반복.
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;  //1프레임 대기
        }
    }

    IEnumerator RetroActionCoroutine()
    {
        //서브머신건 팔을 90도 돌렸기 떄문에 Y축이 아닌 X축으로 반동을 줌.
        Vector3 recoilBack 
            = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack 
            = new Vector3(currentGun.retroActionFineSightForce, currentGun.FineSightOriginPos.y, currentGun.FineSightOriginPos.z);

        if (!isFineSightMode) //일반 조준상태일 때
        {
            currentGun.transform.localPosition = originPos;

            //반동시작구간
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition 
                    = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            //원위치
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition 
                    = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else  //정조준 상태
        {
            currentGun.transform.localPosition = currentGun.FineSightOriginPos;

            //반동시작구간
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition
                    = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            //원위치
            while (currentGun.transform.localPosition != currentGun.FineSightOriginPos)
            {
                currentGun.transform.localPosition
                    = Vector3.Lerp(currentGun.transform.localPosition, currentGun.FineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    private void PlaySE(AudioClip clip) 
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
