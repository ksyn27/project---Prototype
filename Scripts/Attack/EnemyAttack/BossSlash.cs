using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossSlash : MonoBehaviour
{
    [SerializeField] private int damage;
    public float deleteTime = 1.0f;
    public EnemyParam myParam;
    public Collider col;
    public float attackDelay;
    [SerializeField] private AudioClip slashClip;

   private void Start()
    {
        col = GetComponent<BoxCollider>();
        col.enabled = false;
        StartCoroutine(AttackTime());
    }

    IEnumerator AttackTime()
    {        
        SoundManager.instance.SoundPlay("slash", slashClip);
        yield return new WaitForSeconds(attackDelay);

        col.enabled = true;
    }
    private void Update()
    {
        Destroy(gameObject, deleteTime);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerParam enemy = other.GetComponent<PlayerParam>();

            enemy.SetEnemyAttack(myParam.EnemyRandomAttack() + damage);
        }
    }
}
