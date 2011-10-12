using System.Collections.Generic;

namespace RhinoMocksExamples.SystemUnderTest
{
    public interface IGameResultsService
    {
        string FirstName { get; }
        string FavoriteBand { get; set; }
        void DoSomething();
        int GetMagicNumber(string s);
        void PublishWinners(string first, string second);
        void PublishScores(IEnumerable<int> numbers);
    }

    public class Game
    {
        public void StartGame(IGameResultsService resultsService)
        {
            resultsService.DoSomething();
        }

        public int GetMagicNumberTwice(IGameResultsService resultsService)
        {
        	var first = resultsService.GetMagicNumber("foo");
        	var second = resultsService.GetMagicNumber("bar");
        	return first + second;
        }

        public void IgnoreTheService(IGameResultsService resultsService) { }

        public int CalculateMagicNumber(IGameResultsService resultsService, string magicWord)
        {
            return resultsService.GetMagicNumber(magicWord) + 7;
        }

        public int CalculateMagicNumber(IGameResultsService resultsService)
        {
            return resultsService.GetMagicNumber(resultsService.FavoriteBand) + 7;
        }

        public void UpdateFavoriteBand(IGameResultsService resultsService, string newFavoriteBand)
        {
            resultsService.FavoriteBand = newFavoriteBand;
        }

        public void SendScores(IGameResultsService resultsService, int endingNumber)
        {
            var numbers = new List<int>();
            int u = 0;
            int d = endingNumber;
            while (d - u > 0)
            {
                numbers.Add(u);
                numbers.Add(d);
                u++;
                d--;
            }
            if (u == d)
            {
                numbers.Add(u);
            }
            resultsService.PublishScores(numbers);
        }
    }
}















