Shader "Custom/LowHealth"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 0.1)) = 0
    }
    SubShader
    {
        Cull off
        ZWrite off
        ZTest Always

        Pass
        {
            CGPROGRAM
            
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float _BlurSize;

            struct vertIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vertOut
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // vertex shader
            vertOut vert(vertIn v)
            {
                vertOut o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // fragment shader
            fixed4 frag(vertOut p) : SV_TARGET
            {
                fixed4 col = 0;
                for(float index = 0; index < 10; index++)
                {
                    float2 uv = p.uv + float2(0, (index / 9 - 0.5) * _BlurSize);
                    col += tex2D(_MainTex, uv);
                }
                col = col / 10;
                return col;
            }

            ENDCG
        }
        Pass
        {
            CGPROGRAM
            
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float _BlurSize;

            struct vertIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vertOut
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // vertex shader
            vertOut vert(vertIn v)
            {
                vertOut o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // fragment shader
            fixed4 frag(vertOut p) : SV_TARGET
            {
                float invAspect = _ScreenParams.y / _ScreenParams.x;
                fixed4 col = 0;
                for(float index = 0; index < 10; index++)
                {
                    float2 uv = p.uv + float2((index / 9 - 0.5) * _BlurSize * invAspect, 0);
                    col += tex2D(_MainTex, uv);
                }
                col = col / 10;
                return col;
            }

            ENDCG
        }
    }
}
