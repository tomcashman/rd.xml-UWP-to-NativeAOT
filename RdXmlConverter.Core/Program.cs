/**
 * Apache License
 * Version 2.0, January 2004
 * http://www.apache.org/licenses/
 * 
 * Copyright 2022 Thomas Cashman
 * 
 * See LICENSE file
 */
using CommandLine;
using RdXml;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace RdXmlConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CliOptions>(args)
                   .WithParsed<CliOptions>(options =>
                   {
                       if(!Directory.Exists(options.InputDllDirectory))
                       {
                           Console.Error.WriteLine(options.InputDllDirectory + " is not a directory");
                           return;
                       }

                       Run(options);
                   });
        }

        static void Run(CliOptions options)
        {
            string [] dlls = Directory.GetFiles(options.InputDllDirectory, "*.dll");
            if(dlls.Length == 0)
            {
                Console.Error.WriteLine("No DLLs in specified directory");
                return;
            }
            foreach(string dllFilename in dlls)
            {
                Assembly.LoadFrom(dllFilename);
            }

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.OmitXmlDeclaration = true;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = true;
            xmlDocument.Load(options.InputRdXml);

            RdDirective directive = new RdDirective(xmlDocument);
            StringWriter result = new StringWriter();
            directive.WriteNativeAOT(XmlWriter.Create(result, xmlWriterSettings));
            File.WriteAllText(options.OutputRdXml, result.ToString());
        }
    }
}
