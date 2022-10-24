Shader "Custom/Dither"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        _DitherPattern ("Dithering Pattern", 2D) = "white" {}
        _MinDis ("Minimum Dither Distance", float) = 20.0
        _MaxDis ("Maximum Dither Distance", float) = 100.0
        _Alpha ("Alpha", float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        // dithering (https://www.ronja-tutorials.com/post/042-dithering)
        CGPROGRAM

        #pragma surface surf Standard
        #pragma target 3.0

        // textures
        sampler2D _MainTex;
        sampler2D _DitherPattern;

        // dither variables
        float4 _DitherPattern_TexelSize;
        float _MinDis;
        float _MaxDis;

        struct Input 
        {
            float2 uv_MainTex;
            float4 screenPos;
        };

        // surface shader
        void surf(Input i, inout SurfaceOutputStandard o) 
        {
            // get colour
            float3 col = tex2D(_MainTex, i.uv_MainTex);
            o.Albedo = col.rgb;

            // calculate dither and normal values
            float2 screenPos = i.screenPos.xy / i.screenPos.w;
            float2 ditherCoord = screenPos * _ScreenParams.xy * _DitherPattern_TexelSize.xy;
            float dither = tex2D(_DitherPattern, ditherCoord);

            // calculate relative distance
            float relDis = i.screenPos.w - _MinDis;
            relDis = relDis / (_MaxDis - _MinDis);

            clip(relDis - dither.r);
        }

        // struct vertIn
        // {
        //     float4 vertex : POSITION;
        //     float2 uv : TEXCOORD0;
        // };

        // struct vertOut
        // {
        //     float4 position : SV_POSITION;
        //     float2 uv : TEXCOORD0;
        //     float4 screenPosition : TEXCOORD1;
        // };

        // vertex shader
        // vertOut vert(vertIn v)
        // {
        //     vertOut o;
        //     o.position = UnityObjectToClipPos(v.vertex);
        //     o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        //     o.screenPosition = ComputeScreenPos(o.position);
        //     return o;
        // }

        // // pixel shader
        // fixed4 frag(vertOut i) : SV_TARGET
        // {
        //     // calculate normal and dither values
        //     fixed4 col = tex2D(_MainTex, i.uv);
        //     float2 screenPos = i.screenPosition.xy / i.screenPosition.w;
        //     float2 ditherCoord = screenPos * _ScreenParams.xy * _DitherPattern_TexelSize.xy;
        //     float dither = tex2D(_DitherPattern, ditherCoord);

        //     // calculate relative distance
        //     float relDis = i.screenPosition.w - _MinDis;
        //     relDis = relDis / (_MaxDis - _MinDis);

        //     clip(relDis - dither.r);
        //     float4x4 thresholdMatrix = {
        //         1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
        //         13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
        //         4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
        //         16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
        //     };
        //     clip(_Alpha - thresholdMatrix[i.position.x % 4][i.position.y % 4]);
        //     return col;
        // }
        ENDCG
    }
    Fallback "Standard"
}