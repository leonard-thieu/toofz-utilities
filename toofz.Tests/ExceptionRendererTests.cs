﻿using System.CodeDom.Compiler;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.Tests
{
    class ExceptionRendererTests
    {
        [TestClass]
        public class RenderStackTrace
        {
            [TestMethod]
            [Ignore("Paths will not match on different machines.")]
            public void StackTraceFromThrownException_RendersStackTraceCorrectly()
            {
                // Arrange
                var ex = ExceptionHelper.GetThrownException();
                using (var sw = new StringWriter())
                using (var indentedTextWriter = new IndentedTextWriter(sw))
                {
                    // Act
                    ExceptionRenderer.RenderStackTrace(ex.StackTrace, indentedTextWriter);
                    var output = sw.ToString();

                    // Assert
                    var expected = @"
StackTrace:
       at toofz.Tests.ExceptionHelper.ThrowException() in S:\Projects\toofz\toofz.Tests\ExceptionHelper.cs:line 10
       at toofz.TestsShared.Record.Exception(Action testCode) in C:\projects\toofz-testsshared\toofz.TestsShared\Record.cs:line 33";
                    Assert.AreEqual(expected, output);
                }
            }

            [TestMethod]
            [Ignore("Not sure why this one fails on AppVeyor.")]
            public void StackTraceFromUnthrownException_RendersStackTraceCorrectly()
            {
                // Arrange
                var stackTraceStr = @"   at toofz.Tests.ExceptionHelper.ThrowException() in S:\Projects\toofz\toofz.Tests\ExceptionHelper.cs:line 10
   at toofz.TestsShared.Record.Exception(Action testCode) in C:\projects\toofz-testsshared\toofz.TestsShared\Record.cs:line 33";
                var ex = new UnthrownException(stackTraceStr);
                using (var sw = new StringWriter())
                using (var indentedTextWriter = new IndentedTextWriter(sw))
                {
                    // Act
                    ExceptionRenderer.RenderStackTrace(ex.StackTrace, indentedTextWriter);
                    var output = sw.ToString();

                    // Assert
                    var expected = @"
StackTrace:
       at toofz.Tests.ExceptionHelper.ThrowException() in S:\Projects\toofz\toofz.Tests\ExceptionHelper.cs:line 10
       at toofz.TestsShared.Record.Exception(Action testCode) in C:\projects\toofz-testsshared\toofz.TestsShared\Record.cs:line 33";
                    Assert.AreEqual(expected, output);
                }
            }
        }
    }
}
