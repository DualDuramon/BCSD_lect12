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
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip effectSound;      //ä�� �Ҹ�
    [SerializeField]
    private AudioClip effectSound2;     //�ı� �Ҹ�

    public void Mining()
    {
        audioSource.clip = effectSound; //ä���Ҹ� ���
        audioSource.Play();

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
        audioSource.clip = effectSound2; //�ı��Ҹ� ���
        audioSource.Play();

        col.enabled = false;
        Destroy(go_Rock);

        go_Debris.SetActive(true);
        Destroy(go_Debris, destroyTime);
    }
}
