using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(CharacterController))]
public class FSMEnemy : FSMDefalut
{

    #region Variables
    protected CharacterController controller;
    protected NavMeshAgent agent;
    protected State myState = State.Idle;
    [HideInInspector]
    public Transform target;

    [Header("Distance")]
    [SerializeField]
    protected float findDistance = 5.0f;
    [SerializeField]
    protected float attackDistance = 1f;


    float rotAngleSecond = 360;

    [Space]
    [Header("CheckState")]
    [SerializeField]
    protected bool isAttack = false;
    [SerializeField]
    protected bool isHit = false;
    [SerializeField]
    bool isDie = false;


    [Header("Time")]
    public float delayAttackTime = 1f;
    public float hitDelay = 0.3f;
    public float deadTime = 2f;
    [Space]

   
  
    
    [HideInInspector]
    public PlayerParam enemyParam;
    [HideInInspector]
    public EnemyParam myParam;

    [Header("Object")]
    [SerializeField]
    private AudioClip attackSound;
    public GameObject hitObj;

    #endregion Variables
    private void Awake()
    {
        my_anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        myParam = GetComponent<EnemyParam>();
      
    }

    void TypeDistanceInit()
    {
        switch(myParam.myType)
        {
            case enemyType.Skeleton:
                findDistance = 5.0f;
                attackDistance = 1f;
                break;
            case enemyType.Wizard:
                findDistance = 6.0f;
                attackDistance = 5f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (myParam.isDead == true)
        {
            DeadState();
            return;
        }
        target = GameObject.FindGameObjectWithTag("Player").transform;
        switch (myState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Walk:
                WalkState();
                break;
            case State.Attack:
                AttackState();
                break;
            case State.Hit:
                HitState();
                break;
            case State.Dead:
                break;
        }

    }
    


    void ChangeState(int ani, State newState)
    {
        if (myState == newState)
            return;

        myState = newState;
        ChangeAni(ani);
    }


    void IdleState()
    {
        
        if (FindTarget() <= findDistance && FindTarget() >= attackDistance)
        {
            ChangeState(ANI_WALK, State.Walk);
        }else if (FindTarget() <= attackDistance)
        {
            agent.ResetPath();
            myState = State.Attack;

        }


    }

    protected float FindTarget()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        return distance;
    }

    void WalkState()
    {

        if (!isHit)
        {

            agent.SetDestination(target.position);
            if (FindTarget() <= attackDistance)
            {
                agent.ResetPath();
                myState = State.Attack;

            }
            else if (FindTarget() >= findDistance)
            {
                agent.ResetPath();
                ChangeState(ANI_IDLE, State.Idle);
            }
            else
            {
                TurnToDestination();
                MoveToDestination();
            }
        }
    }
    void TurnToDestination()
    {
       // if (agent.velocity.sqrMagnitude >= 0.1f * 0.1f)
      //  {
            Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAngleSecond);
       // }
    }

    void MoveToDestination()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            controller.Move(agent.velocity * Time.deltaTime);
        }
        else
        {
            controller.Move(Vector3.zero);
            ChangeAni(ANI_IDLE);
            myState = State.Idle;
        }
    }

    void AttackState()
    {
        ChangeAni(ANI_IDLE);
        TurnToDestination();
        if (FindTarget()>= attackDistance && !isAttack)
        {
            ChangeState(ANI_WALK, State.Walk);
        }else
        {
            if (!isAttack && myParam.myType != enemyType.Boss)
                StartCoroutine(NormalAttack());
        }
    }

    IEnumerator NormalAttack()
    {
        isAttack = true;
        switch(myParam.myType)
        {
            case enemyType.Skeleton:
                SoundManager.instance.SoundPlay("Sword", attackSound);
                my_anim.SetTrigger("NormalAttack");
                break;
            case enemyType.Wizard:
                SoundManager.instance.SoundPlay("Magic", attackSound);
                WizardEnemy spell = GetComponent<WizardEnemy>();
                spell.target = target;
                my_anim.SetTrigger("Attack");
                break;

        }
        yield return new WaitForSeconds(delayAttackTime);
        isAttack = false;
    }
    void HitState()
    {
        if(!isHit)
            StartCoroutine(HitStart());
        UIManager.instance.EnemyHpUpdate(myParam);

    }
    IEnumerator HitStart()
    {
        isHit = true;
        hitObj.SetActive(true);
        myParam.SetEnemyAttack(enemyParam.GetAttackPower());
        if (myParam.myType != enemyType.Boss)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            my_anim.SetTrigger("Hit");
            yield return new WaitForSeconds(hitDelay);
            hitObj.SetActive(false);
            isHit = false;
            ChangeState(ANI_IDLE, State.Idle);
        }
        else
        {
            ChangeState(ANI_IDLE, State.Idle);
            yield return new WaitForSeconds(hitDelay);
            isHit = false;
            hitObj.SetActive(false);
        }
    }


    private void LateUpdate()
    {
        transform.position = agent.nextPosition;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Weapon" || other.tag == "PlayerSkill")
        {
            enemyParam = other.GetComponentInParent<PlayerParam>();
                myState = State.Hit;

        }
    }
    void DeadState()
    {
        if (isDie == false && isHit == false)
        {
            isDie = true;
            controller.enabled = false;
            myParam.GiveLoot(enemyParam);
            my_anim.SetTrigger("Dead");
            UIManager.instance.hpOb.SetActive(false);
            StopAllCoroutines();
            
            if (myParam.myType == enemyType.Boss)
               Destroy(gameObject, deadTime);
            else
               StartCoroutine(DeadMonster());   
            
        }
    }

    IEnumerator DeadMonster()
    {
        isAttack = false;
        isHit = false;
        hitObj.SetActive(false);
        yield return new WaitForSeconds(deadTime);
        gameObject.SetActive(false);
        isDie = false;
        controller.enabled = true;  
        my_anim.SetTrigger("Reset");
        ChangeState(ANI_IDLE, State.Idle);
        myParam.isDead = false;
        myParam.myHp = myParam.maxHp;
        EnemyManager emManager = gameObject.GetComponentInParent<EnemyManager>();
        emManager.enemyObjectPool.Add(gameObject);


    }
}
