Shader "UI/CircleMask" {
    Properties {
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Radius", Float) = 0.2
        _Color ("Color", Color) = (0,0,0,0.7)
    }
    SubShader {
        Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off ZTest Always

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Center;
            float _Radius;
            float4 _Color;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.uv;

                // Ratio largeur / hauteur de l’écran
                float aspect = _ScreenParams.x / _ScreenParams.y;

                // Décale par rapport au centre
                float2 delta = uv - _Center.xy;

                // Compense le ratio → compresser X
                delta.x *= aspect;

                // Distance corrigée
                float d = length(delta);

                if (d < _Radius) {
                    return fixed4(0,0,0,0); // trou transparent
                }
                return _Color;
            }
            ENDCG
        }
    }
}
