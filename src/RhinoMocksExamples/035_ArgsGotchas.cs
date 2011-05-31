using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExamples.SystemUnderTest;
using Should.Extensions.AssertExtensions;
using Is = Rhino.Mocks.Constraints.Is;

namespace RhinoMocksExamples
{
    public partial class Gotchas
    {
        [TestFixture]
        public class Arg_of_T_cannot_mix_with_simple_arg_types
        {
            private ISampleInterface _mock;
            private InteractingClass _interactor;

            [SetUp]
            public void SetUp()
            {
                _mock = MockRepository.GenerateMock<ISampleInterface>();
                _interactor = new InteractingClass();
            }

            [Test]
            public void You_can_stub_and_assert_directly_with_types()
            {
                const string extraParameter = "my string";
                _mock.Stub(m => m.MethodThatReturnsInteger(extraParameter)).Return(6);

                var result = _interactor.AddSevenToTheInterface(_mock, extraParameter);
                
                result.ShouldEqual(6 + 7);
                _mock.AssertWasCalled(m => m.MethodThatReturnsInteger(extraParameter));
            }

            [Test]
            public void You_can_stub_and_assert_with_arg_constraints()
            {
                _mock.Stub(m => m.MethodThatReturnsInteger(Arg<string>.Is.Anything)).Return(6);

                var result = _interactor.AddSevenToTheInterface(_mock, "my string");

                result.ShouldEqual(6 + 7);
                _mock.AssertWasCalled(m => m.MethodThatReturnsInteger(Arg<string>.Matches(s => s.StartsWith("my "))));
            }

            [Test]
            public void Mixing_types_and_arg_constraints_throws_an_exception()
            {
                //Throws an invalid operation exception.
                _mock.Stub(m => m.MethodWithTwoParameters("first string", Arg<string>.Is.Anything));
            }
        }
    }
}