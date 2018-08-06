using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleZombieAcademy : Academy
{
    // TODO : move objList to GroundClass
    [HideInInspector]
    public GameObject[] playerAgentList;

    [HideInInspector]
    public GameObject[] zombieList;
    


    [HideInInspector]
    public GameObject[] groundList;

    public int totalScore;
    //public Text scoreText;

    public static simpleZombieAcademy instance;

    public void Start() // 
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public override void AcademyReset()
    {
        groundList = GameObject.FindGameObjectsWithTag("ground");

        Debug.Log("ACADEMY RESET : ground Count = " + groundList.Length);


        // TODO : 이런건 어디서??
        // ObjectManager.instance.FreeAllObj(ObjectType.missile);
        

        for (int i = 0; i<groundList.Length; ++i )
        {
            groundList[i].GetComponent<zombieGround>().ClearGround();
            groundList[i].GetComponent<zombieGround>().SetGameStartGround();
        }

        //total SCORE ????


        /*
        //groundList = FindObjectsOfType<zombieGround>();
        //Debug.Log("IN  AcademyReset : BotAgentCount = " + BotAgentList.Length);

        
        totalScore = 0;

        //ObjectManager.instance.FreeAllObj(ObjectType.missile);
        */
    }

    public override void AcademyStep()
    {
        //Debug.Log("academy step called");

        /*
        scoreText.text = string.Format(@"Score: {0}", totalScore);
        */
    }
}
