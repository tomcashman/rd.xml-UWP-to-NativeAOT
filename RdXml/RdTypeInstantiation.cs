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
    public class RdTypeInstantiation : RdElement
    {
        public const string ELEMENT_TAG = "TypeInstantiation";

        public List<RdType> Types { get; } = new List<RdType>();
        public List<RdTypeInstantiation> TypeInstantiations { get; } = new List<RdTypeInstantiation>();
        public List<RdMethod> Methods { get; } = new List<RdMethod>();
        public List<RdMethodInstantiation> MethodInstantiations { get; } = new List<RdMethodInstantiation>();
        public List<RdProperty> Properties { get; } = new List<RdProperty>();
        public List<RdField> Fields { get; } = new List<RdField>();
        public List<RdEvent> Events { get; } = new List<RdEvent>();

        public RdTypeInstantiation(RdElement parent, XmlElement xmlElement) : base(parent, xmlElement)
        {
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

            XmlNodeList methodList = xmlElement.GetElementsByTagName(RdMethod.ELEMENT_TAG);
            foreach (XmlNode methodNode in methodList)
            {
                Methods.Add(new RdMethod(this, (XmlElement)methodNode));
            }

            XmlNodeList methodInstantiationList = xmlElement.GetElementsByTagName(RdMethodInstantiation.ELEMENT_TAG);
            foreach (XmlNode methodInstantiationNode in methodInstantiationList)
            {
                MethodInstantiations.Add(new RdMethodInstantiation(this, (XmlElement)methodInstantiationNode));
            }

            XmlNodeList propertyList = xmlElement.GetElementsByTagName(RdProperty.ELEMENT_TAG);
            foreach (XmlNode propertyNode in propertyList)
            {
                Properties.Add(new RdProperty(this, (XmlElement)propertyNode));
            }

            XmlNodeList fieldList = xmlElement.GetElementsByTagName(RdField.ELEMENT_TAG);
            foreach (XmlNode fieldNode in fieldList)
            {
                Fields.Add(new RdField(this, (XmlElement)fieldNode));
            }

            XmlNodeList eventList = xmlElement.GetElementsByTagName(RdEvent.ELEMENT_TAG);
            foreach (XmlNode eventNode in eventList)
            {
                Events.Add(new RdEvent(this, (XmlElement)eventNode));
            }
        }

        public override void WriteNativeAOT(XmlDocument xmlDocument, XmlElement parentElement, HashSet<Type> writtenTypes)
        {
            string typeName = TypeName;
            Type resolvedType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t => t.FullName == typeName).First();
            if (writtenTypes.Add(resolvedType))
            {
                XmlElement xmlElement;

                if(!(this is RdType))
                {
                    //TypeInstantiation has additional Arguments field
                    //TODO: Implement Arguments field support
                    xmlElement = xmlDocument.CreateElement(XmlElement.LocalName);
                    xmlElement.SetAttribute(ATTRIBUTE_NAME, TypeName);
                    parentElement.AppendChild(xmlElement);
                }
                else
                {
                    xmlElement = xmlDocument.CreateElement(XmlElement.LocalName);
                    xmlElement.SetAttribute(ATTRIBUTE_NAME, TypeName);
                    parentElement.AppendChild(xmlElement);
                }

                if (HasReflectAttribute)
                {
                    xmlElement.SetAttribute(RdElement.ATTRIBUTE_DYNAMIC, VALUE_REQUIRED_ALL);
                }
                if (HasMarshalDelegateAttribute)
                {
                    xmlElement.SetAttribute(RdElement.ATTRIBUTE_MARSHAL_DELEGATE, VALUE_REQUIRED_ALL);
                }
                if (HasMarshalStructureAttribute)
                {
                    xmlElement.SetAttribute(RdElement.ATTRIBUTE_MARSHAL_STRUCTURE, VALUE_REQUIRED_ALL);
                }
            }

            foreach (RdType type in Types)
            {
                type.WriteNativeAOT(xmlDocument, parentElement, writtenTypes);
            }
            foreach (RdTypeInstantiation typeInstantiation in TypeInstantiations)
            {
                typeInstantiation.WriteNativeAOT(xmlDocument, parentElement, writtenTypes);
            }
            foreach (RdMethod method in Methods)
            {
                method.WriteNativeAOT(xmlDocument, parentElement, writtenTypes);
            }
            foreach (RdMethodInstantiation methodInstantiation in MethodInstantiations)
            {
                methodInstantiation.WriteNativeAOT(xmlDocument, parentElement, writtenTypes);
            }
            foreach (RdProperty property in Properties)
            {
                property.WriteNativeAOT(xmlDocument, parentElement, writtenTypes);
            }
            foreach (RdField field in Fields)
            {
                field.WriteNativeAOT(xmlDocument, parentElement, writtenTypes);
            }
            foreach (RdEvent @event in Events)
            {
                @event.WriteNativeAOT(xmlDocument, parentElement, writtenTypes);
            }
        }

        public string TypeName
        {
            get
            {
                StringBuilder result = new StringBuilder();
                string thisName = XmlElement.GetAttribute(ATTRIBUTE_NAME);
                if (Parent is RdNamespace @namespace && !(Parent is RdAssembly))
                {
                    string parentNamespace = @namespace.NamespaceName;
                    if (!thisName.StartsWith(parentNamespace))
                    {
                        result.Append(parentNamespace);
                        if (!parentNamespace.EndsWith("."))
                        {
                            result.Append('.');
                        }
                    }
                }
                else if (Parent is RdType type)
                {
                    string parentTypeName = type.TypeName;
                    if (!thisName.StartsWith(parentTypeName))
                    {
                        result.Append(parentTypeName);
                        if (!parentTypeName.EndsWith("."))
                        {
                            result.Append('.');
                        }
                    }
                }
                else if (Parent is RdTypeInstantiation typeInstantiation)
                {
                    string parentTypeName = typeInstantiation.TypeName;
                    if (!thisName.StartsWith(parentTypeName))
                    {
                        result.Append(parentTypeName);
                        if (!parentTypeName.EndsWith("."))
                        {
                            result.Append('.');
                        }
                    }
                }
                result.Append(thisName);
                return result.ToString();
            }
        }
    }
}
