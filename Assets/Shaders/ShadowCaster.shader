Shader "Custom/InvisibleShadowCaster"
{
    SubShader
    {
        Tags { "Queue"="AlphaTest" "RenderType"="Transparent" }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(float4 vertex : POSITION)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(vertex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return float4(0, 0, 0, 0); // Invisible but still interacts with shadows
            }
            ENDCG
        }
    }
}
