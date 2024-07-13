using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage";   //다음으로 이동할 씬의 이름
    
    public void ClickStart()
    {
        Debug.Log("로딩");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("불러오기중");
        
    }

    public void ClickEixt()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }

}
