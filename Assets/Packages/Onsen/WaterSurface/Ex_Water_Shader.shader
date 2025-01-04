// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Ex_Water_Shader"
{
	Properties
	{
		_Opacity("Opacity", Range( 0 , 1)) = 0.8
		_WaterColor("WaterColor", Color) = (0.2,0.2,1,0)
		[Toggle(_USEWATERTEXTURE_ON)] _UseWaterTexture("UseWaterTexture", Float) = 0
		_Tex_WaterColor("Tex_WaterColor", 2D) = "white" {}
		_Tex_WaterNormal("Tex_WaterNormal", 2D) = "bump" {}
		_Normal_Intensity("Normal_Intensity", Range( 0 , 1)) = 1
		_WaterSpeed("WaterSpeed", Vector) = (0,0,0,0)
		_WaveTex("WaveTex", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _USEWATERTEXTURE_ON
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _WaveTex;
		uniform float4 _WaveTex_ST;
		uniform sampler2D _Tex_WaterNormal;
		uniform float2 _WaterSpeed;
		uniform float4 _Tex_WaterNormal_ST;
		uniform float _Normal_Intensity;
		uniform float4 _WaterColor;
		uniform sampler2D _Tex_WaterColor;
		uniform float4 _Tex_WaterColor_ST;
		uniform float _Opacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_WaveTex = i.uv_texcoord * _WaveTex_ST.xy + _WaveTex_ST.zw;
			float2 temp_output_2_0_g1 = uv_WaveTex;
			float2 break6_g1 = temp_output_2_0_g1;
			float temp_output_25_0_g1 = ( pow( 0.5 , 3.0 ) * 0.1 );
			float2 appendResult8_g1 = (float2(( break6_g1.x + temp_output_25_0_g1 ) , break6_g1.y));
			float4 tex2DNode14_g1 = tex2D( _WaveTex, temp_output_2_0_g1 );
			float temp_output_4_0_g1 = 2.0;
			float3 appendResult13_g1 = (float3(1.0 , 0.0 , ( ( tex2D( _WaveTex, appendResult8_g1 ).g - tex2DNode14_g1.g ) * temp_output_4_0_g1 )));
			float2 appendResult9_g1 = (float2(break6_g1.x , ( break6_g1.y + temp_output_25_0_g1 )));
			float3 appendResult16_g1 = (float3(0.0 , 1.0 , ( ( tex2D( _WaveTex, appendResult9_g1 ).g - tex2DNode14_g1.g ) * temp_output_4_0_g1 )));
			float3 normalizeResult22_g1 = normalize( cross( appendResult13_g1 , appendResult16_g1 ) );
			float4 color25 = IsGammaSpace() ? float4(0,0,1,1) : float4(0,0,1,1);
			float2 uv_Tex_WaterNormal = i.uv_texcoord * _Tex_WaterNormal_ST.xy + _Tex_WaterNormal_ST.zw;
			float2 panner19 = ( 1.0 * _Time.y * _WaterSpeed + uv_Tex_WaterNormal);
			float4 lerpResult22 = lerp( color25 , float4( UnpackNormal( tex2D( _Tex_WaterNormal, panner19 ) ) , 0.0 ) , _Normal_Intensity);
			o.Normal = BlendNormals( normalizeResult22_g1 , lerpResult22.rgb );
			float2 uv_Tex_WaterColor = i.uv_texcoord * _Tex_WaterColor_ST.xy + _Tex_WaterColor_ST.zw;
			float2 panner14 = ( 1.0 * _Time.y * _WaterSpeed + uv_Tex_WaterColor);
			#ifdef _USEWATERTEXTURE_ON
				float4 staticSwitch9 = tex2D( _Tex_WaterColor, panner14 );
			#else
				float4 staticSwitch9 = _WaterColor;
			#endif
			o.Albedo = staticSwitch9.rgb;
			o.Alpha = _Opacity;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
1920;9;1920;1131;1788.369;653.9434;1.701228;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;18;-1091.766,359.3788;Inherit;True;Property;_Tex_WaterNormal;Tex_WaterNormal;5;0;Create;True;0;0;0;False;0;False;None;None;False;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;17;-1076.406,-237.7135;Inherit;True;Property;_Tex_WaterColor;Tex_WaterColor;4;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.Vector2Node;15;-793.9368,128.4097;Inherit;False;Property;_WaterSpeed;WaterSpeed;7;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-820.3067,534.6863;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-807.3066,-73.91326;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;19;-536.9074,539.8862;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;25;-212.6119,221.7412;Inherit;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;0;False;0;False;0,0,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;1;-260.6543,-3.546851;Inherit;True;Property;_WaveTex;WaveTex;10;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;23;-278.1422,670.2645;Inherit;False;Property;_Normal_Intensity;Normal_Intensity;6;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;-305.1373,428.9102;Inherit;True;Property;_NormalMap;NormalMap;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;14;-540.4376,-71.79015;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;22;96.51762,286.4994;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;7;-238.8375,-238.1894;Inherit;True;Property;_WaterTexture;WaterTexture;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;10;-78.93742,-456.59;Inherit;False;Property;_WaterColor;WaterColor;2;0;Create;True;0;0;0;False;0;False;0.2,0.2,1,0;0.2,0.2,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;6;6.862623,2.309651;Inherit;False;NormalCreate;8;;1;e12f7ae19d416b942820e3932b56220f;0;4;1;SAMPLER2D;;False;2;FLOAT2;0,0;False;3;FLOAT;0.5;False;4;FLOAT;2;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;9;309.7627,-343.49;Inherit;False;Property;_UseWaterTexture;UseWaterTexture;3;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;5;235.6625,469.0093;Float;False;Property;_Opacity;Opacity;1;0;Create;True;0;0;0;False;0;False;0.8;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;11;305.8625,3.609759;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;629.7141,-169.5341;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Ex_Water_Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;False;Transparent;;AlphaTest;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;2;18;0
WireConnection;16;2;17;0
WireConnection;19;0;20;0
WireConnection;19;2;15;0
WireConnection;12;0;18;0
WireConnection;12;1;19;0
WireConnection;14;0;16;0
WireConnection;14;2;15;0
WireConnection;22;0;25;0
WireConnection;22;1;12;0
WireConnection;22;2;23;0
WireConnection;7;0;17;0
WireConnection;7;1;14;0
WireConnection;6;1;1;0
WireConnection;9;1;10;0
WireConnection;9;0;7;0
WireConnection;11;0;6;0
WireConnection;11;1;22;0
WireConnection;0;0;9;0
WireConnection;0;1;11;0
WireConnection;0;9;5;0
ASEEND*/
//CHKSM=0DD66D061360C7D9DD4E936CC1FE5970766E214B