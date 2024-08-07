using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static bool isChangeWeapon = false;  //무기 중복 교체 방지.
 
    [SerializeField]
    private float changeWeaponDelayTime;        //무기 교체 딜레이
    [SerializeField]
    private float changeWeaponEndDelayTime;     //무기 교체 완료 딜레이

    public static Transform currentWeapon;      //현재 무기의 transform
    public static Animator currentWeaponAnim;   //현재 무기 animator
    [SerializeField]
    private string currentWeaponType;           //현재 무기 타입

    //무기 관리 변수들
    [SerializeField] 
    private Gun[] guns;
    [SerializeField] 
    private CloseWeapon[] hands;
    [SerializeField]
    private CloseWeapon[] axes;
    [SerializeField]
    private CloseWeapon[] pickAxes;

    //무기 딕셔너리들
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickAxeDictionary = new Dictionary<string, CloseWeapon>();


    [SerializeField]
    private GunController theGunController;         //총기형 무기 컨트롤러
    [SerializeField]
    private HandController theHandController;       //Hand형 무기 컨트롤러
    [SerializeField]
    private AxeController theAxeController;         //Axe형 무기 컨트롤러
    [SerializeField]
    private PickAxeController thePickAxeController; //PickAxe형 무기 컨트롤러

    void Start()
    {
        //딕셔너리 초기화 : guns, hands, axes
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
                //무기교체 실행(맨손)
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //무기교체 실행(서브머신건)
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //무기교체 실행(도끼)
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                //무기교체 실행(도끼)
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "PickAxe"));
            }
        }   
    }

    //무기 교체 코루틴
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

    private void CanclePreWeaponAction()    //이전 무기 행동 취소 함수
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
    
    private void WeaponeChange(string type, string name) //무기 교체 함수
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

    public IEnumerator WeaponInCoroutine()  //무기 집어넣기 코루틴
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("WeaponOut");

        yield return new WaitForSeconds(changeWeaponDelayTime);
        
        currentWeapon.gameObject.SetActive(false);
    }

    public void WeaponOut()
    {
        isChangeWeapon = false;
        currentWeapon.gameObject.SetActive(true);
    }
}
