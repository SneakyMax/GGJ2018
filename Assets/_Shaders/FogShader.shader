Shader "Custom/FogShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_GradientRange ("Gradient Range", Float) = 10
		_MinViewDist ("Minimum View Distance", Float) = 20
		_FogColor ("Fog Color", Color) = (1,1,1)
		_MinDistTransitionDist ("Min Dist Transition Dist", Float) = 10
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 worldDirection : TEXCOORD1;
			};

			float4x4 _ClipToWorld;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				// Construct a vector on the Z = 0 plane corresponding to our screenspace location.
				float4 clip = float4((v.uv.xy * 2.0f - 1.0f) * float2(1, -1), 0.0f, 1.0f);
				// Use matrix computed in script to convert to worldspace.
				o.worldDirection = mul(_ClipToWorld, clip) - _WorldSpaceCameraPos;

				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;

			sampler2D _IdentifiedTex;
			sampler2D _IdentifiedDepth;

			fixed4 _FogColor;

			float _GradientRange;
			float _CurrentPingDist;
			float _MinViewDist;
			float _MinDistTransitionDist;
			float _IdentifiedOpacity;

			// Gets a z buffer depth from a linear depth (inverse of LinearEyeDepth)
			inline float ZBufferDepth(float eyeDepth)
			{
				return (1.0 / (eyeDepth * _ZBufferParams.z)) - (_ZBufferParams.w / _ZBufferParams.z);
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// Color of the normally rendered scene
				float4 color = tex2D(_MainTex, i.uv);
				// Color of the object that has been highlighted as being identified
				float4 col_identified = tex2D(_IdentifiedTex, i.uv);
				// Float depth of the identified object (stored in render texture)
				float identified_depth_float = tex2D(_IdentifiedDepth, i.uv);
				float slightly_closer_identified = ZBufferDepth(LinearEyeDepth(identified_depth_float) - 0.5);
				
				// Float depth of the fragment
				float depth_float = tex2D(_CameraDepthTexture, i.uv);
				// Depth of the fragment in meters
				float depth_meters = DECODE_EYEDEPTH(depth_float);

				// Get the world space coordinates at the fragment
				float3 worldspace = i.worldDirection * depth_meters + _WorldSpaceCameraPos;
				// Distance from the camera to the world space coordinates at the fragment
				float point_distance = distance(worldspace, _WorldSpaceCameraPos);

				// Close light is the minimum distance you can see. Fade it from _MinViewDist - _MinDistTransitionDist -> _MinViewDist
				float closelight = 1.0 - (clamp((point_distance - _MinViewDist + _MinDistTransitionDist), 0, _MinDistTransitionDist) / _MinDistTransitionDist);
				float smoothcloselight = smoothstep(0, 1, closelight); // Better smoothing
				
				// Light the area in the pings
				float pinglight = (_GradientRange - clamp(abs(_CurrentPingDist - point_distance), 0, _GradientRange)) / _GradientRange;
				float smoothpinglight = smoothstep(0, 1, pinglight);

				// Take the brighter of the ping and the close lighting
				float light_factor = max(smoothcloselight, smoothpinglight);

				// Darken to _FogColor based on the light factor
				float4 lighted = lerp(_FogColor, color, light_factor);

				// Fade each identified object by its alpha value
				float4 fadedIdentified = lerp(float4(0, 0, 0, 0), float4(col_identified.rgb, 1), col_identified.a);

				// Merge the identified objects with the main scene using the depth maps to blend correctly
				return lerp(lighted, fadedIdentified, step(depth_float, slightly_closer_identified));
			}
			ENDCG
		}
	}
}
