using System;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Exceptions;
using Should.Extensions.AssertExtensions;

namespace RhinoMocksExamples
{
    public class Behavior
    {
        //TODO: revise this to make it my own, remove unused classes, and stow RhinoMocksTests.cs
        [Test]
        public void You_can_tell_the_stub_to_throw_an_exception_when_a_method_is_called()
        {
            var stub = MockRepository.GenerateStub<ISampleClass>();

            // calling the method with "foo" as the parameter will throw exception
            stub.Stub(s => s.MethodThatReturnsInteger("foo")).Throw(new InvalidOperationException());

            Assert.Throws<InvalidOperationException>(() => stub.MethodThatReturnsInteger("foo"));
        }

        [Test]
        public void You_can_check_to_see_if_a_method_was_called()
        {
            var stub = MockRepository.GenerateStub<ISampleClass>();

            stub.MethodThatReturnsInteger("foo");

            stub.AssertWasCalled(s => s.MethodThatReturnsInteger("foo"));
            stub.AssertWasCalled(s => s.MethodThatReturnsInteger(Arg<string>.Is.Anything));
        }

        [Test]
        public void You_can_check_to_see_if_a_method_was_called_a_certain_number_of_times()
        {
            var stub = MockRepository.GenerateStub<ISampleClass>();

            stub.MethodThatReturnsInteger("foo");
            stub.MethodThatReturnsInteger("bar");

            // this will pass
            stub.AssertWasCalled(s => s.MethodThatReturnsInteger("foo"), o => o.Repeat.Once());

            // call the method a second time
            stub.MethodThatReturnsInteger("foo");

            // now this will fail because we called it a second time
            Assert.Throws<ExpectationViolationException>(
                () => stub.AssertWasCalled(s => s.MethodThatReturnsInteger("foo"), o => o.Repeat.Once()));

            // some other options
            stub.AssertWasCalled(s => s.MethodThatReturnsInteger("foo"), o => o.Repeat.Times(2));
            stub.AssertWasCalled(s => s.MethodThatReturnsInteger("foo"), o => o.Repeat.AtLeastOnce());
            stub.AssertWasCalled(s => s.MethodThatReturnsInteger("foo"), o => o.Repeat.Twice());
        }

        [Test]
        public void You_can_check_to_see_if_a_method_was_not_called()
        {
            var stub = MockRepository.GenerateStub<ISampleClass>();

            stub.MethodThatReturnsInteger("foo");

            stub.AssertWasNotCalled(s => s.MethodThatReturnsInteger("asdfdsf"));
            stub.AssertWasNotCalled(s => s.MethodThatReturnsObject(Arg<int>.Is.Anything));
            stub.AssertWasNotCalled(s => s.VoidMethod());
        }

        [Test]
        public void You_can_tell_events_on_a_stub_to_fire()
        {
            var stub = MockRepository.GenerateStub<ISampleClass>();
            var eventWasHandled = false;

            // attach an event handler
            stub.SomeEvent += (args, e) => eventWasHandled = true;

            // raise the event
            stub.Raise(s => s.SomeEvent += null, this, EventArgs.Empty);

            eventWasHandled.ShouldBeTrue();
        }

        [Test]
        public void You_can_do_arbitrary_stuff_when_a_method_is_called()
        {
            var stub = MockRepository.GenerateStub<ISampleClass>();
            stub.Stub(s => s.MethodThatReturnsInteger(Arg<string>.Is.Anything))
                .Return(0) // you have to call Return() even though we're about to override it
                .WhenCalled(method =>
                {
                    string param = (string)method.Arguments[0];
                    method.ReturnValue = int.Parse(param);
                });

            stub.MethodThatReturnsInteger("3").ShouldEqual(3);
            stub.MethodThatReturnsInteger("6").ShouldEqual(6);
        }
    }
}