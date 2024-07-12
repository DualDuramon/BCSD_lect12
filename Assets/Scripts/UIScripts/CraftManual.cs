using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class Craft
{
    public string craftName;            //이름
    public GameObject go_Prefab;        //실제 설치될 프리팹
    public GameObject go_PreviewPrefab; //미리보기 프리팹

}

public class CraftManual : MonoBehaviour
{
    //상태변수
    private bool isActivated = false;
    private bool isPreviewActivated = false;

    [SerializeField] private GameObject go_BaseUI;  //기본 Base UI
    [SerializeField] private Craft[] craft_Fire;    //모닥불탭 
    private GameObject go_Preview;                  //선택한 건축물의 미리보기 프리팹
    private GameObject go_Prefab;                   //선택한 건축물 생성 프리팹

    [SerializeField] private Transform tf_Player;    //플레이어 트랜스폼

    //Raycast 필요 변수
    private RaycastHit hitInfo;
    
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;

    public void SlotClick(int slotNum)     //Slot UI 버튼 클릭시 미리보기 생성 함수
    {
        go_Preview = Instantiate(craft_Fire[slotNum].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_Fire[slotNum].go_Prefab;
        isPreviewActivated = true;
        GameManager.isOpenCraftManual = false;
        go_BaseUI.SetActive(false);
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {
            Window();
        }

        if (isPreviewActivated)
        {
            PreviewPositionUpdate();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Build();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancle();
        }
    }

    private void Build()    //건축물 생성 함수
    {
        if (isPreviewActivated 
            && go_Preview.GetComponent<PreviewObject>().IsBuildable())     //미리보기가 활성화 및 건설 가능한지를 따짐
        {
            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }

    private void PreviewPositionUpdate()    //미리보기 프리펩 위치 업데이트 함수 : 플레이어 크로스헤어 따라가게
    {
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 location = hitInfo.point; 
                go_Preview.transform.position = location;
            }
        }
    }

    private void Cancle()   //ESC누르면 관련 UI 및 프리뷰 취소
    {
        if (isPreviewActivated)
            Destroy(go_Preview);

        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;

        go_BaseUI.SetActive(false);
    }

    private void Window()
    {
        if (!isActivated)
        {
            OpenWindow();
        }
        else
        {
            CloseWindow();
        }
    }
    
    private void OpenWindow()   //BaseUI 활성화 함수
    {
        isActivated = true;
        GameManager.isOpenCraftManual = true;
        go_BaseUI.SetActive(true);

    }

    private void CloseWindow()  //BaseUI 비활성화 함수
    {
        isActivated = false;
        GameManager.isOpenCraftManual = false;
        go_BaseUI.SetActive(false);
    }
    


}
