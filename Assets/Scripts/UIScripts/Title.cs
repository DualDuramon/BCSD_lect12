using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    //싱글턴
    public static Title instance;
    private SaveNLoad theSaveNLoad;
    
    public string sceneName = "GameStage";   //다음으로 이동할 씬의 이름

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void ClickStart()
    {
        Debug.Log("로딩");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("불러오기중");
        StartCoroutine(LoadCoroutine());

    }

    IEnumerator LoadCoroutine()     //게임화면 로딩 코루틴
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }

        theSaveNLoad = FindObjectOfType<SaveNLoad>();
        theSaveNLoad.LoadData();
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void ClickEixt()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }

}
