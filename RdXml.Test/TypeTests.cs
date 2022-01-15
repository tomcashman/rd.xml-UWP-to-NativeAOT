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
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RdXml.Test
{
    [TestClass]
    public class TypeTests
    {
        [TestMethod]
        public void TestSubType()
        {
            string inputXml = @"<Directives xmlns=""http://schemas.microsoft.com/netfx/2013/01/metadata\"">
   <Application>
      <Type Name=""Dictionary"">
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
        }
    }
}
