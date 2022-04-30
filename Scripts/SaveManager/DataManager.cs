using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    public string name;
    public int level = 1;
    public float myHp = 100f;
    public float myMp = 100f;
    public int exp = 0;
    public int money = 100;
    public string sceneName;


    public Vector3 playerPos;
    public List<int> invenArrayNumber = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();

}
public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public PlayerData playerData = new PlayerData();

    private PlayerParam player;
    private Inventory theInven;


    public string path;
    public int nowSlot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;      
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != null)
        {
            Destroy(instance.gameObject);
        }



        path = Path.Combine(Application.dataPath, "PlayerData");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void GameSave()
    {


        if (SceneManager.GetActiveScene().name != "Lobby")
        {
            playerData.sceneName = SceneManager.GetActiveScene().name;
            player = FindObjectOfType<PlayerParam>();
            theInven = FindObjectOfType<Inventory>();
            Slot[] slots = theInven.GetSlots();
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    playerData.invenArrayNumber.Add(i);
                    playerData.invenItemName.Add(slots[i].item.itemName);
                    playerData.invenItemNumber.Add(slots[i].itemCount);

                }
            }
            playerData.name = player.myName;
            playerData.level = player.level;
            playerData.money = player.myMoney;
            playerData.myHp = player.myHp;
            playerData.myMp = player.myMp;
            playerData.exp = player.myExp;
            playerData.playerPos = player.transform.position;

        }
        else
            playerData.sceneName = "Village";
        string json_Data = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(path + nowSlot.ToString() + ".json", json_Data);


    }



    public void GameLoad()
    {
        string jsonData = File.ReadAllText(path + nowSlot.ToString() + ".json");
        playerData = JsonUtility.FromJson<PlayerData>(jsonData);

    }

    public void DataClear()
    {
        nowSlot = -1;
        playerData = new PlayerData();
    }

}
