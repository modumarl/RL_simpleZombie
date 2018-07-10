using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAcademy : Academy   {

    [HideInInspector]
    public GameObject[] BotAgentList;

    [HideInInspector]
    public GameObject[] BotEnemyList;

    [HideInInspector]
    public BattleArea[] listArea;

    public int totalScore;
    //public Text scoreText;


    public static BattleAcademy instance;

    public void Start() // 
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public override void AcademyReset()
    {
        //BotAgentList.

        BotAgentList = GameObject.FindGameObjectsWithTag("BotAgent");
        BotEnemyList = GameObject.FindGameObjectsWithTag("BotEnemy");
        listArea = FindObjectsOfType<BattleArea>();

        Debug.Log("IN  AcademyReset : BotAgentCount = " + BotAgentList.Length);


        foreach (BattleArea ba in listArea)
        {
            ba.ResetBattleArea(BotAgentList, BotEnemyList);
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
