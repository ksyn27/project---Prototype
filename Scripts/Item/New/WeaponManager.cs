using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static bool isChangeWeapon = false;

    [SerializeField]
    private Weapon[] weapons;

    private Dictionary<string, Weapon> weaponDictionary = new Dictionary<string, Weapon>();

   // [SerializeField]
    //private string currentWeaponType;

    [SerializeField]
    private FSMPlayer player;

    public static Transform currentWeapon;
    


    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = GetComponentInChildren<Weapon>().gameObject.GetComponent<Transform>();
        for (int i = 0; i < weapons.Length; i++)
        {
            weaponDictionary.Add(weapons[i].weaponName, weapons[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeapon == null)
        {
            currentWeapon = GetComponentInChildren<Weapon>().gameObject.GetComponent<Transform>();
            //for (int i = 0; i < weapons.Length; i++)
            //{
            //    weaponDictionary.Add(weapons[i].weaponName, weapons[i]);
            //}
        }
    }

    public void ChangeWeapon(string _name)
    {
        isChangeWeapon = true;
        player.WeaponChange(weaponDictionary[_name]);

        
    }
}
