sampler s : register(s0);
texture2D xWallTexture;
texture2D xBorderTexture;
texture2D xGrassTexture;
texture2D xSourceTexture;
texture2D xLavaTexture;
float xTime;
float2 xPixelSize;
float xUnitSize;
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
sampler lava = sampler_state {
	texture = (xLavaTexture);
	MinFilter = Linear; // Minification Filter
	MagFilter = Linear; // Magnification Filter
	MipFilter = Linear; // Mip-mapping
	AddressU = Wrap; // Address Mode for U Coordinates
	AddressV = Wrap; // Address Mode for V Coordinates
};
float4 PixelShaderFunctionSAVE(float2 coords: TEXCOORD0) : COLOR0
{
	float4 val = tex2D(source, coords);
	
	float src = val.r;
	float4 col;
	float2 texCoords = (coords + scrolling) * 150 / xUnitSize;
	if (src < 0.5)
	{
		col = tex2D(grass, texCoords);
		if (col.r < 0.03)
		{
			col = tex2D(lava, texCoords * 1.5 + xTime * 4) * 2;
		}
	}
	else
	{
		float2 step = xPixelSize * xUnitSize / 4;
		float interpfactor = 0.0f;

		// Gradient des bords.
		interpfactor += abs(tex2D(source, coords + float2(0, step.y)).r - src);
		interpfactor += abs(tex2D(source, coords + float2(0, -step.y)).r - src);
		interpfactor += abs(tex2D(source, coords + float2(step.x, 0)).r - src);
		interpfactor += abs(tex2D(source, coords + float2(-step.x, 0)).r - src);
		interpfactor *= 1.9;
		
		// Effectue le mélange entre la texture de bordure du mur
		// et la texture intérieure.
		float sinVal = sin(texCoords.x + xTime * 4) * 0.7;
		col = lerp(tex2D(wall, texCoords + float2(0, sinVal)) * exp(sinVal * 1.4),//float2(0, sin(texCoords.x + xTime * 4) * 0.7)),//float2(0, sin(texCoords.x*16 + xTime * 32) * 0.025)),
					tex2D(border, texCoords  + xTime),//tex2D(border, pos * 5),
					saturate(interpfactor));
	}
	float lightning = 0.5 + val.b/2;
	return float4(col.rgb * lightning, 1);
}
float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 val = tex2D(source, coords);

	float2 src = float2(val.r, val.b);
	float4 col;
	float2 texCoords = (coords + scrolling) * 150 / xUnitSize;

	// Gradient
	float2 step = xPixelSize * xUnitSize / 4;
	float2 grad = float2(0.0f, 0.0f);

	// Gradient des bords.
	grad += abs(tex2D(source, coords + float2(0, step.y)).rb - src);
	grad += abs(tex2D(source, coords + float2(0, -step.y)).rb - src);
	grad += abs(tex2D(source, coords + float2(step.x, 0)).rb - src);
	grad += abs(tex2D(source, coords + float2(-step.x, 0)).rb - src);
	grad *= 1.9;

	if (src.r < 0.5)
	{
		col = tex2D(grass, texCoords);
		if (col.r < 0.03)
		{
			col = tex2D(lava, texCoords * 1.5 + xTime * 4) * 2;
		}
	}
	else
	{

		// Effectue le mélange entre la texture de bordure du mur
		// et la texture intérieure.
		float sinVal = sin(texCoords.x + xTime * 4) * 0.7;
		col = lerp(tex2D(wall, texCoords + float2(0, sinVal)) * exp(sinVal * 1.4),//float2(0, sin(texCoords.x + xTime * 4) * 0.7)),//float2(0, sin(texCoords.x*16 + xTime * 32) * 0.025)),
			tex2D(border, texCoords + xTime),//tex2D(border, pos * 5),
			saturate(grad.r));
	}
	float lightning = lerp(0.1 + val.b / 2, 0.6, grad.g);
	return float4(col.rgb * lightning, 1);
}
technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}