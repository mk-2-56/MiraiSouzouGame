Shader "005_Simulation_Shader"
{

	Properties
	{
		_InputTexture("InputTexture", 2D) = "" {}
		_Attenuation("Attenuation", Range(0, 0.2)) = 0.005
		_PhaseVelocity("PhaseVelocity", Range(0.0, 0.5)) = 0.02
	}

	SubShader{

		Lighting Off
		Blend One Zero

		Pass {
			CGPROGRAM

			#include "UnityCustomRenderTexture.cginc"
			#pragma vertex CustomRenderTextureVertexShader
			#pragma fragment frag
			#pragma target 3.0

			sampler2D _InputTexture;
			half _PhaseVelocity;
			float _Attenuation;

			float4 frag(v2f_customrendertexture i) : SV_Target
			{
				float2 uv = i.globalTexcoord;

				float du = 1.0 / _CustomRenderTextureWidth;
				float dv = 1.0 / _CustomRenderTextureHeight;
				float3 duv = float3(du, dv, 0) * 3;

				float2 c = tex2D(_SelfTexture2D, uv);
				float p = (2 * c.r - c.g + _PhaseVelocity * (
					tex2D(_SelfTexture2D, uv - duv.zy).r +
					tex2D(_SelfTexture2D, uv + duv.zy).r +
					tex2D(_SelfTexture2D, uv - duv.xz).r +
					tex2D(_SelfTexture2D, uv + duv.xz).r - 4 * c.r)) * 0.999;

				p = p + tex2D(_InputTexture, uv).r;
				p = p * (1 - _Attenuation);

				return float4(p, c.r, 0, 0);
			}
		ENDCG
		}
	}

}