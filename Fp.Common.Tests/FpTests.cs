using System;
using System.Linq;
using Xunit;

namespace Fp.Common.Tests
{
    public class FpTests
    {
        [Fact]
        public void Pipe_Tests()
        => Assert.True(1.Pipe(y => y * 2) == 2);


        [Fact]
        public void Identity_Tests()
        => Assert.True((new[] { 1, 2, 3 }).Select(Fp.Id).Count() > 0);

        [Fact]
        public void Compose_Tests()
        {
            int f(int v) => v * 1;            
            int g(int v) => v * 10;
            int h(int v) => v * 100;            
            int j(int v) => v * 1000;
            
            var fComposed = (new Func<int, int>(f))
                            .Compose(g)
                            .Compose(h)
                            .Compose(j);

            Assert.True(fComposed(1) == 1111);
        }

        [Fact]
        public void Curry_Tests()
        {
            int add(int x, int y) => x + y;
            
            var Inc = (new Func<int, int, int>(add)).Curry()(1);

            Assert.True(Inc(1) == 2);
        }
    }
}
