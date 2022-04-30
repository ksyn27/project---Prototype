using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BaseParam : MonoBehaviour
{

    public int level { get; set; }

    public float myHp { get; set; }
    public float myMp { get; set; }

    public float maxHp { get; set; }
    public float maxMp { get; set; }

    public float attPow { get; set; }
    public float defense { get; set; }

    public bool isDead { get; set; }

    private void Start()
    {
        Initialized();
    }

    public virtual void Initialized()
    {

    }

    
    public void SetEnemyAttack(float enemyAttackpower)
    {
        myHp -= (enemyAttackpower-defense);
        UpdateReciveAttack();

    }

    protected virtual void UpdateReciveAttack()
    {

        if(myHp <=0)
        {
            myHp = 0;
            isDead = true;
            Debug.Log("Im Dead");
        }
    }
}
