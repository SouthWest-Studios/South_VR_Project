Shader "Custom/OutlineOnly"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth ("Outline Width", Range(0.001, 0.05)) = 0.005
        _NormalThreshold ("Normal Threshold", Range(0.1, 10)) = 1.5
        _DepthThreshold ("Depth Threshold", Range(0.001, 0.1)) = 0.01
        _DepthRange ("Depth Range", Range(0.1, 10)) = 5.0
    }

    SubShader
    {
        Tags { 
            "RenderType"="Opaque" 
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Geometry+1"
        }

        Pass
        {
            Name "ZPrepass"
            ColorMask 0
            ZWrite On

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                return half4(0, 0, 0, 0);
            }
            ENDHLSL
        }

        Pass
        {
            Name "OutlinePass"
            Cull Front
            ZTest Less
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            // Implementación completa de todas las funciones necesarias
            float DecodeFloatRG(float2 enc)
            {
                float2 kDecodeDot = float2(1.0, 1.0/255.0);
                return dot(enc, kDecodeDot);
            }

            float3 DecodeViewNormalStereo(float4 enc4)
            {
                float kScale = 1.7777;
                float3 nn = enc4.xyz * float3(2*kScale, 2*kScale, 0) + float3(-kScale, -kScale, 1);
                float g = 2.0 / dot(nn.xyz, nn.xyz);
                float3 n;
                n.xy = g * nn.xy;
                n.z = g - 1;
                return n;
            }

            void DecodeDepthNormal(float4 enc, out float depth, out float3 normal)
            {
                depth = DecodeFloatRG(enc.zw);
                normal = DecodeViewNormalStereo(enc);
            }

            TEXTURE2D(_CameraDepthNormalsTexture);
            SAMPLER(sampler_CameraDepthNormalsTexture);

            void SampleDepthNormal(float2 uv, out float3 normal, out float depth)
            {
                float4 cdn = SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, uv);
                DecodeDepthNormal(cdn, depth, normal);
            }

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float _OutlineWidth;
                float _NormalThreshold;
                float _DepthThreshold;
                float _DepthRange;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                float3 positionOS = input.positionOS.xyz + input.normalOS * _OutlineWidth * 0.01;
                output.positionCS = TransformObjectToHClip(positionOS);
                output.uv = input.uv;
                float3 worldPos = TransformObjectToWorld(input.positionOS.xyz);
                output.viewDirWS = GetWorldSpaceViewDir(worldPos);
                return output;
            }

            static float2 sobelSamplePoints[4] = {
                float2(-1, 1), float2(1, 1), float2(-1, -1), float2(1, -1)
            };

            half4 frag(Varyings input) : SV_Target
            {
                float2 sobelNormal = 0;
                float2 sobelDepth = 0;
                
                [unroll]
                for (int i = 0; i < 4; i++)
                {
                    float2 sampleUV = input.uv + sobelSamplePoints[i] * _OutlineWidth;
                    
                    float3 normal;
                    float depth;
                    SampleDepthNormal(sampleUV, normal, depth);
                    
                    normal = normal * 2.0 - 1.0;
                    float2 kernel = sobelSamplePoints[i];
                    sobelNormal += normal.xy * kernel;
                    sobelDepth += depth * kernel;
                }
                
                float normalEdge = length(sobelNormal) * _NormalThreshold;
                float depthEdge = length(sobelDepth) * _DepthThreshold;
                float sceneDepth = LinearEyeDepth(SampleSceneDepth(input.uv), _ZBufferParams);
                depthEdge *= saturate(sceneDepth / _DepthRange);
                float edge = max(normalEdge, depthEdge);
                edge = smoothstep(0.3, 0.7, edge);
                return float4(_OutlineColor.rgb, _OutlineColor.a * edge);
            }
            ENDHLSL
        }
    }
}