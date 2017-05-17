using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorgrenAttack : MonoBehaviour
{

    public int attackType;
    public Player target;
    public string attackName;
    public int damageValue;
    public string afterEffect;

    public ThorgrenAttack(int type, Player targ)
    {
        attackType = type;
        target = targ;

        if (attackType == 1)
        {
            this.smash();
        }
        else if (attackType == 2)
        {
            this.kick();
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

    protected void smash()
    {
        Thorgren thor = GameManager.instance.thorgrenScript.thor;
        attackName = "Smash";
        damageValue = 15;
        if (thor.rightArmHP == 0)
        {
            damageValue -= 5;
        }
        if (thor.leftArmHP == 0)
        {
            damageValue -= 5;
        }
        afterEffect = "none";
    }
    protected void kick()
    {
        Thorgren thor = GameManager.instance.thorgrenScript.thor;
        attackName = "Kick";
        damageValue = 15;
        if (thor.rightLegHP == 0)
        {
            damageValue -= 5;
        }
        if (thor.leftLegHP == 0)
        {
            damageValue -= 5;
        }
        afterEffect = "none";
    }
}