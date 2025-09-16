Shader "Custom/UnlitFresnelEmission"
{
    Properties
    {
        [HDR] _EmissionColor ("Emission Color", Color) = (1, 1, 1, 1)
        _Intensity ("Intensity", Float) = 1.0
        _FresnelPower ("Fresnel Power", Float) = 1.0
        _Size ("Size", Float) = 0.05
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            float4 _EmissionColor;
            float _Intensity;
            float _FresnelPower;
            float _Size;

            v2f vert (appdata v)
            {
                v2f o;

                float3 worldNormal = normalize (mul ((float3x3) unity_ObjectToWorld, v.normal));
                float3 worldPos = mul (unity_ObjectToWorld, v.vertex).xyz;

                // Offset Along The World Normal
                worldPos += worldNormal * _Size;

                o.pos = UnityWorldToClipPos (worldPos);
                o.worldNormal = worldNormal;
                o.worldPos = worldPos;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

                // Add Fresnel Effect
                float fresnel = pow (1.0 - saturate (dot (i.worldNormal, viewDir)), _FresnelPower);

                // Emission 
                float3 emission = (_EmissionColor.rgb * _Intensity) * fresnel;

                return float4 (emission, 1.0);
            }
            ENDCG
        }
    }
}
