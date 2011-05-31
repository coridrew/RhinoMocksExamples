using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExamples.SystemUnderTest;

namespace RhinoMocksExamples
{
    [TestFixture]
    public class Collections
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
        public void IsAnything_works_with_collections()
        {
            _interactor.BuildTheNumberList(_mock, 5);

            _mock.AssertWasCalled(m => m.MethodWithEnumerable(Arg<IEnumerable<int>>.Is.Anything));
        }

        //TODO: build more examples here.
    }
}