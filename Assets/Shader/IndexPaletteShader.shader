Shader "Custom/IndexPaletteShader"
{
    //show values to edit in inspector
    Properties{
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        _PaletteTex2D ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _NormalMult ("Normal Outline Multiplier", Range(0,4)) = 1
        _NormalBias ("Normal Outline Bias", Range(1,4)) = 1
        _DepthMult ("Depth Outline Multiplier", Range(0,4)) = 1
        _DepthBias ("Depth Outline Bias", Range(1,4)) = 1
    }

    SubShader{
        // markers that specify that we don't need culling 
        // or comparing/writing to the depth buffer
        Cull Off
        ZWrite Off 
        ZTest Always

        Tags{"RenderPipeline" = "UniversalRenderPipeline"}

        Pass{
            CGPROGRAM
            //include useful shader functions
            #include "UnityCG.cginc"

            //define vertex and fragment shader
            #pragma vertex vert
            #pragma fragment frag

            //the rendered screen so far
            sampler2D _MainTex;
            //the depth normals texture
            sampler2D _CameraDepthNormalsTexture;
            sampler2D _PaletteTex2D;
            //texelsize of the depthnormals texture
            float4 _CameraDepthNormalsTexture_TexelSize;

            //variables for customising the effect
            float4 _OutlineColor;
            float _NormalMult;
            float _NormalBias;
            float _DepthMult;
            float _DepthBias;
            

            //the object data that's put into the vertex shader
            struct appdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            //the data that's used to generate fragments and can be read by the fragment shader
            struct v2f{
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            //the vertex shader
            v2f vert(appdata v){
                v2f o;
                //convert the vertex positions from object space to clip space so they can be rendered
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            void Compare(inout float depthOutline, inout float normalOutline, 
                    float baseDepth, float3 baseNormal, float2 uv, float2 offset){
                //read neighbor pixel
                float4 neighborDepthnormal = tex2D(_CameraDepthNormalsTexture, 
                        uv + _CameraDepthNormalsTexture_TexelSize * offset);
                float3 neighborNormal;
                float neighborDepth;
                DecodeDepthNormal(neighborDepthnormal, neighborDepth, neighborNormal);
                neighborDepth = neighborDepth * _ProjectionParams.z;

                float depthDifference = baseDepth - neighborDepth;
                depthOutline = depthOutline + depthDifference;

                float3 normalDifference = baseNormal - neighborNormal;
                normalDifference = normalDifference.r + normalDifference.g + normalDifference.b;
                normalOutline = normalOutline + normalDifference;
            }

            float4 outline(v2f i) {
                //read depthnormal
                float4 depthnormal = tex2D(_CameraDepthNormalsTexture, i.uv);

                //decode depthnormal
                float3 normal;
                float depth;
                DecodeDepthNormal(depthnormal, depth, normal);

                //get depth as distance from camera in units 
                depth = depth * _ProjectionParams.z;

                float depthDifference = 0;
                float normalDifference = 0;

                Compare(depthDifference, normalDifference, depth, normal, i.uv, float2(1, 0));
                Compare(depthDifference, normalDifference, depth, normal, i.uv, float2(0, 1));
                Compare(depthDifference, normalDifference, depth, normal, i.uv, float2(0, -1));
                Compare(depthDifference, normalDifference, depth, normal, i.uv, float2(-1, 0));

                depthDifference = depthDifference * _DepthMult;
                depthDifference = saturate(depthDifference);
                depthDifference = pow(depthDifference, _DepthBias);

                normalDifference = normalDifference * _NormalMult;
                normalDifference = saturate(normalDifference);
                normalDifference = pow(normalDifference, _NormalBias);

                float outline = normalDifference + depthDifference;
                float4 sourceColor = tex2D(_MainTex, i.uv);
                float4 color = lerp(sourceColor, _OutlineColor, outline);

                return color;
            }

            float4 Gamma2Linear(float4 c)
            {
                return pow(c, 2.2);
            }

            float4 Linear2Gamma(float4 c)
            {
                return pow(c, 1.0 / 2.2);
            }

            fixed4 use2D(v2f i) {
                float4 color = saturate(Linear2Gamma(tex2D(_MainTex, i.uv)));
                float4 cut = floor(color * 31);
                return Gamma2Linear(tex2D(_PaletteTex2D, float2((cut.b * 32 + cut.r + 0.5) / 1024, (cut.g + 0.5) / 32)));
            }

            //the fragment shader
            fixed4 frag(v2f i) : SV_TARGET{
                return use2D(i);
            }
            ENDCG
        }
    }
}
