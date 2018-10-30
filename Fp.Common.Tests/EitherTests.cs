using FluentAssertions;
using Fp.Common.Monads.EitherMonad;
using Xunit;
using static Fp.Common.Monads.EitherMonad.EitherTryHelpers;
using static Fp.Common.Monads.EitherMonad.EitherHelpers;
using System.Threading.Tasks;
using System;

namespace Fp.Common.Tests
{
    public class EitherTests
    {
        [Fact]
        public void Either_Verify_FromBind_RightPath()
        => (from a in Right<string, int>(1)
            from b in Right<string, int>(10)
            from c in Right<string, int>(100)
            from d in Right<string, int>(1000)
            from e in Right<string, int>(10000)
            select a + b + c + d + e)
            .Pipe(x => x.GetRightOrFail("Couldn't get Right"))
            .Should()
            .Be(11111);

        [Fact]
        public void Either_Verify_FromBind_SadPath()
        => (from a in Right<string, int>(1)
            from b in Right<string, int>(10)
            from c in Left<string, int>("Error!!")
            from d in Right<string, int>(1000)
            from e in Right<string, int>(10000)
            select a + b + c + d + e)
            .Pipe(x => x.GetLeftOrFail("Couldn't get Right"))
            .Should()            
            .Be("Error!!");


        [Fact]
        public void Either_Try_Happy()
        => Try(() => 1).IsLeft().Should().BeTrue();

        [Fact]
        public void Either_Try_Sad()
        => Try<int>(() => throw new System.Exception("dsfsd")).IsRight().Should().BeTrue();

        [Fact]
        public void Either_Try_Bind_SadPath()
        => (from a in Try(() => 1)
            from b in Try(() => 10)
            from c in Try<int>(() => throw new System.Exception(""))
            from d in Try(() => 1000)
            from e in Try(() => 10000)
            select a + b + c + d + e)
            .Pipe(x => x.GetLeftOrFail("Couldn't get Right"))
            .Should()
            .Be("Error!!");

        [Fact]
        public async void Either_TryAsync_SadPath()
        => await TryAsync(() => Task.FromResult(1));

        Task<IEither<Exception, int>> foo()
        {
            IEither<Exception, int> v = new Right<Exception, int>(1);

            return Task.FromResult(v);
        }


        public async void Test()
        {
            var b = foo();
            var x = new EitherAsync<Exception, int>(b);

            await x.Value;


        }

    }

    public class EitherAsync<A, B> 
    {
        public Task<IEither<A, B>> Value { get; }
        public EitherAsync(Task<IEither<A,B>> ta)
        {
            Value = ta;
        }       

        public string Type => this.GetType().Name;
    }    
}
