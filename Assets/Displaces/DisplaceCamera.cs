using UnityEngine;

namespace Displaces
{
    public class DisplaceCamera : MonoBehaviour
    {
        public Material worldDisplaceMaterial;
        
        private Camera cam;
        private RenderTexture texture;
        private static readonly int DisplaceTex = Shader.PropertyToID("_DisplaceTex");

        private void Awake()
        {
            cam = GetComponent<Camera>();
            texture = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 16);
            cam.targetTexture = texture;
            worldDisplaceMaterial.SetTexture(DisplaceTex, texture);
        }
    }
}
