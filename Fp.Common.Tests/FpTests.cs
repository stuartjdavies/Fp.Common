using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Fp.Common.Tests
{
    public class FpTests
    {
        [Fact]
        public void Pipe_Tests()
        => 1.Pipe(y => y * 2)
            .Should()
            .Equals(2);

        [Fact]
        public void Identity_Tests()
        => (new[] { 1, 2, 3 })
           .Select(Fp.Id)
           .Count()
           .Should()
           .Equals(new[] { 1, 2, 3 });

        [Fact]
        public void Compose_Tests()
        {
            int f(int v) => v * 1;            
            int g(int v) => v * 10;
            int h(int v) => v * 100;            
            int j(int v) => v * 1000;
            
            (new Func<int, int>(f))
            .Compose(g)
            .Compose(h)
            .Compose(j)
            .Should()
            .Equals(1111);                            
        }

        [Fact]
        public void Curry_Tests()
        {
            int add(int x, int y) => x + y;
            
            var Inc = (new Func<int, int, int>(add)).Curry()(1);

            Inc(1).Should().Equals(2);
        }
    }
}
