using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false; // 인벤토리 활성화 여부
    
    //필요한 컴포넌트
    [SerializeField]
    public GameObject go_Inventory_Base;  //인벤토리
    [SerializeField]
    private GameObject go_SlotsParent;

    //슬롯들
    private Slot[] slots;

    //그외
    [SerializeField] private Item[] items;  //아이템 목록 배열

    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if(inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    private void OpenInventory()
    {
        GameManager.isOpenInventory = true;
        go_Inventory_Base.SetActive(true);
    }

    private void CloseInventory()
    {
        GameManager.isOpenInventory = false;
        go_Inventory_Base.SetActive(false);
    }

    public void AcquireItem(Item item, int amount = 1)
    {
        //장비 아이템이 아닐때만 인벤토리 아이템 중복검사
        if (item.itemType != Item.ItemType.Equipment)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    //이미 획득한 아이템이라면 개수만 추가.
                    if (slots[i].item.itemName == item.itemName)
                    {
                        slots[i].SetSlotCount(amount);
                        return;
                    }
                }
            }

        }

        //인벤토리에 없는 아이템이면
        for (int i = 0; i < slots.Length; i++)
        {
            //빈 슬롯 찾아 아이템 추가
            if (slots[i].item == null) 
            {
                slots[i].AddItem(item, amount);
                return;
            }
        }
    }

    public Slot[] GetSlots() //인벤토리 슬롯 반환 함수 
    {
        return slots;
    }

    public void LoadToInventory(int arrayNum, string itemName, int itemNum)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == itemName)
            {
                slots[arrayNum].AddItem(items[i], itemNum);
            }
        }
    }

}
