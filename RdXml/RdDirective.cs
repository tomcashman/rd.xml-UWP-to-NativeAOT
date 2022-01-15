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
using System.IO;
using System.Text;
using System.Xml;

namespace RdXml
{
    public class RdDirective : RdElement
    {
        public const string ELEMENT_TAG = "Directives";
        public const string APPLICATION_TAG = "Application";
        public const string LIBRARY_TAG = "Library";

        public RdDll Application { get; private set; }
        public List<RdDll> Libraries { get; } = new List<RdDll>();

        public RdDirective(XmlDocument xmlDocument) : base(null, xmlDocument.DocumentElement)
        {
            XmlNodeList applicationNodeList = xmlDocument.GetElementsByTagName(APPLICATION_TAG);
            if(applicationNodeList.Count > 0)
            {
                Application = new RdDll(this, (XmlElement) applicationNodeList[0]);
            }

            XmlNodeList libraryNodeList = xmlDocument.GetElementsByTagName(LIBRARY_TAG);
            foreach(XmlNode xmlNode in libraryNodeList)
            {
                Libraries.Add(new RdDll(this, (XmlElement) xmlNode));
            }
        }

        public void WriteNativeAOT(Stream outputStream)
        {
            XmlDocument result = new XmlDocument();
            HashSet<Type> writtenTypes = new HashSet<Type>();
            WriteNativeAOT(result, result.DocumentElement, writtenTypes);
            result.Save(outputStream);
        }

        public override void WriteNativeAOT(XmlDocument result, XmlElement parentElement, HashSet<Type> writtenTypes)
        {
            XmlElement rootElement = result.CreateElement(ELEMENT_TAG);
            result.AppendChild(rootElement);

            if(Application != null)
            {
                Application.WriteNativeAOT(result, rootElement, writtenTypes);
            }
            foreach(RdDll dll in Libraries)
            {
                dll.WriteNativeAOT(result, rootElement, writtenTypes);
            }
        }

        public static RdDirective FromStream(Stream stream)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);
            return new RdDirective(xmlDocument);
        }

        public static RdDirective FromTextReader(TextReader textReader)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(textReader);
            return new RdDirective(xmlDocument);
        }

        public static RdDirective FromXmlReader(XmlReader xmlReader)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlReader);
            return new RdDirective(xmlDocument);
        }
    }
}
