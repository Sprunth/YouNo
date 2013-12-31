namespace YouNo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Deck
    {
        Queue<Card> internalDeck;
        Queue<Card> discard;

        public Deck(bool populateDeck = true)
        {
            internalDeck = new Queue<Card>();
            discard = new Queue<Card>();

            if (populateDeck)
            {
                // for each color
                for (int color = 0; color < 4; color++)
                {
                    // cards can have values 0-9
                    for (int i = 0; i < 10; i++)
                    {
                        StandardCard c = new StandardCard((CardColor)color, i);
                        internalDeck.Enqueue(c);

                        // numbers 1-9 have 2 of each card
                        if (i != 0)
                        {
                            internalDeck.Enqueue(c);
                        }
                    }

                    // Also each color has 2 of each Action card
                    ActionCard skip = new ActionCard(ActionType.Skip, (CardColor)color);
                    ActionCard reverse = new ActionCard(ActionType.Reverse, (CardColor)color);
                    ActionCard drawtwo = new ActionCard(ActionType.DrawTwo, (CardColor)color);

                    internalDeck.Enqueue(skip);
                    internalDeck.Enqueue(skip);
                    internalDeck.Enqueue(reverse);
                    internalDeck.Enqueue(reverse);
                    internalDeck.Enqueue(drawtwo);
                    internalDeck.Enqueue(drawtwo);
                }

                // Add 4 of each wild card
                for (int i = 0; i < 4; i++)
                {
                    internalDeck.Enqueue(new WildCard(WildType.Wild));
                    internalDeck.Enqueue(new WildCard(WildType.WildDrawFour));
                }

                // end popluation of deck
                ShuffleDeck(ref internalDeck);
            }
        }

        public Card LastPlayedCard { get; set; }
        public CardColor WildColor { get; set; }
        public bool LastCardWasSkip
        {
            get
            {
                if (LastPlayedCard is ActionCard)
                {
                    ActionCard c = (ActionCard)LastPlayedCard;

                    if (c.ActionType == ActionType.Skip)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public bool LastCardWasDrawTwo
        {
            get
            {
                if (LastPlayedCard is ActionCard)
                {
                    ActionCard c = (ActionCard)LastPlayedCard;

                    if (c.ActionType == ActionType.DrawTwo)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public bool LastCardWasWild
        {
            get
            {
                return LastPlayedCard is WildCard ? true : false;
            }
        }

        public void ShuffleDeck(ref Queue<Card> deck)
        {
            List<Card> copy = deck.ToList();

            int n = copy.Count;
            while (n > 1)
            {
                n--;
                int k = YouNoGame.ActiveGame.Rand.Next(n + 1);
                Card tmp = copy[k];
                copy[k] = copy[n];
                copy[n] = tmp;
            }

            deck = new Queue<Card>(copy);
        }

        public Card DrawCard()
        {
            // check if we need to shuffle and refill from the discard
            if (internalDeck.Count <= 0)
            {
                // its technically possible for all the cards in both the draw and discard
                //      to be taken. not sure what to do...
                if (discard.Count <= 0)
                {
                    throw new Exception("No cards in draw or discard deck!");
                }

                // the newest discard will be placed after all the drawcard calls
                internalDeck = new Queue<Card>(discard);
                discard.Clear();
                ShuffleDeck(ref internalDeck);
            }

            return internalDeck.Dequeue();
        }

        public void DiscardCard(Card card)
        {
            LastPlayedCard = card;
            discard.Enqueue(card);
        }

        public void ReverseDeck()
        {
            internalDeck = new Queue<Card>(internalDeck.Reverse());
        }

        public void DebugPrintLastPlayedCard()
        {
            Console.Write("Last played card: ");
            LastPlayedCard.PrintCardInfo();
            Console.WriteLine();
        }

        /// <summary>
        /// Determines whether or not a card can be played
        /// </summary>
        /// <param name="playedCard">The card trying to be played</param>
        /// <returns>True if card can be played, false if otherwise</returns>
        public bool ValidatePlay(Card playedCard)
        {
            // wild is always allowed
            if (playedCard is WildCard)
            {
                return true;
            }
            else if (LastCardWasWild)
            {
                if (WildColor == ((ColorCard)playedCard).Color)
                { return true; }
                // if the color doesn't match and it wasn't wild, fail (regardless of number match)
                return false;
            }
             else if ((LastPlayedCard as ColorCard).Color == (playedCard as ColorCard).Color)
            {
                // check if color match
                return true;
            }
            else
            {
                // check if number match
                if (LastPlayedCard is StandardCard && playedCard is StandardCard)
                {
                    StandardCard lpc = (StandardCard)LastPlayedCard;
                    StandardCard pc = (StandardCard)playedCard;

                    return lpc.Value == pc.Value ? true : false;
                }
                else if (LastPlayedCard is ActionCard && playedCard is ActionCard)
                {
                    // check for action match
                    ActionCard lpc = (ActionCard)LastPlayedCard;
                    ActionCard pc = (ActionCard)playedCard;

                    return lpc.ActionType == pc.ActionType ? true : false;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
