﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Peach.Core;
using Peach.Core.Dom;
using Peach.Core.Analyzers;
using Peach.Core.IO;

namespace Peach.Core.Test.Mutators
{
    [TestFixture]
    class UnicodeBadUtf8MutatorTests : DataModelCollector
    {
        [Test]
        public void Test1()
        {
            // standard test generating bad UTF-8 strings for each <String> element

            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                "<Peach>" +
                "   <DataModel name=\"TheDataModel\">" +
                "       <String name=\"str1\" value=\"Hello, World!\"/>" +
                "   </DataModel>" +

                "   <StateModel name=\"TheState\" initialState=\"Initial\">" +
                "       <State name=\"Initial\">" +
                "           <Action type=\"output\">" +
                "               <DataModel ref=\"TheDataModel\"/>" +
                "           </Action>" +
                "       </State>" +
                "   </StateModel>" +

                "   <Test name=\"Default\">" +
                "       <StateModel ref=\"TheState\"/>" +
                "       <Publisher class=\"Null\"/>" +
                "       <Strategy class=\"Sequential\"/>" +
                "   </Test>" +
                "</Peach>";

            PitParser parser = new PitParser();

            Dom.Dom dom = parser.asParser(null, new MemoryStream(ASCIIEncoding.ASCII.GetBytes(xml)));
            dom.tests[0].includedMutators = new List<string>();
            dom.tests[0].includedMutators.Add("UnicodeBadUtf8Mutator");

            RunConfiguration config = new RunConfiguration();

            Engine e = new Engine(null);
            e.config = config;
            e.startFuzzing(dom, config);

            // verify first two values, last two values, and count (= 151)
            byte[] val1 = { 0xf0, 0x80, 0x81, 0x9b, 0xf8, 0x80, 0x80, 0x81, 0xa1, 0xc1, 0x97, 0x41, 0xe0, 0x81, 0x85, 0xf0, 0x80, 0x81, 0xb1, 0xc1, 0xa3, 0xf0, 0x80, 0x81, 0xab, 0xc1, 0x98, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0x91, 0xe0, 0x80, 0xbb, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x88, 0xf8, 0x80, 0x80, 0x80, 0xa5, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xba, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xb8, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xb5, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xb4, 0xf0, 0x80, 0x80, 0xb7, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xbb, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x97, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xb9, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xa7, 0xc0, 0xa9, 0xf8, 0x80, 0x80, 0x80, 0xb1, 0xe0, 0x80, 0xb2, 0xf0, 0x80, 0x81, 0x92, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xb7, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x87, 0xf0, 0x80, 0x81, 0xa1, 0xf0, 0x80, 0x81, 0xb1, 0x4C, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xb4, 0x61, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xba, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xb3, 0xf8, 0x80, 0x80, 0x80, 0xb8, 0xf8, 0x80, 0x80, 0x81, 0xaf, 0xf8, 0x80, 0x80, 0x80, 0xab, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xb8, 0xf0, 0x80, 0x81, 0xad, 0x2B, 0x4C, 0xf8, 0x80, 0x80, 0x81, 0xac, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xa9, 0xe0, 0x80, 0xaa, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xac, 0xf8, 0x80, 0x80, 0x80, 0xb9, 0xf0, 0x80, 0x81, 0x88, 0xe0, 0x81, 0xa5, 0xc1, 0xa8, 0xc0, 0xa1, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xba, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x82, 0xf0, 0x80, 0x81, 0x87, 0xf0, 0x80, 0x81, 0x84, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xb7, 0xc1, 0xa0, 0xe0, 0x81, 0xb8, 0xf0, 0x80, 0x81, 0x82, 0xf0, 0x80, 0x80, 0xbc, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x8d, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0x84, 0xc1, 0xb1, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x90, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xaf, 0x21, 0x3E, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0x9e, 0xf0, 0x80, 0x80, 0xa1, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xa8, 0xf8, 0x80, 0x80, 0x80, 0xbc, 0xc1, 0x94, 0xf8, 0x80, 0x80, 0x81, 0xb1, 0x5F, 0xf8, 0x80, 0x80, 0x80, 0xb8, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xbc, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xa8, 0xc0, 0xa2, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xa4, 0xf8, 0x80, 0x80, 0x81, 0xa4, 0xf8, 0x80, 0x80, 0x81, 0x83, 0xe0, 0x81, 0x90, 0xf0, 0x80, 0x80, 0xb3, 0xf8, 0x80, 0x80, 0x81, 0xa4, 0xf0, 0x80, 0x81, 0x8d, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xb9, 0xf0, 0x80, 0x81, 0xb4, 0xf0, 0x80, 0x80, 0xa1, 0x25, 0xf0, 0x80, 0x81, 0xb4, 0xf8, 0x80, 0x80, 0x81, 0x9f, 0x25, 0xf8, 0x80, 0x80, 0x81, 0x85, 0xc1, 0x9f, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xbe, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xb4, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xb5, 0xf0, 0x80, 0x80, 0xbd, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x97, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x9c, 0xc0, 0xa7, 0xc1, 0x9a, 0xf8, 0x80, 0x80, 0x81, 0x84, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0x9f, 0xf8, 0x80, 0x80, 0x80, 0xa9, 0xc0, 0xad, 0xf0, 0x80, 0x81, 0xab, 0xf0, 0x80, 0x80, 0xb3, 0xe0, 0x80, 0xa8, 0xf0, 0x80, 0x80, 0xb6, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xa7, 0xe0, 0x81, 0xb1, 0xc0, 0xa6, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xa8, 0xc1, 0x8f, 0xf0, 0x80, 0x81, 0x88, 0xf0, 0x80, 0x81, 0x9c, 0xc0, 0xb1, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0x81, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xbc, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x8b, 0xc0, 0xa9, 0xe0, 0x81, 0xbb, 0xf0, 0x80, 0x81, 0x91, 0xc1, 0xa3, 0xf8, 0x80, 0x80, 0x81, 0xb0, 0xf0, 0x80, 0x80, 0xa0, 0xf0, 0x80, 0x81, 0xb5, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xae, 0xf8, 0x80, 0x80, 0x81, 0xb3, 0xe0, 0x80, 0xbb, 0x00, 0xc0, 0xb4, 0xc1, 0x92, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xb5, 0x78, 0xe0, 0x81, 0xb6, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xa3, 0xf0, 0x80, 0x81, 0x92, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xa9, 0xf0, 0x80, 0x80, 0xbc, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xb1, 0xc1, 0xaa, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xb6, 0xf8, 0x80, 0x80, 0x80, 0xb5, 0xf0, 0x80, 0x80, 0xb7, 0x72, 0xf0, 0x80, 0x80, 0xbf, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xba, 0xe0, 0x80, 0xb8, 0xe0, 0x80, 0xb4, 0xc1, 0xba, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0x87, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xa7, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0x95, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xb5, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0x9a, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0x85, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xa3, 0xc1, 0x9b, 0xe0, 0x81, 0xb2, 0x25, 0xe0, 0x80, 0xae, 0xf8, 0x80, 0x80, 0x81, 0x98, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xb1, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0x94, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xa1, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xb8, 0x53, 0xe0, 0x80, 0xa0, 0x58, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xa2, 0xc1, 0xb9, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xa6, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xbd, 0x00, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xa9, 0xf8, 0x80, 0x80, 0x81, 0xb4, 0xe0, 0x81, 0xb4, 0xf0, 0x80, 0x80, 0xb0, 0xf0, 0x80, 0x80, 0xa2, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0x94, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x92, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xba, 0xf8, 0x80, 0x80, 0x81, 0xb9, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xae, 0x4D, 0xf0, 0x80, 0x81, 0xae, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x88, 0xf0, 0x80, 0x81, 0xa7, 0x6A, 0xc1, 0xa8, 0xf0, 0x80, 0x81, 0xb0, 0xe0, 0x80, 0xbc, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xb8, 0xf0, 0x80, 0x80, 0xa1, 0xf8, 0x80, 0x80, 0x80, 0xa1, 0xe0, 0x81, 0x8c, 0xc0, 0xb6, 0x50, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xa5, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xaa, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xbb, 0xf8, 0x80, 0x80, 0x81, 0x95, 0x40, 0xf8, 0x80, 0x80, 0x80, 0xa3, 0xf8, 0x80, 0x80, 0x80, 0xa2, 0xf8, 0x80, 0x80, 0x81, 0xaa, 0xf8, 0x80, 0x80, 0x81, 0xb7, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xb3, 0xc1, 0x8d, 0xe0, 0x81, 0x8b, 0xc1, 0xab, 0xf0, 0x80, 0x80, 0xba, 0xf0, 0x80, 0x81, 0xad, 0xc0, 0xae, 0x5C, 0x78, 0x63, 0x01, 0x9d, 0x5C, 0xe0, 0x80, 0xad };
            byte[] val2 = { 0xe0, 0x81, 0xa5, 0xc0, 0xa7, 0x2F, 0xf8, 0x80, 0x80, 0x80, 0xa0, 0xf0, 0x80, 0x80, 0xb7, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0x95, 0xe0, 0x81, 0x92, 0x08, 0xf8, 0x80, 0x80, 0x81, 0x81, 0xf0, 0x80, 0x80, 0xbc, 0x01, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xb3, 0xf0, 0x80, 0x80, 0xbe, 0xe0, 0x80, 0xa4, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xaf, 0xf0, 0x80, 0x81, 0x9e, 0xf0, 0x80, 0x81, 0xb2, 0xf0, 0x80, 0x81, 0xb0, 0xc1, 0xbc, 0xe0, 0x81, 0x97, 0xc1, 0x8d, 0x63, 0x3A, 0x59, 0xf0, 0x80, 0x80, 0xa6, 0xf0, 0x80, 0x81, 0x9c, 0xe0, 0x80, 0xb7, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xa7, 0xe0, 0x81, 0x8d, 0xe0, 0x81, 0xa7, 0xf8, 0x80, 0x80, 0x81, 0x8e, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xaf, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xa0, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xb7, 0xe0, 0x80, 0xb4, 0xe0, 0x80, 0xb7 };
            byte[] val3 = { 0xf0, 0x80, 0x81, 0x92, 0xc1, 0xa3, 0xf8, 0x80, 0x80, 0x81, 0x9d, 0xf0, 0x80, 0x81, 0xad, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xa1, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xb9, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x8d, 0xf0, 0x80, 0x81, 0xad, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x96, 0x5A, 0xe0, 0x81, 0xa4, 0xf0, 0x80, 0x81, 0x9e, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xa2, 0x74, 0xf8, 0x80, 0x80, 0x81, 0x86, 0xf0, 0x80, 0x80, 0xb3, 0xc1, 0xb4, 0x23, 0xf0, 0x80, 0x80, 0xb9, 0xf8, 0x80, 0x80, 0x81, 0xaa, 0xe0, 0x80, 0xa4, 0xc1, 0xaf, 0xf0, 0x80, 0x81, 0x8d, 0x45, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xb3, 0xe0, 0x80, 0xbb, 0xe0, 0x80, 0xae, 0xf0, 0x80, 0x81, 0x8d, 0xf0, 0x80, 0x81, 0xb9, 0xf8, 0x80, 0x80, 0x81, 0xa4, 0xe0, 0x80, 0xb2, 0xe0, 0x80, 0xbd, 0xf8, 0x80, 0x80, 0x81, 0x95, 0xc0, 0xad, 0xe0, 0x80, 0xa9, 0xe0, 0x81, 0x87, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xbc, 0xc1, 0x90, 0xf8, 0x80, 0x80, 0x81, 0x9f };
            byte[] val4 = { 0xc1, 0x86, 0x42, 0xf8, 0x80, 0x80, 0x80, 0xba, 0xe0, 0x81, 0x85, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x8b, 0xf8, 0x80, 0x80, 0x81, 0xaa, 0xe0, 0x80, 0xbf, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0x89, 0xf8, 0x80, 0x80, 0x81, 0xa7, 0xf8, 0x80, 0x80, 0x81, 0x84, 0xc1, 0x85, 0xe0, 0x80, 0xb3, 0xf0, 0x80, 0x80, 0xa6, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xaf, 0xf8, 0x80, 0x80, 0x81, 0xa7, 0xc1, 0xb8, 0x71, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x8a, 0xc1, 0xa4, 0xf8, 0x80, 0x80, 0x81, 0x8d, 0xf0, 0x80, 0x81, 0x8e, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0x92, 0xe0, 0x80, 0xa7, 0xc1, 0x86, 0xe0, 0x81, 0x85, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xae, 0xe0, 0x81, 0xa7, 0xe0, 0x81, 0xb4, 0xe0, 0x81, 0x96, 0xf8, 0x80, 0x80, 0x81, 0xb7, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x8f, 0x5C, 0xf8, 0x80, 0x80, 0x81, 0x8c, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xaf, 0xf8, 0x80, 0x80, 0x80, 0xbd, 0xc1, 0x8a, 0xe0, 0x81, 0xb2, 0xe0, 0x81, 0xbd, 0xf0, 0x80, 0x80, 0xac, 0xc0, 0xa6, 0xe0, 0x81, 0xb4, 0xf8, 0x80, 0x80, 0x81, 0xa1, 0xf0, 0x80, 0x80, 0xaa, 0x5A, 0xe0, 0x80, 0xa3, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xa6, 0x48, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x9e, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xab, 0x59, 0x07, 0xc1, 0xb2, 0xf8, 0x80, 0x80, 0x81, 0x81, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xb5, 0xf8, 0x80, 0x80, 0x81, 0x8a, 0xf8, 0x80, 0x80, 0x81, 0x8a, 0xfc, 0x80, 0x80, 0x80, 0x81, 0x93, 0x49, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0x81, 0xf0, 0x80, 0x81, 0xa2, 0xc1, 0x93, 0xf8, 0x80, 0x80, 0x80, 0xbd, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xa1, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x80, 0xb7, 0xf0, 0x80, 0x81, 0x96, 0xf0, 0x80, 0x81, 0x9b, 0xe0, 0x80, 0xbf, 0xe0, 0x80, 0xb8, 0x06, 0xf8, 0x80, 0x80, 0x80, 0xa0, 0x7D, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0xa7, 0xfe, 0x80, 0x80, 0x80, 0x80, 0x81, 0x9b, 0xe0, 0x81, 0x82, 0xf0, 0x80, 0x80, 0xbb, 0x50, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xb8, 0xc0, 0xa0, 0x53, 0xfc, 0x80, 0x80, 0x80, 0x80, 0xb7, 0xf0, 0x80, 0x80, 0xac, 0xc0, 0xa4, 0xfc, 0x80, 0x80, 0x80, 0x81, 0xb1, 0xf8, 0x80, 0x80, 0x81, 0xa6 };

            Assert.AreEqual(151, mutations.Count);
            Assert.AreEqual(val1, (byte[])mutations[0]);
            Assert.AreEqual(val2, (byte[])mutations[1]);
            Assert.AreEqual(val3, (byte[])mutations[mutations.Count - 2]);
            Assert.AreEqual(val4, (byte[])mutations[mutations.Count - 1]);
        }
    }
}

// end
