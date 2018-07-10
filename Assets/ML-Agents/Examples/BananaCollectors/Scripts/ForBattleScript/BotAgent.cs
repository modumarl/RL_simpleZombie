using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAgent : Agent
{
    RayPerception rayPer;
    Rigidbody agentRB;
    ShootAction shootAction;

    public GeneralInfo generalInfo;

    public float missileInitPivot = 2f;
    

    public int _hp;
    float _shootPower;
    float _dpsCoolTime;
    int _attackBonus;
    float _turnSpeed;
    float _moveSpeed;

    //////////////////////////////

    public Teams _curTeam;
    float _shootingTime;

    [HideInInspector]
    public int totalAttackPoint;
    BotState _curState;
    

    public override void InitializeAgent()
    {
        base.InitializeAgent();

        rayPer = GetComponent<RayPerception>();
        agentRB = GetComponent<Rigidbody>();
        shootAction = GetComponent<ShootAction>();


        _shootPower = generalInfo.shotPower;
        _dpsCoolTime = generalInfo.dpsCoolTime;
        _attackBonus = generalInfo.attackBonus;
        _turnSpeed = generalInfo.turnSpeed;
        _moveSpeed = generalInfo.moveSpeed;
        _curTeam = Teams.Team_agent;

        ResetAgenBotValue();
    }

    void ResetAgenBotValue()
    {
        // curState
        _hp = generalInfo.maxHp;
        _curState = BotState.normal;
        _shootingTime = Time.time;

        totalAttackPoint = 0;

        this.gameObject.SetActive(true);
    }

    public override void CollectObservations()
    {
        float rayDistance = 50f;
        float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
        string[] detectableObjects = { "BotAgent", "BotEnemy", "wall", "MissileAgent", "MissileBot" };
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        Vector3 localVelocity = transform.InverseTransformDirection(agentRB.velocity);
        AddVectorObs(localVelocity.x);
        AddVectorObs(localVelocity.z);
        AddVectorObs(System.Convert.ToInt32(_curState));
        AddVectorObs(Time.time - _shootingTime);
        AddVectorObs(_hp);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {

        if (_hp < 1)
            return;

        Vector3 dirToGo = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;
        bool shootCommand = false;

        SetState();

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


        if(shootCommand)
        {
            if(_curState == BotState.normal)
            {
                shootAction.ShootMissile(_curTeam, _shootPower, transform.position + transform.forward * missileInitPivot, transform.forward);
                _curState = BotState.shooting;
                _shootingTime = Time.time;
            }
        }

        /*
        if (agentRB.velocity.sqrMagnitude > 25f) // slow it down
        {
            agentRB.velocity *= 0.95f;
        }
        */
    }

    public override void AgentReset()
    {
        ResetAgenBotValue();
    }

    public override void AgentOnDone()
    {

    }

    //////////////////////////////////////////////////////////////////////////////

    void SetState()
    {
        //Time.time 가지고 setTime
        if(_curState == BotState.shooting)
        {
            if(Time.time > _shootingTime + _dpsCoolTime )
            {
                _curState = BotState.normal;
            }
        }

    }
    

    public void HitByMissile(int dmg)
    {

        if (_hp < 1)
            return;

        int dmgAmount = 0;
        if(dmg > _hp)
        {
            dmgAmount = _hp;
        }
        else
        {
            dmgAmount = dmg;
        }
        _hp -= dmgAmount;
        BattleArea.instance.UpdateDmg(_curTeam, dmgAmount);

        //Debug.LogWarning(_curTeam + "curHP = " + _hp);

        if (_hp <=0)
        {
            //dead 처리
            //this.gameObject.SetActive(false);
            BattleArea.instance.BotDead(_curTeam, this.gameObject);
        }

        UpdateHP_UI();
    }

    public void HitTarget()
    {

        if (_hp < 1)
            return;

        totalAttackPoint += _attackBonus;

        if(_curTeam == Teams.Team_agent)
        {
            BattleArea.instance.totalHitTargetPoint_agent += _attackBonus;
            AddReward(generalInfo.hitTargetReward_agent);
            
            //TODO : Enemy Kill Reward
            //TODO : TeamReward

            //Debug.Log("[agent] totalHitPoint = " + BattleArea.instance.totalHitTargetPoint_agent);
        }
        else if(_curTeam == Teams.Team_enemy)
        {
            //BattleArea.instance.totalHitTargetPoint_enemy += _attackBonus;
            //Debug.Log("[enemy] totalHitPoint = " + BattleArea.instance.totalHitTargetPoint_enemy);
        }

    }


    protected void UpdateHP_UI()
    {
        // UI Setting ==> Component로 빼는게 편하듯
    }


}
