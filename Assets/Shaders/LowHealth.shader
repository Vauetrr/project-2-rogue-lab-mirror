Shader "Custom/LowHealth"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
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
                fixed4 col = tex2D(_MainTex, p.uv);
                col = 1 - col;
                return col;
            }

            ENDCG
        }
    }
}
