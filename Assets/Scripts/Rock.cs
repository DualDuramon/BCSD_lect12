using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp;                 //바위의 체력

    [SerializeField]
    private float destroyTime;      //파편 제거 시간
    [SerializeField]
    private SphereCollider col;     //구체 콜라이더

    //필요한 게임 오브젝트s
    [SerializeField]
    private GameObject go_Rock;     //일반 바위오브젝트
    [SerializeField]
    private GameObject go_Debris;  //깨진 바위 오브젝트

    [SerializeField]
    private GameObject go_EffectPrefab; //채굴 이팩트 프리팹

    //소리 관련 변수
    [SerializeField]
    private string strikeSound; //바위 채굴 사운드
    [SerializeField]
    private string destroySound; //바위 파괴 사운드

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
