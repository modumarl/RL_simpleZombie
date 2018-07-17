using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoScript : MonoBehaviour  {

    public static InfoScript instance { get; private set; }

    public float moveSpeed_playerAgent = 20f;
    public float moveSpeed_zombie = 15f;
    public float turnSpeed_playerAgent = 5f;
    public float turnSpeed_zombie = 5f;

    public float fullHp_playerAgent = 100;
    public float fullHp_zombie = 60;
    public float dmg_playerAgent = 20;
    public float dmg_zombie = 10;

    public float shotCoolTime_playerAgent = 2f;
    public float coolTime_zombie = 0.5f;

    //public float[] rayAngle_agent = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
    public float rayDistacne_agent = 40f;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    
    void Start () {
        if (instance == null)
        {
            instance = this;
        }
    }

}
