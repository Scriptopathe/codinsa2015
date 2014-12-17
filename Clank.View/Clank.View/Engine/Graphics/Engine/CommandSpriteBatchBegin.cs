using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Clank.View.Engine.Graphics.Engine
{
    /// <summary>
    /// Représente un appel à SpriteBatch.Begin().
    /// </summary>
    class CommandSpriteBatchBegin : Command
    {
        public RemoteSpriteBatch Batch { get; set; }
        public SpriteSortMode SortMode { get; set; }
        public BlendState BlendState { get; set; }
        public SamplerState SamplerState { get; set; }
        public RemoteEffect Effect { get; set; }
        public CommandSpriteBatchBegin(RemoteSpriteBatch batch, SpriteSortMode sortMode, BlendState blendState,
            SamplerState samplerState, RemoteEffect effect)
        {
            Batch = batch;
            SortMode = sortMode;
            BlendState = blendState;
            SamplerState = samplerState;
            Effect = effect;
        }
    }
}
