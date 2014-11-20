// Copyright (C) 2013, 2014 Alvarez Josué
//
// This code is free software; you can redistribute it and/or modify it
// under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2.1 of the License, or (at
// your option) any later version.
//
// This code is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public
// License (LICENSE.txt) for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this library; if not, write to the Free Software Foundation,
// Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// The developer's email is jUNDERSCOREalvareATetudDOOOTinsa-toulouseDOOOTfr (for valid email, replace 
// capital letters by the corresponding character)

#define RADIUS  4
#define KERNEL_SIZE (RADIUS * 2 + 1)
float weights[KERNEL_SIZE];
float2 offsets[KERNEL_SIZE];
int KernelSize = 4;

texture Texture;
sampler s : register(s0);

sampler2D TextureSampler = sampler_state
{
    texture = <Texture>;
    MipFilter = None;
    MinFilter = Point;
    MagFilter = Point;

	AddressU = Clamp; // Address Mode for U Coordinates
	AddressV = Clamp; // Address Mode for V Coordinates
};


float4 RadialBlur(float2 texCoord	: TEXCOORD0 ) : COLOR0
{
    float4 color = float4(0.0f, 0.0f, 0.0f, 1.0f);
    for (int i = 0; i < KERNEL_SIZE; i++)
	{
		if(i < KernelSize)
		{
			float4 srcColor = tex2D(s, texCoord + offsets[i]); 
			color.rgb += srcColor.rgb * (weights[i]) ;
		}
	}
    return color;
}

technique Blur
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 RadialBlur();
	}
}