using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCombo : MonoBehaviour
{
    public ParticleSystem ComboImpect;
    PlayerParam myParam;
    public int skillAtt = 12;
    public const float skillMp = 50f;
    BoxCollider myCol;
    [SerializeField] private AudioClip[] comboClip;
    private void Awake()
    {
        ComboImpect = GetComponent<ParticleSystem>();
        myParam = GetComponentInParent<PlayerParam>();
        myCol = GetComponent<BoxCollider>();
    }


    public void StartComboSkill()
    {
        StartCoroutine(ComboTime());
    }

    IEnumerator ComboTime()
    {
        myParam.SetUseSkillMp(skillMp);
        ComboImpect.Play();
        yield return new WaitForSeconds(0.3f);
        SoundManager.instance.SoundPlay("1slash", comboClip[0]);
        yield return new WaitForSeconds(0.1f);
        myCol.enabled = true;
        yield return new WaitForSeconds(0.1f);
        myCol.enabled = false;
        SoundManager.instance.SoundPlay("2slash", comboClip[0]);
        yield return new WaitForSeconds(0.1f);
        myCol.enabled = true;
        yield return new WaitForSeconds(0.1f);
        myCol.enabled = false;
        SoundManager.instance.SoundPlay("3slash", comboClip[1]);
        yield return new WaitForSeconds(0.4f);
        myCol.enabled = true;
        yield return new WaitForSeconds(0.1f);
        myCol.enabled = false;
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            other.GetComponent<EnemyParam>().SetEnemyAttack(myParam.GetAttackPower() + skillAtt);
        }


    }



}
