using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieGround : Area {

    public float groundLength;
    public float centerRandomness;



    public static zombieGround instance;

    float _episodeStartTime;
    
    public float episodeReward
    {
        get { return _episodeReward; }
    }
    float _episodeReward;
    


    [HideInInspector]
    public List<GameObject> _playerAgentList = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> _zombieList = new List<GameObject>();







    ///////////////////////////////////////////////////////////////////////////////////

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start () {
		
	}
	
    /*
    public void ResetArea(GameObject[] agentList, GameObject[] zombieList)
    {

    }
    */

    void SetGameValue()
    {
        _episodeReward = 0;

        _episodeStartTime = Time.time;
    }


    //////////////////////////////////////////
    //public void ResetGround(GameObject[] playerAgentList, GameObject[] zombieList)

    public void ResetGround()
    {
        _playerAgentList.Clear();
        _zombieList.Clear();

        var agentArr = GameObject.FindGameObjectsWithTag("playerAgent");

        for (int i=0; i<agentArr.Length; ++i)
        {
            Vector3 initPos = transform.position;
            initPos.x += Random.Range(-1 * centerRandomness, centerRandomness);
            initPos.z += Random.Range(-1 * centerRandomness, centerRandomness);
            //initPos.y += 1f;

            agentArr[i].transform.position = initPos;
            agentArr[i].transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));

            _playerAgentList.Add(agentArr[i]);

        }

        for(int i = 0; i < InfoScript.instance.initZombieNum; ++i)
        {
            Vector3 initPos = transform.position;
            initPos.x += (Random.Range(-1 * centerRandomness, centerRandomness) - 10);
            initPos.z += (Random.Range(-1 * centerRandomness, centerRandomness) + 10);
            //initPos.y += 1f;

            GameObject zombie =  ObjectManager.instance.Assign(ObjType.zombie, initPos);

            zombie.transform.parent = transform;
            _zombieList.Add(zombie);


            // request from object mgr
            /*
            agentArr[i].transform.position = initPos;
            agentArr[i].transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));

            _playerAgentList.Add(agentArr[i]);
            */
        }
        Debug.Log("ZOMZOMZOM = " + _zombieList.Count);

        /////////////////////////////////////////////////////////////////////////////////

        /*
        foreach (GameObject agent in _playerAgentList)
        {
            if (agent.transform.parent == gameObject.transform)
            {
                //Debug.LogWarning("IN AREA: AgentReset!!@!@!@"); 

                //agent.SetActive(true);


                // TODO : 센터에 생성할것

                agent.GetComponent<playerAgent>().AgentReset();

                agent.transform.position = new Vector3(Random.Range(-groundLength, groundLength), 2f,
                                                       Random.Range(-groundLength, groundLength))
                    + transform.position;
                agent.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));

                _playerAgentList.Add(agent);
            }
        }
        foreach (GameObject zombie in _zombieList)
        {
            if (zombie.transform.parent == gameObject.transform)
            {

                // TODO : 좀비 주변에서 생성할것


                zombie.SetActive(true);
                zombie.transform.position = new Vector3(Random.Range(-groundLength, groundLength), 2f,
                                                       Random.Range(-groundLength, groundLength))
                    + transform.position;
                zombie.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));

                _zombieList.Add(zombie);

                zombie.GetComponent<enemyZombie>().ResetZombie();

            }
        }
        */

        Debug.Log("[Reset GROUND CALLED !!!] agentCount = {}" + _playerAgentList.Count + " , zombie = {}" + _zombieList.Count);

        //set game start value
        SetGameValue();
    }

    public void AgentDead(GameObject agentObj)
    {
        // list 관리 해주기 
        _playerAgentList.Remove(agentObj);

        // agent 수 보고 게임 체크 
        if (GameEndCheck() == true)
        {
            // reward 주고 
            // game reset
        }

        else
        {

        }


        // agent 들에게 통보 

        //



    }


    public void ZombieDead(GameObject zombieObj)
    {



        _zombieList.Remove(zombieObj);


        // 리스트에서 지우고 좀비 생성할지 말지 결정? 
    }

    void GenerateZombie()
    {
        // 모퉁이에서 
        // obj Pool에게서 좀비 할당후 위치 
        // 그리고 리스트로 관리 



    }

    bool GameEndCheck()
    {
        //(리워드는 이함수를 호출하는 func에서 줌)

        bool isGameEnd = false;

        if (_playerAgentList.Count == 0 )
        {
            isGameEnd = true;
        }

        return isGameEnd;
    }

}
