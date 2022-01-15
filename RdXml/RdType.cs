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
    public class RdType : RdTypeInstantiation
    {
        public new const string ELEMENT_TAG = "Type";

        public RdSubtypes SubTypes { get; private set; }
        public RdAttributeImplies AttributeImplies { get; private set; }
        public List<RdGenericParameter> GenericParameters { get; } = new List<RdGenericParameter>();

        public RdType(RdElement parent, XmlElement xmlElement) : base(parent, xmlElement)
        {
            XmlNodeList subtypeList = xmlElement.GetElementsByTagName(RdSubtypes.ELEMENT_TAG);
            if(subtypeList.Count > 0)
            {
                SubTypes = new RdSubtypes(this, (XmlElement)subtypeList[0]);
            }

            XmlNodeList attributeImplesList = xmlElement.GetElementsByTagName(RdAttributeImplies.ELEMENT_TAG);
            if (attributeImplesList.Count > 0)
            {
                AttributeImplies = new RdAttributeImplies(this, (XmlElement)attributeImplesList[0]);
            }

            XmlNodeList genericParametersList = xmlElement.GetElementsByTagName(RdGenericParameter.ELEMENT_TAG);
            foreach (XmlNode genericParameterNode in genericParametersList)
            {
                GenericParameters.Add(new RdGenericParameter(this, (XmlElement)genericParameterNode));
            }
        }

        public override void WriteNativeAOT(XmlDocument xmlDocument, XmlElement parentElement, HashSet<Type> writtenTypes)
        {
            base.WriteNativeAOT(xmlDocument, parentElement, writtenTypes);
        }
    }
}
