using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    PlayerParam player;

    [SerializeField]
    private int itemPower;

    private void Awake()
    {
        player = GetComponentInParent<PlayerParam>();
        player.itemPower = itemPower;
    }
}
