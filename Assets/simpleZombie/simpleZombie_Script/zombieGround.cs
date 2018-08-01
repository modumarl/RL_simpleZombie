using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieGround : Area {

    public float groundLength;

    public static zombieGround instance;





    [HideInInspector]
    public List<GameObject> _playerAgentList = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> _zombieList = new List<GameObject>();







    //////////////////////////////////////////

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	

    public void ResetArea(GameObject[] agentList, GameObject[] zombieList)
    {
        _playerAgentList.Clear();
        _zombieList.Clear();

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


    }

    //////////////////////////////////////////

    public void ResetGround(GameObject[] playerAgentList, GameObject[] zombieList)
    {

    }
}
