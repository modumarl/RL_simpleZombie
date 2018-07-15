using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieGround : Area {


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
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void ResetArea()
    {
    }

    //////////////////////////////////////////

    public void ResetGround(GameObject[] playerAgentList, GameObject[] zombieList)
    {

    }
}
