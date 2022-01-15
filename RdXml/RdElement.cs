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
    public abstract class RdElement
    {
        public const string ATTRIBUTE_NAME = "Name";
        public const string ATTRIBUTE_ACTIVATE = "Activate";
        public const string ATTRIBUTE_BROWSE = "Browse";
        public const string ATTRIBUTE_DYNAMIC = "Dynamic";
        public const string ATTRIBUTE_SERIALIZE = "Serialize";
        public const string ATTRIBUTE_DATA_CONTRACT_SERIALIZER = "DataContractSerializer";
        public const string ATTRIBUTE_DATA_CONTRACT_JSON_SERIALIZER = "DataContractJsonSerializer";
        public const string ATTRIBUTE_XML_SERIALIZER = "XmlSerializer";
        public const string ATTRIBUTE_MARSHAL_OBJECT = "MarshalObject";
        public const string ATTRIBUTE_MARSHAL_DELEGATE = "MarshalDelegate";
        public const string ATTRIBUTE_MARSHAL_STRUCTURE = "MarshalStructure";
        public const string VALUE_REQUIRED_ALL = "Required All";

        protected RdElement Parent { get; }
        protected XmlElement XmlElement { get; }

        public RdElement(RdElement parent, XmlElement xmlElement)
        {
            Parent = parent;
            XmlElement = xmlElement;
        }

        public abstract void WriteNativeAOT(XmlDocument result, XmlElement parentElement, HashSet<Type> writtenTypes);

        public bool HasReflectAttribute
        {
            get
            {
                if(XmlElement.HasAttribute(ATTRIBUTE_ACTIVATE))
                {
                    return true;
                }
                if (XmlElement.HasAttribute(ATTRIBUTE_BROWSE))
                {
                    return true;
                }
                if (XmlElement.HasAttribute(ATTRIBUTE_DYNAMIC))
                {
                    return true;
                }
                if (XmlElement.HasAttribute(ATTRIBUTE_SERIALIZE))
                {
                    return true;
                }
                if (XmlElement.HasAttribute(ATTRIBUTE_DATA_CONTRACT_SERIALIZER))
                {
                    return true;
                }
                if (XmlElement.HasAttribute(ATTRIBUTE_DATA_CONTRACT_JSON_SERIALIZER))
                {
                    return true;
                }
                if (XmlElement.HasAttribute(ATTRIBUTE_XML_SERIALIZER))
                {
                    return true;
                }
                return false;
            }
        }

        public bool HasMarshalDelegateAttribute
        {
            get
            {
                if(XmlElement.HasAttribute(ATTRIBUTE_MARSHAL_OBJECT))
                {
                    return true;
                }
                return XmlElement.HasAttribute(ATTRIBUTE_MARSHAL_DELEGATE);
            }
        }

        public bool HasMarshalStructureAttribute
        {
            get
            {
                if (XmlElement.HasAttribute(ATTRIBUTE_MARSHAL_OBJECT))
                {
                    return true;
                }
                return XmlElement.HasAttribute(ATTRIBUTE_MARSHAL_STRUCTURE);
            }
        }
    }
}
