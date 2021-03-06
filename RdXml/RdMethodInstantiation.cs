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
    public class RdMethodInstantiation : RdElement
    {
        public const string ELEMENT_TAG = "MethodInstantiation";

        public RdMethodInstantiation(RdElement parent, XmlElement xmlElement) : base(parent, xmlElement)
        {
        }

        public override void WriteNativeAOT(XmlDocument result, XmlElement parentElement, HashSet<Type> writtenTypes)
        {
            throw new NotImplementedException();
        }
    }
}
