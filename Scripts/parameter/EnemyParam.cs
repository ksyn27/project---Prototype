using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyType
{
    Skeleton,
    Wizard,
    Boss
}
public class EnemyParam : BaseParam
{
    public string name { get; set; }
    public int exp { get; set; }
    public int rewardMoney { get; set; }
    public int attackMin { get; set; }
    public int attackMax { get; set; }

    
    public enemyType myType;

    public GameObject[] dropItem;
   

    public override void Initialized()
    {
        name = "Monster";
        switch (myType)
        {
            case enemyType.Skeleton:
                maxHp = 100;
                myHp = maxHp;
                attackMin = 2;
                attackMax = 5;
                defense = 1;
                exp = 10;
                break;
            case enemyType.Wizard:
                maxHp = 80;
                myHp = maxHp;
                attackMin = 3;
                attackMax = 5;
                defense = 1;
                exp = 10;
                break;
            case enemyType.Boss:
                maxHp = 1000;
                myHp = maxHp;
                attackMin = 2;
                attackMax = 5;
                defense = 1;
                exp = 10;
                break;


        }
        rewardMoney = Random.Range(10, 31);
        isDead = false;
    }
    public int EnemyRandomAttack()
    {
        int randomAttack = Random.Range(attackMin, attackMax + 1);

        return randomAttack;
    }

    protected override void UpdateReciveAttack()
    {
        base.UpdateReciveAttack();
    }

    public void GiveLoot(PlayerParam enemy)
    {
        enemy.myExp += exp;
        enemy.myMoney += rewardMoney;

        int randomDrop = Random.Range(0, 90);

        if (randomDrop >= 25 && randomDrop < 45)
            Instantiate(dropItem[0] , transform.position, Quaternion.identity);
        else if (randomDrop >= 45 && randomDrop < 65)
            Instantiate(dropItem[1], transform.position, Quaternion.identity);
        else if(randomDrop >65)
            Instantiate(dropItem[Random.Range(2,4)], transform.position, Quaternion.identity);
    }

    
}
