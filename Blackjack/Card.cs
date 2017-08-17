//-----------------------------------------------------------------------
// <copyright file="Card.cs" company="HT">
//     Copyright (c) HT. All rights reserved.
// Hold Card and Deck objects
// </copyright>
// <author>Huy Tran</author>
//-----------------------------------------------------------------------

namespace Blackjack
{
    /// <summary>
    /// A class for a cards
    /// </summary>
    public class Card
    {
        #region Fields

        /// <summary>
        /// Boolean flag for card face up
        /// </summary>
        private bool faceUp = false;

        /// <summary>
        /// Position X of card
        /// </summary>
        private int cardX = 0;

        /// <summary>
        /// Position Y of card
        /// </summary>
        private int cardY = 0;

        /// <summary>
        /// Card Rank
        /// </summary>
        private Rank cardRank;

        /// <summary>
        /// Card Suit
        /// </summary>
        private Suit cardSuit;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>      
        /// <param name="rank">Rank of card to be created</param>
        /// <param name="suit">Suit of card to be created</param>
        /// <param name="x">x position of card</param>
        /// <param name="y">y position of card</param>
        public Card(Rank rank, Suit suit, int x, int y)
        {
            this.cardRank = rank;
            this.cardSuit = suit;
            this.cardX = x;
            this.cardY = y;
            this.faceUp = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the card is face up 
        /// </summary>
         public bool FaceUp
        {
            get { return this.faceUp; }
        }

        /// <summary>
        /// Gets the rank of the card
        /// </summary>
        public Rank Rank
        {
            get { return this.cardRank; }
        }

        /// <summary>
        /// Gets the suit of the card
        /// </summary>
        public Suit Suit
        {
            get { return this.cardSuit; }
        }

        /// <summary>
        /// Gets or sets the centered x location of the card
        /// </summary>
        public int X
        {
            get
            {
                return this.cardX;
            }

            set
            {
                this.cardX = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the centered Y location of the card
        /// </summary>
        public int Y
        {
            get
            {
                return this.cardY;
            }

            set
            {
                this.cardY = value;
            }            
        }

        #endregion

        #region Methods

        /// <summary>
        /// Flip the card over
        /// </summary>
        public void FlipOver()
        {
            this.faceUp = true;
        }

        #endregion
   }
}
