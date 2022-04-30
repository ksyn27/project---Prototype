using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Image playerHp;
    public Image playerMp;

    public GameObject hpOb;
    public Image EnemyHp;

    PlayerParam player;

    //playerStatusUI
    [Header("Status")]
    public GameObject statusUI;
    public bool statusOpen = false;
    public Text level;
    public Text maxHp;
    public Text maxMp;

    public Text att;
    public Text def;
    public Text money;

    public GameObject questUI;
    public bool questOpen = false;

    public GameObject optionUI;
    public bool optionOpen = false;

    public GameObject volUI;
    public bool volOpen = false;

    public GameObject deadState;

    private void Awake()
    {
        var obj = FindObjectsOfType<UIManager>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);

        if (instance == null)
            instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerParam>();

    }

    public void UpdatePlayerUI(PlayerParam playerParam)
    {
        playerHp.fillAmount = (float)playerParam.myHp / (float)playerParam.maxHp;
        playerMp.fillAmount = (float)playerParam.myMp / (float)playerParam.maxMp;
    }


    public void EnemyHpUpdate(EnemyParam enemyparm)
    {
        StopCoroutine(CloseEnemyUI());
        hpOb.SetActive(true);
        EnemyHp.fillAmount = (float)enemyparm.myHp / (float)enemyparm.maxHp;
        StartCoroutine(CloseEnemyUI());
    }
    IEnumerator CloseEnemyUI()
    {
        yield return new WaitForSeconds(3f);
        hpOb.SetActive(false);
    }


    public void OnStatus()
    {
        statusOpen = !statusOpen;
        statusUI.SetActive(statusOpen);
        if (statusOpen == true)
            SetStatus();
    }
    public void OnQuest()
    {
        questOpen = !questOpen;
        questUI.SetActive(questOpen);
    }
    public void OnOption()
    {
        optionOpen = !optionOpen;
        optionUI.SetActive(optionOpen);
    }

    public void OnVolCustom()
    {
        OnOption();
        volOpen = !volOpen;
        volUI.SetActive(volOpen);
    }

    public void BackVol(float vol)
    {
        SoundManager.instance.BackgroundVol(vol);
    }
    public void EffectVol(float vol)
    {
        SoundManager.instance.EffectVol(vol);
    }
    public void MasterVol(float vol)
    {
        SoundManager.instance.MasterVol(vol);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetStatus()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerParam>();
        level.text = player.level.ToString();

        maxHp.text = player.maxHp.ToString("F2");
        maxMp.text = player.maxMp.ToString("F2");

        att.text = player.attPow.ToString("F2");
        def.text = player.defense.ToString("F2");
        money.text = player.myMoney.ToString();

    }

    public void SaveNow()
    {
        DataManager.instance.GameSave();
    }
    public void DeadStateUIOpen()
    {
        deadState.SetActive(true);
    }
    public void DeadStateUIClose()
    {
        deadState.SetActive(false);
    }

}
