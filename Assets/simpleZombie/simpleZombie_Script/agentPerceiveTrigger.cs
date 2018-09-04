using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agentPerceiveTrigger : MonoBehaviour {

    [SerializeField]
    playerAgent _playerAgent;

    public int listCount = 0 ;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "zombie")
        {

            bool isSame = false;
            
            for(int i =0; i<_playerAgent.aroundZombieList.Count; ++i)
            {
                if (other.gameObject.GetInstanceID() == _playerAgent.aroundZombieList[i].GetInstanceID())
                {
                    isSame = true;
                }
            }

            if (isSame == false)
            {
                //Debug.LogWarning("ZOMBIE ADD : " + _playerAgent.aroundZombieList.Count);

                _playerAgent.aroundZombieList.Add(other.gameObject);
                listCount = _playerAgent.aroundZombieList.Count;
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "zombie")
        {
            bool isSame = false;
            for (int i = 0; i < _playerAgent.aroundZombieList.Count; ++i)
            {
                if (other.gameObject.GetInstanceID() == _playerAgent.aroundZombieList[i].GetInstanceID())
                {
                    isSame = true;
                }
            }

            if (isSame == true)
            {
                _playerAgent.aroundZombieList.Remove(other.gameObject);

                //Debug.LogWarning("ZOMBIE REMOVED : " + _playerAgent.aroundZombieList.Count);
                listCount = _playerAgent.aroundZombieList.Count;
            }
        }

    }
}
