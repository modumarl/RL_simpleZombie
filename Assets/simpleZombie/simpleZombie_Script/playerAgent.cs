using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAgent : Agent
{
    public InfoScript infoScript;

    List<GameObject> _perceiveZombieList = new List<GameObject>();
    GameObject _targetZombie_nearest;
    GameObject _targetZombie_minimumHP;

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
        _hitDmg = infoScript.dmg_playerAgent;
        //_rayAngle = infoScript.rayAngle_agent;
        _hp = infoScript.fullHp_playerAgent;
        _renderer.material = infoScript.GetHP_Material(false, _hp);

        _shotCoolTime = infoScript.shotCoolTime_playerAgent;


        ResetAgentValue();
    }



    public override void CollectObservations()
    {

        //_rayAngle = infoScript.rayAngle_agent;
        //Debug.Log(_rayAngle);


        // --- just for use Loop --------
        PerceiveZombie();

        if(_perceiveZombieList.Count !=0)
        {
            SetTarget();
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
        AddVectorObs(_perceiveZombieList.Count);

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


        // 죽은후 obs 정보나 네트워크 보낼 정보는 어떻게...?

        //Vector3 dirToGo = Vector3.zero;
        //Vector3 rotateDir = Vector3.zero;
        //bool shootCommand = false;


        /*
        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
            dirToGo = transform.forward * Mathf.Clamp(vectorAction[0], -1f, 1f);
            rotateDir = transform.up * Mathf.Clamp(vectorAction[1], -1f, 1f);
            shootCommand = Mathf.Clamp(vectorAction[2], 0f, 1f) > 0.3f;
        }
        else // discrete spaceType
        {

        }

        /*
       // agentRB.AddForce(dirToGo * _moveSpeed, ForceMode.Force);
       // transform.Rotate(rotateDir, Time.fixedDeltaTime * _turnDegree);


        if (shootCommand)
        {
            if (_curState == AgentState.normal)
            {


                shootAction.ShootMissile(_curTeam, _shootPower, transform.position + transform.forward * missileInitPivot, transform.forward);
                _curState = AgentState.shotWaiting;
                _shootingTime = Time.time;

            }
        }

        */

    }

    public override void AgentReset()
    {
        ResetAgentValue();
    }

    public override void AgentOnDone()
    {

    }

    ////////////////////////////////////////////////////////////////////////////////


    void Start()
    {

    }

    void PerceiveZombie()
    {
        _perceiveZombieList = _rayPercept.PerceiveObjectList(_rayDistance, _rayAngle, "zombie", 0f, 0f);

        //Debug.LogWarning("listCount = " + _perceiveZombieList.Count);
        //for (int i = 0; i < _perceiveZombieList.Count; ++i)
        //{
        //    Debug.Log(_perceiveZombieList[i].name + " " + _perceiveZombieList[i].GetInstanceID());
        //}
    }
    void SetTarget()
    {
        if(_perceiveZombieList.Count == 0)
        {
            return;
        }

        _targetZombie_minimumHP = _perceiveZombieList[0];
        _targetZombie_nearest = _perceiveZombieList[0];

        var minHp = _targetZombie_minimumHP.GetComponent<enemyZombie>().hp;
        var minDist = Vector3.Distance(_targetZombie_nearest.transform.position, transform.position);

        for (int i=0; i<_perceiveZombieList.Count; ++i)
        {
            var thisZombieHp = _perceiveZombieList[i].GetComponent<enemyZombie>().hp;
            if (minHp > thisZombieHp)
            {
                _targetZombie_minimumHP = _perceiveZombieList[i];
                minHp = thisZombieHp;
            }


            float thisZombieDist = Vector3.Distance(_perceiveZombieList[i].transform.position, transform.position);
            if (minDist > thisZombieDist)
            {
                _targetZombie_nearest = _perceiveZombieList[i];
                minDist = thisZombieDist;
            }
        }
    }


    /*
    private void FixedUpdate()
    {

    }
    */


    ////////////////////////////////////////////////////////////////////////////////

    public void AttackByZombie(int inputDmg)
    {
        // TODO : SendReward

       

        UpdateHp(inputDmg);
    }
    void UpdateHp(int inputDmg)
    {
        if(_curState == AgentState.dead)
        {
            return;
        }


        if(_hp > inputDmg)
        {
            _hp -= inputDmg;
        }

        else
        {
            _hp = 0;
            _curState = AgentState.dead;
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

                Debug.LogWarning("[shot Ready]");
            }
        }

    }

    void Shot(bool shotMinHP)
    {
        if (_curState != AgentState.shotReady)
        {
            return;
        }


        _hitCall++;
        Debug.Log("shot excute = " + _hitCall);

        GameObject targetZombie = null ;
        bool doShot = false;
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
        if (targetZombie == null)
        {
            return;
        }


        bool isdead = targetZombie.GetComponent<enemyZombie>().UpdateDamage_isDead(_hitDmg);

        if(isdead == true)
        {
            if(isTarget_minimumHP == true)
            {
                if(_targetZombie_minimumHP.GetInstanceID() == _targetZombie_nearest.GetInstanceID())
                {
                    _targetZombie_nearest = null;
                }

                _targetZombie_minimumHP = null;


                //TODO : setReward

            }
        }
        else // 죽지는 않음
        {
            //TODO : setReward

        }




        // manage DPS;
        _prevShotTime = Time.time;
        _curState = AgentState.shotWaiting;


        // TODO : particle mgr 요청



        

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

}
