using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour
{
    Dictionary<ObjType, ObjectPool> poolList = new Dictionary<ObjType, ObjectPool>();

    public static ObjectManager instance { get; private set; }

    //-----------------------------------------------------------------------------------
    // inspector field
    public List<GameObject> ObjectList = new List<GameObject>();
    public List<int> SizeList = new List<int>();

    //-----------------------------------------------------------------------------------
    // handler functions
    void Start()
    {
        if (instance == null)
            instance = this;

        if (ObjectList.Count != SizeList.Count)
            Debug.LogError("ObjectPool size is invalied!");

        for (int i = 0; i < ObjectList.Count; ++i)
        {
            var pool = new ObjectPool();
            pool.Init(ObjectList[i], SizeList[i],this.gameObject);
            poolList.Add(GetObjType(i), pool);
        }
    }

    //-----------------------------------------------------------------------------------
    // public functions
    public GameObject Assign(ObjType objType, Vector3 initPos)
    {
        if (objType == ObjType.zombie)
        {
            GameObject zombieObj = poolList[objType].Assign(initPos);
            //zombieObj.transform.GetComponent<enemyZombie>().InitializeZombie();

            return zombieObj;
        }

        else
        {
            return poolList[objType].Assign(initPos);
        }


    }

    //public void FreeAllObj(battleObjectType objType)
    public void FreeAllObj(ObjType objType)
    {
        poolList[objType].FreeAll();
    }

    public void Free(GameObject obj, ObjType objType)
    {
        if (!obj.activeSelf) return;

        int idx = obj.name.IndexOf('_');
        var key = obj.name.Remove(idx);
        obj.transform.parent = transform;
        poolList[objType].Free(obj);
    }

    /*

    public void FreeAll()
    {
        foreach (var pool in poolList)
            pool.Value.FreeAll();
    }

    public void FreeAfter(GameObject obj, float after)
    {
        StartCoroutine(After(obj, after));
    }

    //-----------------------------------------------------------------------------------
    // coroutine functions
    IEnumerator After(GameObject obj, float after)
    {
        yield return new WaitForSeconds(after);
        Free(obj);
    }
    */


    //hard coding
    ObjType GetObjType(int index)
    {
        ObjType objType = ObjType.playerAgent;

        switch(index)
        {
            case 0:
                objType = ObjType.zombie;
                break;

            case 1:
                objType = ObjType.shotParticle;
                break;

            /*
            case 2:
                objType = ObjType.playerAgent;
                break;
                */
        }

        return objType;
    }

}
