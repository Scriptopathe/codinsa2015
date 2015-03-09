using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Codinsa2015.Rendering.Particles
{
    /// <summary>
    /// Classe permettant la gestion de particules.
    /// </summary>
    public sealed class ParticleManager
    {
        #region Variables
        /// <summary>
        /// Liste des particules gérées par ce manager.
        /// </summary>
        List<Particle> m_particles;
        /// <summary>
        /// Liste des particules à supprimer.
        /// </summary>
        List<Particle> m_particlesToDelete;
        #endregion

        #region Properties
        public MapRenderer MapRdr { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Crée une nouvelle instance de ParticleManager.
        /// </summary>
        public ParticleManager(MapRenderer maprdr)
        {
            MapRdr = maprdr;
            m_particlesToDelete = new List<Particle>();
            m_particles = new List<Particle>();
        }

        /// <summary>
        /// Ajoute une particule au manager.
        /// </summary>
        /// <param name="particle"></param>
        public void Add(Particle particle)
        {
            m_particles.Add(particle);
        }
        /// <summary>
        /// Mets à jour toutes les particules gérées par ce Manager.
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            foreach (Particle particle in m_particles)
            {
                particle.Update(time);
                if (particle.IsDisposed)
                    m_particlesToDelete.Add(particle);
            }

            // Supprime les particules "mortes" (disposed).
            foreach (Particle particle in m_particlesToDelete)
            {
                m_particles.Remove(particle);
            }
        }

        /// <summary>
        /// Dessine les particules gérées par ce Manager.
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch, Vector2 viewportOffset, Vector2 scrollingOffset)
        {
            foreach (Particle particle in m_particles)
            {
                if (!particle.IsDisposed)
                    particle.Draw(batch, viewportOffset, scrollingOffset);
            }
        }
        /// <summary>
        /// Supprime la mémoire allouée par ce ParticleManager et les particules qu'il possède.
        /// </summary>
        public void Dispose()
        {
            foreach (Particle particle in m_particles)
            {
                if (!particle.IsDisposed)
                    particle.Dispose();
            }

            m_particles.Clear();
            m_particlesToDelete.Clear();
        }

        /// <summary>
        /// Remets le ParticleManager dans son état initial.
        /// </summary>
        public void Reset()
        {
            Dispose();
        }
        #endregion
    }
}