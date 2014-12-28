using System;
using System.Collections;
using System.Collections.Generic;
using Codinsa2015.Views.Client;
using Microsoft.Xna.Framework;
namespace Codinsa2015.Client
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            /*
            using (Game1 game = new Game1())
            {
                game.Run();
            }*/
            Views.TCPHelper.Initialize(Codinsa2015.Graphics.Client.RemoteClient.__DEBUG_PORT, "127.0.0.1");
            State state = new State();
            Console.WriteLine("Client started...");

            List<Views.SpellView> spells = state.GetSpells();
            int spellId = 0;
            while (true)
            {
                var entities = state.GetEntitiesInSight();

                if(entities.Count != 0)
                {
                    var entity = entities[0];
                    if(!state.IsAutoMoving())
                    {
                        Console.WriteLine("Moving to entity " + entity.ID + ", position = " + entity.Position);
                        state.StartMoveTo(entity.Position);
                    }

                    spellId++; spellId %= spells.Count;

                    Views.SpellDescriptionView spell = state.GetSpellCurrentLevelDescription(spellId);
                    Views.EntityBaseView e = state.GetEntityById(1);
                    Vector2 position = state.GetPosition();

                    Console.WriteLine("Using spell n° " + spellId + ", targetting type = " + spell.TargetType.Type);
                    switch(spell.TargetType.Type)
                    {
                        case Views.TargettingType.Direction:
                            Vector2 dir = entity.Position - position;
                            dir.Normalize();
                            if (entity.Position == position)
                                dir = new Vector2(1, 0);

                            state.UseSpell(spellId, new Views.SpellCastTargetInfoView() { TargetDirection = dir, Type = Views.TargettingType.Direction });
                            break;

                        case Views.TargettingType.Position:
                            state.UseSpell(spellId, new Views.SpellCastTargetInfoView() { TargetPosition = entity.Position, Type = Views.TargettingType.Position });
                            break;

                        case Views.TargettingType.Targetted:
                            state.UseSpell(spellId, new Views.SpellCastTargetInfoView() { TargetId = entity.ID, Type = Views.TargettingType.Targetted });
                            break;
                    }

                }
            }
        }
    }
#endif
}

