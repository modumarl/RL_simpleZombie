using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : MonoBehaviour
{
    // obj pool에 요청해서 

    public void ShootMissile(Teams masterTeam, float shootPower, Vector3 initPos, Vector3 shootDir)
    {
        //Debug.LogWarning("shoot CALLED!");

        GameObject missile = ObjectManager.instance.Assign(ObjectType.missile, initPos);
        missile.GetComponent<Missile>().SetTeam(masterTeam);
        missile.GetComponent<Missile>().masterBot = this.gameObject;

        //Debug.Log("MASETER BOT = " + this.gameObject);

        missile.GetComponent<Rigidbody>().AddForce(shootDir * shootPower, ForceMode.Impulse);

    }

}
