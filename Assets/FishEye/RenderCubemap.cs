using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RenderCubemap : MonoBehaviour
{
    public RenderTexture cubemap;
    public int resolution = 256;
    public Camera uiCamera; // Reference to the UI camera

    void Initialize()
    {
        if (cubemap == null)
        {
            cubemap = new RenderTexture(resolution, resolution, 24);
            cubemap.dimension = TextureDimension.Cube;
        }
    }

    void Awake()
    {
        Initialize();
    }

    void LateUpdate()
    {
        Camera cam = GetComponent<Camera>();
        cam.RenderToCubemap(cubemap, 63, Camera.MonoOrStereoscopicEye.Mono);

        if (uiCamera != null)
        {
            RenderTexture previousRT = uiCamera.targetTexture;
            uiCamera.targetTexture = cubemap;
            uiCamera.Render();
            uiCamera.targetTexture = previousRT;
        }
    }
}

