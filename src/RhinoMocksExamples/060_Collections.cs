using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExamples.SystemUnderTest;
using Is = Rhino.Mocks.Constraints.Is;

namespace RhinoMocksExamples
{
    [TestFixture]
    public class Collections
    {
        private IGameResultsService _mock;
        private Game _game;

        [SetUp]
        public void SetUp()
        {
            _mock = MockRepository.GenerateMock<IGameResultsService>();
            _game = new Game();
        }

        [Test]
        public void IsAnything_works_with_collections()
        {
            _game.SendScores(_mock, 5);

            _mock.AssertWasCalled(
                m => m.PublishScores(Arg<IEnumerable<int>>.Is.Anything));
        }

        [Test]
        public void Count_verifies_list_length()
        {
            _game.SendScores(_mock, 5);

            _mock.AssertWasCalled(
                m => m.PublishScores(
                    Arg<IEnumerable<int>>.List.Count(Is.Equal(6))));
        }

        [Test]
        public void ContainsAll_verifies_at_least_the_specified_list()
        {
            var minimumExpected = new[] {3, 4, 5};

            _game.SendScores(_mock, 8);

            _mock.AssertWasCalled(
                m => m.PublishScores(
                    Arg<IEnumerable<int>>.List.ContainsAll(minimumExpected)));
        }

        [Test]
        public void Element_verifies_an_element_at_an_index()
        {
            _game.SendScores(_mock, 6);

            _mock.AssertWasCalled(
                m => m.PublishScores(
                    Arg<IEnumerable<int>>.List.Element(3, Is.GreaterThan(2))));
        }

        [Test, Explicit("This will fail, to show that List.Equal verifies the order of items.")]
        public void Equal_verifies_all_elements_in_order()
        {
            var expected = new[] {0, 1, 2, 3};
            // correct order:
            // var expected = new[] {0, 3, 1, 2};

            _game.SendScores(_mock, 3);

            _mock.AssertWasCalled(
                m => m.PublishScores(
                    Arg<IEnumerable<int>>.List.Equal(expected)));
        }
    }
}















