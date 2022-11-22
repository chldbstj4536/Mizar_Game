Shader "Hidden/NewImageEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AddColor("AddColor", Color) = (0,0,0,0)
        _CurTime("Time", Range(0.0, 1.0)) = 0
        [MaterialToggle]_IsOut("Is FadeOut", float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            fixed4 _AddColor;
            float _CurTime;
            float _IsOut;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - 0.0001f);
                col.rgb = col.rgb * abs(_IsOut - _CurTime);
                return col + _AddColor;
            }
            ENDCG
        }
    }
}
