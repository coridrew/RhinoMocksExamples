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
                var mock = MockRepository.GenerateMock<IGameResultsService>();
                var game = new Game();

                game.StartGame(mock);

                mock.AssertWasCalled(m => m.DoSomething());
            }

            [Test]
            public void Stub_as_a_noun_is_just_scaffolding()
            {
                var stub = MockRepository.GenerateStub<IGameResultsService>();
                var game = new Game();
                stub.Stub(s => s.GetMagicNumber(Arg<string>.Is.Anything)).Return(3);

                var result = game.CalculateMagicNumber(stub);

                result.ShouldEqual(3 + 7);
            }

            [Test]
            public void Stub_as_a_verb_sets_up_behavior_on_mocks_and_stubs()
            {
                var mock = MockRepository.GenerateMock<IGameResultsService>();
                var stub = MockRepository.GenerateStub<IGameResultsService>();
                var game = new Game();
                mock.Stub(m => m.GetMagicNumber(Arg<string>.Is.Anything)).Return(2);
                stub.Stub(m => m.GetMagicNumber(Arg<string>.Is.Anything)).Return(5);

                var result = game.CalculateMagicNumber(mock);

                result.ShouldEqual(2 + 7);
            }
        }

        [TestFixture]
        public class Mocks_constructed_in_Fixture_Setup_will_fool_you
        {
            private IGameResultsService _mock;
            private Game _game;

            [TestFixtureSetUp]
            public void TestFixtureSetUp()
            {
                _mock = MockRepository.GenerateMock<IGameResultsService>();
                _game = new Game();
            }

            [Test]
            public void Any_recorded_calls_from_the_first_test()
            {
                _game.StartGame(_mock);
                _mock.AssertWasCalled(m => m.DoSomething());
            }

            [Test]
            public void Are_still_around_in_the_second_test()
            {
                //Falsely passing test.
                _mock.AssertWasCalled(m => m.DoSomething());
            }
        }
    }
}















