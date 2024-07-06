using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    //필요한 컴포넌트
    [SerializeField]
    private GameObject go_Base;     //UI창 베이스
    
    [SerializeField]
    private Text textItemName;      //아이템 이름 텍스트
    [SerializeField]
    private Text textItemDesc;      //아이템 설명 텍스트
    [SerializeField]
    private Text textItemHowTo;     //아이템 사용법 텍스트


    public void ShowToolTip(Item item, Vector3 itemPos)
    {
        go_Base.SetActive(true);
        
        //툴팁박스 오프셋
        itemPos +=
            new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f,
            -go_Base.GetComponent<RectTransform>().rect.height * 0.6f
            , 0f);  

        go_Base.transform.position = itemPos;

        textItemName.text = item.itemName;
        textItemDesc.text = item.itemDescription;

        if(item.itemType == Item.ItemType.Equipment)
        {
            textItemHowTo.text = "우클릭-장착";
        }
        else if (item.itemType == Item.ItemType.Used)
        {
            textItemHowTo.text = "우클릭-먹기";
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
