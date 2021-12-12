#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0
	#define PS_SHADERMODEL ps_4_0
#endif

matrix WorldViewProjection;

float2 u_resolution;
float2 u_time;

texture Texture;
sampler TextureSampler = sampler_state
{
    Texture = <Texture>;
};


float random (float2 _st) {
    return frac(sin(dot(_st.xy,
                         float2(12.9898,78.233)))*
        43758.5453123);
}

// Based on Morgan McGuire @morgan3d
// https://www.shadertoy.com/view/4dS3Wd
float noise (float2 _st) {
    float2 i = floor(_st);
    float2 f = frac(_st);

    // Four corners in 2D of a tile
    float a = random(i);
    float b = random(i + float2(1.0, 0.0));
    float c = random(i + float2(0.0, 1.0));
    float d = random(i + float2(1.0, 1.0));

    float2 u = f * f * (3.0 - 2.0 * f);

    return lerp(a, b, u.x) +
            (c - a)* u.y * (1.0 - u.x) +
            (d - b) * u.x * u.y;
}

#define NUM_OCTAVES 10

float fbm ( float2 _st) {
    float v = 0.0;
    float a = 0.5;
    float2 shift = float2(100.0, 100.0);
    // Rotate to reduce axial bias

    float sinus = sin(0.5);
    float cosinus = cos(0.5);

    float2x2 rot = { cosinus, sinus, // row 1
                     -sinus, cosinus // row 2
                   };   
    /*
        float2x2 rot = { cos(0.5), sin(0.5), // row 1
                     -sin(0.5), cos(0.50) // row 2
                   };  
    */

    for (int i = 0; i < NUM_OCTAVES; ++i) {
        v += a * noise(_st);

        _st = mul(rot , _st) * 2.0 + shift;

        a *= 0.5;
    }
    return v;
}

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
    /*
    float4 texColor = tex2D(TextureSampler, input.TextureCoordinates) * input.Color;

    float2 st = input.TextureCoordinates / u_resolution.xy*3.;
    // st += st * abs(sin(u_time*0.1)*3.0);
    float3 color = float3(0, 0, 0);

    float2 q = float2(0, 0);
    q.x = fbm( st + 0.00 * u_time);
    q.y = fbm( st + float2(1.0, 1));

    float2 r = float2(0, 0);
    r.x = fbm( st + 1.0*q + float2(1.7,9.2)+ 0.15*u_time );
    r.y = fbm( st + 1.0*q + float2(8.3,2.8)+ 0.126*u_time);

    float f = fbm(st+r);
        /*
    color = lerp(float3(0.101961,0.619608,0.666667),
                float3(0.666667,0.666667,0.498039),
                clamp((f*f)*4.0,0.0,1.0));

    color = lerp(color,
                float3(0,0,0.164706),
                clamp(length(q),0.0,1.0));

    color = lerp(color,
                float3(0.666667,1,1),
                clamp(length(r.x),0.0,1.0));
    */
    //float4 colorOut = float4((f*f*f+.6*f*f+.5*f)*color,1.);

    //float4 colorOut = float4(f* 0.8,f * 0.8,f * 0.9, f);


    float4 colorOut = 0.5f * fbm(input.TextureCoordinates / u_resolution.xy*3) + 0.5f * fbm(input.TextureCoordinates / u_resolution.xy*3 + u_time);;// * random(u_time);

    return colorOut;// * input.Color;
}

technique BasicColorDrawing
{
	pass P0
	{
        VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};