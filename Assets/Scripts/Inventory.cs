using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false; // �κ��丮 Ȱ��ȭ ����
    
    //�ʿ��� ������Ʈ
    [SerializeField]
    public GameObject go_Inventory_Base;  //�κ��丮
    [SerializeField]
    private GameObject go_SlotsParent;

    //���Ե�
    private Slot[] slots;
    
    
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
        go_Inventory_Base.SetActive(true);
    }

    private void CloseInventory()
    {
        go_Inventory_Base.SetActive(false);
    }

    public void AcquireItem(Item item, int amount = 1)
    {
        //��� �������� �ƴҶ��� �κ��丮 ������ �ߺ��˻�
        if (item.itemType != Item.ItemType.Equipment)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    //�̹� ȹ���� �������̶�� ������ �߰�.
                    if (slots[i].item.itemName == item.itemName)
                    {
                        slots[i].SetSlotCount(amount);
                        return;
                    }
                }
            }

        }

        //�κ��丮�� ���� �������̸�
        for (int i = 0; i < slots.Length; i++)
        {
            //�� ���� ã�� ������ �߰�
            if (slots[i].item == null) 
            {
                slots[i].AddItem(item, amount);
                return;
            }
        }
    }
}
