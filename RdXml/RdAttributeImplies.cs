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
    public class RdAttributeImplies : RdElement
    {
        public const string ELEMENT_TAG = "AttributeImplies";

        public RdAttributeImplies(RdElement parent, XmlElement xmlElement) : base(parent, xmlElement)
        {
        }

        public override void WriteNativeAOT(XmlDocument xmlDocument, XmlElement parentElement, HashSet<Type> writtenTypes)
        {
            RdType parentTypeElement = (RdType)Parent;
            string typeName = parentTypeElement.TypeName;
            Type resolvedType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t => t.FullName == typeName).First();

            IEnumerable<Type> typesWithAttribute = AppDomain.CurrentDomain.GetAssemblies()
                                    .SelectMany(t => t.GetTypes())
                                    .Where(t => t.GetCustomAttributes(resolvedType, true).Length > 0);
            foreach (Type type in typesWithAttribute)
            {
                AppendType(xmlDocument, parentElement, writtenTypes, type,
                    HasReflectAttribute,
                    HasMarshalDelegateAttribute,
                    HasMarshalStructureAttribute);
            }
        }
    }
}
