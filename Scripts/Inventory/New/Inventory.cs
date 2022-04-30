using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SloatsParent;

    private Slot[] slots;


    public Slot[] GetSlots() { return slots; }
    [SerializeField] private Item[] items;
    public void LoadToInven(int _arrayNum,string _itemName,int _itemNum)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if(items[i].itemName == _itemName)
            {
                slots[_arrayNum].AddItem(items[i], _itemNum);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        slots = go_SloatsParent.GetComponentsInChildren<Slot>();
        PlayerData player = DataManager.instance.playerData;

        StartCoroutine(LoadInven());
    }

    IEnumerator LoadInven()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerData player = DataManager.instance.playerData;
        for (int i = 0; i < player.invenItemName.Count; i++)
        {
            LoadToInven(player.invenArrayNumber[i],player.invenItemName[i], player.invenItemNumber[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }
    private void TryOpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            OnOffInventory();
        }
    }
    public void OnOffInventory()
    {
        inventoryActivated = !inventoryActivated;
        go_InventoryBase.SetActive(inventoryActivated);
    }

    public void AcquireItem(Item _item , int _count=1)
    {
        if (_item.itemType != ItemType.Equipment)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                    slots[i].AddItem(_item,_count);
                    return;
            }
        }
    }
}
