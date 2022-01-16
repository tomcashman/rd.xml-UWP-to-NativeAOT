/**
 * Apache License
 * Version 2.0, January 2004
 * http://www.apache.org/licenses/
 * 
 * Copyright 2022 Thomas Cashman
 * 
 * See LICENSE file
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RdXml.Test.Sample;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace RdXml.Test
{
    [TestClass]
    public class TypeTests
    {
        private XmlWriterSettings xmlWriterSettings;

        [TestInitialize]
        public void TestInit()
        {
            xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.OmitXmlDeclaration = true;
        }
        
        [TestMethod]
        public void TestSubTypes()
        {
            string inputXml = @"<Directives xmlns=""http://schemas.microsoft.com/netfx/2013/01/metadata\"">
   <Application>
      <Type Name=""RdXml.RdElement"">
        <Subtypes Activate=""Public"" Dynamic=""Public""/>
      </Type>
     </Application>
   </Directives>";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(inputXml);
            RdDirective directive = new RdDirective(xmlDocument);
            Assert.IsNotNull(directive.Application);
            Assert.AreEqual(1, directive.Application.Types.Count);
            Assert.IsNotNull(directive.Application.Types[0].SubTypes);

            StringWriter result = new StringWriter();
            directive.WriteNativeAOT(XmlWriter.Create(result, xmlWriterSettings));

            string expectedXml = @"<Directives>
  <Application>
    <Type Name=""RdXml.RdElement"" />
    <Type Name=""RdXml.RdAssembly"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdAttributeImplies"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdDirective"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdDll"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdEvent"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdField"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdGenericParameter"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdMethod"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdMethodInstantiation"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdNamespace"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdProperty"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdSubtypes"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdType"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdTypeInstantiation"" Dynamic=""Required All"" />
  </Application>
</Directives>";
            Assert.AreEqual(expectedXml, result.ToString());
        }

        [TestMethod]
        public void TestNamespace()
        {
            string inputXml = @"<Directives xmlns=""http://schemas.microsoft.com/netfx/2013/01/metadata\"">
   <Application>
      <Namespace Name=""RdXml"" Dynamic=""Public"" />
     </Application>
   </Directives>";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(inputXml);
            RdDirective directive = new RdDirective(xmlDocument);
            Assert.IsNotNull(directive.Application);
            Assert.AreEqual(1, directive.Application.Namespaces.Count);

            StringWriter result = new StringWriter();
            directive.WriteNativeAOT(XmlWriter.Create(result, xmlWriterSettings));

            string expectedXml = @"<Directives>
  <Application>
    <Type Name=""RdXml.RdAssembly"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdAttributeImplies"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdDirective"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdDll"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdElement"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdEvent"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdField"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdGenericParameter"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdMethod"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdMethodInstantiation"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdNamespace"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdProperty"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdSubtypes"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdType"" Dynamic=""Required All"" />
    <Type Name=""RdXml.RdTypeInstantiation"" Dynamic=""Required All"" />
  </Application>
</Directives>";
            Assert.AreEqual(expectedXml, result.ToString());
        }

        [TestMethod]
        public void TestAttributeImplies()
        {
            Type type = typeof(TestAttribute);

            string inputXml = @"<Directives xmlns=""http://schemas.microsoft.com/netfx/2013/01/metadata\"">
   <Application>
      <Type Name=""RdXml.Test.Sample.TestAttribute"">
        <AttributeImplies Dynamic=""Public""/>
      </Type>
     </Application>
   </Directives>";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(inputXml);
            RdDirective directive = new RdDirective(xmlDocument);
            Assert.IsNotNull(directive.Application);
            Assert.AreEqual(1, directive.Application.Types.Count);

            StringWriter result = new StringWriter();
            directive.WriteNativeAOT(XmlWriter.Create(result, xmlWriterSettings));

            string expectedXml = @"<Directives>
  <Application>
    <Type Name=""RdXml.Test.Sample.TestAttribute"" />
    <Type Name=""RdXml.Test.Sample.TestClass"" Dynamic=""Required All"" />
  </Application>
</Directives>";
            Assert.AreEqual(expectedXml, result.ToString());
        }
    }
}
