Shader "Custom/Terrain" {
	Properties {
		_ColorA ("Color A", Color) = (0.8,0.9,0.9,1)   
		_ColorB ("Color B", Color) = (0.8,0.9,0.9,1)   
    	_Level ("Level", Float) = 300
		_MainTex ("Base (RGB)", 2D) = "white" {}		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		fixed4 _ColorA;
		fixed4 _ColorB;
		float _Level;
		
		struct Input {
			float2 uv_MainTex;
			float3 customColor;
        	float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			if (fmod(IN.worldPos.y, 2) >= 0 && fmod(IN.worldPos.y, 2)  < 0.99f)
				o.Albedo = c.rgb * _ColorB;
			else
				o.Albedo = c.rgb * _ColorA;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
