using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotEnemy : MonoBehaviour
{
    //Rule base로 움직임

    // Area로 부터 모든 좌표를 받아서 자기 자신의 좌표와 비교해서 미사일을 쏴댐

    // + 랜덤성 부여?

    public GeneralInfo generalInfo;
    public float missileInitPivot = 2f;


    Rigidbody enemyRB;
    ShootAction shootAction;

    int _hp;
    float _shootPower;
    float _dpsCoolTime;
    int _attackBonus;
    float _turnSpeed;
    float _moveSpeed;

    //////////////////////////////

    public Teams _curTeam;
    float _curDpsTime;
    float _shootingTime;

    [HideInInspector]
    public int totalAttackPoint;
    BotState _curState;


    ////////////////////////////
    // for EnemyBot
    GameObject targetObj;
    public float shootAimRandomValue;
    public float distanceFromAgent;

    [HideInInspector]
    public BattleArea battleArea;

    List<GameObject> agentObjList = new List<GameObject>();


    public Text textMsg;

    /////////////////////////////////////////////////////////////////////////////
    void Start()
    {
        enemyRB = GetComponent<Rigidbody>();
        shootAction = GetComponent<ShootAction>();

        SetEnemyBotvalue();
        battleArea = transform.parent.GetComponent<BattleArea>();

        ResetEnemyBot();
    }

    void SetEnemyBotvalue()
    {
        _shootPower = generalInfo.shotPower;
        _dpsCoolTime = generalInfo.dpsCoolTime;
        _attackBonus = generalInfo.attackBonus;
        _turnSpeed = generalInfo.turnSpeed;
        _moveSpeed = generalInfo.moveSpeed;
        _curTeam = Teams.Team_enemy;


    }

    public void ResetEnemyBot()
    {
        //curState
        _hp = generalInfo.maxHp;
        _curState = BotState.normal;
        _shootingTime = Time.time;

        // for EnemyBot
        targetObj = null;
        agentObjList = battleArea._areaAgentList;
    }

    private void FixedUpdate()
    {
        EnemySimpleRuleAI();
    }

    void EnemySimpleRuleAI()
    {
        SetState();

        SearchTargetObject();
        MoveToAgentAndShoot();
    }

    void SetState()
    {
        //Time.time 가지고 setTime
        if (_curState == BotState.shooting)
        {
            if (Time.time > _shootingTime + _dpsCoolTime)
            {
                _curState = BotState.normal;
            }
        }
    }
    void SearchTargetObject()
    {

        if(targetObj && targetObj.GetComponent<BotAgent>()._hp < 1)
        {
            targetObj = null;
        }

     


      

        //Debug.Log("live agentCount = " + agentLiveCount);

        /*
        if(agentLiveCount == 0) // temp ?
        {
            targetObj = null;
            return;
        }
        */

     

        float nearestDist = 999999999;

        if (targetObj != null && targetObj.GetComponent<BotAgent>()._hp>0)
        {
            nearestDist = Vector3.Distance(targetObj.transform.position, transform.position);
        }
//         if (targetObj.activeSelf ==true)
//         {
//             nearestDist = Vector3.Distance(targetObj.transform.position, transform.position);
//         }

        //check nearest obj
        for (int i = 0; i < agentObjList.Count; ++i)
        {
            if (agentObjList[i].GetComponent<BotAgent>()._hp >0 && agentObjList[i] != targetObj)
            {
                float dist = Vector3.Distance(agentObjList[i].transform.position, transform.position);

                if (nearestDist > dist)
                {
                    targetObj = agentObjList[i];
                    nearestDist = dist;
                }
            }
        }
    }

    void MoveToAgentAndShoot()
    {
        if (targetObj == null)
            return;


        if (_curState == BotState.normal && targetObj != null)
        {
            //Debug.Log("forward = " + transform.forward);
            float randomness = 0.2f;
            Vector3 randVec = new Vector3(Random.Range(-1 * randomness, randomness), 0, Random.Range(-1 * randomness, randomness));


            shootAction.ShootMissile(_curTeam, _shootPower, transform.position + transform.forward * missileInitPivot, transform.forward + randVec);
            _curState = BotState.shooting;
            _shootingTime = Time.time;
        }


        {
            if (Vector3.Distance(targetObj.transform.position, transform.position) < 12)
                return;
  
        }

        //transform.LookAt(targetObj.transform);
        //enemyRB.AddRelativeForce(_moveSpeed, power);
        float mod = 0.5f;

        Vector3 dirToGo = (targetObj.transform.position - transform.position).normalized;
        enemyRB.AddForce(dirToGo * _moveSpeed * mod, ForceMode.Force);

        Quaternion neededRotation = Quaternion.LookRotation(targetObj.transform.position - transform.position);
        Quaternion interpolatedRotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * _turnSpeed);

        transform.rotation = interpolatedRotation;
        //Debug.Log("need = " + neededRotation + " | interPol = " + interpolatedRotation);

        //shoot

        ShowMsg(string.Format("{0} Move({2}) to Target{1}", name, targetObj.name, _moveSpeed));


    }

    public void HitByMissile(int dmg)
    {
        int dmgAmount = 0;
        if (dmg > _hp)
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

        if (_hp <= 0)
        {
            //dead 처리
            this.gameObject.SetActive(false);
            BattleArea.instance.BotDead(_curTeam, this.gameObject);

        }

        //UpdateHP_UI();
    }


    public void ShowMsg(string msg)
    {
        if (this.textMsg == null)
            return;


        textMsg.text = msg;


        return;

       // msg.t
    }

    /*
    public GameObject missilePrefab;
    public int enemyHp
    {
        get { return _hp; }
    }

    void Awake()
    {
        _curTeam = Teams.Team_enemy;
        InitBot();
    }

    void OnEnable()
    {
        _hp = GeneralInfo.instance.botHp;
    }



    // Use this for initialization
    void Start () {

        //Debug.Log("curPower" + _shootPower);
	}
	
	// Update is called once per frame
	void Update () {

        RangeSearch();
        if (_curState == BotState.shooting)
        {
            _curDpsTime -= Time.deltaTime;
        }

        KeyBoardInput();
    }

    public override void InitializeAgent()
    {
        // 의미 x
    }
    public override void CollectObservations()
    {
        // 의미 x
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // todo : 의미 x 혹은 자체 움직임 짤껏


    }
    */
}
