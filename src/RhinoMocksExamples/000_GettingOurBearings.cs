using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExamples.SystemUnderTest;

namespace RhinoMocksExamples
{
	public static class GameTester_WithoutRhinoMocks
	{
		[TestFixture]
		public class When_creating_the_magic_number
		{
			private Game _game;
			private MockGameResultsService _service;

			[SetUp]
			public void Setup()
			{
				_game = new Game();
				_service = new MockGameResultsService();
			}

			[Test]
			public void Should_combine_numbers_from_the_results_service()
			{
				const int firstNumber = 5;
				const int secondNumber = 7;
				_service.MagicNumberToReturn = firstNumber;
				_service.SecondMagicNumberToReturn = secondNumber;

				var result = _game.GetMagicNumberTwice(_service);

				Assert.That(result, Is.EqualTo(firstNumber + secondNumber));
				Assert.That(_service.GetMagicNumberCalls, Is.EqualTo(2), "Expected service to be called twice");
			}

			private class MockGameResultsService : IGameResultsService
			{
				#region To support tests

				public int MagicNumberToReturn { get; set; }
				public int SecondMagicNumberToReturn { get; set; }

				public int DoSomethingCalls { get; private set; }
				public bool DoSomethingWasCalled { get { return DoSomethingCalls > 0; } }

				public int GetMagicNumberCalls { get; private set; }
				public bool GetMagicNumberWasCalled { get { return GetMagicNumberCalls > 0; } }

				public int PublishWinners2ParamsCalls { get; private set; }
				public bool PublishWinners2ParamsWasCalled { get { return PublishWinners2ParamsCalls > 0; } }

				public int PublishWinners1ParamCalls { get; private set; }
				public bool PublishWinners1ParamWasCalled { get { return PublishWinners1ParamCalls > 0; } }
				
				#endregion

				public string FirstName { get; set; } //Read-only on the interface, but setter is needed here.
				public string FavoriteBand { get; set; }
				public void DoSomething()
				{
					DoSomethingCalls++;
				}

				public int GetMagicNumber(string s)
				{
					GetMagicNumberCalls++;
					return GetMagicNumberCalls == 1 ? MagicNumberToReturn : SecondMagicNumberToReturn;
				}

				public void PublishWinners(string first, string second)
				{
					PublishWinners2ParamsCalls++;
				}

				public void PublishScores(IEnumerable<int> numbers)
				{
					PublishWinners1ParamCalls++;
				}
			}
		}
	}

	public static class GameTester
	{
		[TestFixture]
		public class When_creating_the_magic_number
		{
			private Game _game;
			private IGameResultsService _service;

			[SetUp]
			public void Setup()
			{
				_game = new Game();
				_service = MockRepository.GenerateMock<IGameResultsService>();
			}

			[Test]
			public void Should_combine_numbers_from_the_results_service()
			{
				const int firstNumber = 5;
				const int secondNumber = 7;
				_service.Stub(s => s.GetMagicNumber(Arg<string>.Is.Anything)).Return(firstNumber).Repeat.Once();
				_service.Stub(s => s.GetMagicNumber(Arg<string>.Is.Anything)).Return(secondNumber).Repeat.Once();

				var result = _game.GetMagicNumberTwice(_service);

				Assert.That(result, Is.EqualTo(firstNumber + secondNumber));
				_service.AssertWasCalled(s => s.GetMagicNumber(Arg<string>.Is.Anything), opt => opt.Repeat.Twice());
			}
		}
	}
}














