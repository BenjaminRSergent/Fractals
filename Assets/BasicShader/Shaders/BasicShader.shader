Shader "Unlit/BasicShader"
{
	Properties
	{
		_Tint("Tint", Color) = (1,1,1,1)
		_OffsetScale("Offset Scale", float) = 0.1
	}
		SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex BasicVertexProgram
			#pragma fragment BasicFragmentProgram

			#include "UnityCG.cginc"

			float rand(float3 seed)
			{
				return frac(sin(dot(seed.xyz ,float3(12.9898,78.233,45.5432))) * 43758.5453);
			}

			float4 _Tint;
			float _OffsetScale;
			struct Interpolators {
				float vertexSeed : TEXCOORD0;
				float4 position : SV_POSITION;
				float3 localPosition : TEXCOORD1;
			};
			Interpolators BasicVertexProgram(float4 position : POSITION)
			{

				Interpolators interp;
				interp.localPosition = position.xyz;
				interp.vertexSeed = rand(position);

				float4 vertexOffset;
				vertexOffset.x = rand(interp.vertexSeed);
				vertexOffset.y = rand(vertexOffset.x);
				vertexOffset.z = rand(vertexOffset.z);
				vertexOffset = _SinTime[3] * _OffsetScale * normalize(vertexOffset);


				float len = length(position.xyz);
				float sinVal = sin(_Time*10) / 2 + 0.5;
				float offsetScale = (1 + sinVal / len);
				interp.position = UnityObjectToClipPos(position * offsetScale);
				return interp;
			}
			float4 BasicFragmentProgram(Interpolators interp) : SV_TARGET
			{
				float colorOffset = (interp.vertexSeed % 0.1 + _Time*10);
				float3 finalColor = interp.localPosition + 0.5;
				finalColor.r = (finalColor.r + colorOffset) % 1.0;
				finalColor.g = (finalColor.g + colorOffset) % 1.0;
				finalColor.b = (finalColor.b + colorOffset) % 1.0;
				
				return float4(finalColor,1);
			}

			ENDCG
		}
	}
}
