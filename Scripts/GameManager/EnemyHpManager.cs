using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHpManager : MonoBehaviour
{
    public EnemyParam myParam;

    public Image Hp;

    Transform camPosition;

    private void Awake()
    {
        myParam = GetComponentInParent<EnemyParam>();
        camPosition = Camera.main.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + camPosition.rotation * Vector3.forward, camPosition.rotation* Vector3.up);
        Hp.fillAmount = (float)myParam.myHp / (float)myParam.maxHp;
    }
}
