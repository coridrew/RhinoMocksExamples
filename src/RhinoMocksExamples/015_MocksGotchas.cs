using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExamples.SystemUnderTest;

namespace RhinoMocksExamples
{
    public partial class Gotchas
    {
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