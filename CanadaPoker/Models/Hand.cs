namespace CanadaHoldemPoker.Models
{
	public class Hand
	{
		public Card[] cards;
		public Player player;
		public HandValue handValue;

		public Hand(Card[] cards, Player player)
		{
			// TO DO, what if cards is not 5 long? Throw an exception? Or let them play on hard mode with fewer cards than everyone else. :D
			this.cards = cards;
			this.player = player;
		}
	}

	public class HandValue
	{
		public HandType handType = HandType.High;
		/// <summary>
		/// The card used to make make this type of hand. For example a three of a kind made up of 3's would have this set to 3.
		/// </summary>
		public int cardValue = 0;

		public HandValue(int cardValue, HandType handType)
		{
			this.cardValue = cardValue;
			this.handType = handType;
		}

		public enum HandType
		{
			Flush = 3,
			ThreeOfaKind = 2,
			OnePair = 1,
			High = 0
		}
	}
}
