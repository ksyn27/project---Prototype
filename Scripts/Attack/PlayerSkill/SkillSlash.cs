using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlash : MonoBehaviour
{
    public ParticleSystem slashImpect;
    PlayerParam myParam;
    public int skillAtt = 12;
    public const float skillMp = 20f;
    private void Awake()
    {
        slashImpect = GetComponent<ParticleSystem>();
        myParam = GetComponentInParent<PlayerParam>();
    }


    public void StartSlashSkill()
    {
        StartCoroutine(SlashTime());
    }

    IEnumerator SlashTime()
    {
        myParam.SetUseSkillMp(skillMp);
        slashImpect.Play();
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Monster")
        {
            other.GetComponent<EnemyParam>().SetEnemyAttack(myParam.GetAttackPower() + skillAtt);
        }
    }

}
