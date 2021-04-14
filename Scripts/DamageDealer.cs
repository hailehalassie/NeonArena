using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour {

    [SerializeField] int damage1 = 100;
    [SerializeField] int damage2 = 400;


    public int GetDamage1()
    {
        return damage1;
    }

    public int GetDamage2()
    {
        return damage2;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
