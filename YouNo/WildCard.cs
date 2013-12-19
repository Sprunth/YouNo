
namespace YouNo
{
    using System;

    public enum WildType { Wild, WildDrawFour }

    class WildCard : Card
    {
        public WildType Type;

        public WildCard(WildType type)
        {
            this.Type = type;
        }

        public override void PlayCard(Player currentPlayer)
        {
            bool validColor = false;
            while (!validColor)
            {
                Console.WriteLine("What color for the next player?");
                Console.WriteLine("Red | Green | Blue | Yellow");

                string choice = Console.ReadLine();
                switch (choice.ToLower()[0])
                {
                    case 'r':
                        {
                            YouNoGame.ActiveGame.CardDeck.WildColor = CardColor.Red;
                            validColor = true;
                            break;
                        }
                    case 'g':
                        {
                            YouNoGame.ActiveGame.CardDeck.WildColor = CardColor.Green;
                            validColor = true;
                            break;
                        }
                    case 'b':
                        {
                            YouNoGame.ActiveGame.CardDeck.WildColor = CardColor.Blue;
                            validColor = true;
                            break;
                        }
                    case 'y':
                        {
                            YouNoGame.ActiveGame.CardDeck.WildColor = CardColor.Yellow;
                            validColor = true;
                            break;
                        }
                    default: { break; }
                }
            }

            // wild color set
            Console.Write("Next Card has to be ");
            Card.BackgroundColorPrep(YouNoGame.ActiveGame.CardDeck.WildColor);
            Console.WriteLine(YouNoGame.ActiveGame.CardDeck.WildColor.ToString());
            Console.ResetColor();

            switch (this.Type)
            {
                case WildType.Wild:
                    {
                        break;
                    }
                case WildType.WildDrawFour:
                    {
                        // draw 4 cars TODO
                        for (int i = 0; i < 4; i++)
                        {
                            Card drawnCard = YouNoGame.ActiveGame.CardDeck.DrawCard();
                            currentPlayer.AddCard(drawnCard);
                        }
                        break;
                    }
            }

            base.PlayCard(currentPlayer);
        }
    }
}
