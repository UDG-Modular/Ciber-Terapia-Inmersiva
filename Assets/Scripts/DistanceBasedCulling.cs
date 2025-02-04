using UnityEngine;

public class DistanceCulling : MonoBehaviour
{
    public float cullingDistance = 50.0f;
    private Renderer rend;
    private Transform mainCamera;

    void Start()
    {
        rend = GetComponent<Renderer>();
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(mainCamera.position, transform.position);
        if (distance > cullingDistance)
        {
            if (rend.enabled)
                rend.enabled = false;
        }
        else
        {
            if (!rend.enabled)
                rend.enabled = true;
        }
    }
}
