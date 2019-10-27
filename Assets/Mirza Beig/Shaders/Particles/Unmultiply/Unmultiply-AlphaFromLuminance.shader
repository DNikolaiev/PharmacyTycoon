
Shader "Mirza Beig/Particles/Unmultiply/Alpha from Luminance"
{
	Properties
	{
		_TintColor("Tint Color", Color) = (0.5, 0.5, 0.5, 0.5)

		_MainTex("Particle Texture", 2D) = "white" {}
		_InvFade("Soft Particles Factor", Range(0.01, 8.0)) = 1.0
	}

	Category
	{
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		
		Blend One OneMinusSrcAlpha
		Blend SrcAlpha OneMinusSrcAlpha

		ColorMask RGB
		Cull Off Lighting Off ZWrite Off

		SubShader 
		{
			Pass 
			{

				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag

				#pragma multi_compile_particles
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				fixed4 _TintColor;

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;

					float2 texcoord : TEXCOORD0;
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;

					float2 texcoord : TEXCOORD0;

	#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD1;
	#endif

					UNITY_FOG_COORDS(2)
				};

				float4 _MainTex_ST;

				v2f vert(appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);

	#ifdef SOFTPARTICLES_ON
					o.projPos = ComputeScreenPos(o.vertex);
					COMPUTE_EYEDEPTH(o.projPos.z);
	#endif

					o.color = v.color;
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

					UNITY_TRANSFER_FOG(o, o.vertex);

					return o;
				}

				sampler2D_float _CameraDepthTexture;
				float _InvFade;

				// ...

				fixed getLuminance(fixed4 colour)
				{
					float lumR = 0.2126f;
					float lumG = 0.7152f;
					float lumB = 0.0722f;

					float luminance =
						colour.r * lumR +
						colour.g * lumG +
						colour.b * lumB;

					return luminance;
				}
				fixed getLuminance2(fixed4 colour)
				{
					float lumR = 0.2990f;
					float lumG = 0.5870f;
					float lumB = 0.1140f;

					float luminance =
						colour.r * lumR +
						colour.g * lumG +
						colour.b * lumB;

					return luminance;
				}

				// Most accurate, but also slower.

				fixed getLuminance3(fixed4 colour)
				{
					float lumR = pow(0.2990f, 2.0f);
					float lumG = pow(0.5870f, 2.0f);
					float lumB = pow(0.1140f, 2.0f);

					float luminance =
						colour.r * lumR +
						colour.g * lumG +
						colour.b * lumB;

					luminance = sqrt(luminance);

					return luminance;
				}

				// ...
				
				fixed4 frag(v2f i) : SV_Target
				{
					// Get texture colour.

					float4 texColour = tex2D(_MainTex, i.texcoord);
					
					// Tint.

					fixed4 colour = _TintColor * texColour;

					#ifdef SOFTPARTICLES_ON

					// Scene depth.

					//float sceneZ = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)).r);
					float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));

					// Distance to the camera.

					float partZ = i.projPos.z;

					// Soft particles (soft factor).
					// "Comparing depth values of the particle with depth values of world geometry (in view space)." - Special Effects with Depth (Siggraph, 2011).

					// float softFactor = saturate((depthEye - zEye) * fade)

					// Inverse fade: 0.0f = off.

					float fade = saturate(_InvFade * (sceneZ - partZ));
					colour.a *= fade;

				#endif

					// Input colour.

					//colour *= i.color * 2.0f;

					colour *= i.color * 2.0f;

					// Alpha channel from luminance.
					
					//colour.a = getLuminance(colour);
					colour.a = getLuminance2(colour);
					//colour.a = getLuminance3(colour);

					colour.a *= i.color.a;

					// Fog towards nothing.

					UNITY_APPLY_FOG(i.fogCoord, colour);

					return colour;
				}
				ENDCG
			}
		}
	}
}