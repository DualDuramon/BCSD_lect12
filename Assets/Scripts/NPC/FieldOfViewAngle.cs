using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle;       //�þ߰� -> 120��, 130��...
    [SerializeField] private float viewDistance;    //�þ� �Ÿ� -> 10m, 100m
    [SerializeField] private LayerMask targetMask;  //Ÿ�� ����ũ -> �÷��̾� ����ũ

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
        angle += transform.eulerAngles.y;    //���� NPC�� Rotation.y ���� ���� z���� �����̹Ƿ� �׿� ���� ����
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));

    }

    private void View()     //�þ߰� �缳�� �Լ�
    {
        Vector3 leftBoundary = BoundaryAngle(-viewAngle * 0.5f);  //���� �þ߰� ���
        Vector3 rightBoundary = BoundaryAngle(viewAngle * 0.5f);  //������ �þ߰� ���

        Debug.DrawRay(transform.position + transform.up, leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, rightBoundary, Color.red);

        //�ֺ��� �ִ� collider�� ������
        Collider[] target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < target.Length; i++)
        {
            Transform targetTransform = target[i].transform;
            if (targetTransform.name == "Player")
            {
                //�÷��̾ ����Ű�� ����
                Vector3 direction = (targetTransform.position - transform.position).normalized;
                float angle = Vector3.Angle(direction, transform.forward);

                if (angle < viewAngle * 0.5f)   //�þ� ���� ������
                {
                    RaycastHit rayHit;
                    if (Physics.Raycast(transform.position + transform.up, direction, out rayHit, viewDistance))
                    {
                        if (rayHit.transform.name == "Player")
                        {
                            Debug.Log("�÷��̾ ���� �þ� �ȿ� ����");
                            Debug.DrawRay(transform.position + transform.up, direction, Color.blue);
                            thePig.Run(rayHit.transform.position);

                        }
                    }
                }
            
            }
        }
    }

}
