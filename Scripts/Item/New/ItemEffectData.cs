using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;
    [Tooltip("HP,MP�� �����մϴ�.")]
    public string[] part;
    public int[] point;

}
public class ItemEffectData : MonoBehaviour
{   
    private const string HP = "HP", MP = "MP";

    [SerializeField]
    private ItemEffect[] itemEffects;
    [SerializeField]
    private WeaponManager weaponManager;
    [SerializeField]
    private PlayerParam player;

    private void Start()
    {
        player = FindObjectOfType<PlayerParam>();
        weaponManager = FindObjectOfType<WeaponManager>();

    }

    public void UseItem(Item _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            weaponManager.ChangeWeapon(_item.itemName);
        }
        else if (_item.itemType == ItemType.Potion)
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                if (itemEffects[x].itemName == _item.itemName)
                {
                    for (int y = 0; y < itemEffects[x].part.Length; y++)
                    {
                        switch (itemEffects[x].part[y])
                        {
                            case HP:
                                player.IncreaseHP(itemEffects[x].point[y]);
                                break;
                            case MP:
                                player.IncreaseMP(itemEffects[x].point[y]);
                                break;
                            default:
                                Debug.Log("�߸��� ȿ���� ����Ϸ��մϴ�");
                                break;
                        }
                        Debug.Log(_item.itemName + "�� ����߽��ϴ�.");

                    }
                    return;
                }
            }
            Debug.Log("��ġ�ϴ� �������� �����ϴ�");
        }
    }
}
