using System;
using FluentAssertions;
using Ninject;
using Xunit;

namespace RestMocker.Core.Spec
{
    public class IocFactorySpec
    {
        interface ITest { }

        class Test1 : ITest { }

        class Test2 : ITest { }


        [Fact]
        public void ShouldTestSimpleIoCCreation()
        {
            // Arrange
            // Act
            var testee = IocFactory.Instance;

            // Assert
            testee.Should().NotBeNull();
            testee.Configuration.Should().NotBeNull();
            testee.Logger.Should().NotBeNull();
        }

        [Fact]
        public void ShouldTestSingletonInstance()
        {
            // Arrange
            // Act
            var testee1 = IocFactory.Instance;
            var testee2 = IocFactory.Instance;

            // Assert
            testee1.GetHashCode().ShouldBeEquivalentTo(testee2.GetHashCode());
        }

        [Fact]
        public void ShouldRegisterNonExistingInstance()
        {
            // Arrange
            var testee = IocFactory.Instance;

            // Act
            // Assert

            // No instance registered
            Action act = () => testee.Resolve<ITest>();
            act.ShouldThrow<ActivationException>();
        
            // Register class Test1
            testee.Register<ITest, Test1>();
            var inst = testee.Resolve<ITest>();
            inst.GetType().Should().Be(typeof (Test1));

            // Re-register ITest with Test2
            testee.Register<ITest,Test2>();
            inst = testee.Resolve<ITest>();
            inst.GetType().Should().Be(typeof(Test2));

            // Re-register it back with instance of Test1
            testee.Register<ITest, Test1>(new Test1());
            inst = testee.Resolve<ITest>();
            inst.GetType().Should().Be(typeof(Test1));

            // Re-register it with instance of Test2
            testee.Register<ITest, Test2>(new Test2());
            inst = testee.Resolve<ITest>();
            inst.GetType().Should().Be(typeof(Test2));
        }
    }
}
