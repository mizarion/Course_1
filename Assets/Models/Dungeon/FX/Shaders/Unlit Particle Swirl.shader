// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit Particle Swirl"
{
	Properties
	{
		[HDR]_TintColor("TintColor", Color) = (1,1,1,1)
		[NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
		[NoScaleOffset]_DistortionMap("Distortion Map", 2D) = "white" {}
		_Distortion("Distortion", Range( 0 , 5)) = 0.1
		_Spin("Spin", Range( 0 , 1)) = 0.1
		_Softness("Softness", Range( 0 , 1)) = 1
		_AlphaEdge("Alpha Edge", Range( 1 , 5)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow nolightmap  nodynlightmap nodirlightmap nometa noforwardadd 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform float4 _TintColor;
		uniform sampler2D _MainTex;
		uniform fixed _Spin;
		uniform sampler2D _DistortionMap;
		uniform fixed _Distortion;
		uniform sampler2D _CameraDepthTexture;
		uniform fixed _Softness;
		uniform fixed _AlphaEdge;

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float cos137 = cos( ( _Spin * _Time.y ) );
			float sin137 = sin( ( _Spin * _Time.y ) );
			float2 rotator137 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos137 , -sin137 , sin137 , cos137 )) + float2( 0.5,0.5 );
			float cos129 = cos( ( -0.2 * _Spin * _Time.y ) );
			float sin129 = sin( ( -0.2 * _Spin * _Time.y ) );
			float2 rotator129 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos129 , -sin129 , sin129 , cos129 )) + float2( 0.5,0.5 );
			float4 tex2DNode144 = tex2D( _MainTex, ( rotator137 + ( ( tex2D( _DistortionMap, rotator129 ).r - 0.5 ) * _Distortion ) ) );
			o.Emission = ( _TintColor * i.vertexColor * tex2DNode144 ).rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth139 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth139 = abs( ( screenDepth139 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( ( _Softness * 10.0 ) ) );
			float4 tex2DNode145 = tex2D( _MainTex, i.uv_texcoord );
			o.Alpha = saturate( ( _TintColor.a * i.vertexColor.a * tex2DNode144.a * saturate( ( distanceDepth139 * distanceDepth139 ) ) * ( ( tex2DNode145.a + tex2DNode145.a ) * _AlphaEdge ) ) );
		}

		ENDCG
	}
}
/*ASEBEGIN
Version=15301
-1903;28;1912;1000;2469.302;1766.074;2.598543;True;True
Node;AmplifyShaderEditor.SimpleTimeNode;125;-1421.76,-47.27169;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;124;-1499.264,-207.9537;Fixed;False;Property;_Spin;Spin;5;0;Create;True;0;0;False;0;0.1;0.281;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;128;-1151.754,-291.0192;Float;False;3;3;0;FLOAT;-0.2;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;127;-1431.341,-485.739;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;129;-1016.127,-724.0125;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;131;-254.5177,-130.277;Fixed;False;Property;_Softness;Softness;6;0;Create;True;0;0;False;0;1;0.753;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;130;-708.6906,-730.8994;Float;True;Property;_DistortionMap;Distortion Map;3;1;[NoScaleOffset];Create;True;0;0;False;0;48de1ef02a200d3409309e4c10fd9c33;8054b37172673484fae716f2cb90af4b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;132;-706.9435,-419.4993;Fixed;False;Constant;_Float3;Float 3;5;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;-38.9588,38.29953;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;133;-1152.05,-121.5694;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;134;-665.1512,-311.2573;Fixed;False;Property;_Distortion;Distortion;4;0;Create;True;0;0;False;0;0.1;0.31;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;135;-495.0991,-466.4396;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;139;136.4104,-92.95148;Float;False;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;-282.0959,-395.4456;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;137;-876.4212,-199.0435;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;145;-163.4808,-873.0101;Float;True;Property;_TextureSample2;Texture Sample 2;2;1;[NoScaleOffset];Create;True;0;0;False;0;5b303ff28ad9368468a2edd759cf458d;c3d66a8056f9db345b1ea380aa7e815d;True;0;False;white;Auto;False;Instance;144;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;164;244.9312,-685.0405;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;376.2847,6.820923;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;163;-117.2892,-606.0953;Fixed;False;Property;_AlphaEdge;Alpha Edge;7;0;Create;True;0;0;False;0;1;0;1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;140;-81.3573,-340.907;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;165;403.9673,-576.6858;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;144;113.2149,-411.059;Float;True;Property;_MainTex;MainTex;2;1;[NoScaleOffset];Create;True;0;0;False;0;5b303ff28ad9368468a2edd759cf458d;5b303ff28ad9368468a2edd759cf458d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;142;402.5097,-168.728;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;161;497.5858,-975.7304;Float;False;Property;_TintColor;TintColor;0;1;[HDR];Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;143;494.5422,-791.029;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;146;791.5137,-374.4146;Float;False;5;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;819.1062,-685.6522;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;147;965.5985,-508.1176;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;42;1183.93,-671.3324;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Unlit Particle Swirl 2;False;False;False;False;False;False;True;True;True;False;True;True;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;-1;True;73;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;128;1;124;0
WireConnection;128;2;125;0
WireConnection;129;0;127;0
WireConnection;129;2;128;0
WireConnection;130;1;129;0
WireConnection;136;0;131;0
WireConnection;133;0;124;0
WireConnection;133;1;125;0
WireConnection;135;0;130;1
WireConnection;135;1;132;0
WireConnection;139;0;136;0
WireConnection;138;0;135;0
WireConnection;138;1;134;0
WireConnection;137;0;127;0
WireConnection;137;2;133;0
WireConnection;145;1;127;0
WireConnection;164;0;145;4
WireConnection;164;1;145;4
WireConnection;141;0;139;0
WireConnection;141;1;139;0
WireConnection;140;0;137;0
WireConnection;140;1;138;0
WireConnection;165;0;164;0
WireConnection;165;1;163;0
WireConnection;144;1;140;0
WireConnection;142;0;141;0
WireConnection;146;0;161;4
WireConnection;146;1;143;4
WireConnection;146;2;144;4
WireConnection;146;3;142;0
WireConnection;146;4;165;0
WireConnection;148;0;161;0
WireConnection;148;1;143;0
WireConnection;148;2;144;0
WireConnection;147;0;146;0
WireConnection;42;2;148;0
WireConnection;42;9;147;0
ASEEND*/
//CHKSM=FE2023F63BBF496050033A8C659286AF4EBFD55F