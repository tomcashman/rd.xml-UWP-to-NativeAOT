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
using System.Reflection;
using System.Text;
using System.Xml;
using System.Linq;

namespace RdXml
{
    public class RdField : RdElement
    {
        public const string ELEMENT_TAG = "Field";

        public RdField(RdElement parent, XmlElement xmlElement) : base(parent, xmlElement)
        {
        }

        public override void WriteNativeAOT(XmlDocument result, XmlElement parentElement, HashSet<Type> writtenTypes)
        {
            RdType parentTypeElement = (RdType)Parent;
            string typeName = parentTypeElement.TypeName;
            Type parentType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t => t.FullName == typeName).First();
            AppendType(result, parentElement, writtenTypes, parentType, true, false, false);

            FieldInfo fieldInfo = parentType.GetField(XmlElement.GetAttribute(ATTRIBUTE_NAME));
            if (fieldInfo == null)
            {
                return;
            }
            AppendType(result, parentElement, writtenTypes, fieldInfo.FieldType, true, false, false);
        }
    }
}
