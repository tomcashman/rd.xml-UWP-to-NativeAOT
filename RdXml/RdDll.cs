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

namespace RdXml
{
    public class RdDll : RdElement
    {
        public List<RdAssembly> Assemblies { get; } = new List<RdAssembly>();
        public List<RdNamespace> Namespaces { get; } = new List<RdNamespace>();
        public List<RdType> Types { get; } = new List<RdType>();
        public List<RdTypeInstantiation> TypeInstantiations { get; } = new List<RdTypeInstantiation>();

        public RdDll(RdElement parent, XmlElement xmlElement) : base(parent, xmlElement)
        {
            XmlNodeList assemblyNodeList = xmlElement.GetElementsByTagName(RdAssembly.ELEMENT_TAG);
            foreach(XmlNode assemblyNode in assemblyNodeList)
            {
                Assemblies.Add(new RdAssembly(this, (XmlElement) assemblyNode));
            }
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
            XmlElement xmlElement = xmlDocument.CreateElement(XmlElement.Name);
            parentElement.AppendChild(xmlElement);

            foreach(RdAssembly assembly in Assemblies)
            {
                assembly.WriteNativeAOT(xmlDocument, xmlElement, writtenTypes);
            }
            foreach(RdNamespace @namespace in Namespaces)
            {
                @namespace.WriteNativeAOT(xmlDocument, xmlElement, writtenTypes);
            }
            foreach(RdType type in Types)
            {
                type.WriteNativeAOT(xmlDocument, xmlElement, writtenTypes);
            }
            foreach(RdTypeInstantiation typeInstantiation in TypeInstantiations)
            {
                typeInstantiation.WriteNativeAOT(xmlDocument, xmlElement, writtenTypes);
            }
        }
    }
}
