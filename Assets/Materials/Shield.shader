Shader "Unlit/Shield"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _FresnelPower ("Fresnel Power", Range(0, 10)) = 0.75
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend One OneMinusSrcAlpha
        Zwrite On
        AlphaToMask On

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal : COLOR;
                float3 viewDir : COLOR2;
            };
            
            fixed4 _Color;
            float _FresnelPower;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float fresnelValue = 1 - pow(dot(i.normal, i.viewDir), _FresnelPower);
                fixed4 col = _Color * fresnelValue;
                col.a = fresnelValue;
                return col;
            }
            ENDCG
        }
    }
}
