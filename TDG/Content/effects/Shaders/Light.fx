// Our texture sampler
texture Texture;
sampler TextureSampler = sampler_state
{
    Texture = <Texture>;
};

// Light parameters
float3 LightColor = float3(1.0, 1.0, 1.0); // White light
float RimIntensity = 0.5; // Adjust the intensity of the rim light
float RimPower = 4.0;     // Adjust the power of the rim light
float LightRadius = 0.3;  // Adjust the radius of the light source

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

    // Calculate the distance from the light source center
    float2 lightCenter = float2(0.5, 0.5); // Adjust the light source center
    float distanceToLight = distance(lightCenter, input.TextureCordinate);

    // Calculate the rim light contribution based on the light radius and distance
    float rim = saturate(1.0 - (distanceToLight / LightRadius));
    rim = pow(rim, RimPower) * RimIntensity;

    // Combine the original color with the rim light
    float3 finalColor = texColor.rgb + rim * LightColor;

    return float4(finalColor, texColor.a);
}

// Compile our shader HLSL
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}