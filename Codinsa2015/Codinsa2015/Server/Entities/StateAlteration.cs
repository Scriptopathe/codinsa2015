using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Codinsa2015.Server.Entities
{
    /// <summary>
    /// Représente les paramètres d'une altération d'état.
    /// </summary>
    public class StateAlterationParameters
    {
        /// <summary>
        /// Position finale que le dash doit atteindre.
        /// (si le targetting est Direction)
        /// </summary>
        [Clank.ViewCreator.Export("Vector2", "Position finale que le dash doit atteindre (si le targetting est Direction)")]
        public Vector2 DashTargetDirection { get; set; }

        /// <summary>
        /// Entité vers laquelle le dash doit se diriger.
        /// (si le targetting du dash est Entity).
        /// </summary>
        [Clank.ViewCreator.Export("int", "Entité vers laquelle le dash doit se diriger (si le targetting du dash est Entity).")]
        public EntityBase DashTargetEntity { get; set; }


        /// <summary>
        /// Retourne une vue de ces paramètres.
        /// </summary>
        /// <returns></returns>
        public Views.StateAlterationParametersView ToView()
        {
            Views.StateAlterationParametersView view = new Views.StateAlterationParametersView();
            view.DashTargetDirection = new Views.Vector2(DashTargetDirection.X, DashTargetDirection.Y);
            if (DashTargetEntity != null)
                view.DashTargetEntity = DashTargetEntity.ID;
            else
                view.DashTargetEntity = -1;
            return view;
        }
    }

    [Clank.ViewCreator.Enum("Enumère les différentes sources possibles d'altération d'états.")]
    public enum StateAlterationSource
    {
        Consumable,
        Armor,
        Weapon,
        Amulet,
        Boots,
        // Altérations provenant d'une compétence activable.
        SpellActive,
        // Note : les effets SpellPassive sont supprimés à chaque frame.
        // Ils correspondent à des passifs de sorts d'un héros.
        UniquePassive,
    }

    /// <summary>
    /// Représente une altération d'état en cours.
    /// </summary>
    public class StateAlteration
    {
        public const float DURATION_INFINITY = 50000;
        /// <summary>
        /// Représente la source de l'altération d'état.
        /// </summary>
        [Clank.ViewCreator.Export("int", "Id de la source de l'altération d'état.")]
        public EntityBase Source { get; set; }

        /// <summary>
        /// Identifiant de l'altération.
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Représente le type de source de l'altération d'état.
        /// Les altérations d'état peuvent provenir de Consommables,
        /// Armes, Armures, Bottes, et Spells.
        /// </summary>
        [Clank.ViewCreator.Export("StateAlterationSource", "Représente le type de source de l'altération d'état.")]
        public StateAlterationSource SourceType { get; set; }
        /// <summary>
        /// Représente le modèle d'altération d'état appliquée sur une entité.
        /// </summary>
        [Clank.ViewCreator.Export("StateAlterationModelView", "Représente le modèle d'altération d'état appliquée sur une entité.")]
        public StateAlterationModel Model { get; set; }
        /// <summary>
        /// Représente les paramètres de l'altération d'état.
        /// </summary>
        [Clank.ViewCreator.Export("StateAlterationParametersView", "Représente les paramètres de l'altération d'état.")]
        public StateAlterationParameters Parameters { get; set; }

        /// <summary>
        /// Temps restant en secondes pour l'altération d'état.
        /// </summary>
        [Clank.ViewCreator.Export("float", "Temps restant en secondes pour l'altération d'état.")]
        public float RemainingTime { 
            get; 
            set; 
        }

        /// <summary>
        /// Retourne une valeur indiquant si l'intéraction est terminée.
        /// </summary>
        public bool HasEnded(EntityBase dstEntity, GameTime time)
        {
            if(!Model.Type.HasFlag(StateAlterationType.Dash))
            {
                return RemainingTime <= 0;
            }
            else
            {
                Vector2 dstPosition = Vector2.Zero;
                if (Model.DashDirType == DashDirectionType.TowardsEntity)
                {
                    dstPosition = Parameters.DashTargetEntity.Position;
                    return Vector2.Distance(dstPosition, dstEntity.Position) <= Model.DashSpeed * (float)(time.ElapsedGameTime.TotalSeconds);
                }
                else
                    return RemainingTime <= 0;
                
            }
        }
        /// <summary>
        /// Crée une nouvelle altération d'état à partir du modèle donné.
        /// La durée restante de l'altération d'état est déterminée à partir
        /// de la durée contenue dans le modèle d'altération d'état donné.
        /// </summary>
        public StateAlteration(string id, EntityBase source, StateAlterationModel model, StateAlterationParameters parameters, StateAlterationSource sourceType)
        {
            ID = id;
            Parameters = parameters;
            Source = source;
            Model = model;
            SourceType = sourceType;
            RemainingTime = model.GetDuration(source);
        }

        /// <summary>
        /// Mets à jour l'altération d'état (réduit la durée du temps écoulé depuis
        /// le dernier appel à Update()).
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            if(RemainingTime < DURATION_INFINITY)
                RemainingTime -= (float)time.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Fait arrêter l'intéraction de manière prématurée.
        /// </summary>
        public void EndNow()
        {
            RemainingTime = 0;
        }

        /// <summary>
        /// Retourne une vue de cette altération d'état.
        /// </summary>
        /// <returns></returns>
        public Views.StateAlterationView ToView()
        {
            Views.StateAlterationView view = new Views.StateAlterationView();
            view.Source = this.Source.ID;
            view.SourceType = (Views.StateAlterationSource)this.SourceType;
            view.RemainingTime = this.RemainingTime;
            view.Parameters = this.Parameters.ToView();
            view.Model = this.Model.ToView();
            return view;
        }

        /// <summary>
        /// Crée une copy complète de cette altération.
        /// </summary>
        /// <returns></returns>
        public StateAlteration Copy()
        {
            StateAlteration copy = new StateAlteration(this.ID,
                this.Source,
                this.Model.Copy(),
                this.Parameters,
                this.SourceType);
            copy.RemainingTime = this.RemainingTime;
            return copy;
        }
    }
}
