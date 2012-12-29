﻿
//
// Copyright (c) Michael Eddington
//
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in	
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

// Authors:
//   Michael Eddington (mike@dejavusecurity.com)

// $Id$

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Peach.Core;
using Peach.Core.Dom;
using Peach.Core.Analyzers;
using Peach.Core.Cracker;
using Peach.Core.IO;

namespace Peach.Core.Test.CrackingTests
{
	[TestFixture]
	public class ChoiceTests
	{
		[Test]
		public void CrackChoice1()
		{
			string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Peach>\n" +
				"	<DataModel name=\"TheDataModel\">" +
				"		<Choice>"  +
				"			<Blob name=\"Blob10\" length=\"10\" />" +
				"			<Blob name=\"Blob5\" length=\"5\" />"   +
				"		</Choice>" +
				"	</DataModel>"  +
				"</Peach>";

			PitParser parser = new PitParser();
			Dom.Dom dom = parser.asParser(null, new MemoryStream(ASCIIEncoding.ASCII.GetBytes(xml)));

			BitStream data = new BitStream();
			data.LittleEndian();
			data.WriteBytes(new byte[] { 1, 2, 3, 4, 5 });
			data.SeekBits(0, SeekOrigin.Begin);

			DataCracker cracker = new DataCracker();
			cracker.CrackData(dom.dataModels[0], data);

			Assert.IsTrue(dom.dataModels[0][0] is Choice);
			Assert.AreEqual("Blob5", ((Choice)dom.dataModels[0][0])[0].name);
			Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5 }, (byte[])((DataElementContainer)dom.dataModels[0][0])[0].DefaultValue);
		}

		[Test]
		public void MinOccurs0()
		{
			string xml = @"
<Peach>
	<DataModel name=""DM_choice1"">
		<Blob name=""smallData"" length=""2""/>
		<Number size=""32"" token=""true"" value=""1""/>
	</DataModel>

	<DataModel name=""DM_choice2"">
		<Blob name=""BigData"" length=""10""/>
		<Number size=""32"" token=""true"" value=""2""/>
	</DataModel>

	<DataModel name=""DM"">
		<Blob name=""Header"" length=""5""/>
		<Choice name=""options"" minOccurs=""0"">
			<Block ref=""DM_choice1""/>
			<Block ref=""DM_choice2""/>
		</Choice>
	</DataModel>
</Peach>";

			PitParser parser = new PitParser();
			Dom.Dom dom = parser.asParser(null, new MemoryStream(ASCIIEncoding.ASCII.GetBytes(xml)));

			BitStream data = new BitStream();
			data.LittleEndian();
			data.WriteBytes(new byte[] { 11, 22, 33, 44, 55 });
			data.SeekBits(0, SeekOrigin.Begin);

			DataCracker cracker = new DataCracker();
			cracker.CrackData(dom.dataModels[2], data);

			Assert.AreEqual(1, dom.dataModels[2].Count);
			Assert.IsTrue(dom.dataModels[2][0] is Blob);
		}
	}
}

// end
