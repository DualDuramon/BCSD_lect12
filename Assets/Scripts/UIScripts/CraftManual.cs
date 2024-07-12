using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class Craft
{
    public string craftName;            //�̸�
    public GameObject go_Prefab;        //���� ��ġ�� ������
    public GameObject go_PreviewPrefab; //�̸����� ������

}

public class CraftManual : MonoBehaviour
{
    //���º���
    private bool isActivated = false;
    private bool isPreviewActivated = false;

    [SerializeField] private GameObject go_BaseUI;  //�⺻ Base UI
    [SerializeField] private Craft[] craft_Fire;    //��ں��� 
    private GameObject go_Preview;                  //������ ���๰�� �̸����� ������
    private GameObject go_Prefab;                   //������ ���๰ ���� ������

    [SerializeField] private Transform tf_Player;    //�÷��̾� Ʈ������

    //Raycast �ʿ� ����
    private RaycastHit hitInfo;
    
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float range;

    public void SlotClick(int slotNum)     //Slot UI ��ư Ŭ���� �̸����� ���� �Լ�
    {
        go_Preview = Instantiate(craft_Fire[slotNum].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_Fire[slotNum].go_Prefab;
        isPreviewActivated = true;
        go_BaseUI.SetActive(false);
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {
            Window();
        }

        if (isPreviewActivated)
        {
            PreviewPositionUpdate();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Build();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancle();
        }
    }

    private void Build()    //���๰ ���� �Լ�
    {
        if (isPreviewActivated)     //�̸����Ⱑ Ȱ��ȭ ���������� ����
        {
            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }

    private void PreviewPositionUpdate()    //�̸����� ������ ��ġ ������Ʈ �Լ� : �÷��̾� ũ�ν���� ���󰡰�
    {
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, playerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 location = hitInfo.point; 
                go_Preview.transform.position = location;
            }
        }
    }

    private void Cancle()   //ESC������ ���� UI �� ������ ���
    {
        if (isPreviewActivated)
            Destroy(go_Preview);

        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;

        go_BaseUI.SetActive(false);
    }

    private void Window()
    {
        if (!isActivated)
        {
            OpenWindow();
        }
        else
        {
            CloseWindow();
        }
    }
    
    private void OpenWindow()   //BaseUI Ȱ��ȭ �Լ�
    {
        isActivated = true;

        go_BaseUI.SetActive(true);

    }

    private void CloseWindow()  //BaseUI ��Ȱ��ȭ �Լ�
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
    }
    


}
