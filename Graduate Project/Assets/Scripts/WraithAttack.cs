using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraithAttack : MonoBehaviour {
    public int attackType;
    public Player target;
    public string attackName;
    public int damageValue;
    public string afterEffect;

    public WraithAttack(int type, Player targ)
    {
        attackType = type;
        target = targ;

        if (attackType == 1)
        {
            this.spectralBlast();
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

    protected void spectralBlast()
    {
        attackName = "Spectral Blast";
        damageValue = 10;
        afterEffect = "none";
    }
}
