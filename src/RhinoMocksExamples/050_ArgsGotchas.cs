using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExamples.SystemUnderTest;
using Should.Extensions.AssertExtensions;

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
                _mock.Stub(m => m.MethodThatReturnsInteger("my string"))
                    .Return(6);

                var result = _interactor.AddSevenToTheInterface(_mock, "my string");
                
                result.ShouldEqual(6 + 7);
                _mock.AssertWasCalled(m => m.MethodThatReturnsInteger("my string"));
            }

            [Test]
            public void You_can_stub_and_assert_with_arg_constraints()
            {
                _mock.Stub(m => m.MethodThatReturnsInteger(Arg<string>.Is.Anything))
                    .Return(6);

                var result = _interactor.AddSevenToTheInterface(_mock, "my string");

                result.ShouldEqual(6 + 7);
                _mock.AssertWasCalled(
                    m => m.MethodThatReturnsInteger(
                        Arg<string>.Matches(s => s.StartsWith("my "))));
            }

            [Test, Explicit("This throws a Rhino Mocks exception, to point out the incorrect syntax.")]
            public void Mixing_types_and_arg_constraints_throws_an_exception()
            {
                //Throws an invalid operation exception.
                _mock.Stub(
                    m => m.MethodWithTwoParameters(
                        "first string",
                        Arg<string>.Is.Anything));
            }

            [Test]
            public void Use_Arg_Is_when_you_need_to_mix()
            {
                _mock.Stub(
                    m => m.MethodWithTwoParameters(
                        Arg<string>.Is.Equal("first string"),
                        Arg<string>.Is.Anything));
            }
        }
    }
}















