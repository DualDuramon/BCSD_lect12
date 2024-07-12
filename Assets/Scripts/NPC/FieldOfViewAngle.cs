using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle;       //시야각 -> 120도, 130도...
    [SerializeField] private float viewDistance;    //시야 거리 -> 10m, 100m
    [SerializeField] private LayerMask targetMask;  //타겟 마스크 -> 플레이어 마스크

    private Pig thePig;

    void Start()
    {
        thePig = GetComponent<Pig>();
    }

    private void Update()
    {
        View();
    }

    private Vector3 BoundaryAngle(float angle)
    {
        angle += transform.eulerAngles.y;    //현재 NPC의 Rotation.y 값에 따라 z축이 움직이므로 그에 대한 보정
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));

    }

    private void View()     //시야각 재설정 함수
    {
        Vector3 leftBoundary = BoundaryAngle(-viewAngle * 0.5f);  //왼쪽 시야각 경계
        Vector3 rightBoundary = BoundaryAngle(viewAngle * 0.5f);  //오른쪽 시야각 경계

        Debug.DrawRay(transform.position + transform.up, leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, rightBoundary, Color.red);

        //주변에 있는 collider를 가져옴
        Collider[] target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < target.Length; i++)
        {
            Transform targetTransform = target[i].transform;
            if (targetTransform.name == "Player")
            {
                //플레이어를 가리키는 방향
                Vector3 direction = (targetTransform.position - transform.position).normalized;
                float angle = Vector3.Angle(direction, transform.forward);

                if (angle < viewAngle * 0.5f)   //시야 내에 있으면
                {
                    RaycastHit rayHit;
                    if (Physics.Raycast(transform.position + transform.up, direction, out rayHit, viewDistance))
                    {
                        if (rayHit.transform.name == "Player")
                        {
                            Debug.Log("플레이어가 돼지 시야 안에 있음");
                            Debug.DrawRay(transform.position + transform.up, direction, Color.blue);
                            thePig.Run(rayHit.transform.position);

                        }
                    }
                }
            
            }
        }
    }

}
