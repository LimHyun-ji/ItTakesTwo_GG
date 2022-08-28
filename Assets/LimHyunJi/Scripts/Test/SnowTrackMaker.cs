using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{   
    public class SnowTrackMaker : MonoBehaviour
    {
        public Shader _snowFallShader;
        private Material _snowFallMat;
        public Material _material;
        [Range(0.001f,0.1f)]
        public float _flakeAmount;
        [Range(0f,1f)]
        public float _flakeOpacity;
        // Use this for initialization
        void Start () {
           // _material = GetComponent<MeshRenderer>().material;
            _snowFallMat = new Material(_snowFallShader);

        }
        
        // Update is called once per frame
        void Update () {
            _snowFallMat.SetFloat("_FlakeAmount", _flakeAmount);
            _snowFallMat.SetFloat("_FlakeOpacity", _flakeOpacity);
            RenderTexture snow = (RenderTexture)_material.GetTexture("_Splat");

            RenderTexture temp = RenderTexture.GetTemporary(snow.width, snow.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(snow, temp, _snowFallMat);
            Graphics.Blit(temp, snow);
            _material.SetTexture("_Splat", snow);
            RenderTexture.ReleaseTemporary(temp);
        }

    }
}
