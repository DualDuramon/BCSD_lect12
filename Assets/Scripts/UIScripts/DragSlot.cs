using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;
    public Slot dragSlot;           //자신의 원본 slot에 대한 정보

    [SerializeField]
    private Image imageItem;        //아이템 이미지


    private void Start()
    {
        instance = this;
    }

    public void DragSetImage(Image itemImage)   //Drag했다 놨을때의 아이템 이미지 교체 함수
    {
        imageItem.sprite = itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float alpha)   //자신의 이미지 알파값 조절 함수
    {
        Color color = imageItem.color;
        color.a = alpha;
        imageItem.color = color;
    }
}
