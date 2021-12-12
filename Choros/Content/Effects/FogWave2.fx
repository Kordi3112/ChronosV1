#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0
	#define PS_SHADERMODEL ps_4_0
#endif

matrix WorldViewProjection;

float2 u_time;
float2 u_scale;
float2 u_offset;

bool u_borderLines;

texture Texture;
sampler TextureSampler = sampler_state
{
    Texture = <Texture>;
};


struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TextureCoordinates : TEXCOORD0;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION;
	float2 TextureCoordinates : TEXCOORD0;
	float4 Color : COLOR0;
};


float rand(float2 coord){
	// prevents randomness decreasing from coordinates too large
	coord = fmod(coord, 10000.0);
	// returns "random" float between 0 and 1
	return frac(sin(dot(coord, float2(12.9898,78.233))) * 43758.5453);
}

float2 rand2( float2 coord ) {
	// prevents randomness decreasing from coordinates too large
	coord = fmod(coord, 10000.0);
	// returns "random" vec2 with x and y between 0 and 1
    return frac(sin( float2( dot(coord,float2(127.1,311.7)), dot(coord,float2(269.5,183.3)) ) ) * 43758.5453);
}
// Based on Morgan McGuire @morgan3d
// https://www.shadertoy.com/view/4dS3Wd
float noise (float2 _st) {
    float2 i = floor(_st);
    float2 f = frac(_st);

    // Four corners in 2D of a tile
    float a = rand(i);
    float b = rand(i + float2(1.0, 0.0));
    float c = rand(i + float2(0.0, 1.0));
    float d = rand(i + float2(1.0, 1.0));

    float2 u = f * f * (3.0 - 2.0 * f);

    return lerp(a, b, u.x) +
            (c - a)* u.y * (1.0 - u.x) +
            (d - b) * u.x * u.y;
}

float cellular_noise(float2 coord) {
	float2 i = floor(coord);
	float2 f = frac(coord);
	
	float min_dist = 9999999.0;
	// going through the current tile and the tiles surrounding it
	for(float x = -1.0; x <= 1.0; x++) {
		for(float y = -1.0; y <= 1.0; y++) {
			
			// generate a random point in each tile,
			// but also account for whether it's a farther, neighbouring tile
			float2 node = rand2(i + float2(x, y)) + float2(x, y);
			
			// check for distance to the point in that tile
			// decide whether it's the minimum
			//float dist = sqrt((f - node).x * (f - node).x + (f - node).y * (f - node).y);
			
			float2 r = f - node;

			float dist2 = r.x * r.x + r.y * r.y;

			min_dist = min(min_dist, dist2);
		}
	}
	return sqrt(min_dist);
}


float fbm(float2 coord){
	int OCTAVES = 5;
	
	float normalize_factor = 0.0;
	float value = 0.0;
	float scale = 0.5;

	for(int i = 0; i < OCTAVES; i++){
		value += cellular_noise(coord) * scale;
		normalize_factor += scale;
		coord *= 2.0;
		scale *= 0.5;
	}
	return value / normalize_factor;
}

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position, WorldViewProjection);
	output.TextureCoordinates = input.TextureCoordinates;
	output.Color = input.Color;

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(TextureSampler, input.TextureCoordinates) * input.Color; 
	float2 coords;
	if(u_scale.x != 0.223)
		coords = u_scale * (input.TextureCoordinates +u_time +  u_offset) * 300.0;

	float value = fbm(coords);
	
	float borderSize = 0.015f;
	float secBorderSize = 0.02f;

	float R = 0.3 * fbm(0.1 * coords + float2(10,10));
	float G = 0.4 * fbm(0.12 * coords+ float2(-10,10));
	float B = 0.7 * fbm(0.14 *coords+ float2(10,-10));

	if(value == 0.7f)
		value = 1;
	else if(abs(value - 0.4f) < borderSize)
		value = 1;
	else if(abs(value - 0.4f) < secBorderSize)
		value = 0.3f;
	else value = 0;

	

	return color * value * float4(R,G,B,1);
	/*
	float4 color = tex2D(TextureSampler, input.TextureCoordinates) * input.Color; 
	float2 coords;
	if(u_scale.x != 0.223)
		coords = u_scale * (input.TextureCoordinates +u_time +  u_offset) * 300.0;

	float value = fbm(coords);
	
	float borderSize = 0.015f;
	float secBorderSize = 0.02f;

	float R = 0.3 * fbm(0.1 * coords + float2(10,10));
	float G = 0.4 * fbm(0.12 * coords+ float2(-10,10));
	float B = 0.7 * fbm(0.14 *coords+ float2(10,-10));

	if(value == 0.7f)
		value = 1;
	if(abs(value - 0.4f) < borderSize)
		value = 1;
	else if(abs(value - 0.4f) < secBorderSize)
		value = 0.3f;

	

	return color * value * float4(R,G,B,1);
	*/
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};