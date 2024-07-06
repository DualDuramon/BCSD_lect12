using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;       // ȹ���� ������
    public int itemCount;   // ȹ���� �������� ����
    public Image itemImage; // �������� �̹���
    
    //�ʿ��� ������Ʈ
    [SerializeField]
    private Text textCount; // �������� ���� �ؽ�Ʈ
    [SerializeField]
    private GameObject go_CountImage;
    
    private void SetColor(float alpha)      //�������� ����(Ư�� ���İ�) ���� �Լ�.
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    public void AddItem(Item addItem, int count = 1)    //������ ȹ�� �Լ�.
    {
        item = addItem;
        itemCount += count;
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

}
