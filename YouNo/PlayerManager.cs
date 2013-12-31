
namespace YouNo
{
    using System;
    using System.Collections.Generic;

    class PlayerManager
    {
        public Queue<Player> AllPlayers = new Queue<Player>();

        public void CreatePlayer(bool human, string name)
        {
            this.AllPlayers.Enqueue(new Player(human, name));
        }

        public void DealCards()
        {
            // 7 cards per player
            for (int i = 0; i < 7; i++)
            {
                foreach (Player player in AllPlayers)
                {
                    player.AddCard(YouNoGame.ActiveGame.CardDeck.DrawCard());
                }
            }
        }

        public void DebugPrintAllCardsInHands()
        {
            foreach (Player player in AllPlayers)
            {
                Console.Write(player.Name + ": ");
                player.DebugPrintHand();
            }
        }

        public void NextPlayerTurn()
        {
            // Determine who's next
            Player player = AllPlayers.Dequeue();

            // if skip card was last played skip
            if (YouNoGame.ActiveGame.CardDeck.LastCardWasSkip)
            {
                // do nothing
            }
            else if (YouNoGame.ActiveGame.CardDeck.LastCardWasDrawTwo)
            {
                // draw 2 cards and forfeit turn
                Card card1 = YouNoGame.ActiveGame.CardDeck.DrawCard();
                Card card2 = YouNoGame.ActiveGame.CardDeck.DrawCard();

                Console.WriteLine("Last player played DrawTwo");
                Console.WriteLine("You turned is skipped and you draw two cards");
                Console.Write("Drew: ");
                card1.PrintCardInfo();
                Console.Write("Drew: ");
                card2.PrintCardInfo();

                player.AddCard(card1);
                player.AddCard(card2);
            }
            else
            {
                player.MakeTurn();
            }
            // don't need to reset LastCardWasSkip/DrawTwo since it checks the discard

            // Check for victory
            if (player.Hand.Count <= 0)
            {
                YouNoGame.ActiveGame.Win = true;
                YouNoGame.ActiveGame.winningPlayer = player;
            }

            // Put player back at end of queue
            AllPlayers.Enqueue(player);
        }

        public Player NextPlayerPeek()
        {
            return AllPlayers.Peek();
        }

    }
}

