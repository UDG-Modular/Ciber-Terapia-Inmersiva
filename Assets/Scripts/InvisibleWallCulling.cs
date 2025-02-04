using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWallCulling : MonoBehaviour
{
    public float cullingDistance = 50.0f;
    private Collider col;
    private Transform mainCamera;

    void Start()
    {
        col = GetComponent<Collider>();
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(mainCamera.position, transform.position);
        col.enabled = distance <= cullingDistance;
    }
}