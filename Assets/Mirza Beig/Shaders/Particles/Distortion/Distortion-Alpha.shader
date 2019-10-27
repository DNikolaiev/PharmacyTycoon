
Shader "Mirza Beig/Particles/Distortion/Alpha Blended" 
{
	Properties
	{
		_Opacity("Opacity", range(0.0, 2.0)) = 0.5
		_Intensity("Intensity", range(0.0, 10.0)) = 1.0

		_Distortion("Distortion", range(0.0, 2.0)) = 0.05

		_MainTex("Particle Texture", 2D) = "white" {}
		_DistTex("Distortion Texture", 2D) = "white" {}

		_InvFade("Soft Particles Factor", Range(0.01, 8.0)) = 1.0	
	}

	Category 
	{
		// Transparent = other objects drawn before this one.

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType" = "Plane" }

		Blend SrcAlpha OneMinusSrcAlpha

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

				#pragma target 2.0

				#pragma multi_compile_particles
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				sampler2D _DistTex;

				// ...
			
				struct appdata_t 
				{
					float4 vertex : POSITION;

					fixed4 color : COLOR;

					float2 texcoord : TEXCOORD0;
					float2 texcoord_dist : TEXCOORD3;

					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				// ...

				struct v2f 
				{
					float4 vertex : SV_POSITION;

					fixed4 color : COLOR;

					float2 texcoord : TEXCOORD0;

					UNITY_FOG_COORDS(1)

#ifdef SOFTPARTICLES_ON
						float4 projPos : TEXCOORD2;
#endif				
					float2 texcoord_dist : TEXCOORD3;

					// Grabpass texture coordinates.
					
					float4 uvgrab : TEXCOORD4;

					UNITY_VERTEX_OUTPUT_STEREO
				};

				// ...
			
				float4 _MainTex_ST;
				float4 _DistTex_ST;

				float _Intensity;

				v2f vert (appdata_t v)
				{
					v2f o;

					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

					o.vertex = UnityObjectToClipPos(v.vertex);

#ifdef SOFTPARTICLES_ON
					o.projPos = ComputeScreenPos(o.vertex);
					COMPUTE_EYEDEPTH(o.projPos.z);
#endif

					// 0.5 since Unity's regular shaders are gray-tinted?
					// Meh, just make it the full colour. This isn't supposed 
					// to be the same end-result. I'll take out the x2.0f in
					// the fragment part near the end.

					//o.color = v.color;
					o.color = v.color * _Intensity;

					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.texcoord_dist = TRANSFORM_TEX(v.texcoord_dist, _DistTex);

					UNITY_TRANSFER_FOG(o, o.vertex);				

					// Grabpass stuff.
					
					o.uvgrab = ComputeGrabScreenPos(o.vertex);

					return o;
				}

				sampler2D_float _CameraDepthTexture;

				float _InvFade;
				float _Opacity;

				float _Distortion;
				sampler2D _GrabTexture;						

				// ...
						
				fixed4 frag (v2f i) : SV_Target
				{			

					float softParticleAlpha = 1.0;

#ifdef SOFTPARTICLES_ON
					
					float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
					float partZ = i.projPos.z;

					softParticleAlpha = saturate(_InvFade * (sceneZ - partZ));

#endif

					float inputAlpha = i.color.a;

					half mainTextureAlpha = tex2D(_MainTex, i.texcoord).a;
					half4 distortionTextureColour = tex2D(_DistTex, i.texcoord);

					//distortionTextureColour.rg *= distortionTextureColour.b;
					
					half2 distortion = UnpackNormal(distortionTextureColour).rg;
					
					i.uvgrab.xy += distortion * _Distortion;
					fixed4 grabPassTextureColourDistorted = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				
					fixed4 col = grabPassTextureColourDistorted;

					col.a = inputAlpha * mainTextureAlpha * softParticleAlpha * _Opacity;

					UNITY_APPLY_FOG(i.fogCoord, col);

					return col;

				}

				ENDCG 
			}
		}	
	}
}
