using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    bool _isActivate = false;
    public float maxVelocity;
    float _curLifeTime;
    int _dmg;

    Teams team;
    //public string masterName;

    [HideInInspector]
    public GameObject masterBot;

    private void Awake()
    {
        _dmg = GeneralInfo.instance.missileDmg;  
    }

    private void OnEnable()
    {
        _isActivate = true;
        _curLifeTime = GeneralInfo.instance.missileLifeTime;
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    private void OnDisable()
    {
        _isActivate = false;
        //masterName = null;
        masterBot = null;
    }

    public void SetTeam(Teams inputTeam)
    {
        team = inputTeam;

        if(inputTeam == Teams.Team_agent)
        {
            this.gameObject.layer = LayerMask.NameToLayer("missileAgent");
        }
        else 
        {
            this.gameObject.layer = LayerMask.NameToLayer("missileEnemy");
        }
    }

	// Update is called once per frame
	void Update () {
        _curLifeTime -= Time.deltaTime;

        if(_curLifeTime<=0)
        {
            _isActivate = false;
            this.gameObject.SetActive(false);
        }
	}

    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log(other.gameObject);

        if (other.gameObject.CompareTag("wall"))
        {
            _isActivate = false;
            this.gameObject.SetActive(false);
        }
        
        else if (other.gameObject.CompareTag("BotAgent"))
        {
            other.gameObject.GetComponent<BotAgent>().HitByMissile(_dmg);

            /*
            if(masterBot.activeSelf !=false)
            {
                masterBot.GetComponent<BotEnemy>().HitTarget();
            }
            */
            this.gameObject.SetActive(false);
        }
        
        else if (other.gameObject.CompareTag("BotEnemy"))
        {
            other.gameObject.GetComponent<BotEnemy>().HitByMissile(_dmg);

            if (masterBot.activeSelf != false)
            {
                masterBot.GetComponent<BotAgent>().HitTarget();
            }

            this.gameObject.SetActive(false);
        }



    }

}
