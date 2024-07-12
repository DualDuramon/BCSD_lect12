using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    //충돌한 오브젝트 콜라이더 저장 리스트   
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField] private int layerGround;   //지상레이어. 무시하는 애들임
    private const int IGNORE_RAYCAST_LAYER = 2; //레이캐스트 무시 레이어

    [SerializeField] private Material green;    //초록색, 빨간색 메테리얼
    [SerializeField] private Material red;


    private void Update()
    {
        ChangeColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        //레이어를 봐가면서 list에 추가
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

    private void SetColor(Material mat) //자식 개체의 색깔을 바꾸는 함수
    {
        //자식의 개체의 Renderer의 materials 프로퍼티 배열을 가져온 후
        //색을 바꿔 배열을 덮어씌우는 방식
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
