#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler s0;

texture colorMap;

sampler colorSampler = sampler_state
{
  Texture = (colorMap);
  AddressU = CLAMP;
  AddressV = CLAMP;
  MagFilter = LINEAR;
  MinFilter = LINEAR;
  Mipfilter = LINEAR;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(s0, input.TextureCoordinates);
	float4 colorMap = tex2D(colorSampler, input.TextureCoordinates);
	
	//colorMapMask alfa = ~0.5
	if(colorMap.w > 0.4 && colorMap.w < 0.6)
	{
		//in light
		return color;
	}
	else
	{
		return float4(0,0,0,0);
	}

}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};