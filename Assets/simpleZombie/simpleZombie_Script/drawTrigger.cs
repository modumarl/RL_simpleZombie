using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawTrigger : MonoBehaviour
{

    public SphereCollider perceiveTrigger;
    public LineRenderer lineR_circle;

    public float ThetaScale = 0.01f;
    public float radius = 3f;
    private int Size;
    private float Theta = 0f;

    void Start()
    {

        //lineR_circle.SetVertexCount(segments + 1);
        //lineR_circle.useWorldSpace = false;
    }
    private void FixedUpdate()
    {
        CreatePoints();

    }

    void CreatePoints()
    {
        Theta = 0f;
        Size = (int)((1f / ThetaScale) + 1f);
        lineR_circle.SetVertexCount(Size);
        for (int i = 0; i < Size; i++)
        {
            Theta += (2.0f * Mathf.PI * ThetaScale);
            float x = radius * Mathf.Cos(Theta);
            float z = radius * Mathf.Sin(Theta);
            lineR_circle.SetPosition(i, new Vector3(x, 2, z));
        }
    }
}
