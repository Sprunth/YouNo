
namespace YouNo
{
    using System;
    using System.Collections.Generic;

    public class Player
    {
        public List<Card> Hand;

        public Player(bool human, string name)
        {
            HumanPlayer = human;
            Name = name;

            Hand = new List<Card>();
        }

        public bool HumanPlayer { get; set; }
        public string Name { get; set; }

        public void AddCard(Card card)
        {
            Hand.Add(card);
        }

        public void MakeTurn()
        {
            if (HumanPlayer)
            {
                Console.WriteLine("Current Player: " + Name);

                // Draw card and add to hand
                Console.Write("Drew a card: ");
                Card newCard = YouNoGame.ActiveGame.CardDeck.DrawCard();
                newCard.PrintCardInfo();
                Console.WriteLine();
                AddCard(newCard);

                // Get a card selection
                Console.WriteLine("Your hand: ");
                ShowHandWithChoices();
                Console.WriteLine();
                bool validChoice = false;
                int choice = -1;
                while (!validChoice)
                {
                    choice = GetCardChoice();
                    if (YouNoGame.ActiveGame.CardDeck.ValidatePlay(Hand[choice]))
                    {
                        validChoice = true;
                    }
                }

                // test for somehow broken choice
                if (choice == -1)
                    throw new Exception("card choice not set??");

                // Hand[i] is the card played
                Card activeCard = Hand[choice];
                Hand.Remove(activeCard);
                activeCard.PlayCard(this);

            }
            else 
            {
                // if player is bot
                Console.WriteLine("The bot does nothing -- No AI yet!");
            }
        }

        public void ShowHandWithChoices()
        {
            for (int i = 0; i < Hand.Count; i++)
            {
                Console.Write("(" + i + ") ");
                Hand[i].PrintCardInfo();
                Console.Write(" | ");
            }
        }

        public void DebugPrintHand()
        {
            foreach (Card card in Hand)
            {
                card.PrintCardInfo();
                Console.Write("| ");
            }
            Console.WriteLine();
        }

        private int GetCardChoice()
        {
            bool validChoice = false;
            int choiceNum;
            while (!validChoice)
            {
                Console.Write("Which card do you want to play? ");
                string choice = Console.ReadLine();

                if (int.TryParse(choice, out choiceNum))
                {
                    if (choiceNum < Hand.Count)
                    {
                        validChoice = true;
                        return choiceNum;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input");
                }
            }
            throw new Exception("Invalid choice path!");
        }
    }
}
