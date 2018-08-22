using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agentPerceiveTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "zombie")
        {
            Debug.LogWarning("ZOMBIE ENTER!@!@!@!@!@");

            // todo 
            // obj를 parent obj script에 넘기면
            // instance ID 봐 가면서 list 관리 할것 
            // Trigger EXit도 마찬가지 

        }



    }
}
