using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExamples.SystemUnderTest;
using Should.Extensions.AssertExtensions;
using Is = Rhino.Mocks.Constraints.Is;

namespace RhinoMocksExamples
{
    public class Arguments
    {
        private ISampleInterface _stub;
        private ISampleInterface _mock;

        [SetUp]
        public void SetUp()
        {
            _stub = MockRepository.GenerateStub<ISampleInterface>();
            _mock = MockRepository.GenerateMock<ISampleInterface>();
        }

        [Test]
        public void You_can_specify_an_argument_when_stubbing_behavior()
        {
            const int intForFoo = 5;
            _stub.Stub(s => s.MethodThatReturnsInteger("foo")).Return(intForFoo);

            _stub.MethodThatReturnsInteger("foo").ShouldEqual(intForFoo);
            _stub.MethodThatReturnsInteger("bar").ShouldEqual(0);
        }

        [Test]
        public void Use_Is_Anything_when_args_are_not_important()
        {
            const int intToReturn = 5;
            _stub.Stub(s => s.MethodThatReturnsInteger(Arg<string>.Is.Anything))
                .Return(intToReturn);

            _stub.MethodThatReturnsInteger("foo").ShouldEqual(intToReturn);
            _stub.MethodThatReturnsInteger("bar").ShouldEqual(intToReturn);
            _stub.MethodThatReturnsInteger(null).ShouldEqual(intToReturn);
        }

        [Test]
        public void Use_Matches_to_define_conditions_for_args()
        {
            const int intForLongerStrings = 5;
            _stub.Stub(s =>
                      s.MethodThatReturnsInteger(
                          Arg<string>.Matches(arg => arg != null && arg.Length > 2)))
                .Return(intForLongerStrings);

            _stub.MethodThatReturnsInteger("fooo").ShouldEqual(intForLongerStrings);
            _stub.MethodThatReturnsInteger("foo").ShouldEqual(intForLongerStrings);
            _stub.MethodThatReturnsInteger("fo").ShouldEqual(0);
            _stub.MethodThatReturnsInteger("f").ShouldEqual(0);
            _stub.MethodThatReturnsInteger(null).ShouldEqual(0);
        }

        [Test]
        public void Use_GetArgumentsForCallsMadeOn_to_inspect_arguments()
        {
            var interactor = new InteractingClass();

            interactor.CallTheInterfaceTwice(_mock);

            IList<object[]> argsPerCall =
                _mock.GetArgumentsForCallsMadeOn(m => m.MethodThatReturnsInteger(null));
            argsPerCall[0][0].ShouldEqual("foo");
            argsPerCall[1][0].ShouldEqual("bar");
        }

        [Test]
        public void Use_List_OneOf_to_test_if_arg_is_in_a_list()
        {
            var permittedValues = new[] { "hello", "hi", "o hai" };

            _mock.MethodThatReturnsInteger("hi");

            _mock.AssertWasCalled(
                m => m.MethodThatReturnsInteger(Arg<string>.List.OneOf(permittedValues)));
        }
    }
}