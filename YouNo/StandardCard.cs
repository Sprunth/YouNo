
namespace YouNo
{
    class StandardCard : ColorCard
    {
        public StandardCard(CardColor color, int value)
        {
            this.Color = color;
            this.Value = value;
        }

        public int Value { get; set; }

        public override void PlayCard(Player currentPlayer)
        {
            // Nothing needs to be done...I think

            base.PlayCard(currentPlayer);
        }
    }
}
