using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
public class Potal : MonoBehaviour
{
    GameObject player;

    [SerializeField]
    private Transform startPos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player.GetComponent<FSMPlayer>().isPotal = true;
            if (SceneManager.GetActiveScene().name == "Village")
            {
                // GameManager.instance.SavePlayerDataJason();
                SceneLoad.LoadScene("Dungeon");
            }
            else if (SceneManager.GetActiveScene().name == "Dungeon")
            {
                SceneLoad.LoadScene("Boss");
            }
            else if (SceneManager.GetActiveScene().name == "Boss")
            {
                SceneLoad.LoadScene("Village");
            }
        }
    }

    private void Awake()
    {
       player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<FSMPlayer>().isPotal == true)
        {
            startPos = GameObject.Find("StartPos").transform;
            player.GetComponent<NavMeshAgent>().enabled = false;

            player.transform.position = startPos.position;

            player.GetComponent<NavMeshAgent>().enabled = true;
            player.GetComponent<FSMPlayer>().isPotal = false;
        }
    }
}
