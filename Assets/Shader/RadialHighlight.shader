Shader "Custom/RadialHighlight" {
    Properties {
        _Offset("Highlight Offset", float)= 4
		//_Scale("Planet Scale", float) = 1
	}

	SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha One
		Cull Off Lighting Off ZWrite Off
		
		Pass {
            CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };
			
            v2f vert (appdata_t v)
			{
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.color = v.color;
                o.texcoord = v.texcoord;
                return o;
            }
			
			float _Offset;
            //float _Scale;
			
            fixed4 frag (v2f i) : SV_Target
			{
                float maxPlayerSpeed = 7.75;
                float intensityMultiplier = 150;
                float width = 0.1;
								
                float2 centeredUVs = i.texcoord * 2.0 - 1.0;
                float distance = sqrt(centeredUVs.x * centeredUVs.x + centeredUVs.y * centeredUVs.y);
                float ringOffset = clamp(_Offset, 0.1, maxPlayerSpeed);
				
                ringOffset = maxPlayerSpeed - ringOffset;
				
                float outerRegion = (ringOffset + width) / maxPlayerSpeed;
                float innerRegion = (ringOffset - width) / maxPlayerSpeed;
                float highlight = (outerRegion - distance) * (distance - innerRegion);
				
                return highlight * (1 / width) * intensityMultiplier;
            }


			ENDCG 
		}	
	}
}

