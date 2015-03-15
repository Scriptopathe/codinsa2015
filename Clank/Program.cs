using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clank.Core.Generation;
using Clank.Core.Generation.Languages;
namespace Clank.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            #region New
            string s1 = @" 

        public class Array<T>
        {
            string Name()
            { 
                string CS = 'List';
                string Python = 'List';
                string Java = 'Haha';
            }

            string Item(int id)
            {
                string CS = '[$id]';
                string Python = '[$id]';
            }
        }

".Replace("'", "\"");

            string s2 = @"
main { 
    state {
        public class Patate<T>
        {
            public string Name()
            { 
                string CS = 'List';
                string Python = 'List';
                int haha = '6556';
            }

            string PATATE(int id)
            {
                string CS = '[$id]';
                string Python = '[$id]';
            }
        }
    }
    state {
        public class Hello { }
    }
}
".Replace("'", "\"");

            string script = @"

#include outside
main {
    macro 
    {
        #include myScript

		public class List<T> 
        { 
            string Name()
            {
                string Python = 'List';
                string CS = 'List';
            }
			
            public T get(int index)
            {
                string Python = '[$index]';
                string CS   = '[$index$]';
            }
        }

        public class Vector2
        {
            string Name()
            {
                string CS = 'Vector2';
                string Python = 'vec2';
            }
            
            # Obtient la position du vecteur.
            int GetX() { string CS = 'X'; string Python = 'x'; }
            int GetY() { string CS = 'Y'; string Python = 'y'; }
            
            # Définit la position du vecteur.
            int SetX(int x) { string CS = 'X = $x$'; string Python = 'x = $x$'; }
            int SetY(int y) { string CS = 'Y = $y$'; string Python = 'y = $y$'; }
        }
    }

    state
    {
        public class Ship<T>
        {
            public int Speed;
            public string Name;
            public T Position;
            public constructor Ship<T> New(T pos)
            {
                Speed = 5;
                Name = 'Spitfire';
                Position = pos;
            }
            public static Ship<T> Create()
            {
                Ship<T> bllb;   
                
            }
            public void Nothing() { }
        }

        List<Ship<Vector2>> Ships;
    }

    access
    {
        public Ship<Vector2> GetShip(int index)
        {
            int blbl = clientId;
            return state.Ships.get(index);
            
        }
        void FunctionWithErrors(int shit)
        {
            string blblb = 56;
            int carotte = 'hihi';
            return 5;
            return 6;
            int[] things;
            Ship<Vector2> shippp = Ship<Vector2>.Create();
            void voide = shippp.Nothing();
            shippp = GetShip(1);
            int haha;
            if(haha + 5) { int jeSuisUnInt = 5; }
            elsif(haha + 5) { int jeSuisUnInt = 5; }
            Ship<Vector2> shiiip = Ship<Vector2>.New(Vector2.New());
            while(haha + 5) { int jeSuisdeUnInt = 5; }
            #carotte.JeFaisPlanter();
        }
        public Vector2 GetShipPosition(int index)
        {
            return state.Ships.get(index).Position;
        }
    }

    write
    {
        public bool SetShipPosition(int index, Vector2 position)
        {
            state.Ships.get(index).Position = position;
            return true;
        }

        public bool MoveShip(int index, Vector2 dir)
        {
            Ship<Vector2> ship = state.Ships.get(index);
            ship.Position.SetX(ship.Position.GetX() + dir.GetX());
            ship.Position.SetY(ship.Position.GetX() + dir.GetX());
            
            return true;
        }
    }
}
".Replace("'", "\"");
            
            #endregion

            string generationLog;
            GenerationTarget serverTarget = new GenerationTarget("cs", "Serveur.cs");
            List<GenerationTarget> clientTargets = new List<GenerationTarget>() { new GenerationTarget("cs", "Client.cs"),
                                                            new GenerationTarget("Python", "Client.py") };
            ProjectGenerator generator = new ProjectGenerator();
            // System.IO.File.ReadAllText("samplescript.clank")
            List<OutputFile> files2 = generator.Generate("#include samplescript.clank", serverTarget, clientTargets, out generationLog);
            // Loader
            Generation.Preprocessor.MemoryIncludeLoader loader = new Generation.Preprocessor.MemoryIncludeLoader();
            loader.AddFile("myScript", s1);
            loader.AddFile("outside", s2);
            loader.AddFile("main", script);
            generator.Preprocessor.ScriptIncludeLoader = loader;

            List<OutputFile> files = generator.Generate(script, serverTarget, clientTargets, out generationLog);

            
            // TODO : 
            // - documenter changements récents. ---> TODO
            // - Surcharge des fonctions. OK
            // - Modificateurs macros : jsontype = array ! OK
            // - implémenter java OK
            // - implémenter cpp : headers / implémentation. OK
            // - implémenter python
            // - checks : si serializable :
            //  - vérifier que toutes les variables le sont  ok
            //  - serializable + pas public = warning        ok
            // - write/access : arguments de type public et serializable.
            // Remplir la todo-list !
            // TODO 2 : le retour de la vengeance :
            // - Documenter macro fonctions
            // - Enums !!!
            // - Gérer les paramètres de métadonnées pour les classes (ex : C# namespace, java / python / cpp includes).
        }
    }
}

