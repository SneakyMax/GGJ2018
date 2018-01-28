Shader "Custom/FogShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_GradientRange ("Gradient Range", Float) = 10
		_MinViewDist ("Minimum View Distance", Float) = 20
		_FogColor ("Fog Color", Color) = (1,1,1)
		_MinDistTransitionDist ("Min Dist Transition Dist", Float) = 10
		_IdentifiedColor ("Identifided Obj Color", Color) = (1,1,1,1)
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
			fixed4 _IdentifiedColor;

			float _GradientRange;
			float _CurrentPingDist;
			float _MinViewDist;
			float _MinDistTransitionDist;
			float _IdentifiedOpacity;

			fixed4 frag (v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex, i.uv);
				float rawdepth = tex2D(_CameraDepthTexture, i.uv);
				float depth = DECODE_EYEDEPTH(rawdepth);
				float4 col_identified = tex2D(_IdentifiedTex, i.uv);
				float identified_depth = tex2D(_IdentifiedDepth, i.uv);

				// Multiply by worldspace direction (no perspective divide needed).
				float3 worldspace = i.worldDirection * depth + _WorldSpaceCameraPos;
				float point_distance = distance(worldspace, _WorldSpaceCameraPos);

				float closelight = 1.0 - (clamp((point_distance - _MinViewDist + _MinDistTransitionDist), 0, _MinDistTransitionDist) / _MinDistTransitionDist);
				float smoothcloselight = smoothstep(0, 1, closelight);
				
				float pinglight = (_GradientRange - clamp(abs(_CurrentPingDist - point_distance), 0, _GradientRange)) / _GradientRange;
				float smoothpinglight = smoothstep(0, 1, pinglight);
				float dist = max(smoothcloselight, smoothpinglight);
				float4 lighted = lerp(_FogColor, color, dist);
				float4 fadedIdentified = lerp(float4(0, 0, 0, 0), float4(col_identified.rgb, 1), col_identified.a);
				//return lerp(lighted, fadedIdentified, 0.5);
				return lerp(lighted, fadedIdentified, step(rawdepth, identified_depth));
			}
			ENDCG
		}
	}
}
