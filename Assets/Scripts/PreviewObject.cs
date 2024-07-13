using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    //�浹�� ������Ʈ �ݶ��̴� ���� ����Ʈ   
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField] private int layerGround;   //�����̾�. �����ϴ� �ֵ���
    private const int IGNORE_RAYCAST_LAYER = 2; //����ĳ��Ʈ ���� ���̾�

    [SerializeField] private Material green;    //�ʷϻ�, ������ ���׸���
    [SerializeField] private Material red;


    private void Update()
    {
        ChangeColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        //���̾ �����鼭 list�� �߰�
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Add(other);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Remove(other);
        }
    }

    private void ChangeColor()
    {
        if(colliderList.Count > 0)
        {
            SetColor(red);
        }
        else
        {
            SetColor(green);
        }
    }

    private void SetColor(Material mat) //�ڽ� ��ü�� ������ �ٲٴ� �Լ�
    {
        //�ڽ��� ��ü�� Renderer�� materials ������Ƽ �迭�� ������ ��
        //���� �ٲ� �迭�� ������ ���
        foreach(Transform th_child in this.transform)
        {
            var newMaterials = new Material[th_child.GetComponent<Renderer>().materials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }
            th_child.GetComponent<Renderer>().materials = newMaterials;
        }
    }

    public bool IsBuildable()
    {
        return colliderList.Count == 0;
    }
}
