using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyZombie : MonoBehaviour {

    public InfoScript infoScript;

    float _moveSpeed;
    float _turnSpeed;
    float _hitDmg;

    Rigidbody zombieRB;

    public bool isDead
    {
        get { return _isDead; }
    }

    bool _isDead;

    float _hp;
    public float hp
    {
        get { return _hp; }
    }
     
    
	void Start ()
    {
        zombieRB = GetComponent<Rigidbody>();
        initializeZombie();
    }


    private void FixedUpdate()
    {
        
    }




    void initializeZombie()
    {
        infoScript = InfoScript.instance;
        _moveSpeed = infoScript.moveSpeed_zombie;
        _turnSpeed = infoScript.turnSpeed_zombie;
        _hitDmg = infoScript.dmg_zombie;
        _hp = infoScript.fullHp_zombie;

        _isDead = true; // TODO : set this in mgr
    }

    public void UpdateDamage(float recevDmg)
    {
        if (_isDead == true)
        {
            Debug.LogError("[Zombie HP ]smth WRONG!!!");
            return;
        }

        _hp -= recevDmg;

        if(_hp <= 0)
        {
            _isDead = true;
            _hp = 0;
        }



    }
}
