using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Xunit.Sdk;

namespace Bonjwa.API.Tests.TestUtils
{
    public sealed class PlainFileDataAttribute : DataAttribute
    {
        public string[] FilePaths { get; private set; }

        public PlainFileDataAttribute(params string[] filePaths)
        {
            FilePaths = filePaths;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            foreach (var filePath in FilePaths)
            {
                yield return new object[] { File.ReadAllText(filePath) };
            }
        }
    }
}
