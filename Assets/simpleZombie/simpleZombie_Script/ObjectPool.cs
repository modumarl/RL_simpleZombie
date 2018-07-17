using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    GameObject _objType = null;
    List<GameObject> _objList = new List<GameObject>();

    //List<GameObject> _activeObjList = new List<GameObject>();
    //public List<GameObject> activeObjList = new List<GameObject>();
    //     public List<GameObject> activeObjList
    //     {
    //         get { return _activeObjList; }
    //     }

    int _allocSize;

    public void Init(GameObject objType, int size, GameObject parentObj)
    {
        _objType = objType;
        _allocSize = size;
        MemAlloc(parentObj);
    }

    void MemAlloc(GameObject parentObj)
    {
        for (int i = 0; i < _allocSize; ++i)
        {
            GameObject obj = GameObject.Instantiate(_objType) as GameObject;
            obj.name = _objType.name + "_" + _objList.Count;
            obj.transform.parent = parentObj.transform;
            //obj.transform.localScale = Vector3.one;
            obj.SetActive(false);

            _objList.Add(obj);
        }
    }

    public GameObject Assign(Vector3 initPos)
    {
        GameObject obj = null;

        for (int i = 0; i < _allocSize; ++i)
        {
            if (_objList[i].activeSelf == false)
            {
                _objList[i].transform.position = initPos;
                _objList[i].SetActive(true);
                obj = _objList[i];
                break;
            }
        }
        // TODO bullet growing

        if (obj == null)
        {
            Debug.LogError("Assign Failed");
        }
        return obj;
    }


    //     //public void Assign(Vector3 initPos)
    //     public void Assign(Vector3 initPos, int tileValue, int curX, int curY)
    //     {
    //         for (int i = 0; i < _allocSize; ++i)
    //         {
    //             if (_objList[i].activeSelf == false)
    //             {
    //                 _objList[i].SetActive(true);
    //                 _activeObjList.Add(_objList[i]);
    //                 //Debug.Log("Recv initPos In Pool = " + initPos);
    //                 //_objList[i].transform.localScale = Vector3.one;
    // 
    //                 _objList[i].transform.localPosition = initPos;
    //                 _objList[i].transform.localScale = Vector3.one;
    //                 //_objList[i].transform.OverlayPosition(initPos, Camera.main, UICamera.mainCamera);
    //                 //                 Debug.Log("posed Obj Name = " + _objList[i].name);
    //                 //                 Debug.Log("Setted initPos In Pool = " + _objList[i].transform.position);
    // 
    //                 var tileObjScript = _objList[i].transform.GetComponent<TileObj_puz>();
    //                 tileObjScript.value = tileValue;
    //                 tileObjScript.curX = curX;
    //                 tileObjScript.curY = curY;
    // 
    // 
    //                 return;
    //             }
    //         }
    // 
    //         Debug.LogWarning("Assign Failed");
    //     }

    public void Free(GameObject obj)
    {
        obj.SetActive(false);
        // _activeObjList.Remove(obj);
    }

    public void FreeAll()
    {
        for (int i = 0; i < _allocSize; ++i)
        {
            _objList[i].SetActive(false);
        }
    }

    public List<GameObject> GetObjList()
    {
        return _objList;
    }

}
