using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    //�ʿ��� ������Ʈ
    [SerializeField]
    private GameObject go_Base;     //UIâ ���̽�
    
    [SerializeField]
    private Text textItemName;      //������ �̸� �ؽ�Ʈ
    [SerializeField]
    private Text textItemDesc;      //������ ���� �ؽ�Ʈ
    [SerializeField]
    private Text textItemHowTo;     //������ ���� �ؽ�Ʈ


    public void ShowToolTip(Item item, Vector3 itemPos)
    {
        go_Base.SetActive(true);
        
        //�����ڽ� ������
        itemPos +=
            new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f,
            -go_Base.GetComponent<RectTransform>().rect.height * 0.6f
            , 0f);  

        go_Base.transform.position = itemPos;

        textItemName.text = item.itemName;
        textItemDesc.text = item.itemDescription;

        if(item.itemType == Item.ItemType.Equipment)
        {
            textItemHowTo.text = "��Ŭ��-����";
        }
        else if (item.itemType == Item.ItemType.Used)
        {
            textItemHowTo.text = "��Ŭ��-�Ա�";
        }
        else
        {
            textItemHowTo.text = "";
        }
    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
