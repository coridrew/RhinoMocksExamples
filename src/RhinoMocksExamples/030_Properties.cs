using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExamples.SystemUnderTest;
using Should.Extensions.AssertExtensions;

namespace RhinoMocksExamples
{
    [TestFixture]
    public class Properties
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
        public void You_can_set_properties_on_a_stub()
        {
            const string expectedProperty = "set";
            _stub.Property = expectedProperty;
            _stub.Property.ShouldEqual(expectedProperty);
        }

        [Test]
        public void Stubbing_properties_on_a_stub_throws_an_exception()
        {
            _stub.Stub(i => i.Property).Return("this will throw");
        }

        [Test]
        public void Properties_will_not_be_set_on_a_mock()
        {
            const string expectedProperty = "set";
            _mock.Property = expectedProperty;
            //This will fail:
            _mock.Property.ShouldEqual(expectedProperty);
        }

        [Test]
        public void Use_the_Stub_method_to_set_up_properties_on_a_mock()
        {
            const string expectedProperty = "set";
            _mock.Stub(m => m.Property).Return(expectedProperty);
            _mock.Property.ShouldEqual(expectedProperty);
        }

        [Test]
        public void Use_AssertWasCalled_for_interaction_tests_on_a_mock()
        {
            const string expectedProperty = "set";
            var interactor = new InteractingClass();

            interactor.SetPropertyOnTheInterface(_mock, expectedProperty);

            //Interaction-based test:
            _mock.AssertWasCalled(m => m.Property = expectedProperty);
        }

        [Test]
        public void Use_PropertyBehavior_for_state_based_tests_on_a_mock()
        {
            const string expectedProperty = "set";
            var interactor = new InteractingClass();
            _mock.Stub(m => m.Property).PropertyBehavior();

            interactor.SetPropertyOnTheInterface(_mock, expectedProperty);

            //State-based test:
            _mock.Property.ShouldEqual(expectedProperty);
        }

        [Test]
        public void Using_AssertWasCalled_will_not_work_for_properties_on_a_stub()
        {
            const string expectedProperty = "set";
            var interactor = new InteractingClass();

            interactor.SetPropertyOnTheInterface(_stub, expectedProperty);

            //Incorrectly failing test:
            _stub.AssertWasCalled(m => m.Property = expectedProperty);
        }
    }
}