using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Graphics.Engine
{
    /// <summary>
    /// Représente un appel à SpriteBatch.End().
    /// </summary>
    public class CommandSpriteBatchEnd : Command
    {
        public RemoteSpriteBatch Batch { get; set; }
        public CommandSpriteBatchEnd(RemoteSpriteBatch batch)
        {
            Batch = batch;
        }
    }
}
