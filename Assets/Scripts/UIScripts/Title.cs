using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    //�̱���
    public static Title instance;
    private SaveNLoad theSaveNLoad;
    
    public string sceneName = "GameStage";   //�������� �̵��� ���� �̸�

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
        Debug.Log("�ε�");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("�ҷ�������");
        StartCoroutine(LoadCoroutine());

    }

    IEnumerator LoadCoroutine()     //����ȭ�� �ε� �ڷ�ƾ
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
        Debug.Log("��������");
        Application.Quit();
    }

}
