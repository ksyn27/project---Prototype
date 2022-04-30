using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : FSMEnemy
{ 

    
    [Header("Attack")]
    [SerializeField] private int attackNum = 0;
    [SerializeField] private float slashDis = 9f;
    [SerializeField] private float norDis = 6f;
    [SerializeField] private float tauntDis = 3f;
    [SerializeField] private float skillZone = 7f;

    [Header("SkillTime")]
    public float slashDelay = 5f;
    public float norDelay = 1f;
    public float tauntDelay = 3f;

    [Header("SkillTime")]
    public bool slashCheck = false;
    public bool tauntCheck = false;

    [Header("SKILLOBJ")]
    [SerializeField] private GameObject slashAtEff;
    [SerializeField] private GameObject tauntAtEff;
    [SerializeField] private GameObject norAtEff;


 

    private void Awake()
    {
        my_anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        myParam = GetComponent<EnemyParam>();
 
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AttackAI());
 
    }


    IEnumerator AttackAI()
    {
        yield return new WaitForSeconds(0.1f);
        AttackDistanceCheck();
        yield return new WaitForSeconds(1f);
        
        isAttack = true;
        if (myState == State.Attack)
        {
            switch (attackNum)
            {
                case 3:
                    StartCoroutine(AttackSlash());
                    break;
                case 2:
                    StartCoroutine(NormalSlash());
                    break;
                case 1:
                    StartCoroutine(TauntAttack());
                    break;
                case 0:
                    StartCoroutine(AttackAI());
                    break;
            }
        }
        else
            StartCoroutine(AttackAI());
        
    }

    void AttackDistanceCheck()
    {
        if (FindTarget() <= findDistance)
        {
            if (skillZone <= FindTarget() && !slashCheck)
            {
                attackDistance = slashDis;
                attackNum = 3;
            }
            else if(FindTarget() <= skillZone || slashCheck == true)
            {
                attackDistance = norDis;
                attackNum = Random.Range(1, 3);
            }
        }
        Debug.Log(myState);
    }


    IEnumerator AttackSlash()
    {
        isAttack = true;
        my_anim.SetTrigger("Attack");
        my_anim.SetInteger("AttackNum",attackNum);
        StartCoroutine(SkillCoolTime(slashDelay));
        yield return new WaitForSeconds(3f);
        isAttack = false;
        StartCoroutine(AttackAI());
    }
    IEnumerator NormalSlash()
    {
        isAttack = true;

        my_anim.SetTrigger("Attack");
        my_anim.SetInteger("AttackNum", attackNum);
        yield return new WaitForSeconds(2.5f);
        isAttack = false;
        StartCoroutine(AttackAI());

    }
    IEnumerator TauntAttack()
    {
        isAttack = true;
        my_anim.SetTrigger("Attack");
        my_anim.SetInteger("AttackNum", attackNum);
        yield return new WaitForSeconds(3f);
        isAttack = false;
        StartCoroutine(AttackAI());

    }


    IEnumerator SkillCoolTime(float coolTime)
    {
        slashCheck = true;
        while(coolTime >1.0f)
        {
            coolTime -= Time.deltaTime;
            //coll.text = coolTime.ToString();
            yield return new WaitForFixedUpdate();
        }
        slashCheck = false;

    }

    public void ShotSlash()
    {
        GameObject attSkill = Instantiate(slashAtEff, transform.position, Quaternion.identity);
        attSkill.transform.LookAt(target);
        attSkill.GetComponent<BossSlash>().myParam = myParam;
    }
    public void ShotTaunt()
    {
        GameObject attSkill = Instantiate(tauntAtEff, transform.position, Quaternion.identity);
        attSkill.GetComponent<BossSlash>().myParam = myParam;
    }
    public void ShotNor()
    {
        GameObject attSkill = Instantiate(norAtEff, transform.position, Quaternion.identity);
        attSkill.transform.LookAt(target);
        attSkill.GetComponent<BossSlash>().myParam = myParam;
    }
    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, slashDis);


        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, findDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, norDis);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, skillZone);


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

    }
}
