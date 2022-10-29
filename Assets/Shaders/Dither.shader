Shader "Custom/Dither"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        _DitherPattern ("Dithering Pattern", 2D) = "white" {}
        _Radius ("Dither Radius", Range(0, 2)) = 1
        _Feather ("Dither Feather", Range(0, 100)) = 5
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
        float _Radius, _Feather, _Alpha;

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

            // calculate dither circle
            float2 middlePos = (screenPos * 2 - 1);
            middlePos.y *= _ScreenParams.y / _ScreenParams.x;
            float circle = length(middlePos);
            float mask = smoothstep(_Radius, _Radius + _Feather, circle);

            clip(mask - dither.r);
        }

        ENDCG
    }
    Fallback "Standard"
}