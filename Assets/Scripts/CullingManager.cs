using UnityEngine;
using System.Collections.Generic;

public class CullingManager : MonoBehaviour
{
    public static CullingManager Instance { get; private set; }

    public float cullingDistance = 50.0f;
    private List<Renderer> renderers = new List<Renderer>();
    private Transform mainCamera;
    private Plane[] frustumPlanes;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (Camera.main != null)
        {
            mainCamera = Camera.main.transform;
        }
        else
        {
            Debug.LogError("Main camera not found. Please tag your camera as 'MainCamera'.");
        }
    }

    void Update()
    {
        if (mainCamera == null) return;

        Camera cam = Camera.main;
        frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);

        foreach (var rend in renderers)
        {
            float distance = Vector3.Distance(mainCamera.position, rend.transform.position);
            bool inView = GeometryUtility.TestPlanesAABB(frustumPlanes, rend.bounds);

            if (distance > cullingDistance || !inView)
            {
                rend.enabled = false;
            }
            else
            {
                rend.enabled = true;
            }
        }
    }

    public void RegisterRenderer(Renderer rend)
    {
        if (!renderers.Contains(rend))
        {
            renderers.Add(rend);
        }
    }

    public void UnregisterRenderer(Renderer rend)
    {
        if (renderers.Contains(rend))
        {
            renderers.Remove(rend);
        }
    }
}
