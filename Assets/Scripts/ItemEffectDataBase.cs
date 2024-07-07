using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName; //아이템 이름(키값)

    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY 만 가능합니다.")]
    public string[] part;     //효과가 적용될 부분

    public int[] num;         //효과 정도

}

public class ItemEffectDataBase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;
    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    //필요한 컴포넌트
    [SerializeField]
    private StatusController thePlayerStatus;
    [SerializeField]
    private WeaponManager theWeaponMananger;
    [SerializeField]
    private SlotToolTip theSlotToolTip;

    public void ShowToolTip(Item item, Vector3 pos)
    {
        theSlotToolTip.ShowToolTip(item, pos);
    }

    public void HideToolTip()
    {
        theSlotToolTip.HideToolTip();
    }

    public void useItem(Item item)
    {
        if(item.itemType == Item.ItemType.Equipment)
        {
            StartCoroutine(theWeaponMananger.ChangeWeaponCoroutine(item.weaponType, item.itemName));

        }
        else if(item.itemType == Item.ItemType.Used)
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                if (itemEffects[x].itemName == item.itemName)
                {
                    for (int y = 0; y < itemEffects[x].part.Length; y++)
                    {
                        //소모품의 회복 부위에 따른 회복 효과
                        switch (itemEffects[x].part[y])
                        {
                            case HP:
                                thePlayerStatus.IncreaseHp(itemEffects[x].num[y]);
                                break;
                            case SP:
                                thePlayerStatus.IncreaseSp(itemEffects[x].num[y]);
                                break;
                            case DP:
                                thePlayerStatus.IncreaseDp(itemEffects[x].num[y]);
                                break;
                            case HUNGRY:
                                thePlayerStatus.IncreaseHungry(itemEffects[x].num[y]);
                                break;
                            case THIRSTY:
                                thePlayerStatus.IncreaseThirsty(itemEffects[x].num[y]);
                                break;
                            case SATISFY:
                                break;
                            default:
                                Debug.Log("잘못된 Status 부위. HP, SP, DP, HUNGRY, THIRSTY, SATISFY 만 가능합니다.");
                                break;
                        }
                        Debug.Log(item.itemName + "을 사용했습니다.");
                    }
                    return;
                }
            }
            Debug.Log("ItemEffectDatabase에 일치하는 itemName이 없습니다.");
        }
    }
}
