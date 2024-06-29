using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp;                 //������ ü��

    [SerializeField]
    private float destroyTime;      //���� ���� �ð�
    [SerializeField]
    private SphereCollider col;     //��ü �ݶ��̴�

    //�ʿ��� ���� ������Ʈs
    [SerializeField]
    private GameObject go_Rock;     //�Ϲ� ����������Ʈ
    [SerializeField]
    private GameObject go_Debris;  //���� ���� ������Ʈ

    [SerializeField]
    private GameObject go_EffectPrefab; //ä�� ����Ʈ ������

    //�Ҹ� ���� ����
    [SerializeField]
    private string strikeSound; //���� ä�� ����
    [SerializeField]
    private string destroySound; //���� �ı� ����

    public void Mining()
    {
        SoundManager.instance.PlaySE(strikeSound);
        var clone = Instantiate(go_EffectPrefab, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);

        hp--;
        if(hp<= 0)
        {
            Destruction();
        }
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroySound);
        col.enabled = false;
        Destroy(go_Rock);

        go_Debris.SetActive(true);
        Destroy(go_Debris, destroyTime);
    }
}
