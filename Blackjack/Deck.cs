//-----------------------------------------------------------------------
// <copyright file="Deck.cs" company="HT">
//     Copyright (c) HT. All rights reserved.
// Hold Card and Deck objects
// </copyright>
// <author>Huy Tran</author>
//-----------------------------------------------------------------------

namespace Blackjack
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// A Deck of cards
    /// </summary>
    public class Deck
    {
        #region Fields

        /// <summary>
        /// count of current face down cards of deck
        /// </summary>
        private int count;

        /// <summary>
        /// Boolean flag if deck is empty card face up
        /// </summary>
        private bool empty;

        /// <summary>
        /// List of cars in deck
        /// </summary>
        private List<Card> deck;

        /// <summary>
        /// temp card used for shuffling
        /// </summary>
        private Card tempCard = null;

        /// <summary>
        /// Random Generator
        /// </summary>
        private Random rand = new Random();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Deck"/> class.
        /// </summary>      
        public Deck()
        {
            this.deck = new List<Card>(52);
            this.count = 0;
            this.empty = true;

            ////this.Shuffle();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of cards in the deck 
        /// </summary>
        public int Count
        {
            get { return this.count; }
        }

        /// <summary>
        /// Gets a value indicating whether the deck is empty
        /// </summary>
        public bool Empty
        {
            get { return this.empty; }
        }

        #endregion
        
        #region Methods      

        /// <summary>
        /// Add cards
        /// </summary>
        public void AddCards()
        {
            if (this.deck != null)
            {
                this.deck.Clear();
            }
            else
            {
                return;
            }

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    Card newCard = new Card(rank, suit, 0, 0);
                    this.deck.Add(newCard);
                }
            }

            if ((this.deck != null) && (this.deck.Count > 0))
            {
                this.count = this.deck.Count;
                this.empty = false;
            }
        }

        /// <summary>
        /// Shuffles the deck
        /// </summary>
        public void Shuffle()
        {          
            if ((this.deck != null) && (this.deck.Count > 1))
            {
                for (int i = this.deck.Count - 1; i > 0; i--)
                {
                    int n = this.rand.Next(i + 1);
                    this.tempCard = this.deck[i];
                    this.deck[i] = this.deck[n];
                    this.deck[n] = this.tempCard;
                    this.tempCard = null;
                }
            }
        }

        /// <summary>
        /// Takes the top card from the deck. If the deck is empty, returns null 
        /// </summary>
        /// <returns>the the top card of deck</returns>
        public Card TakeTopCard()
        {
            if ((this.deck != null) && (this.deck.Count > 0))
            {
                Card tempCard = this.deck[0];
                this.deck.RemoveAt(0);
                this.count = this.deck.Count;
                if (this.deck.Count == 0)
                {
                    this.empty = true;
                }

                return tempCard;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
