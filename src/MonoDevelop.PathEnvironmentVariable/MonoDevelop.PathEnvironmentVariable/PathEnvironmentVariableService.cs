//
// PathEnvironmentVariableOptionsPanel.cs
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
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.PathEnvironmentVariable
{
	static class PathEnvironmentVariableService
	{
		static PathEnvironmentVariableService ()
		{
			OriginalPathEnvironmentValue = Environment.GetEnvironmentVariable ("PATH");
			ModifiedPathEnvironmentValue = ConfigurationProperty.Create<string> ("ModifiedPathEnvironmentVariable", string.Empty);
		}

		public static string OriginalPathEnvironmentValue { get; private set; }
		public static ConfigurationProperty<string> ModifiedPathEnvironmentValue { get; private set; }

		public static void Init ()
		{
			if (string.IsNullOrEmpty (ModifiedPathEnvironmentValue.Value))
				return;

			UpdatePathEnvironmentValue (ModifiedPathEnvironmentValue);
		}

		public static void UpdatePathEnvironmentValue (string pathEnvironmentVariableValue)
		{
			ModifiedPathEnvironmentValue.Value = pathEnvironmentVariableValue;

			// Reset old PATH environment variable in case new value references it.
			Environment.SetEnvironmentVariable ("PATH", OriginalPathEnvironmentValue);

			if (string.IsNullOrEmpty (pathEnvironmentVariableValue))
				return;

			// Update PATH environment variable.
			pathEnvironmentVariableValue = ExpandPath (pathEnvironmentVariableValue);
			Environment.SetEnvironmentVariable ("PATH", pathEnvironmentVariableValue);

			string value = Environment.GetEnvironmentVariable ("PATH");
			System.Diagnostics.Debug.WriteLine ("PATH=" + value);
		}

		static string ExpandPath (string pathEnvironmentVariableValue)
		{
			if (string.IsNullOrEmpty (pathEnvironmentVariableValue))
				return pathEnvironmentVariableValue;

			if (Platform.IsWindows) {
				return pathEnvironmentVariableValue.Replace ("%PATH%", OriginalPathEnvironmentValue, StringComparison.InvariantCultureIgnoreCase);
			} else {
				return pathEnvironmentVariableValue.Replace ("$PATH", OriginalPathEnvironmentValue, StringComparison.InvariantCultureIgnoreCase);
			}
		}
	}
}
