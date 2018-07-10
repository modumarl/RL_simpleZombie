using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAgentTEMP
{

    /*

    public GameObject missilePrefab;
    public int agentHp
    {
        get { return _hp; }
    }

    Rigidbody agentRB;


    void Awake()
    {
        _curTeam = Teams.Team_agent;
        agentRB = GetComponent<Rigidbody>();
        InitBot();
    }

    void OnEnable()
    {
        _hp = GeneralInfo.instance.botHp;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        RangeSearch();
        if (_curState == BotState.shooting)
        {
            _curDpsTime -= Time.deltaTime;
        }

        KeyBoardInput();
    }

    public override void InitializeAgent()
    {
        Debug.Log("[  BOT AGENT  ] InitializeAgent");

    }
    public override void CollectObservations()
    {
        Debug.Log("[  BOT AGENT  ] InitializeAgent");

    }

    /////////////////////////////////////////////////////////////
    public override void BotAgentAction(float[] act)
    {
        Vector3 dirToGo = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;


        //if (!_isActive)

        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
            dirToGo = transform.forward * Mathf.Clamp(act[0], -1f, 1f);
            rotateDir = transform.up * Mathf.Clamp(act[1], -1f, 1f);
        }
        else
        {
            switch ((int)(act[0]))
            {
                case 1:
                    dirToGo = transform.forward;
                    break;
                case 2:
                    Shoot();
                    break;
                case 3:
                    rotateDir = -transform.up;
                    break;
                case 4:
                    rotateDir = transform.up;
                    break;
            }
        }

        agentRB.AddForce(dirToGo * _moveSpeed , ForceMode.VelocityChange);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * _turnSpeed);


        if (agentRB.velocity.sqrMagnitude > 25f) // slow it down
        {
            agentRB.velocity *= 0.95f;
        }
        if (shoot)
        {
            myLaser.transform.localScale = new Vector3(1f, 1f, 1f);
            Vector3 position = transform.TransformDirection(RayPerception.PolarToCartesian(25f, 90f));
            Debug.DrawRay(transform.position, position, Color.red, 0f, true);
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 2f, position, out hit, 25f))
            {
                if (hit.collider.gameObject.tag == "agent")
                {
                    hit.collider.gameObject.GetComponent<BananaAgent>().Freeze();
                }
            }
        }
        
        else
        {
            myLaser.transform.localScale = new Vector3(0f, 0f, 0f);

        }
    }

    */

}
