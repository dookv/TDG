// Our texture sampler
texture Texture;
sampler TextureSampler = sampler_state
{
    Texture = <Texture>;
};

// Constants for wave parameters set these whe  you call the shder in c#
float Amplitude; // Adjust the wave height
float Frequency;  // Adjust the wave frequency
float Speed;      // Adjust the wave speed

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
    // Calculate the texture coordinate offsets based on the wave effect
    float xOffset = Amplitude * sin(input.TextureCordinate.y * Frequency + Speed * input.Position.x);
    float yOffset = 0; // No vertical displacement in this example

    // Apply the texture coordinate offsets
    float2 texCoord = input.TextureCordinate + float2(xOffset, yOffset);

    // Sample the texture using the modified texture coordinates
    float4 texColor = tex2D(TextureSampler, texCoord);

    return texColor * input.Color; // Keep the original color
}

// Compile our shader HLSL
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}