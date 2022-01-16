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
    public class RdMethod : RdElement
    {
        public const string ELEMENT_TAG = "Method";

        public RdMethod(RdElement parent, XmlElement xmlElement) : base(parent, xmlElement)
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

            foreach(MethodInfo methodInfo in parentType.GetMethods())
            {
                if(!methodInfo.Name.Equals(XmlElement.GetAttribute(ATTRIBUTE_NAME)))
                {
                    continue;
                }
                if(methodInfo.ReturnType != null)
                {
                    AppendType(result, parentElement, writtenTypes, methodInfo.ReturnType, true, false, false);
                }
                ParameterInfo[] parameters = methodInfo.GetParameters();
                if(parameters == null)
                {
                    continue;
                }
                foreach(ParameterInfo parameter in parameters)
                {
                    AppendType(result, parentElement, writtenTypes, parameter.ParameterType, true, false, false);
                }
            }
        }
    }
}
