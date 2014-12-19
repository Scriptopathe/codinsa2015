using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Server
{
    /// <summary>
    /// Classe permettant d'effectuer des actions à des intervalles de temps particuliers.
    /// </summary>
    public class Scheduler
    {
        public delegate void ActionDelegate();
        class ExecutionUnit
        {
            public float RemainingTime;
            public ActionDelegate Action;
            public bool HasEnded { get { return RemainingTime <= 0; } }
            public void Update(GameTime time) { RemainingTime -= (float)time.ElapsedGameTime.TotalSeconds; }
            public ExecutionUnit(ActionDelegate action, float timer)
            {
                RemainingTime = timer;
                Action = action;
            }
        }
        List<ExecutionUnit> m_units;
        /// <summary>
        /// Crée une nouvelle instance de Scheduler.
        /// </summary>
        public Scheduler()
        {
            m_units = new List<ExecutionUnit>();
        }

        /// <summary>
        /// Demande au Scheduler de prévoir une action (action) au bout d'un certain
        /// nombre de secondes (timer)
        /// </summary>
        /// <param name="action">action à exécuter une fois le timer expiré</param>
        /// <param name="timer">temps avant expiration du timer (en secondes)</param>
        public void Schedule(ActionDelegate action, float timer)
        {
            m_units.Add(new ExecutionUnit(action, timer));
        }

        /// <summary>
        /// Mets à jour le Scheduler, et exécute les actions prévues.
        /// </summary>
        public void Update(GameTime time)
        {
            List<ExecutionUnit> todelete = new List<ExecutionUnit>();
            foreach(ExecutionUnit unit in m_units)
            {
                unit.RemainingTime -= (float)time.ElapsedGameTime.TotalSeconds;
                if(unit.HasEnded)
                {
                    todelete.Add(unit);
                    unit.Action();
                }
            }

            foreach (ExecutionUnit unit in todelete) { m_units.Remove(unit); }
        }
    }
}
