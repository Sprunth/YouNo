
namespace YouNo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

                // Draw cards until the player can play a card
                if (!ValidChoicesInHand())
                {
                    bool validCard = false;
                    while (!validCard)
                    {
                        // Draw card and add to hand
                        Console.Write("No valid card in hand. Drawing a card: ");
                        Card drawn = YouNoGame.ActiveGame.CardDeck.DrawCard();
                        drawn.PrintCardInfo();
                        Console.WriteLine();
                        AddCard(drawn);

                        validCard = YouNoGame.ActiveGame.CardDeck.ValidatePlay(drawn) ? true : false;
                    }

                }

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
                    else
                    {
                        Console.WriteLine("Invalid choice");
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
                AIDecision();
                
                //Console.WriteLine("The bot does nothing -- No AI yet!");
            }
        }

        /// <summary>
        /// Checks to see if the player has any valid cards to play
        /// </summary>
        /// <returns>True if there are valid cards, false if otherwise</returns>
        public bool ValidChoicesInHand()
        {
            foreach (Card c in Hand)
            {
                if (YouNoGame.ActiveGame.CardDeck.ValidatePlay(c))
                    return true;
            }
            return false;
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

        private void AIDecision()
        {
            if (!ValidChoicesInHand())
            {
                bool validCard = false;
                while (!validCard)
                {
                    // Draw card and add to hand
                    Card drawn = YouNoGame.ActiveGame.CardDeck.DrawCard();
                    AddCard(drawn);

                    validCard = YouNoGame.ActiveGame.CardDeck.ValidatePlay(drawn) ? true : false;
                }
            }

            // weighted choices for each card
            Dictionary<Card, double> validCards = new Dictionary<Card, double>();
            foreach (Card c in Hand)
            {
                if (YouNoGame.ActiveGame.CardDeck.ValidatePlay(c))
                {
                    // all values are defaulted to 0 weight
                    validCards.Add(c, 0);
                }
            }
            // evenly weight all the cards first, with a small chance
            foreach (Card c in validCards.Keys)
            {
                validCards[c] = 2;
            }

            // Figure out how many of each color are valid
            // the more are valid, the higher the card should be weighted
            Dictionary<CardColor, int> colorCount = new Dictionary<CardColor, int>();
            foreach (CardColor color in (CardColor[])Enum.GetValues(typeof(CardColor)))
                colorCount.Add(color, 0);
            foreach (Card c in validCards.Keys)
            {
                if (c is ColorCard)
                {
                    ColorCard card = (ColorCard)c;
                    colorCount[card.Color] += 1;
                }
            }
            // colorCount has counts of how many valid colors are in hand
            for (int i = 0; i < Enum.GetValues(typeof(ColorCard)).Length; i++)
            {
                foreach (Card c in validCards.Keys)
                {
                    if (c is ColorCard)
                    {
                        ColorCard card = (ColorCard)c;
                        validCards[c] += 20 * colorCount[card.Color] * (Enum.GetValues(typeof(ColorCard)).Length - i);
                    }
                }
            }

            // add other weights here if needed

            // Add up how much weight there is
            double totalWeight = 0;
            foreach (KeyValuePair<Card, double> pair in validCards)
            {
                totalWeight += pair.Value;
            }
            // selection
            double selection = YouNoGame.ActiveGame.Rand.NextDouble()*totalWeight;
            Card cardToPlay = null;
            while (selection > 0)
            {
                KeyValuePair<Card, double> draw = validCards.First();
                selection -= draw.Value;
                cardToPlay = draw.Key;
                validCards.Remove(draw.Key);
            }
            cardToPlay.PlayCard(this);
        }
    }
}
