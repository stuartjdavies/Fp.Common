using System;
using Xunit;

namespace Fp.Common.Tests
{
    public class FpTests
    {
        [Fact]
        public void Pipe_Tests()
        {
            var x = 1;

            Assert.True(x.Pipe(y => y * 2) == 2);
        }
    }
}
