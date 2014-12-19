using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Graphics.Server
{
    /// <summary>
    /// Représente un appel à Effect.Parameters["name"].SetValue().
    /// </summary>
    public class CommandSetEffectParameterValue : Command
    {
        /// <summary>
        /// Effet associé à cette commande.
        /// </summary>
        public RemoteEffect Effect { get; set; }
        public string ParameterName { get; set; }
        public object Value { get; set; }

        public CommandSetEffectParameterValue() { }
        public CommandSetEffectParameterValue(RemoteEffect effect, string parameterValue, object value)
        {
            Effect = effect;
            ParameterName = parameterValue;
            Value = value;
        }
    }
}
