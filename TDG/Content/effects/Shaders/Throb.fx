// Our texture sampler

float SINLOC;//pos within a sin curve
//sin repeats, so if you feed it in a float  you willl get gradual patern

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
	
	float4 color;
	if(texColor.a != 0)//a = alpha value, 0 = clear, 1 = opaque
	{
		
		color = float4(texColor.r + (texColor.r - filterColor.r) * SINLOC,
						texColor.g + (texColor.g - filterColor.g) * SINLOC,
						texColor.b + (texColor.b - filterColor.b) * SINLOC, texColor.a);//SINLOC is location on the sin curve 
	}
	else
	{
		
		color = float4(texColor.r, texColor.b, texColor.g, texColor.a);
	}
	

	return color * filterColor;
	
	
	//Other shader that pulses from red to white
	    // Calculate the interpolation factor based on the sinusoidal effect
    //float factor = (1 + SINLOC) * 0.5;

    // Interpolate between white and filterColor (red) using the factor
    //float4 interpolatedColor = lerp(float4(1, 1, 1, texColor.a), filterColor, factor);

    //return interpolatedColor * texColor; // Apply the interpolated color to the texture
	
}

// Compile our shader HLSL
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
