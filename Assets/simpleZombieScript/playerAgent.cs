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


    float _moveSpeed;

    float _shotCoolTime;
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
