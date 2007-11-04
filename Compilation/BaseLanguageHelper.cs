/*
    Reflexil .NET assembly editor.
    Copyright (C) 2007 Sebastien LEBRETON

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

#region " Imports "
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Mono.Cecil;
#endregion

namespace Reflexil.Compilation
{
    /// <summary>
    /// Base class for code generation
    /// </summary>
    internal abstract class BaseLanguageHelper : IReflectionVisitor, ILanguageHelper
    {

        #region " Constants "
        protected const string BASIC_SEPARATOR = ", ";
        protected const string GENERIC_TYPE_TAG = "`";
        protected const string PARAMETER_LIST_START = "(";
        protected const string PARAMETER_LIST_END = ")";
        protected const string REFERENCE_TYPE_TAG = "&";
        protected const string NAMESPACE_SEPARATOR = ".";
        protected const string SPACE = " ";
        protected const string QUOTE = "\"";
        protected string[] DEFAULT_NAMESPACES = { "System", "System.Collections.Generic", "System.Text" };
        #endregion

        #region " Fields "
        private StringBuilder m_identedbuilder = new StringBuilder();
        private int m_identlevel = 0;
        protected Dictionary<string, string> m_aliases = new Dictionary<string, string>();
        protected bool m_fullnamespaces = true;
        #endregion

        #region " Properties "
        protected int IdentLevel
        {
            get
            {
                return m_identlevel;
            }
            set
            {
                m_identlevel = value;
            }
        }
        #endregion

        #region " Methods "

        #region " Abstract "
        /// <summary>
        /// Write a method signature to the text buffer
        /// </summary>
        /// <param name="mdef">Method definition</param>
        protected abstract void WriteMethodSignature(MethodDefinition mdef);

        /// <summary>
        /// Write a method body to the text buffer
        /// </summary>
        /// <param name="mdef">Method definition</param>
        protected abstract void WriteMethodBody(MethodDefinition mdef);

        /// <summary>
        /// Write a type signature to the text buffer
        /// </summary>
        /// <param name="tdef">Type definition</param>
        protected abstract void WriteTypeSignature(TypeDefinition tdef);

        /// <summary>
        /// Write a field to the text buffer
        /// </summary>
        /// <param name="fdef">Field definition</param>
        protected abstract void WriteField(FieldDefinition fdef);

        /// <summary>
        /// Write a comment to the text buffer
        /// </summary>
        /// <param name="comment">Comment</param>
        protected abstract void WriteComment(string comment);

        /// <summary>
        /// Write fields stubs to the text buffer
        /// </summary>
        /// <param name="fields">Fields stubs</param>
        protected abstract void WriteFieldsStubs(FieldDefinitionCollection fields);

        /// <summary>
        /// Write methods stubs to the text buffer
        /// </summary>
        /// <param name="mdef">Method definition to exclude</param>
        /// <param name="methods">Methods definitions</param>
        protected abstract void WriteMethodsStubs(MethodDefinition mdef, MethodDefinitionCollection methods);

        /// <summary>
        /// Write default namespaces to the text buffer
        /// </summary>
        protected abstract void WriteDefaultNamespaces();

        /// <summary>
        /// Write a method's owner type to the text buffer
        /// </summary>
        /// <param name="mdef">Method definition</param>
        protected abstract void WriteType(MethodDefinition mdef);
        #endregion

        #region " IReflectionVisitor "
        /// <summary>
        /// Visit a type definition
        /// </summary>
        /// <param name="type">Type definition</param>
        public abstract void VisitTypeDefinition(TypeDefinition type);

        /// <summary>
        /// Visit a field definition
        /// </summary>
        /// <param name="field">Field definition</param>
        public abstract void VisitFieldDefinition(FieldDefinition field);

        /// <summary>
        /// Visit a method definition
        /// </summary>
        /// <param name="method">Method definition</param>
        public abstract void VisitMethodDefinition(MethodDefinition method);

        /// <summary>
        /// Visit a type reference
        /// </summary>
        /// <param name="type">Type reference</param>
        public abstract void VisitTypeReference(TypeReference type);

        /// <summary>
        /// Visit a generic parameter collection
        /// </summary>
        /// <param name="genparams">Generic parameter collection</param>
        public abstract void VisitGenericParameterCollection(GenericParameterCollection genparams);

        /// <summary>
        /// Visit a parameter definition
        /// </summary>
        /// <param name="parameter">Parameter definition</param>
        public abstract void VisitParameterDefinition(ParameterDefinition parameter);

        /// <summary>
        /// Visit a parameter definition collection
        /// </summary>
        /// <param name="parameters"></param>
        public abstract void VisitParameterDefinitionCollection(ParameterDefinitionCollection parameters);
        #endregion

        #region " Not Implemented "
        public virtual void VisitCustomAttributeCollection(CustomAttributeCollection customAttrs) { }
        public virtual void VisitSecurityDeclaration(SecurityDeclaration secDecl) { }
        public virtual void VisitCustomAttribute(CustomAttribute customAttr) { }
        public virtual void VisitSecurityDeclarationCollection(SecurityDeclarationCollection secDecls) { }
        public virtual void VisitOverride(MethodReference ov) { }
        public virtual void VisitOverrideCollection(OverrideCollection meth) { }
        public virtual void VisitGenericParameter(GenericParameter genparam) { }
        public virtual void VisitConstructorCollection(ConstructorCollection ctors) { }
        public virtual void VisitEventDefinitionCollection(EventDefinitionCollection events) { }
        public virtual void VisitFieldDefinitionCollection(FieldDefinitionCollection fields) { }
        public virtual void VisitMethodDefinitionCollection(MethodDefinitionCollection methods) { }
        public virtual void VisitNestedTypeCollection(NestedTypeCollection nestedTypes) { }
        public virtual void VisitPropertyDefinitionCollection(PropertyDefinitionCollection properties) { }
        public virtual void VisitTypeDefinitionCollection(TypeDefinitionCollection types) { }
        public virtual void VisitEventDefinition(EventDefinition evt) { }
        public virtual void VisitModuleDefinition(ModuleDefinition @module) { }
        public virtual void VisitNestedType(TypeDefinition nestedType) { }
        public virtual void VisitPropertyDefinition(PropertyDefinition @property) { }
        public virtual void VisitConstructor(MethodDefinition ctor) { }
        public virtual void TerminateModuleDefinition(ModuleDefinition @module) { }
        public virtual void VisitExternType(TypeReference externType) { }
        public virtual void VisitExternTypeCollection(ExternTypeCollection externs) { }
        public virtual void VisitInterface(TypeReference interf) { }
        public virtual void VisitInterfaceCollection(InterfaceCollection interfaces) { }
        public virtual void VisitMemberReference(MemberReference member) { }
        public virtual void VisitMemberReferenceCollection(MemberReferenceCollection members) { }
        public virtual void VisitMarshalSpec(MarshalSpec marshalSpec) { }
        public virtual void VisitTypeReferenceCollection(TypeReferenceCollection refs) { }
        public virtual void VisitPInvokeInfo(PInvokeInfo pinvk) { }
        #endregion

        #region " Text generation "
        /// <summary>
        /// Change ident level and apply modifications to the text buffer
        /// </summary>
        /// <param name="newidentlevel">Ident level</param>
        protected void ReIdent(int newidentlevel)
        {
            UnIdent();
            IdentLevel = newidentlevel;
            Ident();
        }

        /// <summary>
        /// Pre-ident the text buffer
        /// </summary>
        protected void Ident()
        {
            for (int i = 0; i < IdentLevel; i++)
            {
                m_identedbuilder.Append("\t");
            }
        }

        /// <summary>
        /// Unident the pre-idented text buffer
        /// </summary>
        protected void UnIdent()
        {
            for (int i = 0; i < IdentLevel; i++)
            {
                if ((m_identedbuilder.Length > 0) && (m_identedbuilder[m_identedbuilder.Length - 1] == '\t'))
                {
                    m_identedbuilder.Remove(m_identedbuilder.Length - 1, 1);
                }
            }
        }

        /// <summary>
        /// Replace text in the text buffer
        /// </summary>
        /// <param name="oldvalue">string to replace</param>
        /// <param name="newvalue">replacement string</param>
        protected void Replace(string oldvalue, string newvalue)
        {
            m_identedbuilder.Replace(oldvalue, newvalue);
        }

        /// <summary>
        /// Write a new line to the text buffer
        /// </summary>
        protected void WriteLine()
        {
            m_identedbuilder.AppendLine();
            Ident();
        }

        /// <summary>
        /// Write a string and a new line to the text buffer
        /// </summary>
        /// <param name="str">the string to write</param>
        protected void WriteLine(string str)
        {
            Write(str);
            WriteLine();
        }

        /// <summary>
        /// Write a string to the text buffer
        /// </summary>
        /// <param name="str">the string to write</param>
        protected void Write(string str)
        {
            m_identedbuilder.Append(str);
        }

        /// <summary>
        /// Reset the text buffer 
        /// </summary>
        protected void Reset()
        {
            m_identlevel = 0;
            m_identedbuilder.Length = 0;
        }

        /// <summary>
        /// Get the text buffer
        /// </summary>
        /// <returns></returns>
        protected string GetResult()
        {
            return m_identedbuilder.ToString();
        }
        #endregion

        #region " ILanguageHelper "
        /// <summary>
        /// Generate method signature 
        /// </summary>
        /// <param name="mdef">Method definition</param>
        /// <returns>generated source code</returns>
        public virtual string GetMethodSignature(MethodDefinition mdef)
        {
            Reset();
            WriteMethodSignature(mdef);
            return GetResult();
        }

        /// <summary>
        /// Generate method
        /// </summary>
        /// <param name="mdef">Method definition</param>
        /// <returns>generated source code</returns>
        public virtual string GetMethod(MethodDefinition mdef)
        {
            Reset();
            WriteMethodSignature(mdef);
            WriteMethodBody(mdef);
            return GetResult();
        }

        /// <summary>
        /// Generate field
        /// </summary>
        /// <param name="fdef">Field definition</param>
        /// <returns>generated source code</returns>
        public virtual string GetField(FieldDefinition fdef)
        {
            Reset();
            WriteField(fdef);
            return GetResult();
        }

        /// <summary>
        /// Generate type signature
        /// </summary>
        /// <param name="tdef">Type definition</param>
        /// <returns>generated source code</returns>
        public virtual string GetTypeSignature(TypeDefinition tdef)
        {
            Reset();
            WriteTypeSignature(tdef);
            return GetResult();
        }

        /// <summary>
        /// Generate source code from method declaring type. All others
        /// methods are generated as stubs.
        /// </summary>
        /// <param name="mdef">Method definition</param>
        /// <param name="references">Assembly references</param>
        /// <returns>generated source code</returns>
        public abstract string GenerateSourceCode(MethodDefinition mdef, List<AssemblyNameReference> references);
        #endregion

        #region " Writers "
        /// <summary>
        /// Write methods stubs to the text buffer
        /// </summary>
        /// <param name="mdef">Method definition to exclude</param>
        /// <param name="methods">Methods definitions</param>
        /// <param name="rskw">Region start keyword</param>
        /// <param name="rekw">Region end keyword</param>
        protected void WriteMethodsStubs(MethodDefinition mdef, MethodDefinitionCollection methods, string rskw, string rekw)
        {
            Write(rskw);
            WriteLine("\" Methods stubs \"");
            WriteComment("Do not add or update any method. If compilation fails because of a method declaration, comment it");
            foreach (MethodDefinition smdef in methods)
            {
                if (mdef != smdef)
                {
                    WriteMethodSignature(smdef);
                    WriteMethodBody(smdef);
                    WriteLine();
                }
            }
            WriteLine(rekw);
        }

        /// <summary>
        /// Write fields stubs to the text buffer
        /// </summary>
        /// <param name="fields">Fields stubs</param>
        /// <param name="rskw">Region start keyword</param>
        /// <param name="rekw">Region end keyword</param>
        protected void WriteFieldsStubs(FieldDefinitionCollection fields, string rskw, string rekw)
        {
            Write(rskw);
            WriteLine("\" Fields stubs \"");
            WriteComment("Do not add or update any field. If compilation fails because of a field declaration, comment it");
            foreach (FieldDefinition fdef in fields)
            {
                WriteField(fdef);
                WriteLine();
            }
            WriteLine(rekw);
        }

        /// <summary>
        /// Write referenced assemblies to the text buffer (as a comment)
        /// </summary>
        /// <param name="references">Assembly references</param>
        private void WriteReferencedAssemblies(List<AssemblyNameReference> references)
        {
            WriteComment("[Referenced assemblies]");
            foreach (AssemblyNameReference asmref in references)
            {
                WriteComment(String.Format("- {0} v{1}", asmref.Name, asmref.Version));
            }
        }
        
        /// <summary>
        /// Write a method's owner type to the text buffer
        /// </summary>
        /// <param name="mdef">Method definition</param>
        /// <param name="tkw">Type keyword</param>
        /// <param name="tskw">Type start keyword</param>
        /// <param name="tekw">Type end keywork</param>
        protected void WriteType(MethodDefinition mdef, string tkw, string tskw, string tekw)
        {
            Write(tkw);
            WriteTypeSignature(mdef.DeclaringType as TypeDefinition);
            WriteLine();
            IdentLevel++;
            WriteLine(tskw);

            WriteComment("Limited support!");
            WriteComment("You can only reference methods or fields defined in the class (not in ancestors classes)");
            WriteComment("Fields and methods stubs are needed for compilation purposes only.");
            WriteComment("Reflexil will automaticaly map current type, fields or methods to original references.");
            WriteMethodSignature(mdef);
            WriteMethodBody(mdef);

            WriteLine();
            WriteMethodsStubs(mdef, (mdef.DeclaringType as TypeDefinition).Methods);

            WriteLine();
            WriteFieldsStubs((mdef.DeclaringType as TypeDefinition).Fields);

            ReIdent(IdentLevel - 1);
            WriteLine(tekw);
        }
        #endregion

        #region " Helpers "
        /// <summary>
        /// Replace all aliases in a string
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result string</returns>
        protected string HandleAliases(string str)
        {
            foreach (string alias in m_aliases.Keys)
            {
                str = str.Replace(alias, m_aliases[alias]);
            }
            return str;
        }

        /// <summary>
        /// Write a name to the text buffer. Handles aliases and namespaces.
        /// </summary>
        /// <param name="type">Type reference for namespace</param>
        /// <param name="name">The name to write</param>
        protected void HandleName(TypeReference type, string name)
        {
            name = HandleAliases(name);
            if (!m_fullnamespaces)
            {
                name = name.Replace(type.Namespace + NAMESPACE_SEPARATOR, String.Empty);
            }
            Write(name);
        }

        /// <summary>
        /// Visit a collection which contains IReflectionVisitable items, and write it to the text buffer.
        /// </summary>
        /// <param name="start">Collection start keyword</param>
        /// <param name="end">Collection end keyword</param>
        /// <param name="separator">Collection separator</param>
        /// <param name="always">If true write 'collection end keyword' even if collection is empty</param>
        /// <param name="collection">Collection to visit</param>
        protected virtual void VisitVisitableCollection(string start, string end, string separator, bool always, ICollection collection)
        {
            if (always | collection.Count > 0)
            {
                Write(start);
            }

            bool firstloop = true;
            foreach (IReflectionVisitable item in collection)
            {
                if (!firstloop)
                {
                    Write(separator);
                }
                else
                {
                    firstloop = false;
                }
                if (item is TypeDefinition)
                {
                    VisitTypeReference(item as TypeDefinition);
                }
                else
                {
                    item.Accept(this);
                }
            }

            if (always | collection.Count > 0)
            {
                Write(end);
            }
        }

        /// <summary>
        /// Generate source code from method declaring type. All others
        /// methods are generated as stubs.
        /// </summary>
        /// <param name="mdef">Method definition</param>
        /// <param name="references">Assembly references</param>
        /// <param name="nkw">Namespace keyword</param>
        /// <param name="nskw">Namespace start keyword</param>
        /// <param name="nekw">Namespace end keyword</param>
        /// <returns>generated source code</returns>
        protected string GenerateSourceCode(MethodDefinition mdef, List<AssemblyNameReference> references, string nkw, string nskw, string nekw)
        {
            Reset();

            WriteDefaultNamespaces(); WriteLine();
            WriteReferencedAssemblies(references); WriteLine();

            if (mdef.DeclaringType.Namespace != string.Empty)
            {
                Write(nkw);
                WriteLine(mdef.DeclaringType.Namespace);
                IdentLevel++;
                WriteLine(nskw);
            }

            WriteType(mdef);

            if (mdef.DeclaringType.Namespace != string.Empty)
            {
                ReIdent(IdentLevel - 1);
                WriteLine(nekw);
            }

            return GetResult();
        }
        #endregion

        #endregion

    }
}
