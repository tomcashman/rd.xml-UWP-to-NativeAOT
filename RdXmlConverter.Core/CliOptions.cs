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
using System;
using System.Collections.Generic;
using System.Text;

namespace RdXmlConverter
{
    class CliOptions
    {
        [Option("rdxml", Required = true, HelpText = "Input UWP rd.xml file")]
        public string InputRdXml { get; set; }
        [Option('o', "output", Required = true, HelpText = "Output NativeAOT rd.xml file")]
        public string OutputRdXml { get; set; }
        [Option('i', "input", Required = true, HelpText = "Input directory containing DLLs")]
        public string InputDllDirectory { get; set; }
    }
}
