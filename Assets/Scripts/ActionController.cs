using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range;                    // ���� ������ �ִ� �Ÿ�.
    private bool pickUpActivated = false;   // ���� ������ �� true.
    
    private RaycastHit hitInfo;             //�浹ü ���� ����.
    [SerializeField]
    private LayerMask layerMask;   //Ư�� ������ ���̾�� �����ϵ��� ���̾� ����ũ ����.

    [SerializeField]
    private Text actionText;        //�ൿ�� �����ִ� �ؽ�Ʈ


    private void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickUpActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "�� ȹ���Ͽ����ϴ�.");
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }

    }

    private void CheckItem()    //�������� �ִ����� Ȯ���ϴ� �Լ�
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
        {
            InfoDisappear();
        }
    }

    private void ItemInfoAppear()   //������ ���� ��� �Լ�
    {
        pickUpActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ��" + "<color=yellow>" + "(E)" + "</color>";
    }

    private void InfoDisappear()    //������ ���� ��� ���� �Լ�
    {
        pickUpActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
