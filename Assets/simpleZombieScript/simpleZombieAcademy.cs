using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleZombieAcademy : Academy
{
    /*
    [HideInInspector]
    public GameObject[] BotAgentList;

    [HideInInspector]
    public GameObject[] BotEnemyList;
    */


    [HideInInspector]
    public zombieGround[] groundList;

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
        //groundList = FindObjectsOfType<zombieGround>();
        //Debug.Log("IN  AcademyReset : BotAgentCount = " + BotAgentList.Length);

        foreach (zombieGround zg in groundList)
        {
            // todo

            //zg.ResetBattleArea(BotAgentList, BotEnemyList);

        }

        totalScore = 0;

        ObjectManager.instance.FreeAllObj(ObjectType.missile);
    }

    public override void AcademyStep()
    {
        //Debug.Log("academy step called");

        /*
        scoreText.text = string.Format(@"Score: {0}", totalScore);
        */
    }
}
