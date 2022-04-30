using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardEnemy : MonoBehaviour
{
    public GameObject spell;
    public Transform spellPos;
    public Transform target;

    public void SpellAttack()
    {
        GameObject attack = Instantiate(spell,spellPos.position,Quaternion.identity);
        
        attack.GetComponent<EnemyBullet>().target = target;
        attack.GetComponent<EnemyBullet>().myParam = GetComponent<FSMEnemy>().myParam;
    }
    

}
