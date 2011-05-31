using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Should.Extensions.AssertExtensions;
using Is = Rhino.Mocks.Constraints.Is;

namespace RhinoMocksExamples
{
    public class Arguments
    {
        //TODO: revise this to make it my own
        [Test]
        public void You_can_tell_the_stub_what_value_to_return_when_is_method_is_called_with_specific_arguments()
        {
            var stub = MockRepository.GenerateStub<ISampleClass>();

            stub.Stub(s => s.MethodThatReturnsInteger("foo")).Return(5);

            // calling the method with "foo" as the parameter will return 5
            stub.MethodThatReturnsInteger("foo").ShouldEqual(5);

            // calling the method with anything other than "foo" as the 
            // parameter will return the default value
            stub.MethodThatReturnsInteger("bar").ShouldEqual(0);
        }

        [Test]
        public void You_can_tell_the_stub_what_value_to_return_when_is_method_is_called_with_any_argument()
        {
            var stub = MockRepository.GenerateStub<ISampleClass>();

            stub.Stub(s => s.MethodThatReturnsInteger(Arg<string>.Is.Anything)).Return(5);

            // now it doesn't matter what the parameter is, we'll always get 5
            stub.MethodThatReturnsInteger("foo").ShouldEqual(5);
            stub.MethodThatReturnsInteger("bar").ShouldEqual(5);
            stub.MethodThatReturnsInteger(null).ShouldEqual(5);
        }

        [Test]
        public void You_can_get_fancy_with_parameters_in_stubs()
        {
            var stub = MockRepository.GenerateStub<ISampleClass>();

            // Arg<>.Matches() allows us to specify a lambda expression that specifies
            // whether the return value should be used in this case.  Here we're saying
            // that we'll return 5 if the string passed in is longer than 2 characters.
            stub.Stub(s => s.MethodThatReturnsInteger(Arg<string>.Matches(arg => arg != null && arg.Length > 2)))
                .Return(5);

            stub.MethodThatReturnsInteger("fooo").ShouldEqual(5);
            stub.MethodThatReturnsInteger("foo").ShouldEqual(5);
            stub.MethodThatReturnsInteger("fo").ShouldEqual(0);
            stub.MethodThatReturnsInteger("f").ShouldEqual(0);
            stub.MethodThatReturnsInteger(null).ShouldEqual(0);
        }

        [Test]
        public void Handling_out_parameters_in_stubs()
        {
            var stub = MockRepository.GenerateStub<ISampleClass>();

            // Here's how you stub an "out" parameter.  The "Dummy" part is 
            // just to satisfy the compiler.
            stub.Stub(s => s.MethodWithOutParameter(out Arg<int>.Out(10).Dummy));

            int i = 12345;
            stub.MethodWithOutParameter(out i);
            i.ShouldEqual(10);
        }

        [Test]
        public void Handling_ref_parameters_in_stubs()
        {
            var stub = MockRepository.GenerateStub<ISampleClass>();

            // Here's how you stub an "ref" parameter.  The "Dummy" part is 
            // just to satisfy the compiler.  (Note: Is.Equal() is part of
            // the Rhino.Mocks.Contraints namespace, there is also an 
            // Is.EqualTo() in NUnit... you want the Rhino Mocks one.)
            stub.Stub(s => s.MethodWithRefParameter(ref Arg<string>.Ref(Is.Equal("input"), "output").Dummy));

            // If you call the method with the specified input argument, it will
            // change the parameter to the value you specified.
            string param = "input";
            stub.MethodWithRefParameter(ref param);
            param.ShouldEqual("output");

            // If I call the method with any other input argument, it won't
            // change the value.
            param = "some other value";
            stub.MethodWithRefParameter(ref param);
            param.ShouldEqual("some other value");
        }

        [Test]
        public void You_can_get_the_arguments_of_calls_to_a_method()
        {
            var stub = MockRepository.GenerateStub<ISampleClass>();

            stub.MethodThatReturnsInteger("foo");
            stub.MethodThatReturnsInteger("bar");

            // GetArgumentsForCallsMadeOn() returns a list of arrays that contain
            // the parameter values for each call to the method.
            IList<object[]> argsPerCall = stub.GetArgumentsForCallsMadeOn(s => s.MethodThatReturnsInteger(null));
            argsPerCall[0][0].ShouldEqual("foo");
            argsPerCall[1][0].ShouldEqual("bar");
        }
    }
}