using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAgent : Agent
{
    RayPerception _rayPercept;
    Rigidbody agentRB;

    // need?
    List<GameObject> nearZombieList = new List<GameObject>();



    // ****** TODO ******
    // heading Vector
    // Type입력하면 바뀌어서 능력치 
    // playerType 
    // *******************

    AgentState _curState;
    float _moveSpeed;
    float _turnSpeed;

    float _shotCoolTime;
    float _prevShotTime;
    int _hp;





    //////////////////////////////////////////////////////////////////////////

    public override void InitializeAgent()
    {
        base.InitializeAgent();

        _rayPercept = GetComponent<RayPerception>();

        //agentRB = GetComponent<Rigidbody>();
        //shootAction = GetComponent<ShootAction>();
        //_shootPower = generalInfo.shotPower;
        //_dpsCoolTime = generalInfo.dpsCoolTime;
        //_attackBonus = generalInfo.attackBonus;
        //_turnSpeed = generalInfo.turnSpeed;
        //_moveSpeed = generalInfo.moveSpeed;
        //_curTeam = Teams.Team_agent;

        ResetAgentValue();
    }



    public override void CollectObservations()
    {

    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (_hp < 1)
            return;

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

    void Update()
    {

    }


    ////////////////////////////////////////////////////////////////////////////////


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



    void Shot()
    {
        // zombie에게 shot



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
