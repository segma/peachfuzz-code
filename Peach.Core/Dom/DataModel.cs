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
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Reflection;
using System.Runtime.Serialization;

namespace Peach.Core.Dom
{
	/// <summary>
	/// DataModel is just a top level Block.
	/// </summary>
	[Serializable]
	[PitParsable("DataModel")]
	public class DataModel : Block//, IObjectReference
	{
		/// <summary>
		/// Dom parent of data model if any
		/// </summary>
		/// <remarks>
		/// A data model can be the child of two (okay three) different types,
		///   1. Dom (dom.datamodel collection)
		///   2. Action (Action.dataModel)
		///   3. ActionParam (Action.parameters[0].dataModel)
		///   
		/// This variable is one of those parent holders.
		/// </remarks>
		[NonSerialized]
		public Dom dom = null;

		/// <summary>
		/// Action parent of data model if any
		/// </summary>
		/// <remarks>
		/// A data model can be the child of two (okay three) different types,
		///   1. Dom (dom.datamodel collection)
		///   2. Action (Action.dataModel)
		///   3. ActionParam (Action.parameters[0].dataModel)
		///   
		/// This variable is one of those parent holders.
		/// </remarks>
		[NonSerialized]
		public Action action = null;

		public DataModel()
		{
		}

		public DataModel(string name)
		{
			this.name = name;
		}

		public Object GetRealObject(StreamingContext context)
		{
			// We need to fix parents!
			FixParents(this);

			return this;
		}

		void FixParents(DataElementContainer parent)
		{
			foreach (DataElement child in parent)
			{
				child.parent = parent;

				if (child is DataElementContainer)
				{
					FixParents(child as DataElementContainer);
				}
			}
		}
	}
}

// end
