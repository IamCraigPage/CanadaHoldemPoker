namespace CanadaHoldemPoker.Models
{
	public class Card
	{
		public short? number;
		public Suit? suit;

		public Card(short number, Suit suit)
		{
			this.number = number;
			this.suit = suit;
		}

		public enum Suit
		{
			Hearts,
			Clubs,
			Diamonds,
			Spades
		}
	}
}
