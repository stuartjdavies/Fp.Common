using FluentAssertions;
using Fp.Common.Monads.EitherMonad;
using Xunit;

namespace Fp.Common.Tests
{
    public class EitherTests
    {
        [Fact]
        public void Either_Verify_FromBind_RightPath()
        => (from a in Either<string, int>.ReturnRight(1)
            from b in Either<string, int>.ReturnRight(10)
            from c in Either<string, int>.ReturnRight(100)
            from d in Either<string, int>.ReturnRight(1000)
            from e in Either<string, int>.ReturnRight(10000)
            select a + b + c + d + e)
            .Pipe(x => x.GetRightOrFail("Couldn't get Right"))
            .Should()
            .Be(11111);

        [Fact]
        public void Either_Verify_FromBind_SadPath()
        => (from a in Either<string, int>.ReturnRight(1)
            from b in Either<string, int>.ReturnRight(10)
            from c in Either<string, int>.ReturnLeft("Error!!")
            from d in Either<string, int>.ReturnRight(1000)
            from e in Either<string, int>.ReturnRight(10000)
            select a + b + c + d + e)
            .Pipe(x => x.GetLeftOrFail("Couldn't get Right"))
            .Should()            
            .Be("Error!!");
    }
}
