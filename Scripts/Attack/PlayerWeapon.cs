using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject weaponPos;
    public Collider col;

    private void Start()
    {
        col = weaponPos.GetComponentInChildren<Collider>();
    }
    private void Update()
    {
        if(col == null)
            col = weaponPos.GetComponentInChildren<Collider>();
    }


    public void Use_Weapon()
    {
        col.enabled = true;
    }

    public void Stop_Weapon()
    {
        col.enabled = false;
    }
}
