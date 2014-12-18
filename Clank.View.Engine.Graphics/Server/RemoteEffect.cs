using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.View.Engine.Graphics.Server
{
    /// <summary>
    /// Représente un paramètre d'effet.
    /// </summary>
    public class RemoteEffectParameter
    {
        public RemoteEffect Effect { get; set; }
        public string Name { get; set; }

        public RemoteEffectParameter(RemoteEffect effect, string name)
        {
            Effect = effect;
            Name = name;
        }

        public void SetValue(object o)
        {
            Effect.Server.SendCommand(new CommandSetEffectParameterValue(Effect, 
                Name,
                o));
        }
    }

    /// <summary>
    /// Représente une collection de paramètres d'effet.
    /// (utilisé pour avoir la même syntaxe que Effect pour le changement de 
    /// valeur de paramètres).
    /// </summary>
    public class RemoteEffectParameterCollection
    {
        public RemoteEffect Effect { get; set; }
        public RemoteEffectParameterCollection(RemoteEffect e)
        {
            Effect = e;
        }

        public RemoteEffectParameter this[string name]
        {
            get { return new RemoteEffectParameter(Effect, name); }
        }
    }

    /// <summary>
    /// Représente un paramètre d'effet.
    /// </summary>
    public class RemoteEffectTechnique
    {
        public RemoteEffect Effect { get; set; }
        public string Name { get; set; }

        public RemoteEffectTechnique(RemoteEffect effect, string name)
        {
            Effect = effect;
            Name = name;
        }
    }

    /// <summary>
    /// Représente une collection de paramètres d'effet.
    /// (utilisé pour avoir la même syntaxe que Effect pour le changement de 
    /// valeur de paramètres).
    /// </summary>
    public class RemoteEffectTechniqueCollection
    {
        public RemoteEffect Effect { get; set; }
        public RemoteEffectTechniqueCollection(RemoteEffect e)
        {
            Effect = e;
        }

        public RemoteEffectTechnique this[string name]
        {
            get { return new RemoteEffectTechnique(Effect, name); }
        }
    }

    /// <summary>
    /// Représente un effet distant.
    /// </summary>
    public class RemoteEffect : RemoteGraphicsObject
    {
        #region Effect Compatibility
        /// <summary>
        /// Collection fictive de paramètres d'effet.
        /// </summary>
        public RemoteEffectParameterCollection Parameters
        {
            get;
            set;
        }

        /// <summary>
        /// Collection fictive de techniques d'effet.
        /// </summary>
        public RemoteEffectTechniqueCollection Techniques
        {
            get;
            set;
        }

        /// <summary>
        /// Définit la technique courante de l'effet.
        /// </summary>
        public RemoteEffectTechnique CurrentTechnique
        {
            set
            {
                Server.SendCommand(new CommandSetEffectTechnique(
                    value.Effect, value.Name
                ));
            }
        }
        #endregion
        /// <summary>
        /// Nom de l'asset contenant l'effet.
        /// </summary>
        public string Filename { get; set; }

        public RemoteEffect(GraphicsServer server, string filename) : base(server, false) 
        {
            Parameters = new RemoteEffectParameterCollection(this);
            Filename = filename;
            Register();
        }
    }
}
