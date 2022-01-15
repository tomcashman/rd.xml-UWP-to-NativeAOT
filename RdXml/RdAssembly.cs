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
    public class RdAssembly : RdNamespace
    {
        public new const string ELEMENT_TAG = "Assembly";

        public RdAssembly(RdElement parent, XmlElement xmlElement) : base(parent, xmlElement)
        {
        }

        public override void WriteNativeAOT(XmlDocument xmlDocument, XmlElement parentElement, HashSet<Type> writtenTypes)
        {
            string assemblyName = AssemblyName;

            XmlElement xmlElement = xmlDocument.CreateElement(ELEMENT_TAG);
            xmlElement.SetAttribute(ATTRIBUTE_NAME, assemblyName);
            xmlDocument.AppendChild(xmlElement);

            if (HasReflectAttribute)
            {
                xmlElement.SetAttribute(ATTRIBUTE_DYNAMIC, VALUE_REQUIRED_ALL);
            }
            if(HasMarshalDelegateAttribute || HasMarshalStructureAttribute)
            {
                IEnumerable<Type> delegates = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(t => t.GetTypes())
                        .Where(t => !t.IsClass && t.Assembly.FullName == assemblyName);
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
                        delegateElement.SetAttribute(RdElement.ATTRIBUTE_MARSHAL_DELEGATE, "Required All");
                    }
                    if (HasMarshalStructureAttribute)
                    {
                        delegateElement.SetAttribute(RdElement.ATTRIBUTE_MARSHAL_STRUCTURE, "Required All");
                    }
                    xmlElement.AppendChild(delegateElement);
                }
            }

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
        }

        public new string AssemblyName
        {
            get
            {
                return XmlElement.GetAttribute(ATTRIBUTE_NAME);
            }
        }
    }
}
