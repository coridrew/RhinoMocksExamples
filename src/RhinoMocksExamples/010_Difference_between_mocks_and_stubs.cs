using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExamples.SystemUnderTest;

namespace RhinoMocksExamples
{
    [TestFixture]
    public class Difference_between_mocks_and_stubs
    {
        //The Game is the class under test in this fixture.

        [Test]
        public void Mocks_are_semantically_important_to_the_test()
        {
            var service = MockRepository.GenerateMock<IGameResultsService>();
            var game = new Game();

            game.StartGame(service);
            
            service.AssertWasCalled(s => s.DoSomething());
        }

        [Test]
        public void Stubs_are_supporting_players()
        {
            var service = MockRepository.GenerateStub<IGameResultsService>();
            const string magicWord = "something extra";
            const int magicNumberSeed = 9;
            var game = new Game();
            service.Stub(s => s.GetMagicNumber(magicWord))
                .Return(magicNumberSeed);

            var result = game.CalculateMagicNumber(service, magicWord);
            
            Assert.That(result, Is.EqualTo(magicNumberSeed + 7));
        }

        [Test]
        public void You_can_stub_behavior_for_mocks_too()
        {
            var service = MockRepository.GenerateMock<IGameResultsService>();
            const int magicNumberSeed = 9;
            const string magicWord = "something extra";
            var game = new Game();
            service.Stub(s => s.GetMagicNumber(Arg<string>.Is.Anything))
                .Return(magicNumberSeed);

            var result = game.CalculateMagicNumber(service, magicWord);

            service.AssertWasCalled(s => s.GetMagicNumber(magicWord));
            Assert.That(result, Is.EqualTo(magicNumberSeed + 7));
        }

        [Test]
        public void Stubs_have_settable_properties()
        {
            var service = MockRepository.GenerateStub<IGameResultsService>();
            const string newFavoriteBand = "my string";

            //Rhino Mocks calls this "Property Behavior"
            service.FavoriteBand = newFavoriteBand;
            
            Assert.That(service.FavoriteBand, Is.EqualTo(newFavoriteBand));
        }

        [Test]
        public void Mocks_do_not_have_settable_properties()
        {
            var service = MockRepository.GenerateMock<IGameResultsService>();

            service.FavoriteBand = "my string";
            
            Assert.That(service.FavoriteBand, Is.Null);
        }

        [Test, Explicit("This will fail, since the class under test does not comply with its expectations.")]
        public void Only_mocks_verify_their_expectations()
        {
            //This is a legacy point. 
            //I recommend the AssertWasCalled syntax over the VerifyAll.
            var service = MockRepository.GenerateMock<IGameResultsService>();
            var game = new Game();
            service.Expect(m => m.GetMagicNumber(Arg<string>.Is.Anything)).Return(4);
            
			game.IgnoreTheService(service);

            //This should (and does) throw an expectation violation. 
            //Correctly failing test.
            service.VerifyAllExpectations();
        }

        [Test]
        public void Stubs_do_not_verify_their_expectations()
        {
            //This is a legacy point. 
            //I recommend the AssertWasCalled syntax over the VerifyAll.
            var service = MockRepository.GenerateStub<IGameResultsService>();
            var game = new Game();
            service.Expect(s => s.GetMagicNumber(Arg<string>.Is.Anything)).Return(4);

            game.IgnoreTheService(service);

            //This will silently pass, as if Game were meeting your expectations. 
            //Falsely passing test.
            service.VerifyAllExpectations(); 
        }

        [Test, Explicit("This will fail, since the class under test does not satisfy the assertion.")]
        public void Stubs_now_apply_AssertWasCalled()
        {
            //In prior versions of Rhino Mocks, AssertWasCalled and 
            //AssertWasNotCalled on stubs would silently pass.
            var game = new Game();
            var service = MockRepository.GenerateStub<IGameResultsService>();
            
            game.IgnoreTheService(service);

            //This should (and does) throw an expectation violation in 3.5+. 
            //Correctly failing test.
            service.AssertWasCalled(s => s.GetMagicNumber(Arg<string>.Is.Anything));
        }

        [Test, Explicit("This will fail, since the class under test flouts the assertion.")]
        public void Stubs_now_apply_AssertWasNotCalled()
        {
            //In prior versions of Rhino Mocks, AssertWasCalled and 
            //AssertWasNotCalled on stubs would silently pass.
            var game = new Game();
            var service = MockRepository.GenerateStub<IGameResultsService>();

            game.CalculateMagicNumber(service);

            //This should (and does) throw an expectation violation in 3.6. 
            //Correctly failing test.
            service.AssertWasNotCalled(s => s.GetMagicNumber(Arg<string>.Is.Anything));
        }
    }
}















