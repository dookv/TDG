// Our texture sampler
texture Texture;
sampler TextureSampler = sampler_state
{
    Texture = <Texture>;
};

// Light parameters
float3 OriginalColor = float3(1.0, 1.0, 1.0); // Original light color
float3 LightColor = float3(0.5, 0.0, 0.5);  // Purple light
float RimPower = 4.0;     // Adjust the power of the rim light
float LightDiameter = 0.6;  // Adjust the diameter of the light source
float Time;
// Parameters for pulsating light
float PulsateFrequency = 2.0; // Adjust the frequency of the pulsation
float PulsateAmplitude = 0.5; // Adjust the amplitude of the pulsation

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
    float distanceToLight = distance(lightCenter, input.TextureCordinate) * 2; // Multiply by 2 to get diameter

    // Calculate the normalized distance
    float normalizedDistance = distanceToLight / LightDiameter;

    // Calculate the pulsating factor using sine function based on passed time
    float pulsateFactor = 1.0 + PulsateAmplitude * sin(PulsateFrequency * Time);

    // Interpolate between OriginalColor and LightColor based on pulsating factor
    float3 blendedColor = lerp(OriginalColor, LightColor, pulsateFactor);

    // Calculate the rim light contribution based on the normalized distance
    float rim = saturate(1.0 - normalizedDistance);
    rim = pow(rim, RimPower) * rim; // Pulsation also affects rim intensity

    // Combine the original color with the blended rim light color
    float3 finalColor = texColor.rgb + rim * blendedColor;

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