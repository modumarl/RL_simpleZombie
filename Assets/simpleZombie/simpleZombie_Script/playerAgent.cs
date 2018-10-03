using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAgent : Agent
{
    public InfoScript infoScript;

    //List<GameObject> _perceiveZombieList = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> aroundZombieList = new List<GameObject>();

    GameObject _targetZombie_nearest;
    GameObject _targetZombie_minimumHP;

    public LineRenderer lineR_minimumHp;
    public LineRenderer lineR_nearst;


    // ****** TODO ******
    // heading Vector
    // Type입력하면 바뀌어서 능력치 
    // playerType 
    // *******************

    RayPerception _rayPercept;
    Rigidbody agentRB;
    AgentState _curState;
    MeshRenderer _renderer;

    float _shotCoolTime;
    float _prevShotTime;
    float _hp;
    public float hp
    {
        get { return _hp; }
    }


    int _hitCall = 0;



    //////*imported value*//////
    float _shotRange;
    float _shotDegree;

    float _moveSpeed;
    float _turnDegree;
    float _rayDistance;
    float _hitDmg;
    float[] _rayAngle = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
    string[] _detectableObjects = { "zombie", "wall", "playerAgent", "obstacle" };
    ///////////////////////////

    //////////////////////////////////////////////////////////////////////////

    public override void InitializeAgent()
    {
        //Debug.LogWarning("INIT CALLED");
        infoScript = InfoScript.instance;

        base.InitializeAgent();

        _rayPercept = GetComponent<RayPerception>();
        agentRB = GetComponent<Rigidbody>();
        _renderer = GetComponent<MeshRenderer>();

        _moveSpeed = infoScript.moveSpeed_playerAgent;
        _turnDegree = infoScript.turnDegree_playerAgent;
        _rayDistance = infoScript.rayDistacne_agent;
        _shotDegree = infoScript.shotDegree;
        _shotRange = infoScript.shotRange;

        _hitDmg = infoScript.dmg_playerAgent;
        //_rayAngle = infoScript.rayAngle_agent;
        _hp = infoScript.fullHp_playerAgent;
        _renderer.material = infoScript.GetHP_Material(false, _hp);

        _shotCoolTime = infoScript.shotCoolTime_playerAgent;
        _curState = AgentState.shotReady;

        ResetAgentValue();
    }



    public override void CollectObservations()
    {
        //_rayAngle = infoScript.rayAngle_agent;
        //Debug.Log(_rayAngle);

        // --- just for use Loop --------
        //PerceiveZombie();

        //if(_perceiveZombieList.Count !=0)
        if(aroundZombieList.Count != 0)
        {
            SetTarget();
            SetLineRenderer();
        }

        SetState();

        //--------------------------------



        /////////////

        AddVectorObs(_rayPercept.Perceive(_rayDistance, _rayAngle, _detectableObjects, 0f, 0f));
        Vector3 localVelocity = transform.InverseTransformDirection(agentRB.velocity);
        AddVectorObs(localVelocity.x);
        AddVectorObs(localVelocity.z);
        AddVectorObs(System.Convert.ToInt32(_curState));
        AddVectorObs(Time.time - _prevShotTime);
        AddVectorObs(_hp);

        //todo 
        // nearZombieList 관련한 정보값
        //AddVectorObs(_perceiveZombieList.Count);
        AddVectorObs(aroundZombieList.Count);

        //hp가 일정치 이하인 좀비의 카운트?

    
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {

        /*
        if (_curState == AgentState.dead)
            return;
        */


        int action = Mathf.FloorToInt(vectorAction[0]);
        Vector3 dirToGo = Vector3.zero;

        if (action == 1) // up arrow
        {
            dirToGo = transform.forward;
            agentRB.AddForce(dirToGo * _moveSpeed * 250f, ForceMode.Force);


            /*
            // change to Translate???
            transform.Translate(transform.forward * _moveSpeed);
            Debug.LogWarning("FORWARD = " + transform.forward);
            */
        }

        else if (action == 2) // down arrow
        {
            dirToGo = transform.forward * -1;
            agentRB.AddForce(dirToGo * _moveSpeed * 250f, ForceMode.Force);


            /*
            // change to Translate???
            transform.Translate(-1 * transform.forward * _moveSpeed);
            Debug.LogWarning("FORWARD = " + transform.forward);
            */
        }

        else if (action == 3) // left arrow
        {
            transform.Rotate(transform.up * -1 * _turnDegree);
            //Debug.LogWarning("FORWARD = " + transform.forward);
        }
        else if (action == 4) // right arrow
        {
            transform.Rotate(transform.up * _turnDegree);
            //Debug.LogWarning("FORWARD = " + transform.forward);

        }
        else if (action == 5) // shot minimum HP
        {
            Shot(true);
        }
        else if (action == 6) // shot nearest
        {
            Shot(false);
        }

        else if (action == 7) // idle
        {
        }
    }

    public override void AgentReset()
    {
        ResetAgentValue();
    }

    public override void AgentOnDone()
    {

    }

    ////////////////////////////////////////////////////////////////////////////////

    void PerceiveZombie()
    {
        //_perceiveZombieList = _rayPercept.PerceiveObjectList(_rayDistance, _rayAngle, "zombie", 0f, 0f);

        //Debug.LogWarning("listCount = " + _perceiveZombieList.Count);
        //for (int i = 0; i < _perceiveZombieList.Count; ++i)
        //{
        //    Debug.Log(_perceiveZombieList[i].name + " " + _perceiveZombieList[i].GetInstanceID());
        //}
    }
    void SetTarget()
    {
        //if(_perceiveZombieList.Count == 0)
        if(aroundZombieList.Count  == 0)
        {
            return;
        }

        //_targetZombie_minimumHP = _perceiveZombieList[0];
        //_targetZombie_nearest = _perceiveZombieList[0];
        _targetZombie_minimumHP = aroundZombieList[0];
        _targetZombie_nearest = aroundZombieList[0];

        var minHp = _targetZombie_minimumHP.GetComponent<enemyZombie>().hp;
        var minDist = Vector3.Distance(_targetZombie_nearest.transform.position, transform.position);

        //for (int i=0; i<_perceiveZombieList.Count; ++i)
        for(int i = 0; i< aroundZombieList.Count; ++i)
        {
            //var thisZombieHp = _perceiveZombieList[i].GetComponent<enemyZombie>().hp;
            var thisZombieHp = aroundZombieList[i].GetComponent<enemyZombie>().hp;

            if (minHp > thisZombieHp)
            {
                //_targetZombie_minimumHP = _perceiveZombieList[i];
                _targetZombie_minimumHP = aroundZombieList[i];
                minHp = thisZombieHp;
            }

            //float thisZombieDist = Vector3.Distance(_perceiveZombieList[i].transform.position, transform.position);
            float thisZombieDist = Vector3.Distance(aroundZombieList[i].transform.position, transform.position);

            if (minDist > thisZombieDist)
            {
                //_targetZombie_nearest = _perceiveZombieList[i];
                _targetZombie_nearest = aroundZombieList[i];

                minDist = thisZombieDist;
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////

    public void AttackByZombie(float inputDmg)
    {
        // TODO : SendReward

        UpdateHp(inputDmg);
        Debug.Log(_curState + " curHP = " + _hp);
    }
    void UpdateHp(float inputDmg)
    {
        if(_curState == AgentState.dead)
        {
            return;
        }

        if(_hp > inputDmg)
        {
            _hp -= inputDmg;
            _renderer.material = infoScript.GetHP_Material(true, _hp);
            AddReward(InfoScript.instance.reward_receiveDamage);
        }

        else
        {
            _hp = 0;
            _curState = AgentState.dead;
            AddReward(InfoScript.instance.reward_agentDeath);

            this.gameObject.SetActive(false);
            zombieGround.instance.AgentDead(this.gameObject);
        }
    }


    void SetState()
    {
        //Time.time 가지고 setTime
        if (_curState == AgentState.shotWaiting)
        {
            if (Time.time > _prevShotTime + _shotCoolTime)
            {
                _curState = AgentState.shotReady;
            }
        }

    }

    void Shot(bool shotMinHP)
    {
        // check curState
        if (_curState != AgentState.shotReady)
        {
            return;
        }
                     
        _hitCall++;
        Debug.Log("shot excute = " + _hitCall);

        GameObject targetZombie = null ;
        bool isTarget_minimumHP;
        

        //set Target
        if (shotMinHP)
        {
            targetZombie = _targetZombie_minimumHP;
            isTarget_minimumHP = true;
        }
        else
        {
            targetZombie = _targetZombie_nearest;
            isTarget_minimumHP = false;
        }

        // TODO : Target ZOmbie가 null 일떄 처리...
        if (targetZombie == null || targetZombie.GetComponent<enemyZombie>().isDead == true)
        {
            return;
        }

        // check shot Range
        if (CanShotTarget(targetZombie) == false)
        {
            return;
        }

        bool isdead = targetZombie.GetComponent<enemyZombie>().UpdateDamage_isDead(_hitDmg);

        if(isdead == true)
        {
            zombieGround.instance.ZombieDead(targetZombie, this.gameObject);

            //set target zombie
            if (isTarget_minimumHP == true)
            {
                if(_targetZombie_minimumHP.GetInstanceID() == _targetZombie_nearest.GetInstanceID())
                {
                    _targetZombie_nearest = null;
                }
                _targetZombie_minimumHP = null;
            }
            else
            {
                _targetZombie_nearest = null;
            }

            AddReward(InfoScript.instance.reward_kill);
        }
        else // 죽지는 않음
        {
            //TODO : setReward
            AddReward(InfoScript.instance.reward_shot);
        }

        // manage DPS;
        _prevShotTime = Time.time;
        _curState = AgentState.shotWaiting;

        // TODO : particle mgr 요청
    }

    bool CanShotTarget(GameObject targetObj)
    {
        bool canShotObj = true;

        // 1. check shot range
        var dist = Vector3.Distance(targetObj.transform.position, transform.position);
        if (dist > _shotRange)
        {
            return false;
        }

        // 2. check shot degree
        // cosine 으로 정의 !



        return canShotObj;
    }


    void ResetAgentValue()
    {
        // curState
        //_hp = generalInfo.maxHp;
        //_curState = BotState.normal;
        //_shootingTime = Time.time;
        //
        //totalAttackPoint = 0;

        this.gameObject.SetActive(true);
    }

    void SetLineRenderer()
    {
        if (_targetZombie_minimumHP == null)
        {
            Debug.LogWarning("TARGET NULL!!!!");
            lineR_minimumHp.SetPosition(0, this.gameObject.transform.position);
            lineR_minimumHp.SetPosition(1, this.gameObject.transform.position);
            //lineR_minimumHp.enabled = false;
            //lineR_minimumHp.set
        }

        else if(_targetZombie_minimumHP != null)
        {
            //lineR_minimumHp.enabled = true;
            lineR_minimumHp.SetPosition(0,this.gameObject.transform.position);
            lineR_minimumHp.SetPosition(1, _targetZombie_minimumHP.transform.position);

        }

    }

}
