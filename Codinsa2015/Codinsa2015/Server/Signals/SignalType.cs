using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codinsa2015.Server.Signals
{
    [Clank.ViewCreator.Enum("Enumère la liste des différents signaux existants.")]
    public enum SignalType
    {
        ComingToPosition,
        AttackEntity,
        DefendEntity,
    }
}
