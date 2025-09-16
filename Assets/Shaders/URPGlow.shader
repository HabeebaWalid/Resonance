Shader "Custom/URPGlow"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1) 
        _MainTex ("Texture", 2D) = "white" {}      

        _GlowColor ("Glow Color", Color) = (0, 0, 0, 0) 
        _GlowIntensity ("Glow Intensity", Range (0.0, 100.0)) = 1.0 
        _PulseSpeed ("Pulse Speed", Range (0.1, 10.0)) = 1.0 
        _MinGlow ("Minimum Glow Multiplier", Range (0.0, 1.0)) = 0.5 
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" "RenderType" = "Opaque" }

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

          
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"

           
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                TEXTURE2D (_MainTex); SAMPLER (sampler_MainTex); 
                float4 _MainTex_ST; 

                float4 _GlowColor;
                float _GlowIntensity;
                float _PulseSpeed; 
                float _MinGlow; 
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION; 
                float2 uv : TEXCOORD0;        
                float3 normalOS : NORMAL;     
                UNITY_VERTEX_INPUT_INSTANCE_ID 
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION; 
                float2 uv : TEXCOORD0;           
                float3 normalWS : NORMAL;       
                UNITY_VERTEX_INPUT_INSTANCE_ID 
                UNITY_VERTEX_OUTPUT_STEREO 
            };

            Varyings Vert (Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID (input);
                UNITY_TRANSFER_INSTANCE_ID (input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO (output);

                output.positionCS = TransformObjectToHClip (input.positionOS.xyz);

                output.uv = TRANSFORM_TEX (input.uv, _MainTex);

                output.normalWS = TransformObjectToWorldNormal (input.normalOS);

                return output;
            }

            float4 Frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID (input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX (input);

                float4 baseColor = SAMPLE_TEXTURE2D (_MainTex, sampler_MainTex, input.uv) * _BaseColor;

              
                float pulseFactor = (sin (_Time.y * _PulseSpeed) * 0.5 + 0.5); 
                pulseFactor = lerp (_MinGlow, 1.0, pulseFactor); 

                float3 glow = _GlowColor.rgb * _GlowIntensity * pulseFactor;

                float3 finalColor = baseColor.rgb + glow;

                return float4 (finalColor, baseColor.a);
            }
            ENDHLSL
        }
    }
}
