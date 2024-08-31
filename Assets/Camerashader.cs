using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerashader : MonoBehaviour
{
    public Material ditherMat;
    public Material thresholdMat;

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam  = GetComponent<Camera>();
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        RenderTexture large = RenderTexture.GetTemporary(2460, 1410);
        RenderTexture main = RenderTexture.GetTemporary(1640, 940);

        Vector3[] corners = new Vector3[4];

        cam.CalculateFrustumCorners(new Rect(0,0,1,1),cam.farClipPlane,
            Camera.MonoOrStereoscopicEye.Mono, corners);

        for(int i = 0; i < 4; i++)
        {
            corners[i] = transform.TransformVector(corners[i]);
            corners[i].Normalize();
        }

        ditherMat.SetVector("_BL", corners[0]);
        ditherMat.SetVector("_TL", corners[1]);
        ditherMat.SetVector("_TR", corners[2]);
        ditherMat.SetVector("_BR", corners[3]);

        Graphics.Blit(src, large, ditherMat);
        Graphics.Blit(large, main, thresholdMat);
        Graphics.Blit(main, dest);

        RenderTexture.ReleaseTemporary(large);
        RenderTexture.ReleaseTemporary(main);
    }
}
