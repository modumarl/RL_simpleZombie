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
    
    public float curZombieCount
    {
        get { return _zombieList.Count; }
    }

    public bool isEpisodeStart
    {
        get { return _isEpisodeStart; }
    }
    bool _isEpisodeStart;


    [HideInInspector]
    public List<GameObject> _playerAgentList = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> _zombieList = new List<GameObject>();



    float _prev_GenerateZombieTime;



    ///////////////////////////////////////////////////////////////////////////////////

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _prev_GenerateZombieTime = -1f;
        _isEpisodeStart = false;
    }

    void Start () {
		
	}

    private void FixedUpdate()
    {
        var nexGenTime = _prev_GenerateZombieTime + InfoScript.instance.deltaTime_genZombie;

        if ( _isEpisodeStart == true && nexGenTime < Time.time )
        {
            GenerateZombie();
        }
    }


    /*
    public void ResetArea(GameObject[] agentList, GameObject[] zombieList)
    {

    }
    */

    void ReseetEpisodeReward()
    {
        _episodeReward = 0;
        _episodeStartTime = Time.time;
    }


    //////////////////////////////////////////
    //public void ResetGround(GameObject[] playerAgentList, GameObject[] zombieList)

    public void ResetGroundValue()
    {
        ReseetEpisodeReward();

        _playerAgentList.Clear();
        _zombieList.Clear();
    }

    /// <summary>
    /// called From Academy class
    /// </summary>
    public void SetGameStartGround()
    {
        var agentArr = GameObject.FindGameObjectsWithTag("playerAgent");

        for (int i = 0; i < agentArr.Length; ++i)
        {
            Vector3 initPos = transform.position;
            initPos.x += Random.Range(-1 * centerRandomness, centerRandomness);
            initPos.z += Random.Range(-1 * centerRandomness, centerRandomness);
            //initPos.y += 1f;

            agentArr[i].transform.position = initPos;
            agentArr[i].transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));

            _playerAgentList.Add(agentArr[i]);

        }

        for (int i = 0; i < InfoScript.instance.initZombieNum; ++i)
        {
            Vector3 initPos = transform.position;
            initPos.x += (Random.Range(-1 * centerRandomness, centerRandomness) - 10);
            initPos.z += (Random.Range(-1 * centerRandomness, centerRandomness) + 10);
            //initPos.y += 1f;

            GameObject zombie = ObjectManager.instance.Assign(ObjType.zombie, initPos);

            zombie.transform.parent = transform;
            //zombie.GetComponent<enemyZombie>().InitializeZombie();
            _zombieList.Add(zombie);

        }

        _prev_GenerateZombieTime = Time.time;
        _episodeStartTime = Time.time;
        _isEpisodeStart = true;


        Debug.Log("ZOMZOMZOM = " + _zombieList.Count);
        //Debug.Log("[Reset GROUND CALLED !!!] agentCount = {}" + _playerAgentList.Count + " , zombie = {}" + _zombieList.Count);
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

            simpleZombieAcademy.instance.AcademyReset();
        }

        else
        {

        }


        // agent 들에게 통보 


    }


    public void ZombieDead(GameObject zombieObj, GameObject shooterAgent)
    {
        Debug.Log("[Before] zombieCount ] = " + _zombieList.Count);

        //TODO : FREE obj
        //zombieObj.SetActive(false); // 이거만 해도 되는건가??
        ObjectManager.instance.Free(zombieObj, ObjType.zombie);
        _zombieList.Remove(zombieObj);

        Debug.Log("[After] zombieCount ] = " + _zombieList.Count);


        //add Reward
        //shooterAgent.GetComponent<playerAgent>().AddReward(10f);

        // 리스트에서 지우고 좀비 생성할지 말지 결정? 
    }


    /////////////////////////////////////////////////////////////////////////////////


    void GenerateZombie()
    {
        //더많이 생성???
        for (int i = 0; i < InfoScript.instance.initZombieNum; ++i)
        {
            Vector3 initPos = transform.position;
            initPos.x += (Random.Range(-1 * centerRandomness, centerRandomness) - 10);
            initPos.z += (Random.Range(-1 * centerRandomness, centerRandomness) + 10);
            //initPos.y += 1f;

            GameObject zombie = ObjectManager.instance.Assign(ObjType.zombie, initPos);
            zombie.transform.parent = transform;

            //TODO : 이걸 어케 생성하냐에 따라 버그가 생기고 안생긴다..
            //var zombieScript = zombie.transform.GetComponent<enemyZombie>();
            //zombieScript.InitializeZombie();

            _zombieList.Add(zombie.gameObject);
        }

        // todo : 모퉁이에서 생성

        _prev_GenerateZombieTime = Time.time;

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
