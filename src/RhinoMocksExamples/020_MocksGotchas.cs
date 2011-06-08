using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExamples.SystemUnderTest;
using Should.Extensions.AssertExtensions;

namespace RhinoMocksExamples
{
    public partial class Gotchas
    {
        [TestFixture]
        public class Clarifying_mocks_and_stubs_terminology
        {
            [Test]
            public void Mock_as_a_noun_is_part_of_the_test_assertions()
            {
                var mock = MockRepository.GenerateMock<ISampleInterface>();
                var interactor = new InteractingClass();

                interactor.CallTheInterface(mock);

                mock.AssertWasCalled(m => m.VoidMethod());
            }

            [Test]
            public void Stub_as_a_noun_is_just_scaffolding()
            {
                var stub = MockRepository.GenerateStub<ISampleInterface>();
                var interactor = new InteractingClass();
                stub.Stub(s => s.MethodThatReturnsInteger(Arg<string>.Is.Anything)).Return(3);

                var result = interactor.AddSevenToTheInterface(stub);

                result.ShouldEqual(3 + 7);
            }

            [Test]
            public void Stub_as_a_verb_sets_up_behavior_on_mocks_and_stubs()
            {
                var mock = MockRepository.GenerateMock<ISampleInterface>();
                var stub = MockRepository.GenerateStub<ISampleInterface>();
                var interactor = new InteractingClass();
                mock.Stub(m => m.MethodThatReturnsInteger(Arg<string>.Is.Anything)).Return(2);
                stub.Stub(m => m.MethodThatReturnsInteger(Arg<string>.Is.Anything)).Return(5);

                var result = interactor.AddSevenToTheInterface(mock);

                result.ShouldEqual(2 + 7);
            }
        }

        [TestFixture]
        public class Mocks_constructed_in_Fixture_Setup_will_fool_you
        {
            private ISampleInterface _mock;
            private InteractingClass _interactor;

            [TestFixtureSetUp]
            public void TestFixtureSetUp()
            {
                _mock = MockRepository.GenerateMock<ISampleInterface>();
                _interactor = new InteractingClass();
            }

            [Test]
            public void Any_recorded_calls_from_the_first_test()
            {
                _interactor.CallTheInterface(_mock);
                _mock.AssertWasCalled(m => m.VoidMethod());
            }

            [Test]
            public void Are_still_around_in_the_second_test()
            {
                //Falsely passing test.
                _mock.AssertWasCalled(m => m.VoidMethod());
            }
        }
    }
}















