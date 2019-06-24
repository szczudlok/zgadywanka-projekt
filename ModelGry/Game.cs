using System;
using System.Collections.Generic;

namespace ModelGry
{
    public enum Answer { ToLow = -1, Correct = 0, ToHigh = +1 };
    public enum GameState { InProgress, Broken, Finished };
    public class Game
    {
        public List<Attempt> History;

        public int From { get; private set; } // zmieniają sie z kazda odpowiedzią
        public int To { get; private set; } // zmieniają sie z kazda odpowiedzią
        public int CurrentGuess { get; private set; }
        public GameState State { get; private set; }
        public int Counter { get; private set; } = 0;

        public Game(int min, int max)
        {
            From = min;
            To = max;
            CurrentGuess = int.MinValue;
            State = GameState.InProgress;
            History = new List<Attempt>();
        }
        public int MakeAttempt()
        {
            Random generator = new Random();
            int currentGuess = generator.Next(From, To + 1);
            this.CurrentGuess = currentGuess;
            return currentGuess;
        }

        public void RegisterAttempt(Answer answer)
        {
            Counter++;

            if(answer == Answer.ToLow)
            {
                this.From = this.CurrentGuess + 1;
            } else if (answer == Answer.ToHigh)
            {
                this.To = this.CurrentGuess - 1;
            }else {
                State = GameState.Finished;
            }

            History.Add(new Attempt(this.CurrentGuess, answer));
        }

        public void BreakGame()
        {
            State = GameState.Broken;
        }
    }
}
