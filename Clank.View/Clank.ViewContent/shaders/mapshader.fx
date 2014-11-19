sampler s : register(s0);
texture2D xWallTexture;
texture2D xBorderTexture;
texture2D xGrassTexture;
texture2D xSourceTexture;

float2 scrolling;
sampler wall = sampler_state {
	texture = (xWallTexture);
	MinFilter = Linear; // Minification Filter
	MagFilter = Linear; // Magnification Filter
	MipFilter = Linear; // Mip-mapping
	AddressU = Wrap; // Address Mode for U Coordinates
	AddressV = Wrap; // Address Mode for V Coordinates
};

sampler border = sampler_state {
	texture = (xBorderTexture);
	MinFilter = Linear; // Minification Filter
	MagFilter = Linear; // Magnification Filter
	MipFilter = Linear; // Mip-mapping
	AddressU = Wrap; // Address Mode for U Coordinates
	AddressV = Wrap; // Address Mode for V Coordinates
};

sampler grass = sampler_state {
	texture = (xGrassTexture);
	MinFilter = Linear; // Minification Filter
	MagFilter = Linear; // Magnification Filter
	MipFilter = Linear; // Mip-mapping
	AddressU = Wrap; // Address Mode for U Coordinates
	AddressV = Wrap; // Address Mode for V Coordinates
};

sampler source = sampler_state {
	texture = (xSourceTexture);
	MinFilter = Linear; // Minification Filter
	MagFilter = Linear; // Magnification Filter
	MipFilter = Linear; // Mip-mapping
	AddressU = Clamp; // Address Mode for U Coordinates
	AddressV = Clamp; // Address Mode for V Coordinates
};

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float src = tex2D(source, coords).r;
	if (src < 0.1)
	{
		return tex2D(grass, (coords + scrolling) * 5);
	}
	else
	{
		float2 pos = (coords + scrolling);
		float step = 0.01f;
		float interpfactor = 0.0f;
		const int nb = 2;
		int i;
		for (i = 0; i < nb; i++)
			interpfactor += abs(tex2D(source, coords + float2(0, step * i)).r - src);
		for (i = 0; i < nb; i++)
			interpfactor += abs(tex2D(source, coords + float2(0, -step * i)).r - src);
		for (i = 0; i < nb; i++)
			interpfactor += abs(tex2D(source, coords + float2(step * i, 0)).r - src);
		for (i = 0; i < nb; i++)
			interpfactor += abs(tex2D(source, coords + float2(-step * i, 0)).r - src);
		for (i = 0; i < nb; i++)
			interpfactor += abs(tex2D(source, coords + float2(step * i, step * i)).r - src);
		for (i = 0; i < nb; i++)
			interpfactor += abs(tex2D(source, coords + float2(step * i, -step * i)).r - src);
		for (i = 0; i < nb; i++)
			interpfactor += abs(tex2D(source, coords + float2(step * i, -step * i)).r - src);
		for (i = 0; i < nb; i++)
			interpfactor += abs(tex2D(source, coords + float2(-step * i, -step * i)).r - src);
		interpfactor *= 2;

		return lerp(tex2D(wall, pos  * 5),
					tex2D(border, pos * 5),
					saturate(interpfactor));
	}
	return 0;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}