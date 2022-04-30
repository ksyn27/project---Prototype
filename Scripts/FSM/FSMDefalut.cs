using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMDefalut : MonoBehaviour
{
    #region Variables

    public const int ANI_IDLE = 0;
    public const int ANI_WALK = 1;
    public const int ANI_ATTACK = 2;

    protected Animator my_anim;

    #endregion Variables

    protected enum State
    {
        Idle,
        Walk,
        Attack,
        Hit,
        Dead,
        NoState,
        Skill
 
    }


    // Start is called before the first frame update
    void Awake()
    {
        
    }

    protected void ChangeAni(int state)
    {
        
        my_anim.SetInteger("Anim", state);
    }

}
