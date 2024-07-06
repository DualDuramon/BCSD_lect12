using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range;                    // 습득 가능한 최대 거리.
    private bool pickUpActivated = false;   // 습득 가능할 시 true.
    
    private RaycastHit hitInfo;             //충돌체 정보 저장.
    [SerializeField]
    private LayerMask layerMask;   //특정 아이템 레이어에만 반응하도록 레이어 마스크 설정.

    [SerializeField]
    private Text actionText;        //행동을 보여주는 텍스트


    private void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickUpActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "를 획득하였습니다.");
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }

    }

    private void CheckItem()    //아이템이 있는지를 확인하는 함수
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
        {
            InfoDisappear();
        }
    }

    private void ItemInfoAppear()   //아이템 정보 출력 함수
    {
        pickUpActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득" + "<color=yellow>" + "(E)" + "</color>";
    }

    private void InfoDisappear()    //아이템 정보 출력 해제 함수
    {
        pickUpActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
