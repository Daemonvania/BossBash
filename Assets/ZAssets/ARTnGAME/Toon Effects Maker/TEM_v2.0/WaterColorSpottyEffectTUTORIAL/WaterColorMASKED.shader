Shader "WaterColor/BaseMASKED" {
	Properties{
		_MainTex("Main Tex", 2D) = "white" {}
		_NoiseTex("Noise Tex", 2D) = "white" {}
		_AquarelleTex("AquarelleTex", 2D) = "white" {}
		_Outline("Outline", Range(0,1)) = 0.1
		_OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
		_DiffuseColor("Diffuse Color", Color) = (1, 1, 1, 1)
		_EdgeColor("EdgeColor", Color) = (1, 1, 1, 1)
		_DiffuseSegment("Diffuse Segment", Vector) = (0.1, 0.3, 0.6, 1.0)
		_ColorTop("Top Color", Color) = (1,1,1,1)
		_ColorBot("Bot Color", Color) = (1,1,1,1)
		shadowIntensity("shadow Intensity", Vector) = (0.5,0.5,1,1)

	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			Pass {
				NAME "OUTLINE"

				Cull Front

				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				#include "UnityCG.cginc"

				float _Outline;
				fixed4 _OutlineColor;
				uniform sampler2D _GlobalEffectRT;
				uniform float _OrthographicCamSize;
				uniform float3 _Position;

				struct a2v {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
				};

				struct v2f {
					float4 pos : SV_POSITION;
					float3 worldPos : TEXCOORD2;
					UNITY_FOG_COORDS(3)
				};

				v2f vert(a2v v) {
					v2f o;
					float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
					float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
					pos = pos + float4(normalize(normal), 0) * _Outline;
					o.pos = mul(UNITY_MATRIX_P, pos);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
					UNITY_TRANSFER_FOG(o, o.pos);
					return o;
				}

				float4 frag(v2f i) : SV_Target {
					float2 uv2 = i.worldPos.xz - _Position.xz;
					uv2 = uv2 / (_OrthographicCamSize * 2);
					uv2 += 0.5;
					float fade = tex2D(_GlobalEffectRT, uv2).r;
					float3 ColorTemp = lerp(_OutlineColor.rgb,1, (1 - fade));
					UNITY_APPLY_FOG(i.fogCoord, ColorTemp);
					return float4(ColorTemp, 1);
					}

					ENDCG
				}

				Pass {
					Tags { //"LightMode" = "ForwardBase" 
					}

					CGPROGRAM

					#pragma vertex vert
					#pragma fragment frag
						#pragma multi_compile_fwdadd_fullshadows//#pragma multi_compile_fwdbase
					#pragma multi_compile_fog
					#include "UnityCG.cginc"
					#include "Lighting.cginc"
					#include "AutoLight.cginc"
					#include "UnityShaderVariables.cginc"

					fixed4 _DiffuseColor;
					sampler2D _MainTex, _NoiseTex, _AquarelleTex;
					float4 _MainTex_ST, _EdgeColor, _ColorTop, _ColorBot;
					uniform float4 _ShadowColor;
					fixed4 _DiffuseSegment;

					uniform sampler2D _GlobalEffectRT;
					uniform float _OrthographicCamSize;
					uniform float3 _Position;

					float4 shadowIntensity;

					struct a2v {
						float4 vertex : POSITION;
						float3 normal : NORMAL;
						float4 texcoord : TEXCOORD0;
					};

					struct v2f {
						float4 pos : SV_POSITION;
						float2 uv : TEXCOORD0;
						float2 uv2 : TEXCOORD4;
						fixed3 worldNormal : TEXCOORD1;
						float3 worldPos : TEXCOORD2;
						//	SHADOW_COORDS(3)
						LIGHTING_COORDS(3, 5)
							UNITY_FOG_COORDS(6)
						
					};

					v2f vert(a2v v) {
						v2f o;
						o.pos = UnityObjectToClipPos(v.vertex);
						o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
						o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
						o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
						o.uv2 = mul(unity_ObjectToWorld, v.vertex).xyz;
						TRANSFER_SHADOW(o);
						TRANSFER_VERTEX_TO_FRAGMENT(o)
						UNITY_TRANSFER_FOG(o, o.pos);
						return o;
					}

					fixed4 frag(v2f i) : SV_Target {
						float2 uv = i.uv;
						float2 uv2 = i.worldPos.xz - _Position.xz;
						uv2 = uv2 / (_OrthographicCamSize * 2);
						uv2 += 0.5;
						float fade = (tex2D(_GlobalEffectRT, uv2).r);

						UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos);
						float shadowA = LIGHT_ATTENUATION(i);

						fixed w = 0;
						fixed3 noiseColor = tex2D(_NoiseTex, (i.uv2 + (_Time.x * 3))*0.5 );
						fixed3 aquarelleColor = tex2D(_AquarelleTex, i.uv * 1);
						fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT;

						atten = smoothstep(0.7, 1.5, atten * 2 + noiseColor.r * 1);
						atten = smoothstep(0.2, 0.8, atten);
						if (atten < _DiffuseSegment.x + w) {
							atten = lerp(_DiffuseSegment.x*1.0, _DiffuseSegment.y*0.0, smoothstep((_DiffuseSegment.x - w)*0.01 - 0.1, (_DiffuseSegment.x + w)*0.01 + 0.5, atten));
						}

						atten = lerp(1, atten, fade);
						fixed3 texColor = lerp(tex2D(_MainTex, uv).rgb, _ShadowColor + ambient, 0);
						texColor = lerp(texColor, 1, 1 - tex2D(_MainTex, uv).a);
						fixed3 diffuse = _DiffuseColor.rgb * smoothstep(0.35, 0.4, texColor)*texColor;
						fixed4 c = lerp(0, _ColorTop, i.uv.y)* i.uv.y / 2;
						fixed4 d = lerp(0, _ColorBot, 0.5 - i.uv.y);

						diffuse = lerp(diffuse, 1, (1 - fade));
						diffuse = lerp(_ShadowColor, diffuse*saturate(aquarelleColor*0.45 + 0.65 + noiseColor * 0.15), 1);
						fixed3 DiffuseTemp = ambient + (diffuse) - ((1 - atten)*(1 - _ShadowColor))*_ShadowColor.a; 

						UNITY_APPLY_FOG(i.fogCoord, DiffuseTemp);
						//return fixed4(DiffuseTemp, 1);
						return (fixed4(DiffuseTemp, 1)*(shadowA*shadowIntensity.x + shadowIntensity.y));
					}
						ENDCG
				}
		}
		FallBack "Diffuse"
}