
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
            if (!YouNoGame.ActiveGame.CardDeck.LastCardWasSkip)
            player.MakeTurn();

            // Put player back at end of queue
            AllPlayers.Enqueue(player);
        }

    }
}

