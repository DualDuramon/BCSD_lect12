using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData{
    public Vector3 playerPos;   //플레이어 위치
    public Vector3 playerRot;   //플레이어 회전값

    public List<int> inventoryArrayNum = new List<int>();   //인벤토리 슬롯 넘버
    public List<string> inventoryItemName = new List<string>(); //인벤토리 아이템 이름
    public List<int> inventoryItemNum = new List<int>(); //인벤토리 아이템 개수
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
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/"; //저장할 경로

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        }
    }

    public void SaveData()  //데이터 세이브 함수
    {
        //플레이어 데이터 저장 영역
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

        //저장된 데이터 json반환
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);   //해당 경로에 json파일을 텍스트로 저장

        Debug.Log("저장완료");
        Debug.Log(json);
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME); //해당 경로에 있는 json파일을 텍스트로 읽어옴
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            //로딩 후 플레이어 데이터 적용 영역
            
            //플레이어 트랜스폼 로드
            thePlayer = FindObjectOfType<PlayerController>();
            thePlayer.transform.position = saveData.playerPos;
            thePlayer.transform.eulerAngles = saveData.playerRot;

            //플레이어 인벤토리 로드
            theInven = FindObjectOfType<Inventory>();
            for (int i = 0; i < saveData.inventoryItemName.Count; i++)
            {
                theInven.LoadToInventory(
                    saveData.inventoryArrayNum[i], 
                    saveData.inventoryItemName[i], 
                    saveData.inventoryItemNum[i]
                    );
            }

            Debug.Log("로드 완료");
        }
        else
        {
            Debug.Log("세이브 파일이 없음.");
        }
    }
}
