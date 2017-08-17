//-----------------------------------------------------------------------
// <copyright file="Message.cs" company="HT">
//     Copyright (c) HT. All rights reserved.
// </copyright>
// <author>Huy Tran</author>
//-----------------------------------------------------------------------

namespace Blackjack
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A message
    /// </summary>
    public class Message
    {
        #region Fields

        /// <summary>
        /// String holds text
        /// </summary>
        private string text;

        /// <summary>
        /// Sprite Font holds font
        /// </summary>
        private SpriteFont font;

        /// <summary>
        /// Vector2 holds center position
        /// </summary>
        private Vector2 center;

        /// <summary>
        /// Vector2 holds position
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// Flag for flashing message for winner or draw
        /// </summary>
        private bool flashMode;

        /// <summary>
        /// Color white for message showing, transparent for flashing
        /// </summary>
        private Color whiteOrFlash;

        /// <summary>
        /// Elapsed Time
        /// </summary>
        private float totalMiliSecondsElapsed;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="text">the text for the message</param>
        /// <param name="font">the sprite font for the message</param>
        /// <param name="center">the center of the message</param>
        /// <param name="flashMode">flash mode for winner message</param>
        public Message(string text, SpriteFont font, Vector2 center, bool flashMode)
        {
            this.text = text;
            this.font = font;
            this.center = center;
            this.flashMode = flashMode;
            this.whiteOrFlash = Color.White;

            // calculate position from text and center
            float textWidth = font.MeasureString(text).X;
            float textHeight = font.MeasureString(text).Y;
            this.position = new Vector2(
                center.X - (textWidth / 2),
                center.Y - (textHeight / 2));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text for the message
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value;

                // changing text could change text location
                float textWidth = this.font.MeasureString(this.text).X;
                float textHeight = this.font.MeasureString(this.text).Y;
                this.position.X = this.center.X - (textWidth / 2);
                this.position.Y = this.center.Y - (textHeight / 2);
            }
        }

        /// <summary>
        /// Gets text position
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return this.position;
            }
        }

        /// <summary>
        /// Gets White or Transparent color
        /// </summary>
        /// Gets White or Transparent color
        public Color WhiteOrFlash
        {
            get
            {
                return this.whiteOrFlash;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the flashing button.
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update(GameTime gameTime)
        {
            if (this.flashMode == true)
            {
                // check for advancing animation frame
                this.totalMiliSecondsElapsed += gameTime.ElapsedGameTime.Milliseconds;
                this.FlashText(this.totalMiliSecondsElapsed);
            }
            else
            {
                this.whiteOrFlash = Color.White;
            }
        }

        /// <summary>
        /// Change to Flash Mode
        /// </summary>
        /// <param name="flashMode"> flash mode</param>
        public void ChangeMode(bool flashMode)
        {
            this.flashMode = flashMode;
        }

        /// <summary>
        /// Flash the text based on time of flash per second
        /// </summary>
        /// <param name="totalMiliSecondsElapsed">total time elapse since last animation move</param>
        private void FlashText(float totalMiliSecondsElapsed)
        {
            if (this.totalMiliSecondsElapsed > GameConstants.MessageMiliSecondsPerFlashing)
            {
                // reset frame timer
                this.totalMiliSecondsElapsed = 0;
                if (this.whiteOrFlash == Color.White)
                {
                    this.whiteOrFlash = Color.Transparent;
                }
                else
                {
                    this.whiteOrFlash = Color.White;
                }
            }
        }
        #endregion
    }
}