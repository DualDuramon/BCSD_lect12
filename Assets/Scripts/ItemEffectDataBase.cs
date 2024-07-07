using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName; //������ �̸�(Ű��)

    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY �� �����մϴ�.")]
    public string[] part;     //ȿ���� ����� �κ�

    public int[] num;         //ȿ�� ����

}

public class ItemEffectDataBase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;
    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    //�ʿ��� ������Ʈ
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
                        //�Ҹ�ǰ�� ȸ�� ������ ���� ȸ�� ȿ��
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
                                Debug.Log("�߸��� Status ����. HP, SP, DP, HUNGRY, THIRSTY, SATISFY �� �����մϴ�.");
                                break;
                        }
                        Debug.Log(item.itemName + "�� ����߽��ϴ�.");
                    }
                    return;
                }
            }
            Debug.Log("ItemEffectDatabase�� ��ġ�ϴ� itemName�� �����ϴ�.");
        }
    }
}
