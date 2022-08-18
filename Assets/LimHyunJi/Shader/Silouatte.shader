Shader "Custom/Silhouette"
{
    Properties
    {
        _SilhouetteColor ("Silhouette Color", Color) = (1, 0, 0, 0.5)

        [Space]
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap("Bumpmap", 2D)="bump"{}
        _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
        _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        /****************************************************************
        *                            Pass 1
        *****************************************************************
        * - 메인 패스
        * - 스텐실 버퍼에 Ref 2 기록
        *****************************************************************/
        ZWrite On

        Stencil
        {
            Ref 2
            Pass Replace // Stencil, Z Test 모두 성공한 부분에 2 기록
        }

        CGPROGRAM
        #pragma surface surf Lambert
        #pragma target 3.0
        
        fixed4 _Color;
        sampler2D _MainTex;
        sampler2D _BumpMap;
        float4 _RimColor;
        float _RimPower;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 viewDir;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
            o.Emission = _RimColor.rgb * pow (rim, _RimPower);
        }
        ENDCG

        /****************************************************************
        *                            Pass 2
        *****************************************************************
        * - Zwrite off
        * - ZTest Greater : 다른 물체에 가려진 부분에 단색 실루엣 렌더링
        * - Stencil NotEqual : 다른 실루엣이 그려진 부분에 덮어쓰지 않기
        *****************************************************************/
        ZWrite Off
        ZTest Greater // 가려진 부분에 항상 그린다

        Stencil
        {
            Ref 2
            Comp NotEqual // 패스 1에서 렌더링 성공한 부분에는 그리지 않도록 한다
        }

        CGPROGRAM
        #pragma surface surf nolight alpha:fade noforwardadd nolightmap noambient novertexlights noshadow
        
        struct Input { float4 color:COLOR; };
        float4 _SilhouetteColor;
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Emission = _SilhouetteColor.rgb;
            o.Alpha = _SilhouetteColor.a;
        }
        float4 Lightingnolight(SurfaceOutput s, float3 lightDir, float atten)
        {
            return float4(s.Emission, s.Alpha);
        }
        ENDCG
    }
    FallBack "Diffuse"
}