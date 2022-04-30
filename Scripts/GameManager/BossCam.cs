using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCam : MonoBehaviour
{
    public GameObject[] obj;
    public EnemyParam boss;
    public bool aniPlay = false;
    public bool camEnd = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")&&aniPlay == false)
        {
            StartCoroutine(StartBsCam());
        }
    }
    
    IEnumerator StartBsCam()
    {
        aniPlay = true;
        yield return new WaitForSeconds(0.1f);
        obj[0].SetActive(true);
        SoundManager.instance.BossSound();
        yield return new WaitForSeconds(9.3f);
        obj[0].SetActive(false);
    }

    private void Update()
    {
        if (boss.isDead == true && camEnd == false)
            StartCoroutine(DeadBsCam());
    }
    IEnumerator DeadBsCam()
    {
        camEnd = true;
        yield return new WaitForSeconds(1f);
        obj[1].SetActive(true);
        yield return new WaitForSeconds(6f);
        obj[1].SetActive(false);
    }
}
