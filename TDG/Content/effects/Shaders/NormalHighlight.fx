// Our texture sampler

float xSize;
float ySize;
float xDraw;
float yDraw;

int len1;
int len2;

float4 filterColor;


texture Texture;
sampler TextureSampler = sampler_state
{
    Texture = <Texture>;
};

// This data comes from the sprite batch vertex shader
struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCordinate : TEXCOORD0;
};

// Our pixel shader
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR
{
	float4 texColor = tex2D(TextureSampler, input.TextureCordinate);
	
	float vertPixSize = 1.0f/ySize;
	float horPixSize = 1.0f/xSize;
	
	float4 color = float4(0,0,0,0);
	
	float4 aboveColor = tex2D(TextureSampler, (input.TextureCordinate) + float2(0, -vertPixSize * len1));
	float4 belowColor = tex2D(TextureSampler, (input.TextureCordinate) + float2(0, vertPixSize * len1));
	float4 leftColor = tex2D(TextureSampler, (input.TextureCordinate) + float2(-horPixSize * len1, 0));
	float4 rightColor = tex2D(TextureSampler, (input.TextureCordinate) + float2(-horPixSize * len1, 0));
	
	
	float4 aboveColor2 = tex2D(TextureSampler, (input.TextureCordinate) + float2(0, -vertPixSize * len2));
	float4 belowColor2 = tex2D(TextureSampler, (input.TextureCordinate) + float2(0, vertPixSize * len2));
	float4 leftColor2 = tex2D(TextureSampler, (input.TextureCordinate) + float2(-horPixSize * len2, 0));
	float4 rightColor2 = tex2D(TextureSampler, (input.TextureCordinate) + float2(-horPixSize * len2, 0));

	if(texColor.a > 0 || aboveColor.a > 0 || belowColor.a > 0|| leftColor.a > 0|| rightColor.a > 0|| aboveColor2.a > 0|| belowColor2.a > 0|| leftColor2.a > 0|| rightColor2.a > 0)
	{
		color = float4(filterColor.r, filterColor.b, filterColor.g, filterColor.a);
	}

	return color;
}

// Compile our shader
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
