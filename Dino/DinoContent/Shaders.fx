sampler s0;
float ambientIntensity = 0.6;
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
}