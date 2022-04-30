using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerParam : BaseParam
{
    public string myName { get; set; } 
    public int myExp { get; set; }
    public int nextToLevelExp { get; set; } 
    public int myMoney { get; set; }

    public int itemPower { get; set; }

    bool levelUp = false;

    float currTime = 0;
    public float cureTime = 3f;
    public float cureHeal = 3f;
    
    public override void Initialized()
    {
        myName = DataManager.instance.playerData.name;
        level = DataManager.instance.playerData.level;
        maxHp = 100;
        maxMp = 100;
        defense = 1;
        attPow = 10;
        nextToLevelExp = 100;
        SetStatus();

        myMoney = DataManager.instance.playerData.money;
        myExp = DataManager.instance.playerData.exp;
        myHp = DataManager.instance.playerData.myHp;
        myMp = DataManager.instance.playerData.myMp;

       
    }
    public float GetAttackPower()
    {
        float powerSum = attPow + itemPower;

        return powerSum;
    }

    void LevelUp()
    {
        level++;
        levelUp = true;
        SetStatus();
    }

    public void SetStatus()
    {
        maxHp =100 + (level * 2f);
        if (levelUp == true) { myHp = maxHp; levelUp = false; }  
        maxMp =100 + (level * 2f);
        myMp = maxMp;
        defense = 1+(level / 5f);
        attPow = 10 + (level / 2f);
        nextToLevelExp = 100 * level;
        UIManager.instance.SetStatus();
    }

    public bool CheckMp(float skillmp)
    {

        if(skillmp<=myMp)
        {
            return true;
        }
        return false;
    }
    public void SetUseSkillMp(float skillMp)
    {
        myMp -= skillMp;
       
    }
   

    protected override void UpdateReciveAttack()
    {
        base.UpdateReciveAttack();
    }
    public void IncreaseHP(float _point)
    {
        if (myHp + _point < maxHp)
            myHp += _point;
        else
            myHp = maxHp;

    }
    public void IncreaseMP(float _point)
    {
        if (myMp + _point < maxMp)
            myMp += _point;
        else
            myMp = maxMp;

    }

    private void Update()
    {
        if (nextToLevelExp <= myExp)
        {
            myExp -= nextToLevelExp;
            LevelUp();
        }


        if (isDead == false)
        {
            SetHealth();

            UIManager.instance.UpdatePlayerUI(this);
        }
    }

    void SetHealth()
    {
        currTime += Time.deltaTime;

        if (currTime > cureTime)
        {

            if (myHp < maxHp)
            {
                myHp += cureHeal;
                UIManager.instance.UpdatePlayerUI(this);
            }
            else
                myHp = maxHp;

            if (myMp < maxMp)
            {
                myMp += cureHeal;
                UIManager.instance.UpdatePlayerUI(this);
            }
            else
                myMp = maxMp;

            currTime = 0f;
        }
    }

    public void SetRestart()
    {
        isDead = false;
        myHp = maxHp;
        myMp = maxMp;
        gameObject.GetComponent<FSMPlayer>().ReStart();
    }
}
