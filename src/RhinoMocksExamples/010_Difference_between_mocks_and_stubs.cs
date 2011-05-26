using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExamples.SystemUnderTest;

namespace RhinoMocksExamples
{
    [TestFixture, Description("The InteractingClass is the class under test in this fixture.")]
    public class Difference_between_mocks_and_stubs
    {
        [Test]
        public void Mocks_are_semantically_important_to_the_test()
        {
            var mock = MockRepository.GenerateMock<ISampleInterface>();
            var interactor = new InteractingClass();

            interactor.CallTheInterface(mock);
            
            mock.AssertWasCalled(m => m.VoidMethod());
        }

        [Test]
        public void Stubs_are_supporting_players()
        {
            var stub = MockRepository.GenerateStub<ISampleInterface>();
            const string extraParameter = "something extra";
            const int intToReturn = 9;
            stub.Stub(s => s.MethodThatReturnsInteger(extraParameter)).Return(intToReturn);
            var interactor = new InteractingClass();
            
            var result = interactor.AddWithTheInterface(stub, extraParameter);
            
            Assert.That(result, Is.EqualTo(intToReturn + 7));
        }

        [Test]
        public void You_can_stub_behavior_for_mocks_too()
        {
            var mock = MockRepository.GenerateMock<ISampleInterface>();
            const int intToReturn = 9;
            mock.Stub(s => s.MethodThatReturnsInteger(Arg<string>.Is.Anything)).Return(intToReturn);
            const string extraParameter = "something extra";
            var interactor = new InteractingClass();

            var result = interactor.AddWithTheInterface(mock, extraParameter);

            mock.AssertWasCalled(m => m.MethodThatReturnsInteger(extraParameter));
            Assert.That(result, Is.EqualTo(intToReturn + 7));
        }

        [Test]
        public void Stubs_have_settable_properties()
        {
            var stub = MockRepository.GenerateStub<ISampleInterface>();
            const string extraParameter = "my string";
            stub.Property = extraParameter; //Rhino Mocks calls this "Property Behavior"
            Assert.That(stub.Property, Is.EqualTo(extraParameter));
        }

        [Test]
        public void Mocks_do_not_have_settable_properties()
        {
            var mock = MockRepository.GenerateMock<ISampleInterface>();
            const string extraParameter = "my string";
            mock.Property = "my string";
            Assert.That(mock.Property, Is.Null);
        }
    }
}