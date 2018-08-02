using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Teams
{
    Team_agent,
    Team_enemy,

}

public enum BotState
{
    shooting,
    normal,
    daed,
}

public enum battleObjectType
{
    missile,
    agent,
    enemy,

    //wall


}

//////////////////////////////////////

public enum AgentState
{
    shotReady,
    shotWaiting,
    dead,
    
}

public enum ZombieState
{
    attackReady,
    attackWaiting,
    dead,


}

public enum ObjType
{
    playerAgent,
    zombie,
    shotParticle, 


}