using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Codinsa2015.Server.Entities;
namespace Codinsa2015.Server.Equip
{
    /// <summary>
    /// Représente les différents types de consommables.
    /// </summary>
    [Clank.ViewCreator.Enum("Représente les différents types de consommables.")]
    public enum ConsummableType
    {
        Empty,
        Ward,
        Unward,
    }

    /// <summary>
    /// Représente un résultat d'utilisation de consommable.
    /// </summary>
    [Clank.ViewCreator.Enum("Représente un résultat d'utilisation de consommable.")]
    public enum ConsummableUseResult
    {
        Success,
        SuccessAndDestroyed,
        Fail,
        NotUnits,
    }
    /// <summary>
    /// Représente un modèle de consommable.
    /// </summary>
    public class ConsummableModel : EquipmentModel
    {
        /// <summary>
        /// Obtient ou définit le type du consommable.
        /// </summary>
        public ConsummableType ConsummableType { get; set; }
        /// <summary>
        /// Obtient ou définit le nombre de stacks maximum que peut stacker ce consommable.
        /// </summary>
        public int MaxStackSize { get; set; }

        /// <summary>
        /// Obtient le type de ce modèle d'équipement.
        /// </summary>
        public override EquipmentType Type
        {
            get { return EquipmentType.Consummable; }
            set { }
        }

        public ConsummableModel() { }
    }

    /// <summary>
    /// Représente une stack de consommables de même nature.
    /// </summary>
    public class ConsummableStack
    {
        #region Variables
        Consummable m_currentExecutingConsummable;
        EntityHero m_owner;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit le nombre de consommables dans la stack.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Obtient le modèle définit par ce consommable.
        /// </summary>
        public ConsummableModel Model { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Obtient une valeur indiquant si le consommable est en cours d'utilisation.
        /// </summary>
        public bool UsingStarted
        {
            get
            {
                if (m_currentExecutingConsummable == null)
                    return false;
                return m_currentExecutingConsummable.UsingStarted;
            }
        }

        /// <summary>
        /// Retourne une valeur indiquant le temps restant avant disparition du consommable.
        /// Cette valeur est maintenue à titre indicatif (pour un affichage dans l'UI).
        /// </summary>
        public float RemainingTime
        {
            get
            {
                if (m_currentExecutingConsummable == null)
                    return 0;
                return m_currentExecutingConsummable.RemainingTime;
            }
        }
        /// <summary>
        /// Crée une nouvelle instance de ConsummableStack.
        /// </summary>
        public ConsummableStack(EntityHero owner, ConsummableType type)
        {
            m_owner = owner;
            Count = 1;
            Model = GameServer.GetScene().ShopDB.GetConsummableModelByType(type);
            if (Model == null)
                throw new NotImplementedException("Modèle de consommable " + type + " non implémenté.");
        }

        /// <summary>
        /// Utilise une stack du consommable actuel.
        /// </summary>
        /// <returns></returns>
        public ConsummableUseResult Use()
        {
            if (Count <= 0)
                return ConsummableUseResult.NotUnits;
            
            if(m_currentExecutingConsummable == null)
            {
                // Crée le consommable.
                switch(Model.ConsummableType)
                {
                    case ConsummableType.Ward:
                        m_currentExecutingConsummable = new WardConsummable();
                        break;
                    case ConsummableType.Unward:
                        m_currentExecutingConsummable = new UnwardConsummable();
                        break;
                    case ConsummableType.Empty:
                        return ConsummableUseResult.NotUnits;
                    default:
                        throw new NotImplementedException();

                }
            }


            ConsummableUseResult res = m_currentExecutingConsummable.Use(m_owner);
            if (res == ConsummableUseResult.SuccessAndDestroyed)
            {
                m_currentExecutingConsummable = null;
                Count--;
            }

            return res;
        }
        /// <summary>
        /// Mets à jour cette stack de consommable.
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            if (m_currentExecutingConsummable != null)
                if (m_currentExecutingConsummable.Update(time, m_owner))
                {
                    m_currentExecutingConsummable = null;
                    Count--;
                }

            if(Count == 0)
            {
                Model = GameServer.GetScene().ShopDB.GetConsummableModelByType(ConsummableType.Empty);
            }
        }
        #endregion
    }
    /// <summary>
    /// Représente un consommable.
    /// </summary>
    public abstract class Consummable
    {
        /// <summary>
        /// Retourne une variable indiquant le temps restant avant disparition du consommable.
        /// Cette variable est maintenue à titre indicatif.
        /// </summary>
        public float RemainingTime { get; protected set; }
        /// <summary>
        /// Retourne une valeur indiquant si le consommable a été utilisé.
        /// (i.e. : son utilisation a commencé).
        /// </summary>
        public bool UsingStarted { get; set; }
        /// <summary>
        /// Utilise le consommable.
        /// Retourne true si le consommable doit être détruit.
        /// </summary>
        public abstract ConsummableUseResult Use(EntityHero owner);

        /// <summary>
        /// Mets à jour le consommable.
        /// Retourne true si le consommable doit être détruit.
        /// </summary>
        public virtual bool Update(GameTime time, EntityHero owner)
        {
            RemainingTime -= (float)time.ElapsedGameTime.TotalSeconds;
            if (RemainingTime <= 0) RemainingTime = 0;
            return false;
        }

        /// <summary>
        /// Crée une nouvelle instance de Consummable.
        /// </summary>
        public Consummable() : base()
        {

        }
    }

    /// <summary>
    /// Représente un slot de consommable vide.
    /// </summary>
    public class EmptyConsummable : Consummable
    {

        public override ConsummableUseResult Use(EntityHero owner)
        {
            return ConsummableUseResult.Success;
        }
    }
}
