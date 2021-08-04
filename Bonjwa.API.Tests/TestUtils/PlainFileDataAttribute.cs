using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Xunit.Sdk;

namespace Bonjwa.API.Tests.TestUtils
{
    public class PlainFileDataAttribute : DataAttribute
    {
        private readonly string[] _filePaths;

        public PlainFileDataAttribute(params string[] filePaths)
        {
            _filePaths = filePaths;
        }

        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest)
        {
            foreach (var filePath in _filePaths)
            {
                yield return new object[] { File.ReadAllText(filePath) };
            }
        }
    }
}
