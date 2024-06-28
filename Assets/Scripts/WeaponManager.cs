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
    private CloseWeapon[] hands;
    [SerializeField]
    private CloseWeapon[] axes;
    [SerializeField]
    private CloseWeapon[] pickAxes;

    //���� ��ųʸ���
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickAxeDictionary = new Dictionary<string, CloseWeapon>();


    [SerializeField]
    private GunController theGunController;         //�ѱ��� ���� ��Ʈ�ѷ�
    [SerializeField]
    private HandController theHandController;       //Hand�� ���� ��Ʈ�ѷ�
    [SerializeField]
    private AxeController theAxeController;         //Axe�� ���� ��Ʈ�ѷ�
    [SerializeField]
    private PickAxeController thePickAxeController; //PickAxe�� ���� ��Ʈ�ѷ�

    void Start()
    {
        //��ųʸ� �ʱ�ȭ : guns, hands, axes
        for (int i = 0; i < guns.Length; i++)
            gunDictionary.Add(guns[i].gunName, guns[i]);
        for (int i = 0; i < hands.Length; i++)
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        for (int i = 0; i < axes.Length; i++)
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        for (int i = 0; i < pickAxes.Length; i++)
            pickAxeDictionary.Add(pickAxes[i].closeWeaponName, pickAxes[i]);
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
                //���ⱳü ����(����ӽŰ�)
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //���ⱳü ����(����)
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                //���ⱳü ����(����)
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "PickAxe"));
            }
        }   
    }

    //���� ��ü �ڷ�ƾ
    public IEnumerator ChangeWeaponCoroutine(string type, string name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("WeaponOut");
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
            case "AXE":
                AxeController.isActivate = false;
                break;
            case "PICKAXE":
                PickAxeController.isActivate = false;
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
            theHandController.CloseWeaponChange(handDictionary[name]);
        }
        else if (type == "AXE")
        {
            theAxeController.CloseWeaponChange(axeDictionary[name]);
        }
        else if (type == "PICKAXE")
        {
            thePickAxeController.CloseWeaponChange(pickAxeDictionary[name]);
        }
    }
}
