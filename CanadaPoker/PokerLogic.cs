using System;
using System.Linq;
using System.Collections.Generic;
using CanadaHoldemPoker.Models;

namespace CanadaHoldemPoker
{
	/// <summary>
	/// Summary description for Class1
	/// </summary>
	public static class PokerLogic
	{
		public static List<Hand> DetermineWinner(List<Hand> hands)
        {
			// just send it back if hands is null or contains 1 hand.
			if (hands == null || hands.Count() == 1)
				return hands;

			foreach (var h in hands)
				CalculateHandValue(h);

			var sorted = hands.OrderByDescending(h => h.handValue.handType)
				.ThenByDescending(h => h.handValue.cardValue)
				.ThenByDescending(h => h.cards[0].number)
				.ThenByDescending(h => h.cards[1].number)
				.ThenByDescending(h => h.cards[2].number)
				.ThenByDescending(h => h.cards[3].number)
				.ThenByDescending(h => h.cards[4].number)
				.ToList();

			List<Hand> result = new List<Hand>();
			result.Add(sorted[0]); // return the first one, since nothing else is better.

			// but also return ties
			for (int i=1; i<hands.Count(); i++)
            {
				// if it's a tie, return it too...
				if (sorted[0].handValue.handType == sorted[i].handValue.handType
					&& sorted[0].handValue.cardValue == sorted[i].handValue.cardValue
					&& sorted[0].cards[1].number == sorted[i].cards[1].number
					&& sorted[0].cards[2].number == sorted[i].cards[2].number
					&& sorted[0].cards[3].number == sorted[i].cards[3].number
					&& sorted[0].cards[4].number == sorted[i].cards[4].number)
					result.Add(sorted[i]);
				else
					break;
            }
			return result;
        }
		public static void CalculateHandValue(Hand hand)
        {
			if (hand == null || hand.cards == null)
				return;

			// Sort the cards by number, for later if there are ties.
			hand.cards = hand.cards.OrderByDescending(c => c.number).ToArray();

			// Count cards, so we know our 2/3/4 of a kinds later
			Dictionary<int?, int> duplicates = new Dictionary<int?, int>(); // card number, total
			foreach (var card in hand.cards)
            {
				if (!duplicates.ContainsKey(card.number))
					duplicates.Add(card.number, 1);
				else
					duplicates[card.number] = (int)duplicates[card.number] + 1;
            }


			// Flush A flush is any hand with five cards of the same suit. If two or more players hold a flush, the flush with the highest card wins.
			// If more than one player has the same strength high card, then the strength of the second highest card held wins. This continues through the five highest cards in the player's hands.
			if (hand.cards[0].suit == hand.cards[1].suit.Value 
				&& hand.cards[0].suit == hand.cards[2].suit
				&& hand.cards[0].suit == hand.cards[3].suit
				&& hand.cards[0].suit == hand.cards[4].suit)
            {
				hand.handValue = new HandValue(hand.cards[0].number.Value, HandValue.HandType.Flush);
            }
			// Three of a KindIf more than one player holds three of a kind, then the higher value of the cards used to make the three of kind determines the winner.
			// If two or more players have the same three of a kind, then a fourth card (and a fifth if necessary) can be used as kickers to determine the winner.
			else if (duplicates.Count > 0 && duplicates.Values.Where(cardTotal => cardTotal >= 3).Count() >= 1)
			{
				// Should also know WHAT card it is. Like if you have three 3's, but someone else has three 4's. The three 4's will win. If you both have three 3's then the high card will decide.
				int chosenCard = 0;
				foreach (int card in duplicates.Keys)
                {
					if (duplicates[card] >= 3 && card > chosenCard)
						chosenCard = card;
                }

				hand.handValue = new HandValue(chosenCard, HandValue.HandType.ThreeOfaKind);
			}
			// One Pair If two or more players hold a single pair, then highest pair wins. If the pairs are of the same value, the highest kicker card determines the winner.
			// A second and even third kicker can be used if necessary. High Card When no player has even a pair, then the highest card wins. When both players have identical high cards, the next highest card wins, and so on until five cards have been used. In the unusual circumstance that two players hold the identical five cards, the pot would be split.
			else if (duplicates.Count > 0 && duplicates.Values.Where(cardTotal => cardTotal >= 2).Count() >= 1)
			{
				// Should also know WHAT card it is. Like if you have three 3's, but someone else has three 4's. The three 4's will win. If you both have three 3's then the high card will decide.
				int chosenCard = 0;
				foreach (int card in duplicates.Keys)
				{
					if (duplicates[card] >= 2 && card > chosenCard)
						chosenCard = card;
				}

				hand.handValue = new HandValue(chosenCard, HandValue.HandType.OnePair);
			} else {
				// just use the first card in cards, since it's already sorted.
				hand.handValue = new HandValue(hand.cards[0].number.Value, HandValue.HandType.High);
			}
		}
	}
}
