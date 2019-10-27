
Shader "Mirza Beig/Particles/Distortion/Add" 
{
	Properties 
	{
		_opacity ("Opacity", range(0.0, 2.0)) = 0.5
		
		_MainTex ("Distortion Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01, 8.0)) = 1.0	

		_distortion ("Distortion", range(0.0, 1000.0)) = 10.0
	}

	Category 
	{

		// Transparent = other objects drawn before this one.

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }

		Blend SrcAlpha One

		AlphaTest Greater .01
		ColorMask RGB

		Cull Off Lighting Off ZWrite Off
		
		SubShader {

				// Grab screen behind object into a texture.
				// Accessed in next pass in _GrabTexture.

				GrabPass
				{
					Name "BASE"
					Tags { "LightMode" = "Always" }
				}
		
				// Preturb texture from above using bump map.

				Pass 
				{
					Name "BASE"
					Tags { "LightMode" = "Always"
				}
		
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_particles
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				sampler2D _MainTex;

				// ...
			
				struct appdata_t 
				{
					fixed4 color : COLOR;

					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				// ...

				struct v2f 
				{
					fixed4 color : COLOR;

					float4 vertex : SV_POSITION;

					float2 uv : TEXCOORD0;
												
					#ifdef SOFTPARTICLES_ON
						float4 uv_depth : TEXCOORD1;
					#endif

					float4 uv_grab : TEXCOORD2;

					UNITY_FOG_COORDS(4)
				};

				// ...
			
				float4 _MainTex_ST;	

				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
				
					#ifdef SOFTPARTICLES_ON
						o.uv_depth = ComputeScreenPos(o.vertex);
						COMPUTE_EYEDEPTH(o.uv_depth.z);
					#endif

					o.color = v.color;
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);

					#if UNITY_UV_STARTS_AT_TOP
						float scale = -1.0;
					#else
						float scale = 1.0;
					#endif
	
					o.uv_grab.zw = o.vertex.zw;
					o.uv_grab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5f;
				
					UNITY_TRANSFER_FOG(o, o.vertex);
	
					return o;
				}

				sampler2D_float _CameraDepthTexture;

				float _InvFade;

				float _distortion;

				sampler2D _GrabTexture;
				float4 _GrabTexture_TexelSize;
						
				float _opacity;

				// ...
						
				fixed4 frag (v2f i) : SV_Target
				{				
					// Get texture alpha mask.

					float4 texColour = tex2D(_MainTex, i.uv);
					float opacity = texColour.a * _opacity;

					// Soft particles.
				 
					#ifdef SOFTPARTICLES_ON
				 
						// Scene depth.
				
						//float sceneZ = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.uv_depth)).r);
						float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.uv_depth)));
 
						// Distance to the camera.

						float partZ = i.uv_depth.z;

						// Soft particles (soft factor).
						// "Comparing depth values of the particle with depth values of world geometry (in view space)." - Special Effects with Depth (Siggraph, 2011).
					
						// float softFactor = saturate((depthEye - zEye) * fade)
					
						// Inverse fade: 0.0f = off.

						opacity *= saturate(_InvFade * (sceneZ - partZ));

					#endif

					// Input colour.

					opacity *= i.color.a;
					//opacity *= i.color.a * 2.0f;

					// Distortion.

					texColour.rg *= texColour.b * 5;
				
					half2 bump = UnpackNormal(texColour).rg;
					float2 offset = bump * _distortion * _GrabTexture_TexelSize.xy;

					i.uv_grab.xy = offset * i.uv_grab.z + i.uv_grab.xy;	
					half4 distortionColour = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uv_grab));

					distortionColour.a = opacity;

					// Fog towards black (additive black = nothing).

					//UNITY_APPLY_FOG_COLOR(i.fogCoord, distortionColour, fixed4(0.0f, 0.0f, 0.0f, 0.0f));
  
					return distortionColour;
				}

				ENDCG 
			}
		}	
	}
}
