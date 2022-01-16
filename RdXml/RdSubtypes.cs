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
    public class RdSubtypes : RdElement
    {
        public const string ELEMENT_TAG = "Subtypes";

        public RdSubtypes(RdElement parent, XmlElement xmlElement) : base(parent, xmlElement)
        {
        }

        public override void WriteNativeAOT(XmlDocument xmlDocument, XmlElement parentElement, HashSet<Type> writtenTypes)
        {
            RdType parentTypeElement = (RdType)Parent;
            string typeName = parentTypeElement.TypeName;
            Type resolvedType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t => t.FullName == typeName).First();
            IEnumerable<Type> subtypes = AppDomain.CurrentDomain.GetAssemblies()
                                    .SelectMany(t => t.GetTypes())
                                    .Where(t => resolvedType.IsAssignableFrom(t));
            foreach (Type subtype in subtypes)
            {
                AppendType(xmlDocument, parentElement, writtenTypes, subtype, 
                    HasReflectAttribute, 
                    HasMarshalDelegateAttribute, 
                    HasMarshalStructureAttribute);
            }
        }
    }
}
