//-----------------------------------------------------------------------
// <copyright file="MenuButton.cs" company="HT">
//     Copyright (c) HT. All rights reserved.
// </copyright>
// <author>Huy Tran</author>
//-----------------------------------------------------------------------

namespace Blackjack
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// A class for a menu button
    /// </summary>
    public class MenuButton
    {
        #region Fields

        /// <summary>
        /// Texture2D sprite object of button
        /// </summary>
        private Texture2D sprite;

        /// <summary>
        /// Button width
        /// </summary>
        private int buttonWidth;

        /// <summary>
        /// Drawing Rectangle object of button
        /// </summary>
        private Rectangle drawRectangle;

        /// <summary>
        /// Source Rectangle object of button
        /// </summary>
        private Rectangle sourceRectangle;

        /// <summary>
        /// GameState object of state when clicking button
        /// </summary>
        private GameState clickState;

        /// <summary>
        /// Help flag to see of click started
        /// </summary>
        private bool clickStarted = false;

        /// <summary>
        /// Help flag to see of button released
        /// </summary>
        private bool buttonReleased = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuButton"/> class.
        /// </summary>
        /// <param name="sprite">the sprite for the button</param>
        /// <param name="center">the center of the button</param>
        /// <param name="clickState">the game state to change to when the button is clicked</param>
        public MenuButton(Texture2D sprite, Vector2 center, GameState clickState)
        {
            this.sprite = sprite;
            this.clickState = clickState;
            this.Initialize(center);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the button to check for a button click
        /// </summary>
        /// <param name="mouse">the current mouse state</param>
        public void Update(MouseState mouse)
        {
             // check for mouse over button
            if (this.drawRectangle.Contains(mouse.X, mouse.Y))
            {
                // highlight button
                // this.sourceRectangle.X = this.buttonWidth;

                // check for click started on button
                if (mouse.LeftButton == ButtonState.Pressed &&
                    this.buttonReleased)
                {
                    this.clickStarted = true;
                    this.buttonReleased = false;
                }
                else if (mouse.LeftButton == ButtonState.Released)
                {
                    this.buttonReleased = true;

                    // if click finished on button, change game state
                    if (this.clickStarted)
                    {
                        Game1.ChangeState(this.clickState);
                        this.clickStarted = false;
                    }
                }
            }
            else
            {
                this.sourceRectangle.X = 0;

                // no clicking on this button
                this.clickStarted = false;
                this.buttonReleased = false;
            }
        }

        /// <summary>
        /// Draws the button
        /// </summary>
        /// <param name="spriteBatch">the sprite batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.sprite, this.drawRectangle, this.sourceRectangle, Color.White);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes the button characteristics
        /// </summary>
        /// <param name="center">the center of the button</param>
         private void Initialize(Vector2 center)
        {
            // calculate button width
            // this.buttonWidth = this.sprite.Width / ImagesPerRow;
            this.buttonWidth = this.sprite.Width;

            // set initial draw and source rectangles
            this.drawRectangle = new Rectangle(
                (int)(center.X - (this.buttonWidth / 2)),
                (int)(center.Y - (this.sprite.Height / 2)),
                this.buttonWidth,
                this.sprite.Height);
            this.sourceRectangle = new Rectangle(0, 0, this.buttonWidth, this.sprite.Height);
        }

        #endregion
    }
}
