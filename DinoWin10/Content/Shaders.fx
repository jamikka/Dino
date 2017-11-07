texture lightMask;
sampler s0;
sampler lightSampler = sampler_state { Texture = (lightMask); };
float blurSizeX = 0.002;
float blurSizeY = 0.002;
float blurWeights[4] =
{
	0.07,
	0.13,
	0.19,
	0.21
};

float4 PixShaderFunc(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);
	float4 lightColor = tex2D(lightSampler, coords);
	return color * lightColor;
}

float4 PS_BlurHorizontal(float2 coord : TEXCOORD0 ) : COLOR0
{
    float4 Color = {0, 0, 0, 1};

    Color += tex2D(s0, float2(coord.x - 3.0*blurSizeX, coord.y)) * blurWeights[0];
    Color += tex2D(s0, float2(coord.x - 2.0*blurSizeX, coord.y)) * blurWeights[1];
    Color += tex2D(s0, float2(coord.x - blurSizeX, coord.y)) * blurWeights[2];
    Color += tex2D(s0, coord) * blurWeights[3];
    Color += tex2D(s0, float2(coord.x + blurSizeX, coord.y)) * blurWeights[2];
    Color += tex2D(s0, float2(coord.x + 2.0*blurSizeX, coord.y)) * blurWeights[1];
    Color += tex2D(s0, float2(coord.x + 3.0*blurSizeX, coord.y)) * blurWeights[0];

	/*Color += tex2D(s0, float2(coord.x, coord.y - 3.0*blurSizeY)) * blurWeights[0];
    Color += tex2D(s0, float2(coord.x, coord.y - 2.0*blurSizeY)) * blurWeights[1];
    Color += tex2D(s0, float2(coord.x, coord.y - blurSizeY)) * blurWeights[2];
    Color += tex2D(s0, coord) * blurWeights[3];
    Color += tex2D(s0, float2(coord.x, coord.y + blurSizeY)) * blurWeights[2];
    Color += tex2D(s0, float2(coord.x, coord.y + 2.0*blurSizeY)) * blurWeights[1];
    Color += tex2D(s0, float2(coord.x, coord.y + 3.0*blurSizeY)) * blurWeights[0];*/

    return Color;
}

float4 PS_BlurVertical( float2 coord : TEXCOORD0 ) : COLOR0
{
    float4 Color = {0, 0, 0, 1};

	Color += tex2D(s0, float2(coord.x, coord.y - 3.0*blurSizeY)) * blurWeights[0];
    Color += tex2D(s0, float2(coord.x, coord.y - 2.0*blurSizeY)) * blurWeights[1];
    Color += tex2D(s0, float2(coord.x, coord.y - blurSizeY)) * blurWeights[2];
    Color += tex2D(s0, coord) * blurWeights[3];
    Color += tex2D(s0, float2(coord.x, coord.y + blurSizeY)) * blurWeights[2];
    Color += tex2D(s0, float2(coord.x, coord.y + 2.0*blurSizeY)) * blurWeights[1];
    Color += tex2D(s0, float2(coord.x, coord.y + 3.0*blurSizeY)) * blurWeights[0];

    /*Color += tex2D(s0, float2(coord.x, coord.y - 3.0*blurSizeY)) * 0.09f;
    Color += tex2D(s0, float2(coord.x, coord.y - 2.0*blurSizeY)) * 0.11f;
    Color += tex2D(s0, float2(coord.x, coord.y - blurSizeY)) * 0.18f;
    Color += tex2D(s0, coord) * 0.24f;
    Color += tex2D(s0, float2(coord.x, coord.y + blurSizeY)) * 0.18f;
    Color += tex2D(s0, float2(coord.x, coord.y + 2.0*blurSizeY)) * 0.11f;
    Color += tex2D(s0, float2(coord.x, coord.y + 3.0*blurSizeY)) * 0.09f;*/

    return Color;
}

technique Mask
{
    pass YkkosPass
    {
        PixelShader = compile ps_2_0 PixShaderFunc();
    }
}

technique Blur
{
	pass One
	{
		PixelShader = compile ps_2_0 PS_BlurHorizontal();
	}
	pass Two
	{
		PixelShader = compile ps_2_0 PS_BlurVertical();
	}
}

/*float ambientIntensity = 0.6;
float4 ambientColor = float4(1,1,1,1);
 
 
float4 AmbientPSfunc(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(s0, coords);
	//if (color.a)
	//{
		color.rgb = color.bgr;
	//}
	return color;
}
 
technique Ambient
{
    pass YkkosPass
    {
        PixelShader = compile ps_2_0 AmbientPSfunc();
    }
}*/