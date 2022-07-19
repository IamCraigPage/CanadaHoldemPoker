using System;
using NUnit.Framework;
using System.Collections.Generic;
using CanadaHoldemPoker;
using CanadaHoldemPoker.Models;
using System.Linq;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestNullCards()
        {
            Card[] cards = null;
            Hand hand = new Hand(cards, new Player("Craig"));
            PokerLogic.CalculateHandValue(hand);

            Assert.AreEqual(null, hand.handValue);
            Assert.AreEqual(null, hand.cards);
        }
        [Test]
        public void TestNullPlayer()
        {
            Card[] cards = new Card[] { new Card(1, Card.Suit.Clubs), new Card(7, Card.Suit.Clubs), new Card(8, Card.Suit.Clubs), new Card(9, Card.Suit.Clubs), new Card(10, Card.Suit.Clubs) };
            Hand hand = new Hand(cards, null);
            PokerLogic.CalculateHandValue(hand);

            Assert.AreEqual(HandValue.HandType.Flush, hand.handValue.handType, "Flush");
            Assert.AreEqual(hand.cards[0].number, 10, "High Card");
            Assert.AreEqual(hand.cards[4].number, 1, "Low Card");
        }

        [Test]
        public void TestNullHand()
        {
            PokerLogic.CalculateHandValue(null);
            Assert.Pass("Nothing to check, just don't blow up. :O");
        }


        [Test]
        public void TestFlush1()
        {
            Card[] cards = new Card[] { new Card(1, Card.Suit.Clubs), new Card(7, Card.Suit.Clubs), new Card(8, Card.Suit.Clubs), new Card(9, Card.Suit.Clubs), new Card(10, Card.Suit.Clubs) };
            Hand hand = new Hand(cards, new Player("Craig"));
            PokerLogic.CalculateHandValue(hand);

            Assert.AreEqual(HandValue.HandType.Flush, hand.handValue.handType, "Flush");
            Assert.AreEqual(hand.cards[0].number, 10, "High Card");
            Assert.AreEqual(hand.cards[4].number, 1, "Low Card");
        }

        [Test]
        public void TestThreeOfaKind1()
        {
            Card[] cards = new Card[] { new Card(3, Card.Suit.Hearts), new Card(3, Card.Suit.Clubs), new Card(3, Card.Suit.Diamonds), new Card(2, Card.Suit.Spades), new Card(10, Card.Suit.Clubs) };
            Hand hand = new Hand(cards, new Player("Craig"));
            PokerLogic.CalculateHandValue(hand);

            Assert.AreEqual(HandValue.HandType.ThreeOfaKind, hand.handValue.handType, "Three of a kind");
            Assert.AreEqual(3, hand.handValue.cardValue, "3 in Three of a kind");
            Assert.AreEqual(hand.cards[0].number, 10, "High Card");
            Assert.AreEqual(hand.cards[4].number, 2, "Low Card");
        }

        [Test]
        public void TestHighCard()
        {
            Card[] cards = new Card[] { new Card(1, Card.Suit.Hearts), new Card(7, Card.Suit.Clubs), new Card(8, Card.Suit.Diamonds), new Card(9, Card.Suit.Spades), new Card(10, Card.Suit.Clubs) };
            Hand hand = new Hand(cards, new Player("Craig"));
            PokerLogic.CalculateHandValue(hand);

            Assert.AreEqual(HandValue.HandType.High, hand.handValue.handType, "High Card");
            Assert.AreEqual(hand.cards[0].number, 10, "High Card");
            Assert.AreEqual(hand.cards[4].number, 1, "Low Card");
        }

        [Test]
        public void TestHand1()
        {
            List<Hand> hands = new List<Hand>();
            // Joe: 8S, 8D, AD, QD, JH
            Card[] cards = new Card[] { new Card(8, Card.Suit.Spades), new Card(8, Card.Suit.Diamonds), new Card(14, Card.Suit.Diamonds), new Card(12, Card.Suit.Diamonds), new Card(11, Card.Suit.Hearts) };
            hands.Add(new Hand(cards, new Player("Joe")));

            // Bob: AS, QS, 8S, 6S, 4S
            cards = new Card[] { new Card(14, Card.Suit.Spades), new Card(12, Card.Suit.Spades), new Card(8, Card.Suit.Spades), new Card(6, Card.Suit.Spades), new Card(4, Card.Suit.Spades) };
            hands.Add(new Hand(cards, new Player("Bob")));

            // Sally: 4S, 4H, 3H, QC, 8C
            cards = new Card[] { new Card(4, Card.Suit.Spades), new Card(4, Card.Suit.Hearts), new Card(3, Card.Suit.Hearts), new Card(12, Card.Suit.Clubs), new Card(8, Card.Suit.Clubs) };
            hands.Add(new Hand(cards, new Player("Sally")));

            var winner = PokerLogic.DetermineWinner(hands);

            Assert.AreEqual(1, winner.Count);
            Assert.AreEqual("Bob", winner[0].player.name); // Bob's flush beats Joe's pair.
        }

        [Test]
        public void TestHand2()
        {
            List<Hand> hands = new List<Hand>();
            // Joe: QD, 8D, KD, 7D, 3D
            Card[] cards = new Card[] { new Card(12, Card.Suit.Diamonds), new Card(8, Card.Suit.Diamonds), new Card(13, Card.Suit.Diamonds), new Card(7, Card.Suit.Diamonds), new Card(3, Card.Suit.Diamonds) };
            hands.Add(new Hand(cards, new Player("Joe")));

            // Bob: AS, QS, 8S, 6S, 4S
            cards = new Card[] { new Card(14, Card.Suit.Spades), new Card(12, Card.Suit.Spades), new Card(8, Card.Suit.Spades), new Card(6, Card.Suit.Spades), new Card(4, Card.Suit.Spades) };
            hands.Add(new Hand(cards, new Player("Bob")));

            // Sally: 4S, 4H, 3H, QC, 8C
            cards = new Card[] { new Card(4, Card.Suit.Spades), new Card(4, Card.Suit.Hearts), new Card(3, Card.Suit.Hearts), new Card(12, Card.Suit.Clubs), new Card(8, Card.Suit.Clubs) };
            hands.Add(new Hand(cards, new Player("Sally")));

            var winner = PokerLogic.DetermineWinner(hands);

            Assert.AreEqual(1, winner.Count);
            Assert.AreEqual("Bob", winner[0].player.name); // Bob's flush beats Joe's flush, because Bob's flush has an ace vs Joe's king.
        }

        [Test]
        public void TestHand3()
        {
            List<Hand> hands = new List<Hand>();
            // Joe: 3H, 5D, 9C, 9D, QH 
            Card[] cards = new Card[] { new Card(3, Card.Suit.Hearts), new Card(5, Card.Suit.Diamonds), new Card(9, Card.Suit.Clubs), new Card(9, Card.Suit.Diamonds), new Card(12, Card.Suit.Hearts) };
            hands.Add(new Hand(cards, new Player("Joe")));

            // Jen: 5C, 7D, 9H, 9S, QS 
            cards = new Card[] { new Card(5, Card.Suit.Clubs), new Card(7, Card.Suit.Diamonds), new Card(9, Card.Suit.Hearts), new Card(9, Card.Suit.Spades), new Card(12, Card.Suit.Spades) };
            hands.Add(new Hand(cards, new Player("Jen")));

            // Bob: 2H, 2C, 5S, 10C, AH 
            cards = new Card[] { new Card(2, Card.Suit.Hearts), new Card(2, Card.Suit.Clubs), new Card(5, Card.Suit.Spades), new Card(10, Card.Suit.Clubs), new Card(14, Card.Suit.Hearts) };
            hands.Add(new Hand(cards, new Player("Bob")));

            var winner = PokerLogic.DetermineWinner(hands);

            Assert.AreEqual(1, winner.Count);
            Assert.AreEqual("Jen", winner[0].player.name); // The battle of pairs! Jen's pair is the same as Joe's pair, so she wins it on the 4th highest card tie breaker.
        }

        [Test]
        public void TestHand4()
        {
            List<Hand> hands = new List<Hand>();
            // Joe: 2H, 3D, 4C, 5D, 10H
            Card[] cards = new Card[] { new Card(2, Card.Suit.Hearts), new Card(3, Card.Suit.Diamonds), new Card(4, Card.Suit.Clubs), new Card(5, Card.Suit.Diamonds), new Card(10, Card.Suit.Hearts) };
            hands.Add(new Hand(cards, new Player("Joe")));

            // Jen: 5C, 7D, 8H, 9S, QD 
            cards = new Card[] { new Card(5, Card.Suit.Clubs), new Card(7, Card.Suit.Diamonds), new Card(8, Card.Suit.Hearts), new Card(9, Card.Suit.Spades), new Card(12, Card.Suit.Spades) };
            hands.Add(new Hand(cards, new Player("Jen")));

            // Bob: 2C, 4D, 5S, 10C, JH
            cards = new Card[] { new Card(2, Card.Suit.Clubs), new Card(4, Card.Suit.Diamonds), new Card(5, Card.Suit.Spades), new Card(10, Card.Suit.Clubs), new Card(11, Card.Suit.Hearts) };
            hands.Add(new Hand(cards, new Player("Bob")));

            var winner = PokerLogic.DetermineWinner(hands);

            Assert.AreEqual(1, winner.Count);
            Assert.AreEqual("Jen", winner[0].player.name); // Jen's pair beats the other's high cards.
        }

        [Test]
        public void TestTie2Way()
        {
            List<Hand> hands = new List<Hand>();
            // Joe: 2H, 3D, 4C, 5D, 10H
            Card[] cards = new Card[] { new Card(2, Card.Suit.Hearts), new Card(3, Card.Suit.Diamonds), new Card(4, Card.Suit.Clubs), new Card(5, Card.Suit.Diamonds), new Card(10, Card.Suit.Hearts) };
            hands.Add(new Hand(cards, new Player("Joe")));

            // Jen: 7C, 7D, 8H, 9S, QD 
            cards = new Card[] { new Card(7, Card.Suit.Clubs), new Card(7, Card.Suit.Diamonds), new Card(8, Card.Suit.Hearts), new Card(9, Card.Suit.Spades), new Card(12, Card.Suit.Spades) };
            hands.Add(new Hand(cards, new Player("Jen")));

            // Bob: 7C, 7D, 8H, 9S, QD 
            cards = new Card[] { new Card(7, Card.Suit.Clubs), new Card(7, Card.Suit.Diamonds), new Card(8, Card.Suit.Hearts), new Card(9, Card.Suit.Spades), new Card(12, Card.Suit.Spades) };
            hands.Add(new Hand(cards, new Player("Bob")));

            var winner = PokerLogic.DetermineWinner(hands);

            Assert.AreEqual(2, winner.Count);

            Assert.IsTrue(winner.Exists(w => w.player.name == "Jen"));
            Assert.IsTrue(winner.Exists(w => w.player.name == "Bob"));
        }

        [Test]
        public void TestTie3Way()
        {
            List<Hand> hands = new List<Hand>();
            // Joe: 2H, 3D, 4C, 5D, 10H
            Card[] cards = new Card[] { new Card(2, Card.Suit.Hearts), new Card(3, Card.Suit.Diamonds), new Card(4, Card.Suit.Clubs), new Card(5, Card.Suit.Diamonds), new Card(10, Card.Suit.Hearts) };
            hands.Add(new Hand(cards, new Player("Joe")));

            // Jen: 7C, 7D, 8H, 9S, QD 
            cards = new Card[] { new Card(2, Card.Suit.Hearts), new Card(3, Card.Suit.Diamonds), new Card(4, Card.Suit.Clubs), new Card(5, Card.Suit.Diamonds), new Card(10, Card.Suit.Hearts) };
            hands.Add(new Hand(cards, new Player("Jen")));

            // Bob: 7C, 7D, 8H, 9S, QD 
            cards = new Card[] { new Card(2, Card.Suit.Hearts), new Card(3, Card.Suit.Diamonds), new Card(4, Card.Suit.Clubs), new Card(5, Card.Suit.Diamonds), new Card(10, Card.Suit.Hearts) };
            hands.Add(new Hand(cards, new Player("Bob")));

            List<Hand> winner = PokerLogic.DetermineWinner(hands);

            Assert.AreEqual(3, winner.Count);

            var names = winner.Select(w => w.player.name).ToList();
            Assert.Contains("Jen", names);
            Assert.Contains("Bob", names);
            Assert.Contains("Joe", names);
        }
    }
}
