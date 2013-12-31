
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
                        // Handled by PlayerManager
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
