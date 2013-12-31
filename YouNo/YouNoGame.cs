
namespace YouNo
{
    using System;

    class YouNoGame
    {
        public Random Rand = new Random();

        public YouNoGame()
        {
            Console.SetBufferSize(110, 30);
            Console.SetWindowSize(110, 30);

            ActiveGame = this;
            CardDeck = new Deck();
            
            // create 1 human and 3 CPUs
            PlayerManager = new PlayerManager();
            PlayerManager.CreatePlayer(true, "Human1");
            for (int i = 0; i < 2; i++)
                PlayerManager.CreatePlayer(false, "CPU" + ((int)i + 1));
            PlayerManager.DealCards();

            Card firstCard = CardDeck.DrawCard();
            CardDeck.LastPlayedCard = firstCard;
            CardDeck.DiscardCard(firstCard);

            PlayerManager.DebugPrintAllCardsInHands();
            Console.WriteLine();           
        }

        public static YouNoGame ActiveGame { get; set; }
        public Deck CardDeck { get; set; }
        public PlayerManager PlayerManager { get; set; }
        public bool Win { get; set; }
        public Player winningPlayer { get; set; }

        public void Run()
        {
            Win = false;

            while (!Win)
            {
                Console.Clear();

                CardDeck.DebugPrintLastPlayedCard();

                PlayerManager.NextPlayerTurn();

                Console.WriteLine("Press enter to continue");
                Console.ReadLine();
            }
            
        }
    }
}
