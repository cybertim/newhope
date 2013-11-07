Shader "PostProcess/AdditiveRadialBlur_40_Samples" {
Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Particle Texture", 2D) = "white" {}
	_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	_CenX ("Blur Center X", Float) = 0.5
	_CenY ("Blur Center Y", Float) = 0.5
	_SDist ("Blur Sample Distance", Float) = 0.02
	_Int ("Intensity", Float) = 1.0
	_RInt ("Radius", Float) = 1.0
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	AlphaTest Greater .01
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	// ---- Fragment program cards
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_particles
			#pragma target 3.0

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColor;
			float _CenX;
			float _CenY;
			float _SSamp;
			float _SDist;
			float _Samp;
			float _Int;
			float _RInt;

			struct v2f {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD1;
				#endif
			};
			
			float4 _MainTex_ST;

			v2f vert (appdata_img v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.texcoord = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
				return o;
			}

			sampler2D _CameraDepthTexture;
			float _InvFade;
			
			fixed4 frag (v2f i) : COLOR
			{
				#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
				float partZ = i.projPos.z;
				float fade = saturate (_InvFade * (sceneZ-partZ));
				i.color.a *= fade;
				#endif
				float4 t = tex2D(_MainTex, i.texcoord);
				for(int x = 0; x < 40; x++)
				{
				t += tex2D(_MainTex, i.texcoord + ((i.texcoord - TRANSFORM_TEX(float2(_CenX, _CenY), _MainTex)) * -_SDist * x)) * (_RInt - length(i.texcoord - TRANSFORM_TEX(float2(_CenX, _CenY), _MainTex)) / _RInt);
				}
				t /= 40;
				return _TintColor * t * _Int;
			}
			ENDCG 
		}
	}
}
}