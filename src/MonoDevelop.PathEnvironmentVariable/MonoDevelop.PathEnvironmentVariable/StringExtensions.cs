//
// StringExtensions.cs
//
// Author:
//       Matt Ward <matt.ward@microsoft.com>
//
// Copyright (c) 2018 Microsoft
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
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Text;

namespace MonoDevelop.PathEnvironmentVariable
{
	static class StringExtensions
	{
		public static string Replace (this string original, string oldValue, string newValue, StringComparison comparison)
		{
			var builder = new StringBuilder ();

			string text = null;
			int previousIndex = 0;
			int index = original.IndexOf (oldValue, comparison);

			while (index >= 0) {
				text = original.Substring (previousIndex, index - previousIndex);
				builder.Append (text);
				builder.Append (newValue);

				previousIndex = index + oldValue.Length;
				index = original.IndexOf (oldValue, previousIndex, comparison);
			}

			text = original.Substring (previousIndex);
			builder.Append (text);

			return builder.ToString ();
		}
	}
}
