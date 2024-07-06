using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;       // ȹ���� ������
    public int itemCount;   // ȹ���� �������� ����
    public Image itemImage; // �������� �̹���

    //�ʿ��� ������Ʈ
    [SerializeField]
    private Text textCount; // �������� ���� �ؽ�Ʈ
    [SerializeField]
    private GameObject go_CountImage;

    private WeaponManager theWeaponManager;

    private void Start()
    {
        theWeaponManager = FindObjectOfType<WeaponManager>();
    }
    
    private void SetColor(float alpha)      //�������� ����(Ư�� ���İ�) ���� �Լ�.
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    public void AddItem(Item addItem, int count = 1)    //������ ȹ�� �Լ�.
    {
        item = addItem;
        itemCount = count;
        itemImage.sprite = item.itemImage;      //������ �̹����� ȹ���� �������� �̹�����

        if (item.itemType != Item.ItemType.Equipment)
        {
            textCount.text = itemCount.ToString();
;           go_CountImage.SetActive(true);
        }
        else
        {
            textCount.text = "0";
            go_CountImage.SetActive(false);     //���� ������ ǥ������ �����Ƿ� ��Ȱ��ȭ
        }
        SetColor(1f);
    }

    public void SetSlotCount(int count)        //�κ��丮 �󿡼� ������ �� ���� ������Ʈ �Լ�
    {
        itemCount += count;
        textCount.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
        }

    }

    private void ClearSlot()    //���� ���� �Լ�
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
                    //����
                    StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
                }
                else
                {
                    //���
                    Debug.Log(item.itemName + "�� ����߽��ϴ�.");
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            //�巡�� ���Կ� �ڽ��� ����.
            DragSlot.instance.dragSlot = this;
            //�巡���� ������ �̹����� �ڽ��� �̹����� ����.
            DragSlot.instance.DragSetImage(itemImage);
            //������ ��� ���콺 ��ġ�� ���󰡰���.
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            //������ ��� ���콺 ��ġ�� ���󰡰���.
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

    private void ChangeSlot()   //�ΰ��� ������ ���� �±�ȯ �Լ�
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
