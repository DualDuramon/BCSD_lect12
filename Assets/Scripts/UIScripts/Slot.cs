using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;       // 획득한 아이템
    public int itemCount;   // 획득한 아이템의 개수
    public Image itemImage; // 아이템의 이미지

    //필요한 컴포넌트
    [SerializeField]
    private Text textCount; // 아이템의 개수 텍스트
    [SerializeField]
    private GameObject go_CountImage;

    private WeaponManager theWeaponManager;

    private void Start()
    {
        theWeaponManager = FindObjectOfType<WeaponManager>();
    }
    
    private void SetColor(float alpha)      //아이템의 색상(특히 알파값) 조절 함수.
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    public void AddItem(Item addItem, int count = 1)    //아이템 획득 함수.
    {
        item = addItem;
        itemCount = count;
        itemImage.sprite = item.itemImage;      //슬롯의 이미지를 획득한 아이템의 이미지로

        if (item.itemType != Item.ItemType.Equipment)
        {
            textCount.text = itemCount.ToString();
;           go_CountImage.SetActive(true);
        }
        else
        {
            textCount.text = "0";
            go_CountImage.SetActive(false);     //장비는 개수를 표시하지 않으므로 비활성화
        }
        SetColor(1f);
    }

    public void SetSlotCount(int count)        //인벤토리 상에서 아이템 의 개수 업데이트 함수
    {
        itemCount += count;
        textCount.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
        }

    }

    private void ClearSlot()    //슬롯 비우기 함수
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        textCount.text = "0";
        go_CountImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                if(item.itemType == Item.ItemType.Equipment)
                {
                    //장착
                    StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
                }
                else
                {
                    //사용
                    Debug.Log(item.itemName + "을 사용했습니다.");
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            //드래그 슬롯에 자신을 대입.
            DragSlot.instance.dragSlot = this;
            //드래그한 슬롯의 이미지를 자신의 이미지로 변경.
            DragSlot.instance.DragSetImage(itemImage);
            //슬롯이 계속 마우스 위치를 따라가게함.
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            //슬롯이 계속 마우스 위치를 따라가게함.
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(DragSlot.instance.dragSlot != null)
            ChangeSlot();
    }

    private void ChangeSlot()   //두개의 아이템 슬롯 맞교환 함수
    {
        Item tempItem = item;
        int tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (tempItem != null)
            DragSlot.instance.dragSlot.AddItem(tempItem, tempItemCount);
        else
            DragSlot.instance.dragSlot.ClearSlot();
    }
}
