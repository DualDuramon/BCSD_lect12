using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage";   //�������� �̵��� ���� �̸�
    
    public void ClickStart()
    {
        Debug.Log("�ε�");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("�ҷ�������");
        
    }

    public void ClickEixt()
    {
        Debug.Log("��������");
        Application.Quit();
    }

}
