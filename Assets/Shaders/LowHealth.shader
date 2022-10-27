Shader "Custom/LowHealth"
{
    Properties
    {
        [HideInInspector]_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Radius ("Vignette Radius", Range(0, 2)) = 1
        _Feather ("Vignette Feather", Range(0, 10)) = 1
        _TintColour ("Vignette Colour", Color) = (0.55, 0.01, 0.01, 1)
        _Greyscale ("Greyscale Amount", Range(0, 1)) = 0
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
                normalColour = lerp(normalColour, dot(normalColour, float3(0.3, 0.59, 0.11)), _Greyscale);
                float3 vignetteColour = (1 - col.rgb) * invMask * _TintColour;
                return fixed4(normalColour + vignetteColour, 1);
            }

            ENDCG
        }
    }
}
