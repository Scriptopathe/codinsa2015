using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clank.Core.Model.Language.Macros
{
    /// <summary>
    /// Représente un wrapper permettant d'appeler une fonction sur le serveur distant 
    /// depuis un client.
    /// </summary>
    public class RemoteFunctionWrapper : Instruction
    {
        /// <summary>
        /// Prototype de la fonction à wrapper.
        /// </summary>
        public Language.FunctionDeclaration Func;
        /// <summary>
        /// Id de la fonction.
        /// </summary>
        public int Id;
        /// <summary>
        /// Crée une nouvelle instance de RemoteFunctionWrapper.
        /// </summary>
        /// <param name="func"></param>
        public RemoteFunctionWrapper(Language.FunctionDeclaration func, int id)
        {
            Func = func;
            Source = "generated";
            Id = id;
            Comment = func.Comment;
        }
    }
}
