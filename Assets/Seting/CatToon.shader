Shader "Unlit/CatToon"
{
 
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Diffuse("Color",Color) = (1,1,1,1)
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
			
			//结构体
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            //输入结构
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed3 worldNormal:TEXCOORD1;
                fixed3 worldPos:TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Diffuse;

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld,v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
                
                fixed3 albedo = tex2D(_MainTex,i.uv);

                //漫反射
                fixed3 worldLightDir = UnityWorldSpaceLightDir(i.worldPos);

                float difLight = dot(worldLightDir,i.worldNormal)*0.5+0.5;//使用半兰伯特模型

                fixed3 diffuse = _LightColor0.rgb *albedo *_Diffuse.rgb *difLight;

                return float4( ambient + diffuse,1);
            }
            ENDCG 
        }
    }
        
}
