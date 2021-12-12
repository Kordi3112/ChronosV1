#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float2 pixelSize;
bool antyaliasingIsActive;

float power1;
float power2;
float power3;


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

float4 AntialiseSampler(sampler samp, float2 coords, float power1 ,float power2, float power3)
{
	float zone3 = 0.125 * power3;

	float zone2 = 0.25 * power2;
		
	float zone1 = 0.5;

		/*
		|00|01|02|
		|10|11|12|
		|20|21|22|
		*/

	float4 color00 = tex2D(samp, coords + float2(-pixelSize.x, -pixelSize.y)) * power3;
	float4 color01 = tex2D(samp, coords + float2(0, -pixelSize.y)) * power2;
	float4 color02 = tex2D(samp, coords + float2(pixelSize.x, -pixelSize.y)) * power3;

	float4 color10 = tex2D(samp, coords + float2(-pixelSize.x, 0)) * power2;
	float4 color11 = tex2D(samp, coords + float2(0, 0)) * power1;
	float4 color12 = tex2D(samp, coords + float2(pixelSize.x, 0)) * power2;

	float4 color20 = tex2D(samp, coords + float2(-pixelSize.x, pixelSize.y)) * power3;
	float4 color21 = tex2D(samp, coords + float2(0, pixelSize.y)) * power2;
	float4 color22 = tex2D(samp, coords + float2(pixelSize.x, pixelSize.y)) * power3;


	float4 color = color00 + color01 + color02 + color10 + color11 + color12 + color20 + color21 + color22;

	color /= power3 * 4 + power2 * 4 + power1;

	return color;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color;

	if(antyaliasingIsActive)
		color = AntialiseSampler(s0, input.TextureCoordinates, power1, power2, power3);
	else color = tex2D(s0, input.TextureCoordinates);

	color *= input.Color;

	float4 colorMap;

	//float4 colorMap = tex2D(colorSampler, input.TextureCoordinates);
	if(antyaliasingIsActive)
		colorMap = AntialiseSampler(colorSampler, input.TextureCoordinates, 0.111, 0.111, 0.111);
	else colorMap = tex2D(colorSampler, input.TextureCoordinates);

	

	return float4(color.rgb * colorMap.rgb, color.a);
}



technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};