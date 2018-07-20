using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ray perception component. Attach this to agents to enable "local perception"
/// via the use of ray casts directed outward from the agent. 
/// </summary>
public class RayPerception : MonoBehaviour
{
    List<float> perceptionBuffer = new List<float>();
    List<GameObject> perceptionObjList = new List<GameObject>();

    Vector3 endPosition;
    RaycastHit hit;
    /// <summary>
    /// Creates perception vector to be used as part of an observation of an agent.
    /// </summary>
    /// <returns>The partial vector observation corresponding to the set of rays</returns>
    /// <param name="rayDistance">Radius of rays</param>
    /// <param name="rayAngles">Anlges of rays (starting from (1,0) on unit circle).</param>
    /// <param name="detectableObjects">List of tags which correspond to object types agent can see</param>
    /// <param name="startOffset">Starting heigh offset of ray from center of agent.</param>
    /// <param name="endOffset">Ending height offset of ray from center of agent.</param>
    public List<float> Perceive(float rayDistance,
                         float[] rayAngles, string[] detectableObjects,
                          float startOffset, float endOffset)
    {
        perceptionBuffer.Clear();
        // For each ray sublist stores categorial information on detected object
        // along with object distance.
        foreach (float angle in rayAngles)
        {
            endPosition = transform.TransformDirection(
                PolarToCartesian(rayDistance, angle));
            endPosition.y = endOffset;
            if (Application.isEditor)
            {
                Debug.DrawRay(transform.position + new Vector3(0f, startOffset, 0f),
              endPosition, Color.black, 0.01f, true);
            }
            float[] subList = new float[detectableObjects.Length + 2];
            if (Physics.SphereCast(transform.position +
                                   new Vector3(0f, startOffset, 0f), 0.5f,
                                   endPosition, out hit, rayDistance))
            {
                for (int i = 0; i < detectableObjects.Length; i++)
                {
                    if (hit.collider.gameObject.CompareTag(detectableObjects[i]))
                    {
                        subList[i] = 1;
                        subList[detectableObjects.Length + 1] = hit.distance / rayDistance;
                        break;
                    }
                }
            }
            else
            {
                subList[detectableObjects.Length] = 1f;
            }
            perceptionBuffer.AddRange(subList);
        }
        return perceptionBuffer;
    }

    public List<GameObject> PerceiveObjectList(float rayDistance,
                     float[] rayAngles, string detectObjLayerName,
                      float startOffset, float endOffset)
    {
        perceptionObjList.Clear();
        foreach (float angle in rayAngles)
        {
            endPosition = transform.TransformDirection(
                PolarToCartesian(rayDistance, angle));
            endPosition.y = endOffset;
            if (Application.isEditor)
            {
                Debug.DrawRay(transform.position + new Vector3(0f, startOffset, 0f),
              endPosition, Color.black, 0.01f, true);
            }

            if (Physics.SphereCast(transform.position +
                                   new Vector3(0f, startOffset, 0f), 0.5f,
                                   endPosition, out hit, rayDistance))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer(detectObjLayerName))
                {

                    //List 돌면서 검색해서 그 오브젝트인지 확인

                    bool isSame = false;

                    for(int i=0;i< perceptionObjList.Count; ++i)
                    {
                        if(hit.collider.gameObject.GetInstanceID() == perceptionObjList[i].GetInstanceID())
                        {
                            isSame = true;
                        }
                    }
                    
                    if(isSame == false)
                    {
                        perceptionObjList.Add(hit.collider.gameObject);
                    }
                }

                /*
                for (int i = 0; i < detectableObjects.Length; i++)
                {
                    if (hit.collider.gameObject.CompareTag(detectableObjects[i]))
                    {
                        perceptionObjList.Add(hit.collider.gameObject);
                        break;
                    }
                }
                */
            }
        }
        return perceptionObjList;
    }


    /// <summary>
    /// Converts polar coordinate to cartesian coordinate.
    /// </summary>
    public static Vector3 PolarToCartesian(float radius, float angle)
    {
        float x = radius * Mathf.Cos(DegreeToRadian(angle));
        float z = radius * Mathf.Sin(DegreeToRadian(angle));
        return new Vector3(x, 0f, z);
    }

    /// <summary>
    /// Converts degrees to radians.
    /// </summary>
    public static float DegreeToRadian(float degree)
    {
        return degree * Mathf.PI / 180f;
    }
}
