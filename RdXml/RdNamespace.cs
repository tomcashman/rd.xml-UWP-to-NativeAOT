/**
 * Apache License
 * Version 2.0, January 2004
 * http://www.apache.org/licenses/
 * 
 * Copyright 2022 Thomas Cashman
 * 
 * See LICENSE file
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;

namespace RdXml
{
    public class RdNamespace : RdElement
    {
        public const string ELEMENT_TAG = "Namespace";

        public List<RdNamespace> Namespaces { get; } = new List<RdNamespace>();
        public List<RdType> Types { get; } = new List<RdType>();
        public List<RdTypeInstantiation> TypeInstantiations { get; } = new List<RdTypeInstantiation>();

        public RdNamespace(RdElement parent, XmlElement xmlElement) : base(parent, xmlElement)
        {
            XmlNodeList namespaceList = xmlElement.GetElementsByTagName(RdNamespace.ELEMENT_TAG);
            foreach (XmlNode namespaceNode in namespaceList)
            {
                Namespaces.Add(new RdNamespace(this, (XmlElement)namespaceNode));
            }
            XmlNodeList typeList = xmlElement.GetElementsByTagName(RdType.ELEMENT_TAG);
            foreach (XmlNode typeNode in typeList)
            {
                Types.Add(new RdType(this, (XmlElement)typeNode));
            }
            XmlNodeList typeInstantiationList = xmlElement.GetElementsByTagName(RdTypeInstantiation.ELEMENT_TAG);
            foreach (XmlNode typeInstantiationNode in typeInstantiationList)
            {
                TypeInstantiations.Add(new RdTypeInstantiation(this, (XmlElement)typeInstantiationNode));
            }
        }

        public override void WriteNativeAOT(XmlDocument xmlDocument, XmlElement parentElement, HashSet<Type> writtenTypes)
        {
            string namespaceName = NamespaceName;
            string assemblyName = AssemblyName;

            foreach (RdNamespace @namespace in Namespaces)
            {
                @namespace.WriteNativeAOT(xmlDocument, parentElement, writtenTypes);
            }
            foreach (RdType type in Types)
            {
                type.WriteNativeAOT(xmlDocument, parentElement, writtenTypes);
            }
            foreach (RdTypeInstantiation typeInstantiation in TypeInstantiations)
            {
                typeInstantiation.WriteNativeAOT(xmlDocument, parentElement, writtenTypes);
            }

            //Only write other Namespace types after explicit declarations
            WriteNativeAOTTypes(xmlDocument, parentElement, writtenTypes, namespaceName, assemblyName);
            WriteNativeAOTDelegates(xmlDocument, parentElement, writtenTypes, namespaceName);
        }

        private void WriteNativeAOTTypes(XmlDocument xmlDocument, XmlElement parentElement, HashSet<Type> writtenTypes, string namespaceName, string assemblyName)
        {
            IEnumerable<Type> classes;
            if (!string.IsNullOrEmpty(assemblyName))
            {
                classes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(t => t.GetTypes())
                    .Where(t => t.IsClass && t.Namespace == namespaceName && t.Assembly.FullName == assemblyName);
            }
            else
            {
                classes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(t => t.GetTypes())
                    .Where(t => t.IsClass && t.Namespace == namespaceName);
            }

            foreach (Type type in classes)
            {
                if (!writtenTypes.Add(type))
                {
                    continue;
                }
                XmlElement typeElement = xmlDocument.CreateElement(RdType.ELEMENT_TAG);
                typeElement.SetAttribute(RdElement.ATTRIBUTE_NAME, type.FullName);
                if(HasReflectAttribute)
                {
                    typeElement.SetAttribute(RdElement.ATTRIBUTE_DYNAMIC, VALUE_REQUIRED_ALL);
                }
                parentElement.AppendChild(typeElement);
            }
        }

        private void WriteNativeAOTDelegates(XmlDocument xmlDocument, XmlElement parentElement, HashSet<Type> writtenTypes, string namespaceName)
        {
            IEnumerable<Type> delegates = AppDomain.CurrentDomain.GetAssemblies()
                                    .SelectMany(t => t.GetTypes())
                                    .Where(t => !t.IsClass && t.Namespace == namespaceName);
            foreach (Type type in delegates)
            {
                if (!writtenTypes.Add(type))
                {
                    continue;
                }
                XmlElement delegateElement = xmlDocument.CreateElement(RdType.ELEMENT_TAG);
                delegateElement.SetAttribute(RdElement.ATTRIBUTE_NAME, type.FullName);
                if (HasMarshalDelegateAttribute)
                {
                    delegateElement.SetAttribute(RdElement.ATTRIBUTE_MARSHAL_DELEGATE, VALUE_REQUIRED_ALL);
                }
                if (HasMarshalStructureAttribute)
                {
                    delegateElement.SetAttribute(RdElement.ATTRIBUTE_MARSHAL_STRUCTURE, VALUE_REQUIRED_ALL);
                }
                parentElement.AppendChild(delegateElement);
            }
        }

        public string NamespaceName
        {
            get
            {
                StringBuilder result = new StringBuilder();
                string thisName = XmlElement.GetAttribute(ATTRIBUTE_NAME);
                if (Parent is RdNamespace @namespace && !(Parent is RdAssembly))
                {
                    string parentNamespace = @namespace.NamespaceName;
                    if(!thisName.StartsWith(parentNamespace))
                    {
                        result.Append(parentNamespace);
                        if (!parentNamespace.EndsWith("."))
                        {
                            result.Append('.');
                        }
                    }
                }
                result.Append(thisName);
                return result.ToString();
            }
        }

        public string AssemblyName
        {
            get
            {
                if (Parent is RdAssembly assembly)
                {
                    return assembly.AssemblyName;
                }
                if (Parent is RdNamespace @namespace)
                {
                    return @namespace.AssemblyName;
                }
                return "";
            }
        }

    }
}
