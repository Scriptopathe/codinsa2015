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
	float4 val = tex2D(source, coords);
	
	float src = val.r;
	float4 col;
	if (src < 0.5)
	{
		col = tex2D(grass, (coords + scrolling) * 5);
	}
	else
	{
		float2 pos = (coords + scrolling);
		float step = 0.004f;
		float interpfactor = 0.0f;


		const int nb = 2;
		const int nb2 = 2;

		// Précalcule 
		int i;
		float steps[4];
		for (i = 0; i < nb; i++)
			steps[i] = step * i;

		for (i = 0; i < nb; i++)
			interpfactor += abs(tex2D(source, coords + float2(0, steps[i])).r - src);
		for (i = 0; i < nb; i++)
			interpfactor += abs(tex2D(source, coords + float2(0, -steps[i])).r - src);
		for (i = 0; i < nb; i++)
			interpfactor += abs(tex2D(source, coords + float2(steps[i], 0)).r - src);
		for (i = 0; i < nb; i++)
			interpfactor += abs(tex2D(source, coords + float2(-steps[i], 0)).r - src);
		interpfactor *= 0.35;
		
		for (i = 0; i < nb2; i++)
			interpfactor += abs(tex2D(source, coords + float2(steps[i], steps[i])).r - src);
		for (i = 0; i < nb2; i++)
			interpfactor += abs(tex2D(source, coords + float2(steps[i], -steps[i])).r - src);
		for (i = 0; i < nb2; i++)
			interpfactor += abs(tex2D(source, coords + float2(-steps[i], -steps[i])).r - src);
		for (i = 0; i < nb2; i++)
			interpfactor += abs(tex2D(source, coords + float2(-steps[i], steps[i])).r - src);
		
		
		// Effectue le mélange entre la texture de bordure du mur
		// et la texture intérieure.
		col = lerp(tex2D(wall, pos  * 5),
					tex2D(border, pos * 5),
					saturate(interpfactor));
	}
	float lightning = 0.5 + val.b/2;
	return float4(col.rgb * lightning, 1);
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}