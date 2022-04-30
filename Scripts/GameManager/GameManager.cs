using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerData playerData;

    [SerializeField]
    public PlayerParam player;

    //talkData
    [Header("Talk Object")]
    public bool isAction;
    public Text talkText;
    public GameObject questPanel;
    public GameObject scanob;
    public TalkManager talkManager;
    public int talkIndex;

    //QuestData
    [Header("Quest Object")]
    public QuestManager questManager;


    #region QuestAndTalk
    public void Action(GameObject scanobj)
    {
        if(scanobj == null)
        {
            questPanel.SetActive(false);
            return;
        }
        scanob = scanobj;
        ObjData objData = scanob.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc);

        questPanel.SetActive(isAction);
    
    }

    void Talk(int id, bool isNpc)
    {

        int questTalkIndex = questManager.GetQuestTalkIndex(id);

        string talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);

        if(questManager.isQuest ==true)
        talkData = talkManager.GetTalk(id, talkIndex);


        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            Debug.Log(questManager.CheckQuest(id));
            return;
        }
        if (isNpc)
        {
            talkText.text = talkData;
        }
        else
        {
            talkText.text = talkData;
        }

        isAction = true;
        talkIndex++;
    }
    #endregion QuestAndTalk


    private void Awake()
    {
        if (instance == null)
            instance = this;
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerParam>();

    }
   
    private void Start()
    {
        //GameObject ui = GameObject.Find("OnUi");
        //questPanel = ui.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        
    }

    public void DeadReStart()
    {
        //SavePlayerDataJason();
        StartCoroutine(StartDead());
        
    }
    IEnumerator StartDead()
    {
        UIManager.instance.DeadStateUIOpen();        
        yield return new WaitForSeconds(4f);
        UIManager.instance.DeadStateUIClose();
        SceneLoad.LoadScene("Village");
        while(SceneManager.GetActiveScene().name != "Village")
        {
            yield return null;
        }
        player.SetRestart();
    }

   
} 
