using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    public int damage;
    public float deleteTime = 3.0f;

    public Transform target;
    NavMeshAgent agent;
    public EnemyParam myParam;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

    }
    private void Update()
    {
        agent.SetDestination(target.position);

        Destroy(gameObject, deleteTime);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerParam>().SetEnemyAttack(myParam.EnemyRandomAttack() + damage);
            Debug.Log(myParam.EnemyRandomAttack() + damage);
            Destroy(gameObject);
        }
    }
}