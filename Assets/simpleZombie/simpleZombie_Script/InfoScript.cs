using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoScript : MonoBehaviour  {

    public static InfoScript instance { get; private set; }

    public float moveSpeed_playerAgent = 20f;
    public float moveSpeed_zombie = 15f;
    public float turnDegree_playerAgent = 5f;
    public float turnDegree_zombie = 5f;

    public float fullHp_playerAgent = 50;
    public float fullHp_zombie = 60;
    public float dmg_playerAgent = 20;
    public float dmg_zombie = 10;

    public float shotCoolTime_playerAgent = 2f;
    public float coolTime_zombie = 0.5f;

    //public float[] rayAngle_agent = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
    public float rayDistacne_agent = 40f;

    public Material[] agentHpMat;
    public Material[] zombieHpMat;

    int _agentMatCount;
    int _zombieMatCount;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _agentMatCount = agentHpMat.Length;
        _zombieMatCount = zombieHpMat.Length;
    }

    
    void Start () {
        if (instance == null)
        {
            instance = this;
        }
    }

    public Material GetHP_Material(bool isZombie, float hp)
    {
        Material mat = null;

        if(isZombie == true)
        {
            int remainHitCount = (int)(hp / dmg_playerAgent);
            Debug.Log("remain hit = " + remainHitCount);

            mat = zombieHpMat[remainHitCount];
        }
        else // agent case
        {
            int remainHitCount = (int)(hp / dmg_zombie);
            Debug.Log("remain hit = " + remainHitCount);

            mat = agentHpMat[remainHitCount];
        }


        return mat;
    }

    /*
    public Color GetHPColor(bool isZombie,  float hp)
    {
        Color color = Color.black;

        if(isZombie == true)
        {

        }

        else
        {

        }

        //TODO : hp에 따른 color
        return color;
    }

    */
}
