Shader "Mirza Beig/Particles/Intersection Highlight/Alpha Blended" {
Properties {

	_TintColor ("Tint Color", Color) = (0.5, 0.5, 0.5, 0.5)
	_highlightColour ("Highlight Colour", Color) = (1.0, 1.0, 1.0, 1.0)
	
	_MainTex ("Particle Texture", 2D) = "white" {}
	_InvFade ("Soft Particles Factor", Range(0.01, 32.0)) = 32.0
	
	_highlightEdgeThreshold ("Highlight Edge Threshold", Range(0.0, 32.0)) = 1.0
	_highlightColourStrength ("Highlight Colour Strength", Range(0.0, 32.0)) = 1.0
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	AlphaTest Greater .01
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off
	
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColor;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_FOG_COORDS(1)
											
				float4 uv_depth : TEXCOORD2;

			};
			
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				
				o.uv_depth = ComputeScreenPos(o.vertex);
				COMPUTE_EYEDEPTH(o.uv_depth.z);

				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

				UNITY_TRANSFER_FOG(o, o.vertex);

				return o;
			}

			sampler2D_float _CameraDepthTexture;
			float _InvFade;
			
			uniform float4 _highlightColour;
			uniform float _highlightColourStrength;

			uniform float _highlightEdgeThreshold;
						
			fixed4 frag (v2f i) : SV_Target
			{
				// Get texture colour.

				float4 texColour = tex2D(_MainTex, i.texcoord);

				// Tint.

				fixed4 colour = _TintColor * texColour;

				//

				_highlightColour *= texColour;
 
				// Scene depth.
				
				//float sceneZ = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.uv_depth)).r);
				float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.uv_depth)));
 
				// Distance to the camera.

				float partZ = i.uv_depth.z;
 
				// Depth and this object's values close together = intersection.

				float dist01 = (abs(sceneZ - partZ)) / _highlightEdgeThreshold;
					
				// Attenuate highlight based on difference.					
 
				if (dist01 <= 1.0f)
				{
					colour = lerp(_highlightColour * _highlightColourStrength, colour, dist01);
				}
					
				#ifdef SOFTPARTICLES_ON

					// Soft particles (soft factor).
					// "Comparing depth values of the particle with depth values of world geometry (in view space)." - Special Effects with Depth (Siggraph, 2011).
					
					// float softFactor = saturate((depthEye - zEye) * fade)
					
					// Inverse fade: 0.0f = off.

					float fade = saturate(_InvFade * (sceneZ - partZ));

					colour.a *= fade;

				#endif

				// Input colour.

				colour *= i.color * 2.0f;

				// Fog towards actual fog colour (alpha blending).
				
				UNITY_APPLY_FOG(i.fogCoord, colour);
  
				return colour;
			}
			ENDCG 
		}
	}	
}
}
