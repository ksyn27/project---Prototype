using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyManager : MonoBehaviour
{
    float currentTime;
    public float createTime = 5;

    public FSMEnemy[] enemys;
    public int poolSize;

    public List<GameObject> enemyObjectPool;
    public Vector3[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        enemys = gameObject.GetComponentsInChildren<FSMEnemy>();
        poolSize = enemys.Length;
        enemyObjectPool = new List<GameObject>();
        spawnPoints = new Vector3[enemys.Length]; 
        for (int i = 0; i < poolSize; i++)
        {

            spawnPoints[i] = enemys[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (enemyObjectPool.Count > 0)
        {
           
            currentTime += Time.deltaTime;
            GameObject enemy = enemyObjectPool[0];
            //1ÃÊÁö³²
            if (currentTime > createTime)
            {
                enemy.SetActive(true);

                enemyObjectPool.Remove(enemy);

                int index = Random.Range(0, spawnPoints.Length);

                enemy.GetComponent<NavMeshAgent>().enabled = false;
                enemy.transform.position = spawnPoints[index];
                enemy.GetComponent<NavMeshAgent>().enabled = true;
                currentTime = 0;
            }
        }

    }
}
