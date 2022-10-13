Shader "Custom/Dither"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        _DitherPattern ("Dithering Pattern", 2D) = "white" {}
        _MinDis ("Minimum Dither Distance", float) = 0.0
        _MaxDis ("Maximum Dither Distance", float) = 100.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        // dithering (https://www.ronja-tutorials.com/post/042-dithering)
        Pass
        {
            CGPROGRAM

            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            // textures
            sampler2D _MainTex;
            sampler2D _DitherPattern;

            // dither variables
            float4 _MainTex_ST;
            float4 _DitherPattern_TexelSize;
            float _MinDis;
            float _MaxDis;

            struct vertIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vertOut
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPosition : TEXCOORD1;
            };

            // vertex shader
            vertOut vert(vertIn v)
            {
                vertOut o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPosition = ComputeScreenPos(o.position);
                return o;
            }

            // pixel shader
            fixed4 frag(vertOut i) : SV_TARGET
            {
                // calculate normal and dither values
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 screenPos = i.screenPosition.xy / i.screenPosition.w;
                float2 ditherCoord = screenPos * _ScreenParams.xy * _DitherPattern_TexelSize.xy;
                float dither = tex2D(_DitherPattern, ditherCoord);

                // calculate relative distance
                float relDis = i.screenPosition.w - _MinDis;
                relDis = relDis / (_MaxDis - _MinDis);

                clip(relDis - dither.r);
                return col;
            }
            ENDCG
        }
    }
    Fallback "Standard"
}