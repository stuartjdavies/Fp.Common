using FluentAssertions;
using Fp.Common.Monads.MaybeMonad;
using static Fp.Common.Monads.MaybeMonad.MaybeHelpers;
using System;
using Xunit;

namespace Fp.Common.Tests
{
    public class MaybeTests
    {
        [Fact]
        public void Maybe_Verify_FromBind_SadPath()
        => (from a in Just("Hello ")
            from b in Nothing<string>()
            from c in "World!".ToMaybe()
            from d in " :)".ToMaybe()
            select a + b + c + d)
            .Pipe(x => x.IsNothing())
            .Should()
            .BeTrue();        

        [Fact]
        public void Maybe_Verify_FromBind_HappyPath()
        => (from a in 1.ToMaybe()
            from b in 10.ToMaybe()
            from c in 100.ToMaybe()
            from d in 1000.ToMaybe()
            select a + b + c + d)
            .Pipe(x => x.GetOrFail("Invalid"))
            .Should()
            .Be(1111);        
    }
}
