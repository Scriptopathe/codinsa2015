using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clank.Core.Model.Language;
namespace Clank.Core.Generation.Languages
{
    /// <summary>
    /// Représente un langage de programmation.
    /// Cette classe prend en charge la traduction du code d'un arbre abstrait 
    /// vers le langage de programmation de destination.
    /// </summary>
    [LanguageGenerator("cpp")]
    public class CppGenerator : ILanguageGenerator
    {
        public const string LANG_KEY = "cpp";

        #region Variables
        Clank.Core.Model.ProjectFile m_project;
        #endregion

        /// <summary>
        /// Crée une nouvelle instance de CSGenerator avec un fichier projet passé en paramètre.
        /// </summary>
        /// <param name="project"></param>
        public CppGenerator(Clank.Core.Model.ProjectFile project)
        {
            SetProject(project);
        }
        /// <summary>
        /// Crée une nouvelle instance de CSGenerator.
        /// </summary>
        public CppGenerator() { }
        /// <summary>
        /// Définit le projet contenant les informations nécessaires à la génération de code.
        /// </summary>
        public void SetProject(Model.ProjectFile project)
        {
            m_project = project;
        }
        /// <summary>
        /// Génère les fichiers du projet à partir de la liste des instructions.
        /// </summary>
        public List<OutputFile> GenerateProjectFiles(List<Instruction> instructions, string outputDirectory, bool isServer)
        {
            List<OutputFile> outputFiles = new List<OutputFile>();
            foreach (Model.Language.Instruction inst in instructions)
            {
                if (inst is Model.Language.ClassDeclaration)
                {
                    outputFiles.Add(new OutputFile(outputDirectory + "/" + ((Model.Language.ClassDeclaration)inst).Name + ".cpp",
                        GenerateInstruction(inst)));
                }
                else if(inst is Model.Language.EnumDeclaration)
                {

                }
                else
                {
                    m_project.Log.AddWarning("Instruction de type " + inst.GetType().ToString() + " inattendue.", inst.Line, inst.Character, inst.Source);
                }
            }
            return outputFiles;
        }
        /// <summary>
        /// Génére le code d'une classe.
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        string GenerateClass(ClassDeclaration declaration)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("#include \"../inc/" + declaration.Name + ".h\"");

            // Classe les instruction
            List<Instruction> publicInstructions = new List<Instruction>();
            List<Instruction> privateInstructions = new List<Instruction>();
            foreach(Instruction instruction in declaration.Instructions)
            {
                bool isPublic = false;
                if (instruction is VariableDeclarationInstruction)
                    isPublic = ((VariableDeclarationInstruction)instruction).IsPublic;
                else if (instruction is VariableDeclarationAndAssignmentInstruction)
                    isPublic = ((VariableDeclarationAndAssignmentInstruction)instruction).Declaration.IsPublic;
                else if (instruction is FunctionDeclaration)
                    isPublic = ((FunctionDeclaration)instruction).Func.IsPublic;
                else if (instruction is Model.Language.Macros.ProcessMessageMacro)
                    isPublic = true;
                else if (instruction is Model.Language.Macros.RemoteFunctionWrapper)
                    isPublic = true;

                if (! (instruction is VariableDeclarationInstruction) && !(instruction is VariableDeclarationAndAssignmentInstruction))
                {
                    if (isPublic)
                        publicInstructions.Add(instruction);
                    else
                        privateInstructions.Add(instruction);
                }
            }

            // Génère les instructions publiques.
            // builder.AppendLine("public: ");
            foreach (Instruction instruction in publicInstructions)
            {
                builder.Append(GenerateInstruction(instruction));
                builder.Append("\n");
            }

            builder.AppendLine(GenerateSerializer(declaration));
            builder.AppendLine(GenerateDeserializer(declaration));
            builder.AppendLine(GenerateConstructor(declaration));
            // Génère les instructions privées.
            //if(privateInstructions.Count > 0)
            //    builder.AppendLine("private: ");

            foreach (Instruction instruction in privateInstructions)
            {
                builder.Append(GenerateInstruction(instruction));
                builder.Append("\n");
            }

//            builder.Append("};");
            return builder.ToString();
        }

        /// <summary>
        /// Génère un constructeur initialisant tous les champs bien comme il faut.
        /// </summary>
        /// <param name="decl"></param>
        /// <returns></returns>
        public string GenerateConstructor(ClassDeclaration decl)
        {
            StringBuilder b = new StringBuilder();
            b.AppendLine(decl.Name + "::" + decl.Name + "() {");
            
            /*var varDecls = decl.Instructions.Where(inst => inst is VariableDeclarationInstruction).ToList();

            foreach (var vdecl in varDecls)
            {
                var d = vdecl as VariableDeclarationInstruction;
                if (d.Var.Type.BaseType.JType == JSONType.Array || d.Var.Type.BaseType.JType == JSONType.Object)
                {
                    b.AppendLine("\t" + d.Var.Name + " = " + GenerateTypeInstanceName(d.Var.Type) + "();");
                }
            }*/

            b.AppendLine("}");
            return b.ToString();
        }
        #region Deserialization
        /// <summary>
        /// Génère une fonction de désérialisation pour la classe donnée.
        /// </summary>
        /// <param name="decl"></param>
        /// <returns></returns>
        public string GenerateDeserializer(ClassDeclaration decl)
        {
            StringBuilder sb = new StringBuilder();
            ClankType type = m_project.Types.Types[decl.Name];
            string typename = GenerateTypeName(type);
            string objName = "_obj";
            sb.AppendLine(typename + " " + typename + "::deserialize(std::istream& input) {");
            sb.AppendLine("\t" + typename + " " + objName + " = " + typename + "();");
            foreach (var inst in decl.Instructions)
            {
                Variable attr = null;
                if (inst is VariableDeclarationInstruction)
                {
                    VariableDeclarationInstruction vardecl = (VariableDeclarationInstruction)inst;
                    attr = vardecl.Var;
                }
                else if (inst is VariableDeclarationAndAssignmentInstruction)
                {
                    VariableDeclarationAndAssignmentInstruction vardecl = (VariableDeclarationAndAssignmentInstruction)inst;
                    attr = (Variable)vardecl.Declaration.Var;
                }
                else
                    continue;

                sb.AppendLine("\t// " + attr.Name);
                sb.AppendLine(Tools.StringUtils.Indent(GenerateDeserializationInstruction(attr, objName + "_" + attr.Name)));
                sb.AppendLine("\t" + objName + "." + attr.Name + " = (" + GenerateTypeInstanceNamePrefixed(attr.Type) + ")" + objName + "_" + attr.Name + ";");
            }
            sb.AppendLine("\treturn " + objName + ";");
            sb.AppendLine("}");
            return sb.ToString();
        }
        /// <summary>
        /// Génére l'instruction de désérialisation de la variable donnée, et place le résultat
        /// dans la variable dstVarName.
        /// </summary>
        public string GenerateDeserializationInstruction(Variable variable, string dstVarName)
        {
            string typename = GenerateTypeInstanceName(variable.Type);
            switch (variable.Type.BaseType.JType)
            {
                case JSONType.Bool:
                    
                    return "bool " + dstVarName + "; input >> " + dstVarName + "; input.ignore(1000, '\\n');";
                case JSONType.Int:
                    if(variable.Type.BaseType.IsEnum)
                    {
                        string str = "int " + dstVarName + "_asInt" + "; input >> " + dstVarName + "_asInt; input.ignore(1000, '\\n');\n";
                        str += typename + " " + dstVarName + " = (" + typename + ")" + dstVarName + "_asInt;";
                        return str;
                    }
                    else
                    {
                        return "int " + dstVarName + "; input >> " + dstVarName + "; input.ignore(1000, '\\n');";

                    }
                case JSONType.Float:
                    return "float " + dstVarName + "; input >> " + dstVarName + "; input.ignore(1000, '\\n');";
                case JSONType.String:
                    return "std::string " + dstVarName + "; getline(input, " + dstVarName +");";
                case JSONType.Object:
                    return typename + " " + dstVarName + " = " + typename + "::deserialize(input);";
                case JSONType.Array:
                    StringBuilder builder = new StringBuilder();
                    var elementType = variable.Type.BaseType.JArrayElementType.AsInstance().Instanciate(variable.Type.GenericArguments);
                    string elementTypename = GenerateTypeInstanceName(elementType);
                    string dstVarNameMod = dstVarName.Replace("[", "").Replace("]", "");
                    string itName = dstVarNameMod + "_i";
                    string elementVarName = dstVarNameMod + "_e";
                    string elementCountName = dstVarNameMod + "_count";

                    string parenthesis = typename.EndsWith("]") ? "" : "()";
                    builder.AppendLine(typename + " " + dstVarName + " = " + typename + parenthesis + ";");
                    builder.AppendLine(GenerateDeserializationInstruction(new Variable()
                    {
                        Type = m_project.Types.TypeInstances["int"],
                        Name = ""
                    }, elementCountName));
                    builder.AppendLine("for(int " + itName + " = 0; " + itName + " < " + elementCountName + "; " + itName + "++) {");

                    builder.AppendLine(Tools.StringUtils.Indent(GenerateDeserializationInstruction(new Variable()
                    {
                        Name = "",
                        Type = elementType
                    }, elementVarName)));
                    builder.AppendLine("\t" + dstVarName + ".push_back((" + elementTypename + ")" + elementVarName + ");");
                    builder.AppendLine("}");
                    return builder.ToString();
            }

            throw new NotImplementedException();
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Génère une fonction de sérialisation pour la classe donnée.
        /// </summary>
        /// <param name="decl"></param>
        /// <returns></returns>
        public string GenerateSerializer(ClassDeclaration decl)
        {
            StringBuilder sb = new StringBuilder();
            ClankType type = m_project.Types.Types[decl.Name];
            string typename = GenerateTypeName(type);
            sb.AppendLine("void " + typename + "::serialize(std::ostream& output) {");
            foreach (var inst in decl.Instructions)
            {
                Variable attr = null;
                if (inst is VariableDeclarationInstruction)
                {
                    VariableDeclarationInstruction vardecl = (VariableDeclarationInstruction)inst;
                    attr = vardecl.Var;
                }
                else if (inst is VariableDeclarationAndAssignmentInstruction)
                {
                    VariableDeclarationAndAssignmentInstruction vardecl = (VariableDeclarationAndAssignmentInstruction)inst;
                    attr = (Variable)vardecl.Declaration.Var;
                }
                else
                    continue;

                sb.AppendLine("\t// " + attr.Name);
                sb.AppendLine(Tools.StringUtils.Indent(GenerateSerializationInstruction(
                    new Variable() { Type = attr.Type, Name = "this->" + attr.Name })));
            }
            sb.AppendLine("}");
            return sb.ToString();
        }
        /// <summary>
        /// Génère une instruction de sérialisation de la variable donnée.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public string GenerateSerializationInstruction(Variable srcVar)
        {
            string typename = GenerateTypeInstanceName(srcVar.Type);
            switch (srcVar.Type.BaseType.JType)
            {
                case JSONType.Bool:
                    return "output << (" + srcVar.Name + " ? 1 : 0) << '\\n';";
                case JSONType.Int:
                    return "output << ((int)" + srcVar.Name + ") << '\\n';";
                case JSONType.Float:
                    return "output << ((float)" + srcVar.Name + ") << '\\n';";
                case JSONType.String:
                    return "output << " + srcVar.Name + " << '\\n';";
                case JSONType.Object:
                    return srcVar.Name + ".serialize(output);";
                case JSONType.Array:
                    StringBuilder builder = new StringBuilder();
                    string itname = (srcVar.Name + "_it").Replace("this->", "").Replace("[", "").Replace("]", "");
                    Variable item = new Variable()
                    {
                        Name = srcVar.Name + "[" + itname + "]",
                        Type = srcVar.Type.BaseType.JArrayElementType.AsInstance().Instanciate(srcVar.Type.GenericArguments)
                    };
                    builder.AppendLine("output << " + srcVar.Name + ".size() << '\\n';");
                    builder.AppendLine("for(int " + itname + " = 0; " + itname + " < " + srcVar.Name + ".size(); " + itname + "++) {");
                    builder.AppendLine(Tools.StringUtils.Indent(GenerateSerializationInstruction(item)));
                    builder.AppendLine("}");
                    return builder.ToString();
            }

            throw new NotImplementedException();
        }
        #endregion
        /// <summary>
        /// Génère le code d'une instruction.
        /// </summary>
        public string GenerateInstruction(Instruction instruction)
        {
            if (m_project == null)
                throw new InvalidOperationException("GenerateInstruction cannot be called if SetProject() has not been called before.");

            string comment = "";
            // Génération du commentaire
            if(instruction.Comment != null)
            {
                comment = GenerateComment(instruction);
            }

            // Génération du corps de l'instruction.
            if(instruction is FunctionDeclaration)
            {
                return comment + GenerateFunctionDeclarationInstruction((FunctionDeclaration)instruction);
            }
            else if(instruction is ConstructorDeclaration)
            {
                return comment + GenerateConstructorDeclarationInstruction((ConstructorDeclaration)instruction);
            }
            else if(instruction is ClassDeclaration)
            {
                return comment + GenerateClass((ClassDeclaration)instruction);
            }
            else if(instruction is AffectationInstruction)
            {
                return comment + GenerateAffectationInstruction((AffectationInstruction)instruction);
            }
            else if(instruction is VariableDeclarationInstruction)
            {
                return comment + GenerateDeclarationInstruction((VariableDeclarationInstruction)instruction);
            }
            else if(instruction is VariableDeclarationAndAssignmentInstruction)
            {
                return comment + GenerateDeclarationAndAssignmentInstruction((VariableDeclarationAndAssignmentInstruction)instruction);
            }
            else if(instruction is FunctionCallInstruction)
            {
                return comment + GenerateFunctionCall(((FunctionCallInstruction)instruction).Call) + ";";
            }
            else if(instruction is EnumDeclaration)
            {
                return comment + GenerateEnumInstruction((EnumDeclaration)instruction);
            }
            else if(instruction is Model.Language.Macros.ProcessMessageMacro)
            {
                return comment + GenerateProcessMessageMacro((Model.Language.Macros.ProcessMessageMacro)instruction);
            }
            else if(instruction is ReturnInstruction)
            {
                return comment + GenerateReturnInstruction((ReturnInstruction)instruction);
            }
            else if(instruction is Model.Language.Macros.RemoteFunctionWrapper)
            {
                return comment + GenerateRemoteFunctionWrapper((Model.Language.Macros.RemoteFunctionWrapper)instruction);
            }
            else if(instruction is ConditionalStatement)
            {
                return comment + GenerateConditionalStatement((ConditionalStatement)instruction);
            }
            else if(instruction is PlaceholderInstruction)
            {
                return comment;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Génère le code pour des statements conditionnels.
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        string GenerateConditionalStatement(ConditionalStatement statement)
        {
            StringBuilder builder = new StringBuilder();
            switch(statement.StatementType)
            {
                case ConditionalStatement.Type.If:
                    builder.Append("if");
                    break;
                case ConditionalStatement.Type.Elsif:
                    builder.Append("else if");
                    break;
                case ConditionalStatement.Type.Else:
                    builder.Append("else");
                    break;
                case ConditionalStatement.Type.While:
                    builder.Append("while");
                    break;
                default:
                    throw new NotImplementedException();
            }

            // Ajout de la condition
            if(statement.StatementType != ConditionalStatement.Type.Else)
            {
                builder.Append("(");
                builder.Append(GenerateEvaluable(statement.Condition));
                builder.Append(")");
            }

            // Ajout du code
            builder.Append("\r\n{\r\n");
            foreach(Instruction inst in statement.Code)
            {
                builder.Append(Tools.StringUtils.Indent(GenerateInstruction(inst)));
            }
            builder.Append("\r\n}\r\n");

            return builder.ToString();
        }
        /// <summary>
        /// Génère le code pour un wrapper de fonction à distance (pour les clients).
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        string GenerateRemoteFunctionWrapper(Model.Language.Macros.RemoteFunctionWrapper instruction)
        {
            Function func = instruction.Func.Func;
            StringBuilder builder = new StringBuilder();
            string returnTypeName = GenerateTypeInstanceName(func.ReturnType);
            builder.Append(returnTypeName + " State::" + func.Name + "(");

            // Arg list
            foreach (var arg in func.Arguments)
            {
                builder.Append(GenerateTypeInstanceName(arg.ArgType) + " " + arg.ArgName + (arg == func.Arguments.Last() ? "" : ","));
            }
            builder.Append(")\r\n{\r\n");
            /*
            if (PRINT_DEBUG)
            {
                builder.AppendLine("\tConsole.WriteLine(\"[" + instruction.Func.Func.Name + "]\");");
            }*/
            builder.AppendLine("\tstd::ostringstream output = std::ostringstream();");
            // Sérialise le numéro de la fonction.
            builder.AppendLine(Tools.StringUtils.Indent(GenerateSerializationInstruction(new Variable()
            {
                Name = instruction.Id.ToString(),
                Type = m_project.Types.TypeInstances["int"]
            })));

            // Sérialise les arguments.
            for (int i = 0; i < func.Arguments.Count; i++)
            {
                builder.AppendLine(Tools.StringUtils.Indent(GenerateSerializationInstruction(new Variable()
                {
                    Name = func.Arguments[i].ArgName,
                    Type = func.Arguments[i].ArgType
                })));
            }
            builder.AppendLine("\toutput.flush();");

            builder.AppendLine("\tTCPHelper::tcpsend(output);");
            // Récupère la réponse du serveur.
            builder.AppendLine("\tstd::istringstream input;");
            builder.AppendLine("\tTCPHelper::tcpreceive(input);");

            // Récupère l'objet dans le stream.
            builder.AppendLine(Tools.StringUtils.Indent(GenerateDeserializationInstruction(
            new Variable()
            {
                Type = func.ReturnType
            }, "returnValue")));

            builder.AppendLine("\treturn (" + GenerateTypeInstanceName(func.ReturnType) + ")returnValue;");
            builder.AppendLine("}\r\n\r\n");

            return builder.ToString();
        }
        /// <summary>
        /// Génère le commentaire attaché à l'instruction donné.
        /// </summary>
        string GenerateComment(Instruction inst)
        {
            StringBuilder builder = new StringBuilder();
            string[] lines = Tools.StringUtils.CutInLines(inst.Comment, 80);

            // Génère un commentaire approprié.
            if (inst is FunctionDeclaration || inst is ClassDeclaration || inst is Model.Language.Macros.ProcessMessageMacro)
            {
                builder.AppendLine("/** ");
                foreach (string line in lines)
                {
                    builder.AppendLine(" * " + line);
                }
                builder.AppendLine(" */");
            }
            else
            {
                // Commentaire en plusieurs lignes.
                foreach (string line in lines)
                {
                    builder.AppendLine("// " + line);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Génère le code pour une instruction return.
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        string GenerateReturnInstruction(ReturnInstruction instruction)
        {
            return "return " + GenerateEvaluable(instruction.Value) + ";";
        }

        /// <summary>
        /// Génère le code pour la fonction de traitement des messages.
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        string GenerateProcessMessageMacro(Model.Language.Macros.ProcessMessageMacro instruction)
        {
            /*
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("string State::processRequest(string request, int " + Clank.Core.Model.Language.SemanticConstants.ClientID + ")\r\n{");
            builder.AppendLine("\tReader reader = Reader();");
            builder.AppendLine("\tFastWriter writer = FastWriter();");
            builder.AppendLine("\tValue response = Value(arrayValue);");
            builder.AppendLine("\tValue query = Value(arrayValue);");
            builder.AppendLine("\treader.parse(request, query);");
            builder.AppendLine("\tValue functionArgs = *(query.begin()+1);");
            // Récupère l'id
            builder.AppendLine("\tint functionId = (*query.begin()).asInt();");
            builder.AppendLine("\tswitch functionId\r\n\t{\r\n");
            int id = 0;
            List<FunctionDeclaration> decls = new List<FunctionDeclaration>();
            decls.AddRange(instruction.Access.Declarations);
            decls.AddRange(instruction.Write.Declarations);

            // Switch dans lequel on traite les différents messages.
            foreach(FunctionDeclaration func in decls)
            {
                StringBuilder argList = new StringBuilder();
                argList.Append("(");
                builder.AppendLine("\t\tcase " + id + ":");
                int argId = 0;
                foreach(FunctionArgument arg in func.Func.Arguments)
                {
                    // Variable name and type.
                    string varName = "arg" + id + "_" + argId;
                    string typeName = GenerateTypeInstanceName(arg.ArgType);
                    Variable v = new Variable() 
                    {
                        Name = "(functionArgs.begin() + " + argId + ")",
                        Type = arg.ArgType
                    };

                    // Petit commentaire.
                    builder.AppendLine("\t\t\t// Argument " + argId + "\r\n");

                    // On désérialise la variable.
                    builder.AppendLine(Tools.StringUtils.Indent(
                        GenerateDeserializerInstruction(v, varName), 3
                    ));

                    argList.Append(varName + ", ");
                    argId++;
                }
                argList.Append(Model.Language.SemanticConstants.ClientID + ")");

                // Function call and return
  
                // Sérialise la réponse dans un array JSON.
                builder.AppendLine(Tools.StringUtils.Indent(GenerateSerializerInstruction(
                    new Variable()
                    {
                        Name = func.Func.Name + argList,
                        Type = func.Func.ReturnType
                    },
                    "response_" + id
                    ), 3));


                builder.AppendLine("\t\t\treturn writer.write(root0);");
                
                id++;
            }

            builder.Append("\t}\r\n");
            builder.Append("\treturn \"\";\r\n}\r\n");

            return builder.ToString();*/
            return "";
        }

        #region Expressions
        /// <summary>
        /// Génère une expression évaluable.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        string GenerateEvaluable(Evaluable expr)
        {
            if(expr is BinaryExpressionGroup)
            {
                return GenerateBinaryExpressionGroup((BinaryExpressionGroup)expr);
            }
            else if(expr is UnaryExpressionGroup)
            {
                return GenerateUnaryExpressionGroup((UnaryExpressionGroup)expr);
            }
            else if(expr is VariableAccess)
            {
                return GenerateVariableAccess((VariableAccess)expr);
            }
            else if(expr is FunctionCall)
            {
                return GenerateFunctionCall((FunctionCall)expr);
            }
            else if(expr is Variable)
            {
                return GenerateVariable((Variable)expr);
            }
            else if(expr is BoolLiteral)
            {
                return ((BoolLiteral)expr).Value.ToString().ToLower();
            }
            else if(expr is StringLiteral)
            {
                return "\"" + ((StringLiteral)expr).Value + "\"";
            }
            else if(expr is FloatLiteral)
            {
                return ((FloatLiteral)expr).Value.ToString();
            }
            else if(expr is IntegerLiteral)
            {
                return ((IntegerLiteral)expr).Value.ToString();
            }
            else if(expr is EnumAccess)
            {
                return GenerateEnumAccess((EnumAccess)expr);
            }
            else if(expr is Typename)
            {
                return GenerateTypeInstanceName(((Typename)expr).Name);
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Génère le code représentant une référence vers l'instance de type
        /// passé en paramètre, préfixé d'un namespace si besoin.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GenerateTypeInstanceNamePrefixed(ClankTypeInstance type)
        {
            if(type.BaseType.IsEnum || !type.BaseType.IsBuiltIn)
            {
                return "::" + GenerateTypeInstanceName(type);
            }
            return GenerateTypeInstanceName(type);
        }
        /// <summary>
        /// Génère le code représentant l'instance de type passé en paramètre.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GenerateTypeInstanceName(ClankTypeInstance type)
        {
            if (type.IsGeneric || type.IsArray)
            {
                StringBuilder b = new StringBuilder();
                foreach (ClankTypeInstance tArg in type.GenericArguments)
                {
                    b.Append(GenerateTypeInstanceName(tArg));
                    if (tArg != type.GenericArguments.Last())
                        b.Append(",");
                }

                if(!type.BaseType.IsMacro)
                    return GenerateTypeName(type.BaseType) + "<" + b.ToString() + ">";
                else
                {
                    // Si on a un type macro on va remplacer :
                    // $(T) => type concerné
                    string nativeFuncName = GenerateTypeName(type.BaseType);

                    // Remplace les params génériques par leurs valeurs.
                    for (int i = 0; i < type.BaseType.GenericArgumentNames.Count; i++)
                    {
                        nativeFuncName = nativeFuncName.Replace(SemanticConstants.ReplaceChr + "(" + type.BaseType.GenericArgumentNames[i] + ")",
                            GenerateTypeInstanceName(type.GenericArguments[i]));
                    }

                    return nativeFuncName;
                }
            }

            return GenerateTypeName(type.BaseType);
        }

        /// <summary>
        /// Génère le code représentant le type passé en paramètre.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GenerateTypeName(ClankType type)
        {
            if(type.IsBuiltIn)
            {
                if(type.JType == JSONType.String)
                {
                    return "std::string";
                }
            }
            if (type.IsMacro)
            {

                // Pour les types macro, on remplace le nom du type par le nom du type natif.
                Model.MacroContainer.MacroClass mcClass = m_project.Macros.FindClassByType(type);
                if (!mcClass.LanguageToTypeName.ContainsKey(LANG_KEY))
                    throw new InvalidOperationException("Le nom de la macro classe '" + type.GetFullNameAndGenericArgs() + "' n'est pas renseigné pour le langage '" + LANG_KEY + "'");

                string nativeClassName = mcClass.LanguageToTypeName[LANG_KEY];

                return mcClass.LanguageToTypeName[LANG_KEY];
            }
            else
                if (type.Name.ToLower().Contains("string"))
                    return "std::string";
                else
                    return type.Name;
        }
        /// <summary>
        /// Génère le code d'un accès à un membre d'une énumération.
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        string GenerateEnumAccess(EnumAccess access)
        {
            return access.Type.BaseType.GetFullName() + "." + access.Name;
        }
        /// <summary>
        /// Génère le code d'un appel de fonction.
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        string GenerateFunctionCall(FunctionCall call)
        {
            if (call.Func.IsMacro)
                return GenerateMacroFunctionCall(call);
            if (call.Func.IsConstructor)
                return GenerateConstructorFunctionCall(call);
            string dot = call.Func.IsStatic ? "::" : ".";
            StringBuilder builder = new StringBuilder();
            if (call.Src != null)
                builder.Append(GenerateEvaluable(call.Src) + dot);
            else if (call.IsThisClassFunction)
                builder.Append("this->");

            builder.Append(call.Func.Name);
            builder.Append("(");

            foreach(Evaluable arg in call.Arguments)
            {
                builder.Append(GenerateEvaluable(arg));
                if (arg != call.Arguments.Last())
                    builder.Append(",");
            }

            builder.Append(")");
            return builder.ToString();
        }
        /// <summary>
        /// Génère le code d'un appel de "macro" fonction.
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        string GenerateConstructorFunctionCall(FunctionCall call)
        {
            StringBuilder builder = new StringBuilder();
            // Type name
            builder.Append(GenerateTypeInstanceName(call.Func.ReturnType));

            // Arguments
            builder.Append("(");

            foreach (Evaluable arg in call.Arguments)
            {
                builder.Append(GenerateEvaluable(arg));
                if (arg != call.Arguments.Last())
                    builder.Append(",");
            }

            builder.Append(")");

            return builder.ToString();
        }
        /// <summary>
        /// Génère le code d'un appel de "macro" fonction.
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        string GenerateMacroFunctionCall(FunctionCall call)
        {
            StringBuilder builder = new StringBuilder();

            Clank.Core.Model.MacroContainer macros = m_project.Macros;
            ClankTypeInstance owner;
            
            if (call.IsConstructor)
                owner = call.Func.ReturnType;
            else
                owner = call.Src.Type;

            Model.MacroContainer.MacroClass klass = macros.FindClassByType(owner.BaseType);
            Clank.Core.Model.MacroContainer.MacroFunction func = klass.Functions[call.Func.GetFullName()]; // TODO : GetFullName() ??

            // Nom de la fonctin native dont les paramètres sont entourés par des $
            if (!func.LanguageToFunctionName.ContainsKey(LANG_KEY))
                throw new Exception("La macro-fonction '" + call.Func.GetFullName() + "' n'est pas renseignée pour le langage '" + LANG_KEY + "'.");
            string nativeFuncName = func.LanguageToFunctionName[LANG_KEY];


            // Remplace les paramètres par leurs values.
            for(int i = 0; i < call.Func.Arguments.Count; i++)
            {
                nativeFuncName = nativeFuncName.Replace(SemanticConstants.ReplaceChr + "(" + call.Func.Arguments[i].ArgName + ")",
                    GenerateEvaluable(call.Arguments[i]));
            }


            // Remplace les params génériques par leurs valeurs.
            for(int i = 0; i < call.Func.Owner.GenericArgumentNames.Count; i++)
            {
                nativeFuncName = nativeFuncName.Replace(SemanticConstants.ReplaceChr + "(" + call.Func.Owner.GenericArgumentNames[i] + ")",
                    GenerateTypeInstanceName(owner.GenericArguments[i]));
            }

            // Remplace @self par le nom de la variable.
            nativeFuncName = nativeFuncName.Replace(SemanticConstants.SelfKW, GenerateEvaluable(call.Src));

            builder.Append(nativeFuncName);
            return builder.ToString().Replace(".[", "[");
        }
        /// <summary>
        /// Génère le code d'un accès à une variable.
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        string GenerateVariableAccess(VariableAccess access)
        {
            if(access.Left.Type.GetFullName() == Core.Model.Language.SemanticConstants.StateClass)
            {
                return "this->" + access.VariableName;
            }
            string left = GenerateEvaluable(access.Left);
            if (left == "this")
                return left + "->" + access.VariableName;
            else
                return left + "." + access.VariableName;
        }
        /// <summary>
        /// Génère le code d'une variable.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        string GenerateVariable(Variable variable)
        {
            return variable.Name;
        }
        /// <summary>
        /// Génère le code d'un opérateur.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        string GenerateOperator(Operator op)
        {
            switch(op)
            {
                case Operator.Add: return "+";
                case Operator.Affectation: return "=";
                case Operator.And: return "&";
                case Operator.Div: return "/";
                case Operator.Dot: return ".";
                case Operator.Equals: return "==";
                case Operator.LazyAnd: return "&&";
                case Operator.LazyOr: return "||";
                case Operator.Minus: return "-";
                case Operator.Mod: return "%";
                case Operator.Mult: return "*";
                case Operator.New: return "new ";
                case Operator.Not: return "!";
                case Operator.NotEquals: return "!=";
                case Operator.Or: return "|";
                case Operator.Xor: return "^";
            }

            throw new NotImplementedException();
        }
        /// <summary>
        /// Génère une expression de groupe unaire.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        string GenerateUnaryExpressionGroup(UnaryExpressionGroup expr)
        {
            return GenerateOperator(expr.Operator) + " " + GenerateEvaluable(expr.Operand);
        }
        /// <summary>
        /// Génère une expression de groupe binaire.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        string GenerateBinaryExpressionGroup(BinaryExpressionGroup expr, bool parenthesis=true)
        {
            if(parenthesis)
                return "(" + GenerateEvaluable(expr.Operand1) + " " + GenerateOperator(expr.Operator) + " " + GenerateEvaluable(expr.Operand2) + ")";
            else
                return GenerateEvaluable(expr.Operand1) + " " + GenerateOperator(expr.Operator) + " " + GenerateEvaluable(expr.Operand2);
        }
        #endregion

        #region Instructions
        /// <summary>
        /// Génère le code d'une enum.
        /// </summary>
        /// <param name="decl"></param>
        /// <returns></returns>
        string GenerateEnumInstruction(EnumDeclaration decl)
        {
            throw new NotImplementedException();
            StringBuilder builder = new StringBuilder();
            builder.Append("enum " + decl.Name + "\n{\n");
            foreach (var kvp in decl.Members)
            {
                string member = kvp.Key;
                int value = kvp.Value;
                builder.Append(Tools.StringUtils.Indent(member, 1));
                builder.Append(" = " + value.ToString());
                if (kvp.Key != decl.Members.Last().Key)
                    builder.Append(',');
                builder.Append("\n");
            }
            builder.Append("}\n");
            return builder.ToString();
        }
        /// <summary>
        /// Génère une instruction d'affectation.
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        string GenerateAffectationInstruction(AffectationInstruction instruction)
        {
            return GenerateBinaryExpressionGroup(instruction.Expression, false) + ";";
        }
        /// <summary>
        /// Génère le code de déclaration d'une instruction.
        /// </summary>
        /// <returns></returns>
        string GenerateDeclarationInstruction(VariableDeclarationInstruction instruction)
        {
            return GenerateTypeInstanceName(instruction.Var.Type) + " " + instruction.Var.Name + ";";
        }

        /// <summary>
        /// Génère le code d'une déclaration + affectation d'une variable.
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        string GenerateDeclarationAndAssignmentInstruction(VariableDeclarationAndAssignmentInstruction instruction)
        {
            return GenerateTypeInstanceName(instruction.Declaration.Var.Type) + " " + GenerateAffectationInstruction(instruction.Assignment);
        }
        /// <summary>
        /// Génère une instruction de déclaration de fonction.
        /// </summary>
        /// <returns></returns>
        string GenerateFunctionDeclarationInstruction(FunctionDeclaration decl)
        {
            StringBuilder builder = new StringBuilder();



            if (decl.Func.IsConstructor)
            {
                // Constructeur

                if (decl.Func.Owner != null)
                    builder.Append(GenerateTypeName(decl.Func.Owner) + "::");
                builder.Append(GenerateTypeName(decl.Func.ReturnType.BaseType));
                
            }
            else
            {
                // Fonction classique
                builder.Append(GenerateTypeInstanceName(decl.Func.ReturnType) + " ");
                if (decl.Func.Owner != null)
                    builder.Append(GenerateTypeName(decl.Func.Owner) + "::");
                builder.Append(decl.Func.Name);
            }
            
            builder.Append("(");

            // Ajout des arguments.
            foreach(FunctionArgument arg in decl.Func.Arguments)
            {
                bool hasRef = arg.ArgType.BaseType.JType == JSONType.Object ||
                                arg.ArgType.BaseType.JType == JSONType.Array;
                builder.Append(GenerateTypeInstanceName(arg.ArgType) + (hasRef ? "& " : " ") + arg.ArgName);
                if (arg != decl.Func.Arguments.Last())
                    builder.Append(", ");
            }
            builder.Append(")\n{\n");

            // Ajout des instructions.
            foreach(Instruction instruction in decl.Code)
            {
                builder.Append(Tools.StringUtils.Indent(GenerateInstruction(instruction), 1));
                builder.Append("\n");
            }

            builder.Append("}");
            return builder.ToString();
        }

        /// <summary>
        /// Génère une instruction de déclaration de constructeur.
        /// </summary>
        /// <returns></returns>
        string GenerateConstructorDeclarationInstruction(ConstructorDeclaration decl)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(decl.Func.Owner.GetFullName() + " ");
            builder.Append("(");

            // Ajout des arguments.
            foreach (FunctionArgument arg in decl.Func.Arguments)
            {
                builder.Append(GenerateTypeInstanceName(arg.ArgType) + " " + arg.ArgName);
                if (arg != decl.Func.Arguments.Last())
                    builder.Append(", ");
            }
            builder.Append(")\n{\n");

            // Ajout des instructions.
            foreach (Instruction instruction in decl.Func.Code)
            {
                builder.Append(Tools.StringUtils.Indent(GenerateInstruction(instruction), 1));
                builder.Append("\n");
            }

            builder.Append("}");
            return builder.ToString();
        }
        #endregion
    }
}
