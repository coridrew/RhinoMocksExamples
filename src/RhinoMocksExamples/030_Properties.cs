using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExamples.SystemUnderTest;
using Should.Extensions.AssertExtensions;

namespace RhinoMocksExamples
{
    [TestFixture]
    public class Properties
    {
        private IGameResultsService _stub;
        private IGameResultsService _mock;

        [SetUp]
        public void SetUp()
        {
            _stub = MockRepository.GenerateStub<IGameResultsService>();
            _mock = MockRepository.GenerateMock<IGameResultsService>();
        }

        [Test]
        public void You_can_set_properties_on_a_stub()
        {
            const string expectedBand = "R.E.M.";
            _stub.FavoriteBand = expectedBand;
            _stub.FavoriteBand.ShouldEqual(expectedBand);
        }

        [Test, Explicit("This will fail because mocks do not implement their property setters.")]
        public void Properties_will_not_be_set_on_a_mock()
        {
            const string expectedBand = "Paul & Storm";
            _mock.FavoriteBand = expectedBand;
            //This will fail:
            _mock.FavoriteBand.ShouldEqual(expectedBand);
        }

        [Test]
        public void Use_the_Stub_method_to_set_up_properties_on_a_mock()
        {
            const string expectedBand = "They Might Be Giants";
            _mock.Stub(m => m.FavoriteBand).Return(expectedBand);
            _mock.FavoriteBand.ShouldEqual(expectedBand);
        }

        [Test, Explicit("This throws a Rhino Mocks exception, to point out the incorrect syntax.")]
        public void Stubbing_properties_on_a_stub_throws_an_exception()
        {
            _stub.Stub(i => i.FavoriteBand).Return("this will throw");
        }

        [Test]
        public void You_can_stub_readonly_properties_on_a_stub()
        {
            const string expectedBand = "Jonathan Coulton";
            _stub.Stub(i => i.FirstName).Return(expectedBand);
            _stub.FirstName.ShouldEqual(expectedBand);
        }

        [Test]
        public void Use_PropertyBehavior_for_state_based_tests_on_a_mock()
        {
            const string expectedBand = "Philip Glass";
            var game = new Game();
            //This is not typical, but it is available:
            _mock.Stub(m => m.FavoriteBand).PropertyBehavior();

            game.UpdateFavoriteBand(_mock, expectedBand);

            //State-based test:
            _mock.FavoriteBand.ShouldEqual(expectedBand);
        }

        [Test]
        public void Use_AssertWasCalled_for_interaction_tests_on_a_mock()
        {
            const string expectedBand = "The Blues Brothers";
            var game = new Game();

            game.UpdateFavoriteBand(_mock, expectedBand);

            //Interaction-based test:
            _mock.AssertWasCalled(m => m.FavoriteBand = expectedBand);
        }

        [Test, Explicit("This will fail because stubs do not record calls to their properties.")]
        public void AssertWasCalled_will_not_work_for_properties_on_a_stub()
        {
            const string expectedBand = "Rick Astley";
            var interactor = new Game();

            interactor.UpdateFavoriteBand(_stub, expectedBand);

            //Incorrectly failing test:
            _stub.AssertWasCalled(m => m.FavoriteBand = expectedBand);
        }
    }
}















