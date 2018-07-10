using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArea : Area {

    public float range;

    public static BattleArea instance;

    int _botAgentHp=0;
    public int botAgentHp
    {
        get { return _botAgentHp; }
    }

    int _botEnemyHp=0;
    public int botEnemyHp
    {
        get { return _botEnemyHp; }
    }

    //hp 점수 
    public float HpRatio
    {
        get { return botAgentHp / botEnemyHp; }
    }

    bool _isGameStarted;
    public bool isGameStarted
    {
        get { return _isGameStarted; }
    }

    public int agentLiveCount
    {
        get { return _areaAgentList.Count; }
    }
        
    public int enemyLiveCount
    {
        get { return _areaEnemyList.Count; }
    }

    [HideInInspector]
    public int totalHitTargetPoint_agent = 0;

    [HideInInspector]
    public int totalHitTargetPoint_enemy = 0;



    [HideInInspector]
    public List<GameObject> _areaAgentList = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> _areaEnemyList = new List<GameObject>();

    //////////////////////////////////////////
    // TODO
    //      바운더리 체크
    //
    //////////////////////////////////////////

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void ResetBattleArea(GameObject[] botAgentList, GameObject[] botEnemyList)
    {
        _areaAgentList.Clear();
        _areaEnemyList.Clear();

        //Debug.LogWarning("IN RESET BATTLE AREA");

        foreach (GameObject agent in botAgentList)
        {
            if (agent.transform.parent == gameObject.transform)
            {
                //Debug.LogWarning("IN AREA: AgentReset!!@!@!@"); 

                //agent.SetActive(true);
                agent.GetComponent<BotAgent>().AgentReset();

                agent.transform.position = new Vector3(Random.Range(-range, range), 2f,
                                                       Random.Range(-range, range))
                    + transform.position;
                agent.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));

                _areaAgentList.Add(agent);
            }
        }

        foreach (GameObject enemy in botEnemyList)
        {
            if (enemy.transform.parent == gameObject.transform)
            {
                enemy.SetActive(true);
                enemy.transform.position = new Vector3(Random.Range(-range, range), 2f,
                                                       Random.Range(-range, range))
                    + transform.position;
                enemy.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));

                _areaEnemyList.Add(enemy);

                enemy.GetComponent<BotEnemy>().ResetEnemyBot();

            }
        }

        Debug.Log("agentCount = " + _areaAgentList.Count + " | enemyCount = " + _areaEnemyList.Count);
    }

    /*
    void SetPoint()
    {
        // 팀별정보 개인별 정보들을 갱신한다
        for (int i = 0; i<_areaAgentList.Count;++i)
        {
         //   _botAgentHp += agentList[i].GetComponent<BotAgent>().agentHp;
        }

        for (int i = 0; i < _areaEnemyList.Count; ++i)
        {
            //_botEnemyHp += enemyList[i].GetComponent<BotEnemy>().enemyHp;
        }
        //Debug.LogWarning("AGENT COUNT = " + _areaAgentList.Count + " | ENEMY COUNT = " + _areaEnemyList.Count);
        //Debug.Log("AGENT TOTAL = " + _botAgentHp + " | ENEMY TOTAL = " + _botEnemyHp);

        totalHitTargetPoint_agent = 0;
        totalHitTargetPoint_enemy = 0;
    }
    */

    void ResetPoint()
    {
        // #TODO : fill this

        /*
        // obj 돌면서 reset 하고 setPoint 하기 가져온다

        // 1. 있는거 다 리셋하고 
        for (int i= 0; i< agentList.Count;++i)
        {
            agentList[i].GetComponent<BotAgent>().Reset();
        }

        for (int i =0;i<enemyList.Count;++i)
        {
            //enemyList[i].GetComponent<BotEnemy>().Reset();
        }
        // 2. 세팅값만큼만 다시 이닛하고

        // 3. 변수 세팅
        SetPoint();
        */
    }

    public void UpdateDmg(Teams team, int dmg )
    {
        if(team == Teams.Team_agent)
        {
            _botAgentHp -= dmg;
            //Debug.Log("[AGENT] totalHP = " + _botAgentHp);
        }

        else
        {
            _botEnemyHp -= dmg;
            //Debug.Log("[ENEMY] totalHP = " + _botEnemyHp);
        }
    }

    public void BotDead(Teams team, GameObject gameobject)
    {
        //Debug.Log("$$  Before  $$ agentNum  = " + _areaAgentList.Count + " enemyNum = " + _areaEnemyList.Count);

        if (team == Teams.Team_agent)
        {
            _areaAgentList.Remove(gameobject);

            if (GameEndCheck_AddTeamReward() == false)
            {
                for (int i = 0; i < _areaAgentList.Count; ++i)
                {
                    _areaAgentList[i].GetComponent<BotAgent>().AddReward(GeneralInfo.instance.deadReward_team);
                }
            }
            else
            {
                //temp
                for(int i =0; i<BattleAcademy.instance.BotAgentList.Length;++i)
                {
                    BattleAcademy.instance.BotAgentList[i].SetActive(true);
                }
                for (int i = 0; i < BattleAcademy.instance.BotEnemyList.Length; ++i)
                {
                    BattleAcademy.instance.BotEnemyList[i].SetActive(true);
                }
                BattleAcademy.instance.AcademyReset();
            }

        }

        else if (team == Teams.Team_enemy)
        {
            _areaEnemyList.Remove(gameobject);
            GameEndCheck_AddTeamReward();

            if (GameEndCheck_AddTeamReward() == false)
            {
                for (int i = 0; i < _areaAgentList.Count; ++i)
                {
                    _areaAgentList[i].GetComponent<BotAgent>().AddReward(GeneralInfo.instance.killReward_team);
                }
            }

            else
            {
                //temp
                for (int i = 0; i < BattleAcademy.instance.BotAgentList.Length; ++i)
                {
                    BattleAcademy.instance.BotAgentList[i].SetActive(true);
                }
                for (int i = 0; i < BattleAcademy.instance.BotEnemyList.Length; ++i)
                {
                    BattleAcademy.instance.BotEnemyList[i].SetActive(true);
                }
                BattleAcademy.instance.AcademyReset();
            }
        }

        //Debug.Log("REMOVED");
        //Debug.Log("$$  After  $$ agentNum  = " + _areaAgentList.Count + " enemyNum = " + _areaEnemyList.Count);

    }

    bool GameEndCheck_AddTeamReward()
    {
        bool isGameEnd = false;

        if(_areaAgentList.Count == 0)
        {
            //enemyWin

            // agent 객체가 없는데 add Reward 어떻게 ?

            isGameEnd = true;
        }
        else if(_areaEnemyList.Count == 0 )
        {
            for (int i =0;i<_areaAgentList.Count;++i)
            {
                _areaAgentList[i].GetComponent<BotAgent>().AddReward(GeneralInfo.instance.teamWin_agent);
            }

            isGameEnd = true;
        }
        else if(_areaAgentList.Count ==0 &&_areaEnemyList.Count == 0)
        {
            //draw
            // agent 객체가 없는데 add Reward 어떻게 ?

            isGameEnd = true;
        }

        return isGameEnd;
    }


    // * TODO




}
