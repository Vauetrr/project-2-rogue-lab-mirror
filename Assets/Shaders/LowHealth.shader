Shader "Custom/LowHealth"
{
    Properties
    {
        [HideInInspector]_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Radius ("Vignette Radius", Range(0, 2)) = 1
        _Feather ("Vignette Feather", Range(0, 10)) = 1
        _TintColour ("Vignette Colour", Color) = (0.55, 0.01, 0.01, 1)
        _Greyscale ("Greyscale Amount", Range(0, 1)) = 0
        _LeftVis ("Left vision offset", Range(0, 0.2)) = 0
        _RightVis ("Right vision offset", Range(0, 0.2)) = 0
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

        sampler2D _MainTex;

        // vignette vars
        float _Radius;
        float _Feather;
        float4 _TintColour;
        float _Greyscale;

        // double vision vars
        float _LeftVis, _RightVis;

        struct vertIn
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct vertOut
        {
            float4 position : SV_POSITION;
            float2 uv : TEXCOORD0;
            float2 leftUv : TEXCOORD1;
            float2 rightUv : TEXCOORD2;
        };

        ENDCG
        
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
                o.leftUv = v.uv - float2(_LeftVis, 0);
                o.rightUv = v.uv + float2(_RightVis, 0);
                return o;
            }

            // fragment shader
            fixed4 frag(vertOut i) : SV_TARGET
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // calculate vignette mask
                float2 newUV = i.uv * 2 - 1;
                float circle = length(newUV);
                float invMask = smoothstep(_Radius, _Radius + _Feather, circle);
                float mask = 1 - invMask;

                // add greyscale and double vision to the normal colour
                float3 normalColour = lerp(tex2D(_MainTex, i.leftUv), tex2D(_MainTex, i.rightUv), 0.5).rgb * mask;
                normalColour = lerp(normalColour, dot(normalColour, float3(0.3, 0.59, 0.11)), _Greyscale);

                // blend vignette and normal colour together
                float3 vignetteColour = (1 - col.rgb) * invMask * _TintColour;
                return fixed4(normalColour + vignetteColour, 1);
            }

            ENDCG
        }
    }
}
