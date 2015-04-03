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
    [LanguageGenerator("java")]
    public class JavaGenerator : ILanguageGenerator
    {
        public const string LANG_KEY = "java";
        public const bool PRINT_DEBUG = true;
        #region Variables
        Clank.Core.Model.ProjectFile m_project;
        #endregion

        /// <summary>
        /// Crée une nouvelle instance de CSGenerator avec un fichier projet passé en paramètre.
        /// </summary>
        /// <param name="project"></param>
        public JavaGenerator(Clank.Core.Model.ProjectFile project)
        {
            SetProject(project);
        }
        /// <summary>
        /// Crée une nouvelle instance de CSGenerator.
        /// </summary>
        public JavaGenerator() { }
        /// <summary>
        /// Définit le projet contenant les informations nécessaires à la génération de code.
        /// </summary>
        public void SetProject(Model.ProjectFile project)
        {
            m_project = project;
        }

        /// <summary>
        /// Obtient la propriété de métadonnée 'metadataProperty' pour le type donné.
        /// Retourne String.Empty par défaut.
        /// </summary>
        string GetMetadata(ClankType type, string metadataProperty)
        {
            Dictionary<string, string> metadata = new Dictionary<string,string>();
            string metadataStr = String.Empty;
            if (type.LanguageMetadata.ContainsKey(LANG_KEY))
                metadata = type.LanguageMetadata[LANG_KEY];
            if (metadata.ContainsKey(metadataProperty))
                metadataStr = metadata[metadataProperty];

            return metadataStr;
        }
        /// <summary>
        /// Obtient le nom du package dans lequel seront contenues toutes les classes du projet.
        /// Ce nom est déterminé par la propriété de métadonnée "package".
        /// </summary>
        /// <returns></returns>
        string GetPackageName()
        {
            return GetMetadata(m_project.Types.Types["State"], "package");
        }


        #region Includes
        /// <summary>
        /// Ajoute un type includable à la collection de types donnée.
        /// </summary>
        void AddToSet(HashSet<string> imports, ClankTypeInstance type)
        {
            string import = null;
            if (type.IsGeneric)
            {
                foreach (var arg in type.GenericArguments)
                {
                    AddToSet(imports, arg);
                }
            }

            if(type.BaseType.IsMacro)
            {
                string metadata = GetMetadata(type.BaseType, "import");
                if (metadata == String.Empty)
                    return;

                import = "import " + GetMetadata(type.BaseType, "import") + ";";
            }
            else
            {
                string package = GetPackageName();
                if (package != String.Empty)
                    package += ".";
                import = "import " + package + type.BaseType.Name + ".*;";
            }

            if (type.BaseType.IsBuiltIn)
                return;


            if (!imports.Contains(import)) imports.Add(import);
        }
        /// <summary>
        /// Ajoute tous les types dont dépend l'instruction inst dans le set passé en paramètres.
        /// </summary>
        void AggregateDependencies(Instruction inst, HashSet<string> imports)
        {
            if (inst is ClassDeclaration)
            {
                ClassDeclaration decl = (ClassDeclaration)inst;
                foreach (var instruction in decl.Instructions)
                {
                    AggregateDependencies(instruction, imports);
                }
            }
            else if (inst is Model.Language.Macros.RemoteFunctionWrapper)
            {
                var decl = (Model.Language.Macros.RemoteFunctionWrapper)inst;
                AggregateDependencies(decl.Func, imports);
            }
            if (inst is VariableDeclarationInstruction)
            {
                VariableDeclarationInstruction decl = (VariableDeclarationInstruction)inst;
                AddToSet(imports, decl.Var.Type);
            }
            else if (inst is VariableDeclarationAndAssignmentInstruction)
            {
                VariableDeclarationAndAssignmentInstruction decl = (VariableDeclarationAndAssignmentInstruction)inst;
                AddToSet(imports, decl.Declaration.Var.Type);
            }
            else if (inst is FunctionDeclaration)
            {
                FunctionDeclaration decl = (FunctionDeclaration)inst;
                AddToSet(imports, decl.Func.ReturnType);
                foreach (var arg in decl.Func.Arguments)
                {
                    AddToSet(imports, arg.ArgType);
                }

                foreach (var instruction in decl.Code)
                {
                    AggregateDependencies(instruction, imports);
                }
            }
            else if (inst is FunctionCallInstruction)
            {
                FunctionCallInstruction decl = (FunctionCallInstruction)inst;
                AddToSet(imports, decl.Call.Func.ReturnType);
                foreach (var arg in decl.Call.Arguments)
                {
                    AddToSet(imports, arg.Type);
                }
            }
        }
        /// <summary>
        /// Génère les déclaration d'inclusion de la classe.
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        string GenerateIncludes(ClassDeclaration declaration)
        {
            StringBuilder builder = new StringBuilder();

            HashSet<string> imports = new HashSet<string>();
            AggregateDependencies(declaration, imports);
            foreach (string inst in imports)
            {
                builder.AppendLine(inst);
            }


            return builder.ToString();
        }
        #endregion

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
                    Model.Language.ClassDeclaration decl = (Model.Language.ClassDeclaration)inst;
                    outputFiles.Add(new OutputFile(outputDirectory + "/" + decl.Name + ".java",
                        GenerateInstruction(inst)));
                }
                else if(inst is Model.Language.EnumDeclaration)
                {
                    Model.Language.EnumDeclaration decl = (Model.Language.EnumDeclaration)inst;
                    outputFiles.Add(new OutputFile(outputDirectory + "/" + decl.Name + ".java", GenerateEnumFile(decl)));
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
        string GenerateClass(ClassDeclaration declaration, bool generateIncludes=true)
        {
            StringBuilder builder = new StringBuilder();

            // Package
            string pkg = GetPackageName();
            if (pkg != String.Empty)
            {
                builder.AppendLine("package " + pkg + ";");
            }

            // Récupère les métadonnées.
            string metadata = GetMetadata(m_project.Types.Types[declaration.Name], "import");
            builder.AppendLine("import java.lang.*;");
            builder.AppendLine("import java.util.ArrayList;");
            builder.AppendLine("import java.io.BufferedReader;");
            builder.AppendLine("import java.io.ByteArrayInputStream;");
            builder.AppendLine("import java.io.ByteArrayOutputStream;");
            builder.AppendLine("import java.io.IOException;");
            builder.AppendLine("import java.io.InputStreamReader;");
            builder.AppendLine("import java.io.OutputStreamWriter;");
            builder.AppendLine("import java.io.UnsupportedEncodingException;");
            builder.AppendLine(GenerateIncludes(declaration));

            // Ajoute les headers supplémentaires
            if(metadata != String.Empty)
            {
                builder.AppendLine(metadata.Replace(";", ";\r\n"));
            }
            builder.AppendLine();
            builder.AppendLine("@SuppressWarnings(\"unused\")");
            string inheritance = declaration.InheritsFrom == null ? "" : " extends " + declaration.InheritsFrom;
            if (declaration.Modifiers.Contains("public"))
                builder.Append("public ");
            builder.Append("class " + declaration.Name);
            
            // Paramètres génériques
            if (declaration.GenericParameters.Count > 0)
            {
                builder.Append("<");
                foreach (string tupe in declaration.GenericParameters)
                {
                    builder.Append(tupe);
                    if (tupe != declaration.GenericParameters.Last())
                        builder.Append(",");
                }
                builder.Append(">");
            }
            // Héritage
            builder.AppendLine(inheritance + "\n{\n");

            // Instructions
            foreach(Instruction instruction in declaration.Instructions)
            {
                if (!(instruction is EnumDeclaration))
                {
                    builder.Append(Tools.StringUtils.Indent(GenerateInstruction(instruction), 1));
                    builder.Append("\n");
                }
            }
            // Constructeur
            builder.AppendLine(Tools.StringUtils.Indent(GenerateConstructor(declaration), 1));

            builder.AppendLine(Tools.StringUtils.Indent(GenerateDeserializer(declaration), 1));
            builder.AppendLine(Tools.StringUtils.Indent(GenerateSerializer(declaration), 1));
            builder.AppendLine("}");
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
            b.AppendLine("public " + decl.Name + "() {");
            var varDecls = decl.Instructions.Where(inst => inst is VariableDeclarationInstruction).ToList();

            foreach (var vdecl in varDecls)
            {
                var d = vdecl as VariableDeclarationInstruction;
                if (d.Var.Type.BaseType.JType == JSONType.Array || d.Var.Type.BaseType.JType == JSONType.Object)
                {
                    b.AppendLine("\t" + d.Var.Name + " = new " + GenerateTypeInstanceName(d.Var.Type) + "();");
                }
            }

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

            sb.AppendLine("public static " + typename + " deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {");
            sb.AppendLine("\t" + typename + " " + objName + " = " + " new " + typename + "();");
            
            foreach(var inst in decl.Instructions)
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

                string varTypename = GenerateTypeInstanceName(attr.Type);
                string intermediateVariableName = objName + "_" + attr.Name;
                sb.AppendLine("\t// " + attr.Name);
                sb.AppendLine(Tools.StringUtils.Indent(GenerateDeserializationInstruction(attr, intermediateVariableName)));
                
                if(attr.Type.BaseType.IsEnum && false)
                {
                    sb.AppendLine("\t" + objName + "." + attr.Name + " = " + varTypename + ".fromValue(" + intermediateVariableName + ");");
                }
                else
                {
                    sb.AppendLine("\t" + objName + "." + attr.Name + " = " + intermediateVariableName + ";");
                }
                
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
                    return "boolean " + dstVarName + " = Integer.valueOf(input.readLine()) == 0 ? false : true;";
                case JSONType.Int:
                    if(variable.Type.BaseType.IsEnum)
                    {
                        return typename + " " + dstVarName + " = " + typename + ".fromValue(Integer.valueOf(input.readLine()));";
                    }
                    else
                    {
                        return "int " + dstVarName + " = Integer.valueOf(input.readLine());";
                    }
                case JSONType.Float:
                    return "float " + dstVarName + " = Float.valueOf(input.readLine());";
                case JSONType.String:
                    return "String " + dstVarName + " = input.readLine();";
                case JSONType.Object:
                    return typename + " " + dstVarName + " = " + typename + ".deserialize(input);";
                case JSONType.Array:
                    StringBuilder builder = new StringBuilder();
                    var elementType = variable.Type.BaseType.JArrayElementType.AsInstance().Instanciate(variable.Type.GenericArguments);
                    string elementTypename = GenerateTypeInstanceName(elementType);
                    string dstVarNameMod = dstVarName.Replace("[", "").Replace("]", "");
                    string itName = dstVarNameMod + "_i";
                    string elementVarName = dstVarNameMod + "_e";
                    string elementCountName = dstVarNameMod + "_count";

                    string parenthesis = typename.EndsWith("]") ? "" : "()";
                    builder.AppendLine(typename + " " + dstVarName + " = new " + typename + parenthesis + ";");
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

                    if(elementType.BaseType.IsEnum && false)
                        builder.AppendLine("\t" + dstVarName + ".add(" + elementTypename + ".fromValue(" + elementVarName + "));");
                    else
                        builder.AppendLine("\t" + dstVarName + ".add((" + elementTypename + ")" + elementVarName + ");");

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
            sb.AppendLine("public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {");
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
                    new Variable() { Type = attr.Type, Name = "this." + attr.Name })));
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
            string endl = "\"\\n\"";
            switch (srcVar.Type.BaseType.JType)
            {
                case JSONType.Bool:
                    return "output.append((" + srcVar.Name + " ? 1 : 0) + " + endl + ");";
                case JSONType.Int:
                    if(srcVar.Type.BaseType.IsEnum)
                        return "output.append(((Integer)(" + srcVar.Name + ".getValue())).toString() + " + endl + ");";
                    else
                        return "output.append(((Integer)" + srcVar.Name + ").toString() + " + endl + ");";
                case JSONType.Float:
                    return "output.append(((Float)" + srcVar.Name + ").toString() + " + endl + ");";
                case JSONType.String:
                    return "output.append(" + srcVar.Name + " + " + endl + ");";
                case JSONType.Object:
                    return srcVar.Name + ".serialize(output);";
                case JSONType.Array:
                    StringBuilder builder = new StringBuilder();
                    string itname = (srcVar.Name + "_it").Replace("this.", "").Replace(".get(", "").Replace(")", "");
                    Variable item = new Variable()
                    {
                        Name = srcVar.Name + ".get(" + itname + ")",
                        Type = srcVar.Type.BaseType.JArrayElementType.AsInstance().Instanciate(srcVar.Type.GenericArguments)
                    };
                    builder.AppendLine("output.append(String.valueOf(" + srcVar.Name + ".size()) + " + endl + ");");
                    builder.AppendLine("for(int " + itname + " = 0; " + itname + " < " + srcVar.Name + ".size();" + itname + "++) {");
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
                return GenerateClass((ClassDeclaration)instruction);
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
                return comment + GenerateEnumFile((EnumDeclaration)instruction);
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
            else if (instruction is PlaceholderInstruction) { return ""; }
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
                    builder.Append("elsif");
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
            builder.Append("public " + returnTypeName + " " + func.Name + "(");
            
            // Arg list
            foreach(var arg in func.Arguments)
            {
                builder.Append(GenerateTypeInstanceName(arg.ArgType) + " " + arg.ArgName + (arg == func.Arguments.Last() ? "" : ","));
            }
            builder.Append(")\r\n{\r\n");
            builder.AppendLine("\ttry {");
            if (PRINT_DEBUG)
            {
                builder.AppendLine("\tSystem.out.println(\"[" + instruction.Func.Func.Name + "]\");");
            }
            builder.AppendLine("\tByteArrayOutputStream s = new ByteArrayOutputStream();");
            builder.AppendLine("\tOutputStreamWriter output = new OutputStreamWriter(s, \"UTF-8\");");
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
            builder.AppendLine("\toutput.close();");
            builder.AppendLine("\tTCPHelper.Send(s.toByteArray());");

            // Récupère la réponse du serveur.
            builder.AppendLine("\tbyte[] response = TCPHelper.Receive();");
            builder.AppendLine("\tByteArrayInputStream s2 = new ByteArrayInputStream(response);");
            builder.AppendLine("\tBufferedReader input = new BufferedReader(new InputStreamReader(s2, \"UTF-8\"));");
            // Récupère l'objet dans le stream.
            builder.AppendLine(Tools.StringUtils.Indent(GenerateDeserializationInstruction(
                new Variable()
                {
                    Type = func.ReturnType
                }, "returnValue")));

            if(func.ReturnType.BaseType.IsEnum && false)
                builder.AppendLine("\treturn " + GenerateTypeInstanceName(func.ReturnType) + ".fromValue(returnValue);");
            else
                builder.AppendLine("\treturn returnValue;");

            builder.AppendLine("\t} catch (UnsupportedEncodingException e) { ");
            builder.AppendLine("\t} catch (IOException e) { }");
            builder.AppendLine("\treturn null;");
            builder.AppendLine("}\r\n\r\n");
            // Corps de la fonction.
            /*
            // --- Send
            builder.AppendLine("\t// Send");
            builder.Append("\tList<object> args = new List<object>() { ");
            // Arg list
            foreach (var arg in func.Arguments)
            {
                builder.Append(arg.ArgName + (arg == func.Arguments.Last() ? "" : ","));
            }
            builder.AppendLine("};");
            // Func id
            builder.AppendLine("\tint funcId = " + instruction.Id + ";");
            builder.AppendLine("\tList<object> obj = new List<object>() { funcId, args };");
            // Sending
            builder.AppendLine("\tTCPHelper.Send(Newtonsoft.Json.JsonConvert.SerializeObject(obj));");

            // --- Receive
            builder.AppendLine("\t// Receive");
            builder.AppendLine("\tstring str = TCPHelper.Receive();");
            builder.AppendLine("\tNewtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(str);");


            // Object
            if (func.ReturnType.BaseType.JType == JSONType.Object || func.ReturnType.BaseType.JType == JSONType.Array)
                builder.Append("\treturn (" + returnTypeName + ")o[0].ToObject(typeof(" + returnTypeName + "));\r\n");
            else // Value
                builder.Append("\treturn o.Value<" + returnTypeName + ">(0);\r\n");

            builder.AppendLine("}\r\n\r\n"); // */
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
                builder.AppendLine("/// <summary>");
                foreach (string line in lines)
                {
                    builder.AppendLine("/// " + line);
                }
                builder.AppendLine("/// </summary>");
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
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("public byte[] ProcessRequest(byte[] request, int " + Clank.Core.Model.Language.SemanticConstants.ClientID + ")\r\n{");
            int id = 0;
            List<FunctionDeclaration> decls = new List<FunctionDeclaration>();
            decls.AddRange(instruction.Access.Declarations);
            decls.AddRange(instruction.Write.Declarations);

            builder.AppendLine("\tSystem.IO.MemoryStream s = new System.IO.MemoryStream(request);");
            builder.AppendLine("\tSystem.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);");
            builder.AppendLine("\t\tSystem.IO.StreamWriter output;");
            // Récupère l'id de la fonction
            builder.AppendLine(Tools.StringUtils.Indent(GenerateDeserializationInstruction(
                new Variable() { Type = m_project.Types.TypeInstances["int"] }, "functionId")));
            builder.AppendLine("\tswitch(functionId)\r\n\t{");
            // Switch dans lequel on traite les différents messages.
            foreach (FunctionDeclaration func in decls)
            {
                StringBuilder argList = new StringBuilder();
                builder.AppendLine("\tcase " + id + ":");
                int argId = 0;
                foreach (FunctionArgument arg in func.Func.Arguments)
                {
                    // Variable name and type.
                    string varName = "arg" + id + "_" + argId;
                    string typeName = GenerateTypeInstanceName(arg.ArgType);
                    builder.AppendLine(Tools.StringUtils.Indent(
                        GenerateDeserializationInstruction(new Variable() { Type = arg.ArgType }, varName),
                        2));

                    argList.Append(varName + ", ");
                    argId++;
                }
                argList.Append(Model.Language.SemanticConstants.ClientID);

                // Crée les streams nécessaires.
                builder.AppendLine("\t\ts = new System.IO.MemoryStream();");
                builder.AppendLine("\t\toutput = new System.IO.StreamWriter(s, BOMLESS_UTF8);");
                builder.AppendLine("\t\toutput.NewLine = \"\\n\";");

                // Génère la variable à retourner à partir de l'appel à la fonction.
                string funcCall = func.Func.Name + "(" + argList + ")";
                string returnvarName = "retValue" + id.ToString();
                builder.AppendLine("\t\t" + GenerateTypeInstanceName(func.Func.ReturnType) + " " + returnvarName + " = " + funcCall +";");
                builder.AppendLine(Tools.StringUtils.Indent(
                    GenerateSerializationInstruction(new Variable() { Name = returnvarName, Type = func.Func.ReturnType }),
                    2));

                // Function call and return
                builder.AppendLine("\t\toutput.Close();");
                builder.AppendLine("\t\treturn s.ToArray();");

                id++;
            }

            builder.Append("\t}\r\n");
            builder.Append("\treturn new byte[0];\r\n}\r\n");
            return builder.ToString();
            /* 
            builder.AppendLine("\tNewtonsoft.Json.Linq.JArray o = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(request);");
            builder.AppendLine("\tint functionId = o.Value<int>(0);");
            builder.AppendLine("\tswitch(functionId)\r\n\t{");
            int id = 0;
            List<FunctionDeclaration> decls = new List<FunctionDeclaration>();
            decls.AddRange(instruction.Access.Declarations);
            decls.AddRange(instruction.Write.Declarations);

            // Switch dans lequel on traite les différents messages.
            foreach(FunctionDeclaration func in decls)
            {
                StringBuilder argList = new StringBuilder();
                builder.AppendLine("\t\tcase " + id + ":");
                int argId = 0;
                foreach(FunctionArgument arg in func.Func.Arguments)
                {
                    // Variable name and type.
                    string varName = "arg" + id + "_" + argId;
                    string typeName = GenerateTypeInstanceName(arg.ArgType);
                    builder.Append("\t\t\t" + typeName + " " + varName + " = ");

                    // Object
                    if(arg.ArgType.BaseType.JType == JSONType.Object || arg.ArgType.BaseType.JType == JSONType.Array)
                        builder.Append("(" + typeName + ")o[1][" + argId + "].ToObject(typeof(" + typeName + "));\r\n");
                    else // Value
                        builder.Append("o[1].Value<" + typeName + ">(" + argId + ");\r\n");


                    argList.Append(varName + ", ");
                    argId++;
                }
                argList.Append(Model.Language.SemanticConstants.ClientID);
                // Function call and return
                builder.Append("\t\t\treturn Newtonsoft.Json.JsonConvert.SerializeObject(new List<object>() { " + func.Func.Name + "(" + argList + ")" + " });\r\n");
                
                id++;
            }

            builder.Append("\t}\r\n");
            builder.Append("\treturn \"\";\r\n}\r\n");

            return builder.ToString(); // */
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
        /// Génère le code représentant l'instance de type passé en paramètre.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GenerateTypeInstanceName(ClankTypeInstance type)
        {
            if (type.IsGeneric)
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
            if(type.IsMacro)
            {
                
                // Pour les types macro, on remplace le nom du type par le nom du type natif.
                Model.MacroContainer.MacroClass mcClass = m_project.Macros.FindClassByType(type);
                if (!mcClass.LanguageToTypeName.ContainsKey(LANG_KEY))
                    throw new InvalidOperationException("Le nom de la macro classe '" + type.GetFullNameAndGenericArgs() + "' n'est pas renseigné pour le langage '" + LANG_KEY + "'");

                string nativeClassName = mcClass.LanguageToTypeName[LANG_KEY];

                return mcClass.LanguageToTypeName[LANG_KEY];
            }
            else if(type.IsBuiltIn)
            {
                switch(type.Name)
                {
                    case "string":
                        return "String";
                    case "bool":
                        return "Boolean";
                    case "int":
                        return "Integer";
                    case "float":
                        return "Float";
                    default:
                        return type.Name;
                }
            }
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

            StringBuilder builder = new StringBuilder();
            if (call.Src != null)
                builder.Append(GenerateEvaluable(call.Src) + ".");
            else if (call.IsThisClassFunction)
                builder.Append("this.");

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
            builder.Append("new ");

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
            Clank.Core.Model.MacroContainer.MacroFunction func;
            Clank.Core.Model.MacroContainer macros = m_project.Macros;
            ClankTypeInstance owner = null;
            if (call.Src == null)
            {
                // Fonction "globale".
                func = macros.FindFunctionByName(call.Func.Name);
            }
            else
            {
                if (call.IsConstructor)
                    owner = call.Func.ReturnType;
                else
                    owner = call.Src.Type;

                Model.MacroContainer.MacroClass klass = macros.FindClassByType(owner.BaseType);
                func = klass.Functions[call.Func.GetFullName()]; // TODO : GetFullName() ??
            }

            // Vérification
            if(!func.LanguageToFunctionName.ContainsKey(LANG_KEY))
            {
                throw new InvalidOperationException("Le nom de la macro fonction '" + func.Function.GetFullName() + "' n'est pas renseigné pour le langage '" + LANG_KEY + "'");
            }


            // Nom de la fonctin native dont les paramètres sont entourés par des $
            string nativeFuncName = func.LanguageToFunctionName[LANG_KEY];


            // Remplace les paramètres par leurs values.
            for(int i = 0; i < call.Func.Arguments.Count; i++)
            {
                nativeFuncName = nativeFuncName.Replace(SemanticConstants.ReplaceChr + "(" + call.Func.Arguments[i].ArgName + ")",
                    GenerateEvaluable(call.Arguments[i]));
            }


            // Remplace les params génériques par leurs valeurs (si fonction non globale)
            if(owner != null)
                for(int i = 0; i < call.Func.Owner.GenericArgumentNames.Count; i++)
                {
                    nativeFuncName = nativeFuncName.Replace(SemanticConstants.ReplaceChr + "(" + call.Func.Owner.GenericArgumentNames[i] + ")",
                        GenerateTypeInstanceName(owner.GenericArguments[i]));
                }

            // Remplace @self par le nom de la variable.
            if(call.Src != null)
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
                return "this." + access.VariableName;
            }

            return GenerateEvaluable(access.Left) + "." + access.VariableName;
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
        string GenerateEnumFile(EnumDeclaration decl)
        {
            StringBuilder builder = new StringBuilder();

            // Ajout du package.
            string pkg = GetPackageName();
            if (pkg != String.Empty)
                builder.AppendLine("package " + pkg + ";");

            // Crée l'enum.
            builder.Append("public enum " + decl.Name + "\n{\n");
            foreach (var kvp in decl.Members)
            {
                string member = kvp.Key;
                int value = kvp.Value;
                builder.Append(Tools.StringUtils.Indent(member, 1));
                builder.Append("(" + value.ToString() + ")");
                if (kvp.Key != decl.Members.Last().Key)
                    builder.Append(',');
                else
                    builder.Append(';');
                builder.Append("\n");
            }

            // Ajoute les champs permettant d'utiliser l'enum avec ses valeurs numériques.
            builder.AppendLine("\tint _value;");
            builder.AppendLine("\t" + decl.Name + "(int value) { _value = value; } ");
            builder.AppendLine("\tpublic int getValue() { return _value; }");
            builder.AppendLine("\tpublic static " + decl.Name + " fromValue(int value) { ");
            builder.AppendLine("\t\t" + decl.Name + " val = " + decl.Members.First().Key + ";");
            builder.AppendLine("\t\tval._value = value;");
            builder.AppendLine("\t\treturn val;");
            builder.AppendLine("\t}");
            builder.AppendLine("}");
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
            StringBuilder builder = new StringBuilder();
            foreach(string modifier in instruction.Modifiers)
            {
                builder.Append(modifier + " ");
            }

            return builder.ToString() + GenerateTypeInstanceName(instruction.Var.Type) + " " + instruction.Var.Name + ";";
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
            if (decl.Func.IsPublic)
                builder.Append("public ");
            if (decl.Func.IsStatic)
                builder.Append("static ");

            if (decl.Func.IsConstructor)
            {
                // Constructeur
                builder.Append(GenerateTypeName(decl.Func.ReturnType.BaseType));
            }
            else
            {
                // Fonction classique
                builder.Append(GenerateTypeInstanceName(decl.Func.ReturnType) + " ");
                builder.Append(decl.Func.Name);
            }
            builder.Append("(");

            // Ajout des arguments.
            foreach(FunctionArgument arg in decl.Func.Arguments)
            {
                builder.Append(GenerateTypeInstanceName(arg.ArgType) + " " + arg.ArgName);
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
            builder.Append("public ");
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
