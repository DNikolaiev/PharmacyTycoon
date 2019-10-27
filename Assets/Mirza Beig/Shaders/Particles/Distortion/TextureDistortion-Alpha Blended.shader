// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.09558821,fgcg:0.09558821,fgcb:0.09558821,fgca:1,fgde:0.02,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32857,y:32675,varname:node_4795,prsc:2|emission-2393-OUT,alpha-798-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32235,y:32601,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0afcdac5cae18544eb368987da8d9891,ntxv:0,isnm:False|UVIN-7430-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:32551,y:32771,varname:node_2393,prsc:2|A-6074-RGB,B-2053-RGB,C-797-RGB,D-9248-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32235,y:32772,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32235,y:32930,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:9248,x:32235,y:33081,varname:node_9248,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:798,x:32551,y:32943,varname:node_798,prsc:2|A-6074-A,B-2053-A,C-797-A,D-8263-R,E-2255-OUT;n:type:ShaderForge.SFN_Time,id:262,x:31569,y:32840,varname:node_262,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:7566,x:31569,y:33011,ptovrint:False,ptlb:U Speed (Main),ptin:_USpeedMain,varname:node_7566,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.01;n:type:ShaderForge.SFN_Append,id:5648,x:31808,y:32996,varname:node_5648,prsc:2|A-7566-OUT,B-6194-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6194,x:31569,y:33095,ptovrint:False,ptlb:V Speed (Main),ptin:_VSpeedMain,varname:node_6194,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.125;n:type:ShaderForge.SFN_Multiply,id:2631,x:31808,y:32840,varname:node_2631,prsc:2|A-262-T,B-5648-OUT;n:type:ShaderForge.SFN_Add,id:7430,x:32035,y:32601,varname:node_7430,prsc:2|A-8456-OUT,B-2631-OUT;n:type:ShaderForge.SFN_TexCoord,id:5640,x:31620,y:33243,varname:node_5640,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ComponentMask,id:7247,x:31788,y:33243,varname:node_7247,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-5640-V;n:type:ShaderForge.SFN_RemapRange,id:1545,x:31963,y:33243,varname:node_1545,prsc:2,frmn:0,frmx:1,tomn:1,tomx:0|IN-7247-OUT;n:type:ShaderForge.SFN_Tex2d,id:9664,x:31121,y:32120,ptovrint:False,ptlb:NoiseTex,ptin:_NoiseTex,varname:node_9664,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-4452-OUT;n:type:ShaderForge.SFN_Lerp,id:900,x:31404,y:32120,varname:node_900,prsc:2|A-9937-UVOUT,B-9664-R,T-6868-OUT;n:type:ShaderForge.SFN_Slider,id:6868,x:30964,y:32349,ptovrint:False,ptlb:Noise Amount,ptin:_NoiseAmount,varname:node_6868,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.25,max:0.25;n:type:ShaderForge.SFN_TexCoord,id:9937,x:31132,y:31942,varname:node_9937,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:3693,x:30503,y:32277,varname:node_3693,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:5786,x:30503,y:32448,ptovrint:False,ptlb:U Speed (Noise),ptin:_USpeedNoise,varname:_USpeed_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Append,id:8151,x:30742,y:32433,varname:node_8151,prsc:2|A-5786-OUT,B-8796-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8796,x:30503,y:32532,ptovrint:False,ptlb:V Speed (Noise),ptin:_VSpeedNoise,varname:_VSpeed_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.05;n:type:ShaderForge.SFN_Multiply,id:1250,x:30742,y:32277,varname:node_1250,prsc:2|A-3693-T,B-8151-OUT;n:type:ShaderForge.SFN_Add,id:4452,x:30939,y:32120,varname:node_4452,prsc:2|A-6116-UVOUT,B-1250-OUT;n:type:ShaderForge.SFN_TexCoord,id:6116,x:30742,y:32120,varname:node_6116,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_TexCoord,id:689,x:31404,y:31964,varname:node_689,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Lerp,id:8456,x:31839,y:32458,varname:node_8456,prsc:2|A-689-UVOUT,B-900-OUT,T-6681-R;n:type:ShaderForge.SFN_Tex2d,id:6681,x:31383,y:32521,ptovrint:False,ptlb:Noise Mask,ptin:_NoiseMask,varname:node_6681,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:2102590c9a6475b49beda4c1bf25bbaa,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:2255,x:32386,y:33351,ptovrint:False,ptlb:Colour Multiplier,ptin:_ColourMultiplier,varname:node_2255,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:8;n:type:ShaderForge.SFN_Tex2d,id:8263,x:32134,y:33298,ptovrint:False,ptlb:Alpha Mask,ptin:_AlphaMask,varname:node_8263,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:e2f5bc4cb0e440843b11a6b2018dc7d9,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:2525,x:32683,y:33049,ptovrint:False,ptlb:MainTex_copy,ptin:_MainTex_copy,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0afcdac5cae18544eb368987da8d9891,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:2964,x:32999,y:33219,varname:node_2964,prsc:2|A-2525-RGB,B-7294-RGB,C-202-RGB,D-7296-OUT;n:type:ShaderForge.SFN_VertexColor,id:7294,x:32683,y:33220,varname:node_7294,prsc:2;n:type:ShaderForge.SFN_Color,id:202,x:32683,y:33378,ptovrint:True,ptlb:Color_copy_copy_copy_copy_copy_copy,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:7296,x:32683,y:33529,varname:node_7296,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:116,x:32999,y:33391,varname:node_116,prsc:2|A-2525-A,B-7294-A,C-202-A,D-3247-OUT;n:type:ShaderForge.SFN_Time,id:8291,x:32017,y:33288,varname:node_8291,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:3609,x:32017,y:33459,ptovrint:False,ptlb:U Speed_copy,ptin:_USpeed_copy,varname:_USpeed_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.01;n:type:ShaderForge.SFN_Append,id:8536,x:32256,y:33444,varname:node_8536,prsc:2|A-3609-OUT,B-3293-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3293,x:32017,y:33543,ptovrint:False,ptlb:V Speed_copy,ptin:_VSpeed_copy,varname:_VSpeed_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.125;n:type:ShaderForge.SFN_Multiply,id:7419,x:32256,y:33288,varname:node_7419,prsc:2|A-8291-T,B-8536-OUT;n:type:ShaderForge.SFN_Add,id:7649,x:32483,y:33049,varname:node_7649,prsc:2|A-3672-OUT,B-7419-OUT;n:type:ShaderForge.SFN_TexCoord,id:5351,x:32606,y:33651,varname:node_5351,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ComponentMask,id:2273,x:32774,y:33651,varname:node_2273,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-5351-V;n:type:ShaderForge.SFN_RemapRange,id:3247,x:32949,y:33651,varname:node_3247,prsc:2,frmn:0,frmx:1,tomn:1,tomx:0|IN-2273-OUT;n:type:ShaderForge.SFN_Tex2d,id:634,x:31609,y:32378,ptovrint:False,ptlb:NoiseTex_copy,ptin:_NoiseTex_copy,varname:_NoiseTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Lerp,id:3081,x:31892,y:32378,varname:node_3081,prsc:2|A-6023-UVOUT,B-634-R,T-7598-OUT;n:type:ShaderForge.SFN_Slider,id:7598,x:31452,y:32607,ptovrint:False,ptlb:Noise Amount_copy,ptin:_NoiseAmount_copy,varname:_NoiseAmount_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.25,max:0.25;n:type:ShaderForge.SFN_TexCoord,id:6023,x:31620,y:32200,varname:node_6023,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:4841,x:30991,y:32535,varname:node_4841,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:8462,x:30991,y:32706,ptovrint:False,ptlb:U Speed (Noise)_copy,ptin:_USpeedNoise_copy,varname:_USpeedNoise_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Append,id:154,x:31230,y:32691,varname:node_154,prsc:2|A-8462-OUT,B-6637-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6637,x:30991,y:32790,ptovrint:False,ptlb:V Speed (Noise)_copy,ptin:_VSpeedNoise_copy,varname:_VSpeedNoise_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.05;n:type:ShaderForge.SFN_Multiply,id:1954,x:31230,y:32535,varname:node_1954,prsc:2|A-4841-T,B-154-OUT;n:type:ShaderForge.SFN_Add,id:344,x:31427,y:32378,varname:node_344,prsc:2|A-5567-UVOUT,B-1954-OUT;n:type:ShaderForge.SFN_TexCoord,id:5567,x:31230,y:32378,varname:node_5567,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_TexCoord,id:6519,x:31018,y:33048,varname:node_6519,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ComponentMask,id:6431,x:31195,y:33048,varname:node_6431,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-6519-U;n:type:ShaderForge.SFN_Multiply,id:2864,x:31421,y:33155,varname:node_2864,prsc:2|A-6431-OUT,B-9936-OUT,C-1263-OUT;n:type:ShaderForge.SFN_Sin,id:8981,x:31589,y:33155,varname:node_8981,prsc:2|IN-2864-OUT;n:type:ShaderForge.SFN_Tau,id:1263,x:31421,y:33306,varname:node_1263,prsc:2;n:type:ShaderForge.SFN_TexCoord,id:7223,x:31892,y:32222,varname:node_7223,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Lerp,id:3672,x:32287,y:32906,varname:node_3672,prsc:2|A-7223-UVOUT,B-3081-OUT,T-7361-OUT;n:type:ShaderForge.SFN_Tex2d,id:5194,x:31724,y:32924,ptovrint:False,ptlb:Noise Mask_copy,ptin:_NoiseMask_copy,varname:_NoiseMask_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:2102590c9a6475b49beda4c1bf25bbaa,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Vector1,id:9936,x:31195,y:33210,varname:node_9936,prsc:2,v1:3;n:type:ShaderForge.SFN_Multiply,id:2318,x:31913,y:33060,varname:node_2318,prsc:2|A-5194-R,B-9523-OUT;n:type:ShaderForge.SFN_Clamp01,id:7361,x:32089,y:33060,varname:node_7361,prsc:2|IN-2318-OUT;n:type:ShaderForge.SFN_Vector1,id:9523,x:31724,y:33094,varname:node_9523,prsc:2,v1:4;proporder:797-6074-8263-7566-6194-9664-6868-6681-5786-8796-2255;pass:END;sub:END;*/

Shader "Custom/Mirza Beig/Particles/Texture Distortion/Alpha Blended" {
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
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
            Blend SrcAlpha OneMinusSrcAlpha
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
                float4 node_3693 = _Time;
                float2 node_4452 = (i.uv0+(node_3693.g*float2(_USpeedNoise,_VSpeedNoise)));
                float4 _NoiseTex_var = tex2D(_NoiseTex,TRANSFORM_TEX(node_4452, _NoiseTex));
                float4 _NoiseMask_var = tex2D(_NoiseMask,TRANSFORM_TEX(i.uv0, _NoiseMask));
                float4 node_262 = _Time;
                float2 node_7430 = (lerp(i.uv0,lerp(i.uv0,float2(_NoiseTex_var.r,_NoiseTex_var.r),_NoiseAmount),_NoiseMask_var.r)+(node_262.g*float2(_USpeedMain,_VSpeedMain)));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_7430, _MainTex));
                float3 emissive = (_MainTex_var.rgb*i.vertexColor.rgb*_TintColor.rgb*2.0);
                float3 finalColor = emissive;
                float4 _AlphaMask_var = tex2D(_AlphaMask,TRANSFORM_TEX(i.uv0, _AlphaMask));
                fixed4 finalRGBA = fixed4(finalColor,(_MainTex_var.a*i.vertexColor.a*_TintColor.a*_AlphaMask_var.r*_ColourMultiplier));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
