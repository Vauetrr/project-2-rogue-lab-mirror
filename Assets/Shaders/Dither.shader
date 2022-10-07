Shader "Custom/Dither"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        _DitherPattern ("Dithering Pattern", 2D) = "white" {}
        _Color1 ("Dither Color 1", Color) = (0, 0, 0, 1)
        _Color2 ("Dither Color 2", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
            float4 _Color1;
            float4 _Color2;

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
                fixed4 col = tex2D(_MainTex, i.uv).r;
                float2 screenPos = i.screenPosition.xy / i.screenPosition.w;
                float2 ditherCoord = screenPos * _ScreenParams.xy * _DitherPattern_TexelSize.xy;
                float dither = tex2D(_DitherPattern, ditherCoord).r;

                // combine together
                float dithered = step(dither, col);
                return lerp(_Color1, _Color2, dithered);
            }
            ENDCG
        }
    }
    Fallback "Standard"
}