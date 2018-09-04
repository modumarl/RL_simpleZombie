using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAgent : Agent
{
    public InfoScript infoScript;

    List<GameObject> _nearZombieList = new List<GameObject>();
    GameObject _targetZombie; 

    // ****** TODO ******
    // heading Vector
    // Type입력하면 바뀌어서 능력치 
    // playerType 
    // *******************

    RayPerception _rayPercept;
    Rigidbody agentRB;
    AgentState _curState;
    float _shotCoolTime;
    float _prevShotTime;
    float _hp;




    //////*imported value*//////
    float _moveSpeed;
    float _turnSpeed;
    float _rayDistance;
    float _hitDmg;
    float[] _rayAngle;
    string[] _detectableObjects = { "zombie", "wall", "playerAgent", "obstacle" };
    ///////////////////////////

    //////////////////////////////////////////////////////////////////////////

    public override void InitializeAgent()
    {
        Debug.LogWarning("INIT CALLED");

        base.InitializeAgent();

        _rayPercept = GetComponent<RayPerception>();
        agentRB = GetComponent<Rigidbody>();

        _moveSpeed = infoScript.moveSpeed_playerAgent;
        _turnSpeed = infoScript.turnSpeed_playerAgent;
        _rayDistance = infoScript.rayDistacne_agent;
        _hitDmg = infoScript.dmg_playerAgent;
        _rayAngle = infoScript.rayAngle_agent;
        _hp = infoScript.fullHp_playerAgent;

        Debug.LogWarning(_rayAngle);

        ResetAgentValue();
    }



    public override void CollectObservations()
    {
        Debug.LogWarning("[[[[ collect CALLED");


        AddVectorObs(_rayPercept.Perceive(_rayDistance, _rayAngle, _detectableObjects, 0f, 0f));
        Vector3 localVelocity = transform.InverseTransformDirection(agentRB.velocity);
        AddVectorObs(localVelocity.x);
        AddVectorObs(localVelocity.z);
        AddVectorObs(System.Convert.ToInt32(_curState));
        AddVectorObs(Time.time - _prevShotTime);
        AddVectorObs(_hp);

        //todo 
        // nearZombieList 관련한 정보값
        AddVectorObs(_nearZombieList.Count);
        
        //hp가 일정치 이하인 좀비의 카운트?

    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        
        if (_curState == AgentState.dead)
            return;

        // 죽은후 obs 정보나 네트워크 보낼 정보는 어떻게...?

        Vector3 dirToGo = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;
        bool shootCommand = false;


        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
            dirToGo = transform.forward * Mathf.Clamp(vectorAction[0], -1f, 1f);
            rotateDir = transform.up * Mathf.Clamp(vectorAction[1], -1f, 1f);
            shootCommand = Mathf.Clamp(vectorAction[2], 0f, 1f) > 0.3f;
        }
        else
        {
            switch ((int)(vectorAction[0]))
            {
                case 1:
                    dirToGo = transform.forward;
                    break;
                case 2:
                    shootCommand = true;
                    break;
                case 3:
                    rotateDir = -transform.up;
                    break;
                case 4:
                    rotateDir = transform.up;
                    break;
            }
        }

        agentRB.AddForce(dirToGo * _moveSpeed, ForceMode.Force);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * _turnSpeed);


        if (shootCommand)
        {
            if (_curState == AgentState.normal)
            {


                /*
                shootAction.ShootMissile(_curTeam, _shootPower, transform.position + transform.forward * missileInitPivot, transform.forward);
                _curState = AgentState.shotWaiting;
                _shootingTime = Time.time;

                */
            }
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
    void Start()
    {

    }

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
                _curState = AgentState.normal;
            }
        }

    }

    void SelectShotTarget()
    {
        //set targetZombie

        // 일단은 제일 hp가 적은 좀비위주로 함


    }

    void Shot()
    {


        // zombie에게 shot


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
