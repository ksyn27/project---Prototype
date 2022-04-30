using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Select : MonoBehaviour
{
    public GameObject create;
    public Text[] slotText;
    public Text newPlayerName;
    bool[] savefile = new bool[3];
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(DataManager.instance.path + $"{i}" +".json"))
            {
                savefile[i] = true;
                DataManager.instance.nowSlot = i;
                DataManager.instance.GameLoad();
                slotText[i].text ="Name : "+ DataManager.instance.playerData.name + "\n"
                                 + "Level : "+ DataManager.instance.playerData.level;

            }
            else
            {
                slotText[i].text = "Empty..."; 
            }
        }
        DataManager.instance.DataClear();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void Slot(int num)
    {
        DataManager.instance.nowSlot = num;
        if (savefile[num])
        {
            DataManager.instance.GameLoad();
            LoadGame();
        }
        else
        {
            CreateGame();
        }
    }

    public void CreateGame()
    {
        create.gameObject.SetActive(true);
    }

    public void LoadGame()
    {
        if (!savefile[DataManager.instance.nowSlot])
        {
            DataManager.instance.playerData.name = newPlayerName.text;
            DataManager.instance.GameSave();
        }
        SceneLoad.LoadScene(DataManager.instance.playerData.sceneName);

    }
}
