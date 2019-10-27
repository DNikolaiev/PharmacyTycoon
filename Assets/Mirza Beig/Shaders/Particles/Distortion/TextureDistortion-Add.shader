// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.09558821,fgcg:0.09558821,fgcb:0.09558821,fgca:1,fgde:0.02,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32857,y:32675,varname:node_4795,prsc:2|emission-3675-OUT;n:type:ShaderForge.SFN_Tex2d,id:658,x:32292,y:32593,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0afcdac5cae18544eb368987da8d9891,ntxv:0,isnm:False|UVIN-7363-OUT;n:type:ShaderForge.SFN_Multiply,id:3675,x:32608,y:32763,varname:node_3675,prsc:2|A-658-RGB,B-7296-RGB,C-4133-RGB,D-3590-OUT,E-374-OUT;n:type:ShaderForge.SFN_VertexColor,id:7296,x:32292,y:32764,varname:node_7296,prsc:2;n:type:ShaderForge.SFN_Color,id:4133,x:32292,y:32922,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:3590,x:32292,y:33073,varname:node_3590,prsc:2,v1:2;n:type:ShaderForge.SFN_Time,id:178,x:31626,y:32811,varname:node_178,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:5352,x:31626,y:33003,ptovrint:False,ptlb:U Speed (Main),ptin:_USpeedMain,varname:_USpeed_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.01;n:type:ShaderForge.SFN_Append,id:3672,x:31865,y:32988,varname:node_3672,prsc:2|A-5352-OUT,B-9762-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9762,x:31626,y:33087,ptovrint:False,ptlb:V Speed (Main),ptin:_VSpeedMain,varname:_VSpeed_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.125;n:type:ShaderForge.SFN_Multiply,id:4474,x:31865,y:32832,varname:node_4474,prsc:2|A-178-T,B-3672-OUT;n:type:ShaderForge.SFN_Add,id:7363,x:32092,y:32593,varname:node_7363,prsc:2|A-7843-OUT,B-4474-OUT;n:type:ShaderForge.SFN_TexCoord,id:8906,x:31666,y:33211,varname:node_8906,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ComponentMask,id:927,x:31834,y:33211,varname:node_927,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-8906-V;n:type:ShaderForge.SFN_RemapRange,id:8203,x:32009,y:33211,varname:node_8203,prsc:2,frmn:0,frmx:1,tomn:1,tomx:0|IN-927-OUT;n:type:ShaderForge.SFN_Tex2d,id:9998,x:31246,y:32132,ptovrint:False,ptlb:NoiseTex,ptin:_NoiseTex,varname:_NoiseTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-8563-OUT;n:type:ShaderForge.SFN_Lerp,id:1756,x:31529,y:32132,varname:node_1756,prsc:2|A-1840-UVOUT,B-9998-R,T-3855-OUT;n:type:ShaderForge.SFN_Slider,id:3855,x:31089,y:32361,ptovrint:False,ptlb:Noise Amount,ptin:_NoiseAmount,varname:_NoiseAmount_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.25,max:0.25;n:type:ShaderForge.SFN_TexCoord,id:1840,x:31246,y:31955,varname:node_1840,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:3874,x:30628,y:32289,varname:node_3874,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:4539,x:30628,y:32460,ptovrint:False,ptlb:U Speed (Noise),ptin:_USpeedNoise,varname:_USpeedNoise_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Append,id:3232,x:30867,y:32445,varname:node_3232,prsc:2|A-4539-OUT,B-9953-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9953,x:30628,y:32544,ptovrint:False,ptlb:V Speed (Noise),ptin:_VSpeedNoise,varname:_VSpeedNoise_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.05;n:type:ShaderForge.SFN_Multiply,id:9007,x:30867,y:32289,varname:node_9007,prsc:2|A-3874-T,B-3232-OUT;n:type:ShaderForge.SFN_Add,id:8563,x:31064,y:32132,varname:node_8563,prsc:2|A-5326-UVOUT,B-9007-OUT;n:type:ShaderForge.SFN_TexCoord,id:5326,x:30867,y:32132,varname:node_5326,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_TexCoord,id:2373,x:31529,y:31976,varname:node_2373,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Lerp,id:7843,x:31896,y:32450,varname:node_7843,prsc:2|A-2373-UVOUT,B-1756-OUT,T-2737-R;n:type:ShaderForge.SFN_Tex2d,id:2737,x:31524,y:32498,ptovrint:False,ptlb:Noise Mask,ptin:_NoiseMask,varname:_NoiseMask_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:2102590c9a6475b49beda4c1bf25bbaa,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:4527,x:32398,y:33405,ptovrint:False,ptlb:Colour Multiplier,ptin:_ColourMultiplier,varname:node_4527,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:8;n:type:ShaderForge.SFN_Multiply,id:374,x:32608,y:32952,varname:node_374,prsc:2|A-658-A,B-7296-A,C-4133-A,D-7924-RGB,E-4527-OUT;n:type:ShaderForge.SFN_Tex2d,id:7924,x:32201,y:33259,ptovrint:False,ptlb:Alpha Mask,ptin:_AlphaMask,varname:node_7924,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:e2f5bc4cb0e440843b11a6b2018dc7d9,ntxv:0,isnm:False;proporder:4133-658-7924-5352-9762-9998-3855-2737-4539-9953-4527;pass:END;sub:END;*/

Shader "Mirza Beig/Particles/Texture Distortion/Additive" {
    Properties {
        _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _AlphaMask ("Alpha Mask", 2D) = "white" {}
        _USpeedMain ("U Speed (Main)", Float ) = -0.01
        _VSpeedMain ("V Speed (Main)", Float ) = -0.125
        _NoiseTex ("NoiseTex", 2D) = "white" {}
        _NoiseAmount ("Noise Amount", Range(0, 0.25)) = 0.25
        _NoiseMask ("Noise Mask", 2D) = "white" {}
        _USpeedNoise ("U Speed (Noise)", Float ) = 0.1
        _VSpeedNoise ("V Speed (Noise)", Float ) = 0.05
        _ColourMultiplier ("Colour Multiplier", Range(0, 8)) = 1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _TintColor;
            uniform float _USpeedMain;
            uniform float _VSpeedMain;
            uniform sampler2D _NoiseTex; uniform float4 _NoiseTex_ST;
            uniform float _NoiseAmount;
            uniform float _USpeedNoise;
            uniform float _VSpeedNoise;
            uniform sampler2D _NoiseMask; uniform float4 _NoiseMask_ST;
            uniform float _ColourMultiplier;
            uniform sampler2D _AlphaMask; uniform float4 _AlphaMask_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_3874 = _Time;
                float2 node_8563 = (i.uv0+(node_3874.g*float2(_USpeedNoise,_VSpeedNoise)));
                float4 _NoiseTex_var = tex2D(_NoiseTex,TRANSFORM_TEX(node_8563, _NoiseTex));
                float4 _NoiseMask_var = tex2D(_NoiseMask,TRANSFORM_TEX(i.uv0, _NoiseMask));
                float4 node_178 = _Time;
                float2 node_7363 = (lerp(i.uv0,lerp(i.uv0,float2(_NoiseTex_var.r,_NoiseTex_var.r),_NoiseAmount),_NoiseMask_var.r)+(node_178.g*float2(_USpeedMain,_VSpeedMain)));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_7363, _MainTex));
                float4 _AlphaMask_var = tex2D(_AlphaMask,TRANSFORM_TEX(i.uv0, _AlphaMask));
                float3 emissive = (_MainTex_var.rgb*i.vertexColor.rgb*_TintColor.rgb*2.0*(_MainTex_var.a*i.vertexColor.a*_TintColor.a*_AlphaMask_var.rgb*_ColourMultiplier));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
