// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/TransparentObstacle"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _Color("Color (RGBA)", Color) = (1, 1, 1, 1) // add _Color property
        _ScreenThreshold("Screen Threshold (xy - Min/Max Horizontal, zw - Vertical", Vector) = (0.1, 0.9, 0.1, 0.9)
    }

        SubShader
        {
            Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull front
            LOD 100

            Pass
            {
                CGPROGRAM

                #pragma vertex vert alpha
                #pragma fragment frag alpha

                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex   : POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex  : SV_POSITION;
                    half2 texcoord : TEXCOORD0;
                    float2 screenPos : TEXCOORD1;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color;
                float4 _ScreenThreshold;

                v2f vert(appdata_t v)
                {
                    v2f o;

                    o.vertex = UnityObjectToClipPos(v.vertex);
                    v.texcoord.x = 1 - v.texcoord.x;
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                    float4 screenPosUnnormalized = ComputeScreenPos(o.vertex);
                    o.screenPos = screenPosUnnormalized.xy / screenPosUnnormalized.w;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.texcoord) * _Color; // multiply by _Color
                    
                    half factor = step(i.screenPos.x, _ScreenThreshold.y) * step(_ScreenThreshold.x, i.screenPos.x) * step(i.screenPos.y, _ScreenThreshold.w) * step(_ScreenThreshold.z, i.screenPos.y);
                    col.a = factor * 0.1 + col.a * (1 - factor);
                    return col;
                }

                ENDCG
            }
        }
}
