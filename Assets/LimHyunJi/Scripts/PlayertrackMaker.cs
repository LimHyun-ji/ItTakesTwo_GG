using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItTakesTwo
{
    public class PlayertrackMaker : MonoBehaviour
    {
        public Shader _drawShader;
        private Material _drawMaterial;
        public Material myMaterial;
        public GameObject _terrain;
        
        private RenderTexture _splatmap;
        RaycastHit _groundHit;
        int _layerMask;

        [Range(0, 200)]
        public float _brushSize;
        [Range(0, 10)]
        public float _brushStrength;
        // Use this for initialization
        void Start () {
            _layerMask = LayerMask.GetMask("Ground");
            _drawMaterial = new Material(_drawShader);
            // _drawMaterial.SetVector("_Color", Color.red);

            //myMaterial = _terrain.GetComponent<MeshRenderer>().material;
            myMaterial.SetTexture("_Splat", _splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));

        }

        // Update is called once per frame
        void Update () 
        {
           
            if (Physics.Raycast(transform.position,-Vector3.up, out _groundHit,1.2f,_layerMask))
            {
                _drawMaterial.SetVector("_Coordinate", new Vector4(_groundHit.textureCoord.x, _groundHit.textureCoord.y, 0, 0));
                _drawMaterial.SetFloat("_Strength", _brushStrength);
                _drawMaterial.SetFloat("_Size", _brushSize);
                RenderTexture temp = RenderTexture.GetTemporary(_splatmap.width, _splatmap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_splatmap, temp);
                Graphics.Blit(temp, _splatmap, _drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }           
            
        }
    }
}

