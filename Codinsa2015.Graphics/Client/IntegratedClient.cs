using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace Codinsa2015.Graphics.Client
{
    /// <summary>
    /// Représente un client graphique intégré.
    /// </summary>
    public class IntegratedClient : GraphicsClient
    {
        /// <summary>
        /// Représente l'instance de GraphicsDevice utilisée pour le dessin etc...
        /// </summary>
        GraphicsDevice m_device;
        /// <summary>
        /// Représente le content manager associé à ce client graphique.
        /// </summary>
        ContentManager m_content;
        /// <summary>
        /// Représente les textures / render targets chargées par ce client.
        /// </summary>
        Dictionary<int, Texture2D> m_textures;
        /// <summary>
        /// Représente les effets chargés par ce client.
        /// </summary>
        Dictionary<int, Effect> m_effects;
        /// <summary>
        /// Représente les sprite batchs chargés par ce client.
        /// </summary>
        Dictionary<int, SpriteBatch> m_batches;
        /// <summary>
        /// Représente les polices chargées par ce client.
        /// </summary>
        Dictionary<int, SpriteFont> m_fonts;


        public IntegratedClient(GraphicsDevice device, ContentManager content)
        {
            m_device = device;
            m_content = content;
            m_textures = new Dictionary<int, Texture2D>();
            m_effects = new Dictionary<int, Effect>();
            m_batches = new Dictionary<int, SpriteBatch>();
            m_fonts = new Dictionary<int, SpriteFont>();
        }

        /// <summary>
        /// Procède à l'exécution de la commande donnée.
        /// </summary>
        public override void ProcessCommand(Server.Command command)
        {
            if (command is Server.CommandCreateObject)
            {
                Server.CommandCreateObject cobj = (Server.CommandCreateObject)command;
                if (cobj.GraphicsObject is Server.RemoteRenderTarget)
                {
                    Server.RemoteRenderTarget remoteTarget = (Server.RemoteRenderTarget)cobj.GraphicsObject;
                    RenderTarget2D target = new RenderTarget2D(m_device, remoteTarget.Width, remoteTarget.Height, false, SurfaceFormat.Color,
                        DepthFormat.Depth24, 1, remoteTarget.Usage);
                    m_textures[remoteTarget.ID] = target;
                }
                else if (cobj.GraphicsObject is Server.RemoteTexture2D)
                {
                    Server.RemoteTexture2D remoteTex = (Server.RemoteTexture2D)cobj.GraphicsObject;
                    if (remoteTex.UnderlyingTexture == null)
                        m_textures[remoteTex.ID] = m_content.Load<Texture2D>(remoteTex.Filename);
                    else
                        m_textures[remoteTex.ID] = remoteTex.UnderlyingTexture;
                }
                else if (cobj.GraphicsObject is Server.RemoteSpriteBatch)
                {
                    Server.RemoteSpriteBatch remoteBatch = (Server.RemoteSpriteBatch)cobj.GraphicsObject;
                    SpriteBatch batch = new SpriteBatch(m_device);
                    m_batches[remoteBatch.ID] = batch;
                }
                else if (cobj.GraphicsObject is Server.RemoteEffect)
                {
                    Server.RemoteEffect remoteEffect = (Server.RemoteEffect)cobj.GraphicsObject;
                    Effect e = m_content.Load<Effect>(remoteEffect.Filename);
                    m_effects[remoteEffect.ID] = e.Clone();
                }
                else if (cobj.GraphicsObject is Server.RemoteSpriteFont)
                {
                    Server.RemoteSpriteFont remoteFont = (Server.RemoteSpriteFont)cobj.GraphicsObject;
                    if (remoteFont.Font == null)
                        m_fonts[remoteFont.ID] = m_content.Load<SpriteFont>(remoteFont.Filename);
                    else
                        m_fonts[remoteFont.ID] = remoteFont.Font;
                }
            }
            else if (command is Server.CommandDisposeObject)
            {
                Server.CommandDisposeObject cobj = (Server.CommandDisposeObject)command;
                if (cobj.GraphicsObject is Server.RemoteRenderTarget)
                {
                    Server.RemoteRenderTarget remoteTarget = (Server.RemoteRenderTarget)cobj.GraphicsObject;
                    m_textures[remoteTarget.ID].Dispose();
                }
                else if (cobj.GraphicsObject is Server.RemoteTexture2D)
                {
                    Server.RemoteTexture2D remoteTex = (Server.RemoteTexture2D)cobj.GraphicsObject;
                    m_textures[remoteTex.ID].Dispose();
                }
                else if (cobj.GraphicsObject is Server.RemoteSpriteBatch)
                {
                    Server.RemoteSpriteBatch remoteBatch = (Server.RemoteSpriteBatch)cobj.GraphicsObject;
                    m_batches[remoteBatch.ID].Dispose();
                }
                else if (cobj.GraphicsObject is Server.RemoteEffect)
                {
                    Server.RemoteEffect remoteEffect = (Server.RemoteEffect)cobj.GraphicsObject;
                    m_effects[remoteEffect.ID].Dispose();
                }
            }
            else if (command is Server.CommandGraphicsDeviceClear)
            {
                Server.CommandGraphicsDeviceClear gdclear = (Server.CommandGraphicsDeviceClear)command;
                m_device.Clear(gdclear.Color);
            }
            else if (command is Server.CommandGraphicsDeviceSetRenderTarget)
            {
                Server.CommandGraphicsDeviceSetRenderTarget cmd = (Server.CommandGraphicsDeviceSetRenderTarget)command;
                if (cmd.RenderTarget == null)
                    m_device.SetRenderTarget(null);
                else
                    m_device.SetRenderTarget((RenderTarget2D)m_textures[cmd.RenderTarget.ID]);
            }
            else if (command is Server.CommandSetEffectParameterValue)
            {
                Server.CommandSetEffectParameterValue cmd = (Server.CommandSetEffectParameterValue)command;
                SetEffectParameterValue(cmd.Effect.ID, cmd.ParameterName, cmd.Value);
            }
            else if (command is Server.CommandSetEffectTechnique)
            {
                Server.CommandSetEffectTechnique cmd = (Server.CommandSetEffectTechnique)command;
                Effect e = m_effects[cmd.Effect.ID];
                e.CurrentTechnique = e.Techniques[cmd.TechniqueName];
            }
            else if (command is Server.CommandSpriteBatchBegin)
            {
                Server.CommandSpriteBatchBegin cmd = (Server.CommandSpriteBatchBegin)command;
                if (cmd.Effect == null)
                    m_batches[cmd.Batch.ID].Begin(cmd.SortMode, cmd.BlendState, cmd.SamplerState, DepthStencilState.Default, RasterizerState.CullNone);
                else
                    m_batches[cmd.Batch.ID].Begin(cmd.SortMode, cmd.BlendState, cmd.SamplerState, DepthStencilState.Default, RasterizerState.CullNone,
                        m_effects[cmd.Effect.ID]);
            }
            else if (command is Server.CommandSpriteBatchDraw)
            {
                Server.CommandSpriteBatchDraw cmd = (Server.CommandSpriteBatchDraw)command;
                Texture2D tex = m_textures[cmd.Texture.ID];
                if (cmd.DestinationRectangle.HasValue)
                {
                    if (cmd.DestinationRectangle.Value.Width < 0 || cmd.DestinationRectangle.Value.Height < 0)
                    {
                        // On ajuste la valeur du dst rect à la valeur de la texture.
                        Rectangle rect = new Rectangle(cmd.DestinationRectangle.Value.X, cmd.DestinationRectangle.Value.Y, tex.Width, tex.Height);
                        m_batches[cmd.Batch.ID].Draw(tex, rect, cmd.SourceRectangle, cmd.Color, cmd.Rotation, cmd.Origin, SpriteEffects.None, cmd.LayerDepth);
                    }
                    else
                        m_batches[cmd.Batch.ID].Draw(tex, cmd.DestinationRectangle.Value, cmd.SourceRectangle, cmd.Color, cmd.Rotation, cmd.Origin, SpriteEffects.None, cmd.LayerDepth);
                }

                else
                {
                    m_batches[cmd.Batch.ID].Draw(tex, new Rectangle(0, 0, tex.Width, tex.Height), cmd.SourceRectangle, cmd.Color, cmd.Rotation, cmd.Origin, SpriteEffects.None, cmd.LayerDepth);
                }
            }
            else if (command is Server.CommandSpriteBatchEnd)
            {
                Server.CommandSpriteBatchEnd cmd = (Server.CommandSpriteBatchEnd)command;
                m_batches[cmd.Batch.ID].End();
            }
            else if (command is Server.CommandSpriteBatchDrawString)
            {
                Server.CommandSpriteBatchDrawString cmd = (Server.CommandSpriteBatchDrawString)command;
                m_batches[cmd.Batch.ID].DrawString(m_fonts[cmd.Font.ID], cmd.String, cmd.Position, cmd.Color, cmd.Rotation, cmd.Origin, cmd.Scale, cmd.SpriteEffects, cmd.LayerDepth);
            }
            else if(command is Server.CommandEndFrame)
            {

            }
            else
                throw new NotImplementedException();
        }



        /// <summary>
        /// Appel polymorphe à effect.Parameters["name"].SetValue(o).
        /// </summary>
        void SetEffectParameterValue(int id, string name, object value)
        {
            if (value is Server.RemoteTexture)
            {
                m_effects[id].Parameters[name].SetValue((Texture2D)m_textures[((Server.RemoteTexture)value).ID]);
            }
            else if (value is Vector2)
            {
                m_effects[id].Parameters[name].SetValue((Vector2)value);
            }
            else if (value is float)
            {
                m_effects[id].Parameters[name].SetValue((float)value);
            }
            else if(value is int)
            {
                m_effects[id].Parameters[name].SetValue((int)value);
            }
            else if (value is Matrix)
            {
                m_effects[id].Parameters[name].SetValue((Matrix)value);
            }
            else
                throw new NotImplementedException();
        }
    }
}
