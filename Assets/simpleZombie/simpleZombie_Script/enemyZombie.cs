using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyZombie : MonoBehaviour {

    public float attackRange = 1.5f;

    float _moveSpeed;
    float _turnSpeed;
    float _hitDmg;

    float _attackCooltime;
    float _prevAttackTime;

    GameObject _targetAgent = null;
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

	void Awake ()
    {
        zombieRB = GetComponent<Rigidbody>();
        _renderer = GetComponent<MeshRenderer>();

        //InitializeZombie();
    }

    private void OnEnable()
    {
        InitializeZombie();
    }

    private void FixedUpdate()
    {
        if(_isDead == true)
        {
            return;
        }

        _targetAgent = SetTarget();
        Chasing_Attack_Agent();
    }

    public void InitializeZombie()
    {
        SetZombieInitValue();
    }

    void SetZombieInitValue()
    {
        InfoScript infoScript = InfoScript.instance;
        _moveSpeed = infoScript.moveSpeed_zombie;
        _turnSpeed = infoScript.turnDegree_zombie;
        _hitDmg = infoScript.dmg_zombie;
        _hp = infoScript.fullHp_zombie;
        _renderer.material = infoScript.GetHP_Material(true, _hp);
        _attackCooltime = infoScript.coolTime_zombie;
        _prevAttackTime = 0f;
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

    GameObject SetTarget()
    {
        //var agentList = zombieGround.instance._playerAgentList;

        var zombieGroundInst = zombieGround.instance;

        GameObject target = null;
        float minDist = float.MaxValue;

        for (int i=0; i< zombieGroundInst._playerAgentList.Count; ++i)
        {
            // 만약 죽어 null 이면 return 

            var dist = Vector3.Distance(zombieGroundInst._playerAgentList[i].transform.position, transform.position);

            if ( dist < minDist && zombieGroundInst._playerAgentList[i].activeSelf == true)
            {
                target = zombieGroundInst._playerAgentList[i];
                minDist = dist;
            }
        }
        return target;
    }

    void Chasing_Attack_Agent()
    {
        if (_targetAgent == null || _targetAgent.activeSelf == false)
        {
            Debug.LogWarning(gameObject.name + ":" + " target NULL");
            return;
        }

        float distance = (_targetAgent.transform.position - transform.position).magnitude;
        if (distance < attackRange && _prevAttackTime + _attackCooltime < Time.time)
        {
            //enemyState = ENEMYSTATE.ATTACK;
            AttackAgent();
        }
        else
        {
            Vector3 dir = _targetAgent.transform.position - transform.position;
            dir.y = 0.0f;
            dir.Normalize();

            zombieRB.AddForce(dir * _moveSpeed * 100f, ForceMode.Force);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), _turnSpeed * Time.deltaTime);

            //characterController.SimpleMove(dir * moveSpeed);
        }
    }

    void AttackAgent()
    {
        if (_targetAgent == null || _targetAgent.activeSelf == false)
        {
            Debug.LogWarning(gameObject.name + ":" + " target NULL");
            return;
        }
        _targetAgent.GetComponent<playerAgent>().AttackByZombie(_hitDmg);
        _prevAttackTime = Time.time;
    }

}
