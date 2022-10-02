Shader "Custom/LowHealth"
{
    Properties
    {
        [HideInInspector]_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 0.5)) = 0
        [KeywordEnum(Low, Medium, High)] _Samples ("Sample Amount", Float) = 0
        [PowerSlider(3)] _StdDeviation ("Standard Deviation", Range(0.00, 0.3)) = 0.02
        _Radius ("Vignette Radius", Range(0, 10)) = 1
        _Feather ("Vignette Feather", Range(0, 10)) = 1
        _TintColour ("Vignette Colour", Color) = (0.55, 0.01, 0.01, 1)
    }
    SubShader
    {
        Cull off
        ZWrite off
        ZTest Always

        CGINCLUDE

        #include "UnityCG.cginc"

        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile _SAMPLES_LOW _SAMPLES_MEDIUM _SAMPLES_HIGH

        #define PI 3.14159265359

        #if _SAMPLES_LOW
            #define SAMPLES 10
        #elif _SAMPLES_MEDIUM
            #define SAMPLES 30
        #else
            #define SAMPLES 100
        #endif

        sampler2D _MainTex;

        // blur vars
        float _BlurSize;
        float _StdDeviation;

        // vignette vars
        float _Radius;
        float _Feather;
        float4 _TintColour;

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

        ENDCG
        
        // vertical blur (https://www.ronja-tutorials.com/post/023-postprocessing-blur/)
        Pass
        {
            CGPROGRAM

            // vertex shader
            vertOut vert(vertIn v)
            {
                vertOut o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // fragment shader
            fixed4 frag(vertOut i) : SV_TARGET
            {
                // failsafe
                if(_StdDeviation == 0) return tex2D(_MainTex, i.uv);

                fixed4 col = 0;
                float sum = 0;
                
                // iterate over nearby pixels and aggregate to blur
                for(float index = 0; index < SAMPLES; index++)
                {
                    float offset = (index / (SAMPLES - 1) - 0.5) * _BlurSize;
                    float2 uv = i.uv + float2(0, offset);
                    float stDevSquared = _StdDeviation*_StdDeviation;
                    float gauss = (1 / sqrt(2*PI*stDevSquared)) * exp(-((offset*offset)/(2*stDevSquared)));
                    sum += gauss;
                    col += tex2D(_MainTex, uv) * gauss;
                }

                col = col / sum;
                return col;
            }

            ENDCG
        }
        // horizontal blur (https://www.ronja-tutorials.com/post/023-postprocessing-blur/)
        Pass
        {
            CGPROGRAM

            // vertex shader
            vertOut vert(vertIn v)
            {
                vertOut o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // fragment shader
            fixed4 frag(vertOut i) : SV_TARGET
            {
                // failsafe
                if(_StdDeviation == 0) return tex2D(_MainTex, i.uv);

                fixed4 col = 0;
                float sum = 0;
                float invAspect = _ScreenParams.y / _ScreenParams.x;
                
                // iterate over nearby pixels and aggregate to blur
                for(float index = 0; index < SAMPLES; index++)
                {
                    float offset = (index / (SAMPLES - 1) - 0.5) * _BlurSize * invAspect;
                    float2 uv = i.uv + float2(offset, 0);
                    float stDevSquared = _StdDeviation*_StdDeviation;
                    float gauss = (1 / sqrt(2*PI*stDevSquared)) * exp(-((offset*offset)/(2*stDevSquared)));
                    sum += gauss;
                    col += tex2D(_MainTex, uv) * gauss;
                }
                
                col = col / sum;
                return col;
            }

            ENDCG
        }
        // vignette (https://www.youtube.com/watch?v=GiQ5OvDN8dE)
        Pass
        {
            CGPROGRAM

            // vertex shader
            vertOut vert(vertIn v)
            {
                vertOut o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // fragment shader
            fixed4 frag(vertOut i) : SV_TARGET
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 newUV = i.uv * 2 - 1;
                float circle = length(newUV);
                float invMask = smoothstep(_Radius, _Radius + _Feather, circle);
                float mask = 1 - invMask;

                float3 normalColour = col.rgb * mask;
                float3 vignetteColour = (1 - col.rgb) * invMask * _TintColour;
                return fixed4(normalColour + vignetteColour, 1);
            }

            ENDCG
        }
    }
}
