using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static bool isChangeWeapon = false;  //���� �ߺ� ��ü ����.
 
    [SerializeField]
    private float changeWeaponDelayTime;        //���� ��ü ������
    [SerializeField]
    private float changeWeaponEndDelayTime;     //���� ��ü �Ϸ� ������

    public static Transform currentWeapon;      //���� ������ transform
    public static Animator currentWeaponAnim;   //���� ���� animator
    [SerializeField]
    private string currentWeaponType;           //���� ���� Ÿ��

    //���� ���� ������
    [SerializeField] 
    private Gun[] guns;
    [SerializeField] 
    private Hand[] hands;

    //���� ��ųʸ���
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();

    [SerializeField]
    private GunController theGunController;         //�ѱ��� ���� ��Ʈ�ѷ�
    [SerializeField]
    private HandController theHandController;       //Hand�� ���� ��Ʈ�ѷ�


    void Start()
    {
        //��ųʸ� �ʱ�ȭ : guns, hands
        for (int i = 0; i < guns.Length; i++)
            gunDictionary.Add(guns[i].gunName, guns[i]);
        for (int i = 0; i < hands.Length; i++)
            handDictionary.Add(hands[i].handName, hands[i]);
    }

    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //���ⱳü ����(�Ǽ�)
                StartCoroutine(ChangeWeaponCoroutine("HAND", "�Ǽ�"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //���ⱳü ����(�Ǽ�)
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
        }   
    }

    //���� ��ü �ڷ�ƾ
    public IEnumerator ChangeWeaponCoroutine(string type, string name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");
        yield return new WaitForSeconds(changeWeaponDelayTime);

        CanclePreWeaponAction();
        WeaponeChange(type, name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = type;
        isChangeWeapon = false;
    }

    private void CanclePreWeaponAction()    //���� ���� �ൿ ��� �Լ�
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunController.CancleFineSight();
                theGunController.CancleReload();
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;
        }
    }
    
    private void WeaponeChange(string type, string name) //���� ��ü �Լ�
    {
        if (type == "GUN")
        {
            theGunController.GunChange(gunDictionary[name]);
        }
        else if(type == "HAND")
        {
            theHandController.HandChange(handDictionary[name]);
        }
    }
}
