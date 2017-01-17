Shader "Custom/Twist" {
	Properties {
		_Freq ("Freq", Range(0,20)) = 0.5
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }

		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

		struct Input {
			float4 color : COLOR;
		};

		float _Freq = 0;

		void vert (inout appdata_full v) {
			v.vertex.x += 0.18 * sin(v.normal.y * _Freq);
			v.vertex.z += 0.18 * -cos(v.normal.y * _Freq) + 0.2;
			//v.vertex.y += 0.18 * sin(v.normal.x * _Freq);
			//v.vertex.z += 0.18 * -cos(v.normal.x * _Freq) + 0.2;
		}
	
		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			o.Albedo = half3(1, 1, 1);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
