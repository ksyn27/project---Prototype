using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FSMPlayer : FSMDefalut
{
    #region Variables
  
    State myState = State.Idle;

    [Header("Speed & Rot")]
    [SerializeField]
    private float rotAnglePerSecond = 360f;
    [SerializeField]
    private float moveSpeed = 3.5f;

 
    private CharacterController characterController;
    private NavMeshAgent agent;

    [Header("PlayerCheck")]
    public bool isAttack = false;
    public bool isHit = false;
    public bool isSkill_1 = false;
    public bool isSkill_2 = false;
    public bool isDash = false;
    bool isDie = false;
    bool dashToMove = false;
    [Space]
    [Header("CoolTime")]
    public Image cool1;
    public Image cool2;
    public float skillC1 = 5f;
    public float skillC2 = 6f;
    public float attackDelay = 0.5f;
    [Space]
    [Header("Paramater")]
    public PlayerParam myParam;
    public EnemyParam enemyParam;


    public float myAttackPower;
    
   
    [SerializeField] private GameObject[] skillSlash;
    int skillindex;

    [Space]
    [Header("SoundClip")]
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip slashClip;


    Weapon currentWeapon;

    [HideInInspector]
    public bool isPotal = false;

    #endregion Variables
    private void Awake()
    {
        var obj = FindObjectsOfType<FSMPlayer>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);
        currentWeapon = GetComponentInChildren<Weapon>();
        my_anim = GetComponent<Animator>();
        myParam = GetComponent<PlayerParam>();
        cool1 = GameObject.Find("coolTime1").GetComponent<Image>();
        cool2 = GameObject.Find("coolTime2").GetComponent<Image>();

    }
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();
        agent.updatePosition = false;
        agent.updateRotation = false;

        if (DataManager.instance.playerData.playerPos != Vector3.zero)
        {
            agent.enabled = false;
            transform.position = DataManager.instance.playerData.playerPos;
            agent.enabled = true;
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
        myAttackPower = myParam.GetAttackPower();
        //MoveState();
        switch(myState)
        {
            case State.Idle:
                break;

            case State.Walk:
                MoveState();
                break;

            case State.Attack:
                AttackState();
                break;

            case State.Hit:
                //HitState();
                break;
            case State.Skill:
                SkillState();
                break;
        }
 
            
    }

    void DeadState()
    {
        if (isDie == false)
        {
            isDie = true;
            characterController.enabled = false;
            agent.ResetPath();
            agent.enabled = false;
            my_anim.SetTrigger("Dead");
            StopAllCoroutines();
            GameManager.instance.DeadReStart();
        }
    }

    public void ReStart()
    {

        isDie = false;
        characterController.enabled = true;
        Transform resPos = GameObject.Find("RestartPos").transform;
        transform.position = resPos.position;
        agent.enabled = true;
        my_anim.SetTrigger("Restart");

    }

    public void MoveCharactor(Vector3 des)
    {
        if (!isAttack && !myParam.isDead && !dashToMove)
        {
            ChangeAni(ANI_WALK);
            myState = State.Walk;
            agent.ResetPath();
            agent.SetDestination(des);
        }
        

    }


    private void MoveState()
    {
        TurnToTargetPos();
        MoveToTargetPos();
    }

    void TurnToTargetPos()
    {
        if (agent.velocity.sqrMagnitude >= 0.1f * 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(agent.steeringTarget - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecond);
       
         }    
    }
    void MoveToTargetPos()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            characterController.Move(agent.velocity * Time.deltaTime);
        }
        else
        {
            Debug.Log("sss");
            characterController.Move(Vector3.zero);
            ChangeAni(ANI_IDLE);
            myState = State.Idle;
        }
    }

    public void ComboSkill(Vector3 hit,int index)
    {
        if (!isAttack && !myParam.isDead)
        {
            skillindex = index;
            switch(index)
            {
                case 0:
                    if(isSkill_1 == false)
                    {
                        agent.ResetPath();
                        agent.isStopped = true;
                        agent.velocity = Vector3.zero;
                        transform.LookAt(hit);
                        myState = State.Skill;
                        SoundManager.instance.SoundPlay("Slash", slashClip);
                        StartCoroutine(SkillCoolTime(skillC1));
                    }
                    break;
                case 1:
                    if (isSkill_2 == false)
                    {
                        agent.ResetPath();
                        agent.isStopped = true;
                        agent.velocity = Vector3.zero;
                        transform.LookAt(hit);
                        myState = State.Skill;
                        StartCoroutine(SkillCoolTime(skillC2));
                    }
                    break;
            }
            
        }
    }

    public void SkillState()
    {
        if(!isAttack)
        StartCoroutine(ComboAttack());
    }

    IEnumerator ComboAttack()
    {
        isAttack = true;
        my_anim.SetInteger("SkillIndex", skillindex);
        my_anim.SetTrigger("Skill");
        yield return new WaitForSeconds(1.5f);
        myState = State.Idle;
        ChangeAni(ANI_IDLE);
        isAttack = false;
        agent.isStopped = false;

    }


    IEnumerator SkillCoolTime(float coolTime)
    {
        float fullTime = coolTime;
        switch (skillindex)
        {
            case 0:
                isSkill_1 = true;
                cool1.enabled = true;
                while (coolTime > 0f)
                {
                    coolTime -= Time.deltaTime;
                        cool1.fillAmount = coolTime / fullTime;
                    yield return new WaitForFixedUpdate();
                }
                isSkill_1 = false;
                cool1.enabled = false;
                break;

            case 1:
                isSkill_2 = true;
                cool2.enabled = true;
                while (coolTime > 0f)
                {
                    coolTime -= Time.deltaTime;
                        cool2.fillAmount = coolTime / fullTime;

                    yield return new WaitForFixedUpdate();
                }
                isSkill_2 = false;
                cool2.enabled = false;

                break;

        }

    

    }

    public void SkillStart()
    {
        GameObject skill_ =  Instantiate(skillSlash[skillindex], transform);

        if(skillindex == 0)
            skill_.GetComponent<SkillSlash>().StartSlashSkill();
        if (skillindex == 1)
            skill_.GetComponent<SkillCombo>().StartComboSkill();
    }


    public void ClickAttack(Vector3 hit)
    {
        if (!isAttack && !myParam.isDead)
        {
            myState = State.Attack;
            agent.ResetPath();
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            transform.LookAt(hit);
            Debug.Log("ssss");
            // ChangeAni(ANI_ATTACK);
            myState = State.Attack;
        }
    }

    void AttackState()
    {

        if (!isAttack)
            StartCoroutine(Attack_time());
    }
    IEnumerator Attack_time()
    {
        my_anim.SetTrigger("NormalAttack");
        SoundManager.instance.SoundPlay("normal", attackClip);
        isAttack = true;
        yield return new WaitForSeconds(attackDelay);
        myState = State.Idle;
        ChangeAni(ANI_IDLE);
        isAttack = false;
        agent.isStopped = false;


    }

    void HitState()
    {
        myParam.SetEnemyAttack(enemyParam.EnemyRandomAttack());
    }


    private void LateUpdate()
    {
        transform.position = agent.nextPosition;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyWeapon")
        {
            enemyParam = other.GetComponentInParent<EnemyParam>();
            //myState = State.Hit;
            HitState();
        }


        //if (other.CompareTag("Item"))
        //{
        //    //FieldItem fielditem = other.GetComponent<FieldItem>();
        //    //if (Inventory.instance.AddItem(fielditem.GetItem()))
        //    //    fielditem.DestroyItem();
        //}

    }

    public void Dash(Vector3 point)
    {

        if (!isDash)
        {
            isDash = true;
            dashToMove = true;
            agent.velocity = Vector3.zero;
            agent.ResetPath();
            my_anim.SetTrigger("Dodge");
            agent.SetDestination(point);
            transform.LookAt(point);
            agent.speed = 1000f;

            StartCoroutine(StopDash(point));
        }
    }
    IEnumerator StopDash(Vector3 point)
    {
        yield return new WaitForSeconds(0.3f);
        dashToMove = false;
        agent.ResetPath();
        agent.speed = moveSpeed;
        yield return new WaitForSeconds(2f);
        isDash = false;

    }

    public void WeaponChange(Weapon _weapon)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        currentWeapon = _weapon;
        WeaponManager.currentWeapon = currentWeapon.GetComponent<Transform>();
        PlayerWeapon pw = gameObject.GetComponent<PlayerWeapon>();
        pw.col = currentWeapon.GetComponent<Collider>();
        currentWeapon.gameObject.SetActive(true);
    }

}
