using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData{
    public Vector3 playerPos;   //�÷��̾� ��ġ
    public Vector3 playerRot;   //�÷��̾� ȸ����

    public List<int> inventoryArrayNum = new List<int>();   //�κ��丮 ���� �ѹ�
    public List<string> inventoryItemName = new List<string>(); //�κ��丮 ������ �̸�
    public List<int> inventoryItemNum = new List<int>(); //�κ��丮 ������ ����
}

public class SaveNLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();
    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "SaveFile.txt";

    private PlayerController thePlayer;
    private Inventory theInven;

    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/"; //������ ���

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        }
    }

    public void SaveData()  //������ ���̺� �Լ�
    {
        //�÷��̾� ������ ���� ����
        thePlayer = FindObjectOfType<PlayerController>();
        theInven = FindObjectOfType<Inventory>();

        saveData.playerPos = thePlayer.transform.position;
        saveData.playerRot = thePlayer.transform.eulerAngles;

        Slot[] slots = theInven.GetSlots();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                saveData.inventoryArrayNum.Add(i);
                saveData.inventoryItemName.Add(slots[i].item.itemName);
                saveData.inventoryItemNum.Add(slots[i].itemCount);
            }
        }

        //����� ������ json��ȯ
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);   //�ش� ��ο� json������ �ؽ�Ʈ�� ����

        Debug.Log("����Ϸ�");
        Debug.Log(json);
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME); //�ش� ��ο� �ִ� json������ �ؽ�Ʈ�� �о��
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            //�ε� �� �÷��̾� ������ ���� ����
            
            //�÷��̾� Ʈ������ �ε�
            thePlayer = FindObjectOfType<PlayerController>();
            thePlayer.transform.position = saveData.playerPos;
            thePlayer.transform.eulerAngles = saveData.playerRot;

            //�÷��̾� �κ��丮 �ε�
            theInven = FindObjectOfType<Inventory>();
            for (int i = 0; i < saveData.inventoryItemName.Count; i++)
            {
                theInven.LoadToInventory(
                    saveData.inventoryArrayNum[i], 
                    saveData.inventoryItemName[i], 
                    saveData.inventoryItemNum[i]
                    );
            }

            Debug.Log("�ε� �Ϸ�");
        }
        else
        {
            Debug.Log("���̺� ������ ����.");
        }
    }
}
