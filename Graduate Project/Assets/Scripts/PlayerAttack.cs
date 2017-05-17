using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public string weaponType;
    public Player target;
    public int damageValue;
    public string afterEffect;

    public PlayerAttack(string type, Player targ)
    {
        weaponType = type;
        target = targ;

        switch (weaponType)
        {
            case "dagger":
                this.daggerAttack();
                break;
        }

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void daggerAttack()
    {
        damageValue = Random.Range(1, 1);
        afterEffect = "none";

        target.healthPoints -= damageValue;
    }
}
