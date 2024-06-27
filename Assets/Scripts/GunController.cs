using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun; //���� �����ϰ� �ִ� ��

    private float currentFireRate;  //���� ���� ��Ÿ��
    private bool isReload = false;
    private bool isFineSightMode = false;  //�����ظ�� ����

    [SerializeField]
    private Vector3 originPos;  //������ �� �ѱ� ���� ��ġ

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
        TryFineSight(); //������ �õ�
    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;  //1�ʿ� 1 ���ҽ�Ŵ.

        }
    }

    private void TryFire()  //��ݽõ� �Լ�
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

    IEnumerator ReloadCoroutine()   //������ �ڷ�ƾ �Լ�
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount; //źâ ����
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if(currentGun.carryBulletCount >= currentGun.reloadBulletCount) //���� ����ִ� ź���� �������� ź�ຸ�� ���� ��
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
            Debug.Log("������ ź�� ���̻� �����ϴ�.");
        }
    }

    private void Shoot()
    {
        currentGun.currentBulletCount--;        //���� �Ѿ� ����
        currentFireRate = currentGun.fireRate;  //�߻� �� ���� ��Ÿ�� �ʱ�ȭ
        currentGun.muzzleFlash.Play();          //�ѱ� ���� Ȱ��ȭ 
        PlaySE(currentGun.fireSound);           //�� �߻� �Ҹ� ���

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine()); //�ݵ� �ڷ�ƾ ����
        Debug.Log("�Ѿ� �߻�!");

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

    private void FineSight()    //�������Լ�
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);

        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else //������ �ƴ� ��
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeActivateCoroutine());
        }
    }

    IEnumerator FineSightActivateCoroutine()
    {
        while (currentGun.transform.localPosition != currentGun.FineSightOriginPos)
        {   //currentGun�� ������ ��ġ�� �� �� ���� �ݺ�.
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.FineSightOriginPos, 0.2f);
            yield return null;  //1������ ���
        }
    }
    IEnumerator FineSightDeActivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {   //currentGun�� ���� ��ġ�� �� �� ���� �ݺ�.
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;  //1������ ���
        }
    }

    IEnumerator RetroActionCoroutine()
    {
        //����ӽŰ� ���� 90�� ���ȱ� ������ Y���� �ƴ� X������ �ݵ��� ��.
        Vector3 recoilBack 
            = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack 
            = new Vector3(currentGun.retroActionFineSightForce, currentGun.FineSightOriginPos.y, currentGun.FineSightOriginPos.z);

        if (!isFineSightMode) //�Ϲ� ���ػ����� ��
        {
            currentGun.transform.localPosition = originPos;

            //�ݵ����۱���
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition 
                    = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            //����ġ
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition 
                    = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else  //������ ����
        {
            currentGun.transform.localPosition = currentGun.FineSightOriginPos;

            //�ݵ����۱���
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition
                    = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            //����ġ
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