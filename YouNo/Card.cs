namespace YouNo
{
    using System;

    public class Card
    {
        public virtual void PlayCard(Player currentPlayer)
        {
            YouNoGame.ActiveGame.CardDeck.LastPlayedCard = this;
            YouNoGame.ActiveGame.CardDeck.DiscardCard(this);
        }

        public void PrintCardInfo()
        {
            Console.ForegroundColor = ConsoleColor.White;
            if (this is StandardCard)
            {
                StandardCard c = (StandardCard)this;

                BackgroundColorPrep(c.Color);

                Console.Write(" " + c.Value);
            }
            else if (this is ActionCard)
            {
                ActionCard c = (ActionCard)this;

                BackgroundColorPrep(c.Color);

                Console.Write(" " + c.ActionType);
            }
            else if (this is WildCard)
            {
                WildCard c = (WildCard)this;
                Console.Write(c.Type);
            }
            else
            {
                throw new Exception("Card is not of the 3 card types!");
            }
            Console.Write(" ");
            Console.ResetColor();
            Console.Write(" ");
        }

        public static void BackgroundColorPrep(CardColor color)
        {
            switch (color)
            {
                case CardColor.Blue: { Console.BackgroundColor = ConsoleColor.DarkBlue; break; }
                case CardColor.Green: { Console.BackgroundColor = ConsoleColor.DarkGreen; break; }
                case CardColor.Red: { Console.BackgroundColor = ConsoleColor.DarkRed; break; }
                case CardColor.Yellow: { Console.BackgroundColor = ConsoleColor.DarkYellow; break; }
            }
        }
    }
}
