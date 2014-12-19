using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Graphics.Server
{
    /// <summary>
    /// Représente un appel à SpriteBatch.Begin().
    /// </summary>
    public class CommandSpriteBatchBegin : Command
    {
        public RemoteSpriteBatch Batch { get; set; }
        public SpriteSortMode SortMode { get; set; }
        public BlendState BlendState { get; set; }
        public SamplerState SamplerState { get; set; }
        public RemoteEffect Effect { get; set; }
        public DepthStencilState DepthStencil { get; set; }
        public RasterizerState RasterizerState { get; set; }

        public CommandSpriteBatchBegin() { }
        public CommandSpriteBatchBegin(RemoteSpriteBatch batch, SpriteSortMode sortMode, BlendState blendState,
            SamplerState samplerState, DepthStencilState depthStencil, RasterizerState rasterizer, RemoteEffect effect)
        {
            Batch = batch;
            SortMode = sortMode;
            BlendState = blendState;
            SamplerState = samplerState;
            Effect = effect;
            DepthStencil = depthStencil;
            RasterizerState = rasterizer;
        }
    }
}
