using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralInfo : MonoBehaviour {

    public static GeneralInfo instance { get; private set; }

    public float shotPower = 10f;
    public float shotRange = 5f;
    public float dpsCoolTime = 5f;
    public int maxHp = 100;
    public float turnSpeed = 250f;
    public float moveSpeed = 2f;

    public float missileLifeTime = 5f;
    public int missileDmg = 5;
    public int attackBonus = 10;

    public float hitTargetReward_agent = 1f;
    public float hitTargetReward_team = 1f;

    public float killReward_agent = 10f;
    public float killReward_team = 10f;

    public float damagedReward_agent = -1f;
    public float damagedReward_team = -1f;

    public float deadReward_agent = -10f;
    public float deadReward_team = -10f;

    public float teamWin_agent = 20f;
    public float teamLose_agent = -20f;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
