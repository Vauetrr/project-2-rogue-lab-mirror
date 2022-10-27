Shader "Custom/DepthOfField" 
{
    Properties 
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 0.5)) = 0
        [KeywordEnum(Low, Medium, High)] _Samples ("Sample Amount", Float) = 0
        [PowerSlider(3)] _StdDeviation ("Standard Deviation", Range(0.00, 0.3)) = 0.02
        _FocDis ("Focus Distance", Range(0.1, 100)) = 10.0
        _FocRng ("Focus Range", Range(0.1, 1000)) = 3.0
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

        sampler2D _MainTex, _CameraDepthTexture, _CoCTex;
        float4 _MainTex_TexelSize;
        float _FocDis, _FocRng, _BlurSize, _StdDeviation;

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

        // get coc values (https://catlikecoding.com/unity/tutorials/advanced-rendering/depth-of-field/)
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
            float frag(vertOut i) : SV_TARGET
            {
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
                depth = LinearEyeDepth(depth);
                float coc = (depth - _FocDis) / _FocRng;
                return clamp(coc, -1, 1) * _BlurSize;
            }

            ENDCG
        }
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
                    // calculate offset for sample
                    float offset = (index / (SAMPLES - 1) - 0.5) * _BlurSize;
                    float2 uv = i.uv + float2(0, offset);

                    // check if the offset should be used
                    float coc = tex2D(_CoCTex, uv).r;
                    if (abs(coc) < abs(offset))
                    {
                        continue;
                    }

                    // accumlate colour data from sample
                    float stDevSquared = _StdDeviation*_StdDeviation;
                    float gauss = (1 / sqrt(2*PI*stDevSquared)) * exp(-((offset*offset)/(2*stDevSquared)));
                    sum += gauss;
                    col += tex2D(_MainTex, uv) * gauss;
                }

                if (sum <= 0) return tex2D(_MainTex, i.uv);
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
                    // calculate offset for sample
                    float offset = (index / (SAMPLES - 1) - 0.5) * _BlurSize * invAspect;
                    float2 uv = i.uv + float2(offset, 0);

                    // check if the offset should be used
                    float coc = tex2D(_CoCTex, uv).r;
                    if (abs(coc) < abs(offset))
                    {
                        continue;
                    }

                    // accumlate colour data from sample
                    float stDevSquared = _StdDeviation*_StdDeviation;
                    float gauss = (1 / sqrt(2*PI*stDevSquared)) * exp(-((offset*offset)/(2*stDevSquared)));
                    sum += gauss;
                    col += tex2D(_MainTex, uv) * gauss;
                }
                
                if (sum <= 0) return tex2D(_MainTex, i.uv);
                col = col / sum;
                return col;
            }

            ENDCG
        }
    }
}