
namespace YouNo
{
    public enum ActionType { Skip, DrawTwo, Reverse }

    public class ActionCard : ColorCard
    {
        public ActionCard(ActionType actionType, CardColor color)
        {
            this.ActionType = actionType;
            this.Color = color;
        }

        public ActionType ActionType { get; set; }

        public override void PlayCard(Player currentPlayer)
        {
            switch (ActionType)
            {
                case YouNo.ActionType.DrawTwo:
                    {
                        Card card1 = YouNoGame.ActiveGame.CardDeck.DrawCard();
                        Card card2 = YouNoGame.ActiveGame.CardDeck.DrawCard();

                        currentPlayer.AddCard(card1);
                        currentPlayer.AddCard(card2);

                        break;
                    }

                case YouNo.ActionType.Reverse:
                    {
                        YouNoGame.ActiveGame.CardDeck.ReverseDeck();

                        break;
                    }
                case YouNo.ActionType.Skip:
                    {
                        // Handled by PlayerManager
                        break;
                    }
            }

            base.PlayCard(currentPlayer);
        }
    }
}
