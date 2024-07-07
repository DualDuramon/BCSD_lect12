using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;
    public Slot dragSlot;           //�ڽ��� ���� slot�� ���� ����

    [SerializeField]
    private Image imageItem;        //������ �̹���


    private void Start()
    {
        instance = this;
    }

    public void DragSetImage(Image itemImage)   //Drag�ߴ� �������� ������ �̹��� ��ü �Լ�
    {
        imageItem.sprite = itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float alpha)   //�ڽ��� �̹��� ���İ� ���� �Լ�
    {
        Color color = imageItem.color;
        color.a = alpha;
        imageItem.color = color;
    }
}
