using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyZombie : MonoBehaviour {

    public InfoScript infoScript;

    float _moveSpeed;
    float _turnSpeed;
    float _hitDmg;

    float _attackCooltime;
    float _prevAttackTime;

    GameObject targetAgent;
    ZombieState _zombieState;



    Rigidbody zombieRB;
    MeshRenderer _renderer;

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
     
    ///////////////////////////////////////////////////////////

	void Start ()
    {
        zombieRB = GetComponent<Rigidbody>();
        _renderer = GetComponent<MeshRenderer>();

        InitializeZombie();
    }


    private void FixedUpdate()
    {
        ChasingAgent();
    }

    public void InitializeZombie()
    {
        SetZombieInitValue();
    }

    void SetZombieInitValue()
    {
        infoScript = InfoScript.instance;
        _moveSpeed = infoScript.moveSpeed_zombie;
        _turnSpeed = infoScript.turnDegree_zombie;
        _hitDmg = infoScript.dmg_zombie;
        _hp = infoScript.fullHp_zombie;
        _renderer.material = infoScript.GetHP_Material(true, _hp);

        _zombieState = ZombieState.attackReady;

        _isDead = false; // TODO : change set this in mgr (why? episiode reset)
    }

    /*
    public void ResetZombie()
    {
        SetZombieInitValue();


    }
    */

    public bool UpdateDamage_isDead(float recevDmg)
    {
        Debug.LogWarning("Zombie HIT!!");

        if (_isDead == true)
        {
            Debug.LogError("[Zombie HP ]smth WRONG!!!");
            return true;
        }

        _hp -= recevDmg;


        if(_hp <= 0)
        {
            _isDead = true;
            _hp = 0;

            return true;
        }
        else
        {
            _renderer.material = InfoScript.instance.GetHP_Material(true, _hp);
            return false;
        }
    }

    void ChasingAgent()
    {

    }

    void AttackAgent()
    {
        // dps 기록 



    }

    void SetTarget()
    {
        //TODO : ground Instance로 부터 좀비정보를 받아다가 서치하면서 가장가까운놈 타겟
        //     : agent죽음정보가 들어오면 다시 re searching


    }
}
                 