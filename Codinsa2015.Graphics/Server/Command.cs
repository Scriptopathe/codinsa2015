using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace Codinsa2015.Graphics.Server
{
    /// <summary>
    /// Représente une commande addressée au serveur graphique.
    /// </summary>
    [XmlInclude(typeof(CommandCreateObject))]
    [XmlInclude(typeof(CommandDisposeObject))]
    [XmlInclude(typeof(CommandEndFrame))]
    [XmlInclude(typeof(CommandGraphicsDeviceClear))]
    [XmlInclude(typeof(CommandGraphicsDeviceSetRenderTarget))]
    [XmlInclude(typeof(CommandSetEffectParameterValue))]
    [XmlInclude(typeof(CommandSetEffectTechnique))]
    [XmlInclude(typeof(CommandSpriteBatchBegin))]
    [XmlInclude(typeof(CommandSpriteBatchDraw))]
    [XmlInclude(typeof(CommandSpriteBatchDrawString))]
    [XmlInclude(typeof(CommandSpriteBatchEnd))]
    public class Command
    {
        
    }
}
