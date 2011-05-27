using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExamples.SystemUnderTest;

namespace RhinoMocksExamples
{
    [TestFixture]
    public class Difference_between_mocks_and_stubs
    {
        //The InteractingClass is the class under test in this fixture.

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

        [Test]
        public void Only_mocks_verify_their_expectations()
        {
            //This is a legacy point. I recommend the AssertWasCalled syntax over the VerifyAll.
            var mock = MockRepository.GenerateMock<ISampleInterface>();
            mock.Expect(m => m.MethodThatReturnsInteger(Arg<string>.Is.Anything)).Return(4);
            var interactor = new InteractingClass();
            interactor.IgnoreTheInterface(mock);

            //This should (and does) throw an expectation violation. Correctly failing test.
            mock.VerifyAllExpectations();
        }

        [Test]
        public void Stubs_do_not_verify_their_expectations()
        {
            //This is a legacy point. I recommend the AssertWasCalled syntax over the VerifyAll.
            var stub = MockRepository.GenerateStub<ISampleInterface>();
            stub.Expect(s => s.MethodThatReturnsInteger(Arg<string>.Is.Anything)).Return(4);
            var interactor = new InteractingClass();
            interactor.IgnoreTheInterface(stub);

            //This will silently pass, as if InteractingClass were meeting your expectations. 
            //Falsely passing test.
            stub.VerifyAllExpectations(); 
        }

        [Test]
        public void Stubs_now_apply_AssertWasCalled()
        {
            //In prior versions of Rhino Mocks, AssertWasCalled and AssertWasNotCalled on 
            //stubs would silently pass.
            var stub = MockRepository.GenerateStub<ISampleInterface>();
            var interactor = new InteractingClass();
            interactor.IgnoreTheInterface(stub);

            //This should (and does) throw an expectation violation in 3.5+. 
            //Correctly failing test.
            stub.AssertWasCalled(s => s.MethodThatReturnsInteger(Arg<string>.Is.Anything));
        }

        [Test]
        public void Stubs_now_apply_AssertWasNotCalled()
        {
            //In prior versions of Rhino Mocks, AssertWasCalled and AssertWasNotCalled on 
            //stubs would silently pass.
            var stub = MockRepository.GenerateStub<ISampleInterface>();
            var interactor = new InteractingClass();
            interactor.AddWithTheInterface(stub);

            //This should (and does) throw an expectation violation in 3.6. 
            //Correctly failing test.
            stub.AssertWasNotCalled(s => s.MethodThatReturnsInteger(Arg<string>.Is.Anything));
        }
    }
}