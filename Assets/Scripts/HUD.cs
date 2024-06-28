using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUD : MonoBehaviour
{
    //�ʿ��� ������Ʈ
    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;

    //�ʿ�� HUD Ȱ��ȭ / ��Ȱ��ȭ
    [SerializeField]
    private GameObject go_BulletHUD;

    //�Ѿ� ���� �ؽ�Ʈ
    [SerializeField]
    private Text[] text_Bullet;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckBullet();
    }

    //�Ѿ� ���� ������Ʈ �Լ� 
    private void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        text_Bullet[0].text = currentGun.carryBulletCount.ToString();
        text_Bullet[1].text = currentGun.reloadBulletCount.ToString();
        text_Bullet[2].text = currentGun.currentBulletCount.ToString();
    }
}
