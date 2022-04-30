using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public GameObject target;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        
        target.transform.position = gameObject.transform.position;
    }

    private void Update()
    {
        target.transform.position = gameObject.transform.position;
    }
}
