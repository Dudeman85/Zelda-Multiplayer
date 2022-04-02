Shader "Sprites/PlayerSpriteShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_CurrentPallette("Current Pallette", int) = 1
		_Pallette1("Pallette 1", 2D) = "white" {}
		_Pallette2("Pallette 2", 2D) = "white" {}
		_Pallette3("Pallette 3", 2D) = "white" {}
		_Pallette4("Pallette 4", 2D) = "white" {}
	}

		SubShader
	{
		// No culling or depth
			Tags{"Queue" = "Transparent"}

		Pass
		{
		Cull Off ZWrite Off ZTest Off Lighting Off
		Blend One OneMinusSrcAlpha

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
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			int _CurrentPallette;
			sampler2D _Pallette1;
			sampler2D _Pallette2;
			sampler2D _Pallette3;
			sampler2D _Pallette4;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				int currentPallette = _CurrentPallette;

				fixed4 col2[5];
				col2[0] = col;
				col2[1] = tex2D(_Pallette1, float2 (col.g, 0));
				col2[2] = tex2D(_Pallette2, float2 (col.g, 0));
				col2[3] = tex2D(_Pallette3, float2 (col.g, 0));
				col2[4] = tex2D(_Pallette4, float2 (col.g, 0));

				col.rgb = col2[currentPallette].rgb;

				col.rgb *= col.a;

				return col;
			}
			ENDCG
		}
	}
}
