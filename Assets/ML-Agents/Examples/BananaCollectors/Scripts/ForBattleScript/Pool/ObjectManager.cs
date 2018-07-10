using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour
{
    Dictionary<ObjectType, ObjectPool> poolList = new Dictionary<ObjectType, ObjectPool>();

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
            poolList.Add(GetObjTypeByIndex(i), pool);
        }
    }

    //-----------------------------------------------------------------------------------
    // public functions
    public GameObject Assign(ObjectType objType, Vector3 initPos)
    {
        return poolList[objType].Assign(initPos);
    }

    public void FreeAllObj(ObjectType objType)
    {
        poolList[objType].FreeAll();
    }

    /*
    public void Free(GameObject obj)
    {
        if (!obj.activeSelf) return;

        int idx = obj.name.IndexOf('_');
        var key = obj.name.Remove(idx);
        poolList[key].Free(obj);
    }

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
    ObjectType GetObjTypeByIndex(int index)
    {
        ObjectType objType = ObjectType.missile; //just for default
        switch(index)
        {
            case 0: 
                objType = ObjectType.agent;
                break;
            case 1:
                objType = ObjectType.enemy;
                break;
            case 2:
                objType = ObjectType.missile;
                break;
        }
        return objType;
    }
}
