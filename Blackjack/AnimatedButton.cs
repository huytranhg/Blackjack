//-----------------------------------------------------------------------
// <copyright file="AnimatedButton.cs" company="HT">
//     Copyright (c) HT. All rights reserved.
// </copyright>
// <author>Huy Tran</author>
//-----------------------------------------------------------------------

namespace Blackjack
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// A class for a animate buttons
    /// </summary>
    public class AnimatedButton
    {
        #region Fields

        /// <summary>
        /// object location  object
        /// </summary>
        private Rectangle drawRectangle;

        /// <summary>
        /// field used to track and draw animations
        /// </summary>
        private Rectangle sourceRectangle;

        /// <summary>
        /// mouse click rectangle
        /// </summary>
        private Rectangle mouseClickRectangle;

        /// <summary>
        /// GameState object of state when clicking button
        /// </summary>
        private GameState clickState;

        /// <summary>
        /// Elapsed Time
        /// </summary>
        private float totalMiliSecondsElapsed;

        /// <summary>
        /// Time lapse between frames
        /// </summary>
        private float miliSecondsPerFrame; 

        /// <summary>
        /// Number of Sprite Sheet Rows
        /// </summary>
        private ushort numOfSpriteSheetRows;

        /// <summary>
        /// Number of Sprite Sheet Columns
        /// </summary>
        private ushort numOfSpriteSheetColumns;

        /// <summary>
        /// Current column
        /// </summary>
        private short currentColumn;

        /// <summary>
        /// Current row
        /// </summary>
        private short currentRow;

        /// <summary>
        /// sprite width
        /// </summary>
        private int spriteWidth;

        /// <summary>
        /// sprite height
        /// </summary>
        private int spriteHeight;

        /// <summary>
        /// frameWidth of a frame
        /// </summary>
        private int frameWidth;

        /// <summary>
        /// frameHeight of a frame
        /// </summary>
        private int frameHeight;

        /// <summary>
        /// Help flag to see of click started
        /// </summary>
        private bool clickStarted;

        /// <summary>
        /// Help flag to see of button released
        /// </summary>
        private bool buttonReleased;

        /// <summary>
        /// Flag to play animation in passive or active mode
        /// </summary>
        private bool animationActiveMode;

        /// <summary>
        /// Count variable of idle frame in passive mode before restarting animation
        /// </summary>
        private ushort idleFrameCount;

        /// <summary>
        /// flag for first time run through
        /// </summary>
        private bool lastFrameHere;

        /// <summary>
        /// flag for animation last frame reach
        /// </summary>
        private bool animationLastFrame;

        /// <summary>
        /// flag for running Backward
        /// </summary>
        private bool lastFrameDrawn;

        /// <summary>
        /// Assigned Key Name
        /// </summary>
        private Keys keyAssigned;

        /// <summary>
        /// Scale Factor for mouse input
        /// </summary>
        private ushort scaleFactor;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedButton"/> class.
        /// </summary>
        /// <param name="center">center position of button</param>
        /// <param name="clickState">the game state to change to when the button is clicked</param>
        /// <param name="numOfSpriteSheetColumns">the number of sprite sheet columns</param>
        /// <param name="numOfSpriteSheetRows">the number of sprite sheet rows</param>
        /// <param name="animationActiveMode">animation active mode</param>
        /// <param name="keyAssigned">key assigned</param>
        /// <param name="spriteWidth">sprite sheet width</param>
        /// <param name="spriteHeight">sprite sheet height</param>
        /// <param name="scaleFactor">scale factor for calculating mouse click rectangle</param>
        public AnimatedButton(
            Vector2 center,
            GameState clickState,
            ushort numOfSpriteSheetColumns,
            ushort numOfSpriteSheetRows,
            bool animationActiveMode,
            Keys keyAssigned,
            int spriteWidth,
            int spriteHeight,
            ushort scaleFactor)
        {
            this.numOfSpriteSheetColumns = numOfSpriteSheetColumns;
            this.numOfSpriteSheetRows = numOfSpriteSheetRows;
            this.clickState = clickState;
            this.currentRow = 0;
            this.currentColumn = 0;
            this.idleFrameCount = 0;
            this.lastFrameHere = false;
            this.lastFrameDrawn = false;
            this.animationLastFrame = false;
            this.animationActiveMode = animationActiveMode;
            this.clickStarted = false;
            this.buttonReleased = true;
            this.keyAssigned = keyAssigned;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.totalMiliSecondsElapsed = 0;
            this.scaleFactor = scaleFactor;
            this.miliSecondsPerFrame = GameConstants.ButtonMiliSecondsPerFrame;

            this.Initialize(center);
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets draw rectangle
        /// </summary>
        public Rectangle DrawRectangle
        {
            get
            {
                return this.drawRectangle;
            }
        }

        /// <summary>
        /// Gets source rectangle
        /// </summary>
        public Rectangle SourceRectangle
        {
            get
            {
                return this.sourceRectangle;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the animated button.
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="mouseState">the mouse state</param>
        /// <param name="keyboardState">the keyboard state</param>
        /// <param name="previousKeyboardState">the previous keyboard state</param>
        public void Update(GameTime gameTime, MouseState mouseState, KeyboardState keyboardState, KeyboardState previousKeyboardState)
        {
            // check for advancing animation frame
            this.totalMiliSecondsElapsed += gameTime.ElapsedGameTime.Milliseconds;

            // check for mouse over button
            if (this.mouseClickRectangle.Contains(mouseState.X, mouseState.Y))
            {
                this.miliSecondsPerFrame = GameConstants.ButtonMiliSecondsPerFrame / 3;

                // check for click started on button
                if (mouseState.LeftButton == ButtonState.Pressed &&
                    this.buttonReleased)
                {
                    this.clickStarted = true;
                    this.buttonReleased = false;
                }
                else if (mouseState.LeftButton == ButtonState.Released)
                {
                    this.buttonReleased = true;

                    // if click finished on button, change game state
                    if (this.clickStarted)
                    {
                        Game1.ChangeState(this.clickState);
                        this.clickStarted = false;
                        return;
                    }
                } 
            }
            else
            {
                this.miliSecondsPerFrame = GameConstants.ButtonMiliSecondsPerFrame;
               
                // no clicking on this button
                this.clickStarted = false;
                this.buttonReleased = false;
            }

            // Check for Space key is pressed and released
            if (keyboardState.IsKeyUp(this.keyAssigned) &&
               previousKeyboardState.IsKeyDown(this.keyAssigned))
            {
                Game1.ChangeState(this.clickState);
                return;
            }

            this.PlayAnimation(this.totalMiliSecondsElapsed);
        }

        /// <summary>
        /// Change animation mode of animated button
        /// </summary>
        /// <param name="animationActiveMode">active or passive mode, active is true, passive is false</param>
        public void ChangeToActiveMode(bool animationActiveMode)
        {
            this.animationActiveMode = animationActiveMode;
        }

        /// <summary>
        /// Reset button settings
        /// </summary>
        public void Reset()
        {
            this.currentRow = 0;
            this.currentColumn = 0;
            this.idleFrameCount = 0;
            this.lastFrameHere = false;
            this.lastFrameDrawn = false;
            this.animationLastFrame = false;
            this.clickStarted = false;
            this.buttonReleased = true;
            this.totalMiliSecondsElapsed = 0;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads the content for the animated button
        /// </summary>
        /// <param name="center">center position of button</param>
        private void Initialize(Vector2 center)
        {
            //// calculate frame size
            this.frameWidth = this.spriteWidth / this.numOfSpriteSheetColumns;
            this.frameHeight = this.spriteHeight / this.numOfSpriteSheetRows;

            //// set initial draw and source rectangles
            this.drawRectangle = new Rectangle(
                (int)(center.X - (this.frameWidth / 2)),
                (int)(center.Y - (this.frameHeight / 2)),
                this.frameWidth,
                this.frameHeight);
            this.sourceRectangle = new Rectangle(0, 0, this.frameWidth, this.frameHeight);

            this.mouseClickRectangle = new Rectangle(
                (int)((center.X * this.scaleFactor) - ((this.frameWidth * this.scaleFactor) / 2)),
                (int)((center.Y * this.scaleFactor) - ((this.frameHeight * this.scaleFactor) / 2)),
                this.frameWidth * this.scaleFactor,
                this.frameHeight * this.scaleFactor);
        }

        /// <summary>
        /// Sets the source rectangle location to correspond with the given row and column number
        /// </summary>
        /// <param name="currentColumn">current column</param>
        /// <param name="currentRow">current row</param>
        private void SetSourceRectangleLocation(short currentColumn, short currentRow)
        {
            // calculate X and Y based on current column and row
            this.sourceRectangle.X = currentColumn * this.frameWidth;
            this.sourceRectangle.Y = currentRow * this.frameHeight;
        }

        /// <summary>
        /// Play button animation in active mode
        /// </summary>
        /// <param name="totalMiliSecondsElapsed">total time elapse since last animation move</param>
        private void PlayAnimation(float totalMiliSecondsElapsed)
        {
            if (this.animationActiveMode == true)
            {
                if (this.totalMiliSecondsElapsed > this.miliSecondsPerFrame)
                {
                    // reset frame timer
                    this.totalMiliSecondsElapsed = 0;

                    if (this.animationLastFrame == false)
                    {
                        this.PlayForward();
                    }
                    else
                    {
                        this.PlayBackward();
                    }
                }
            }
            else
            {       
                if (this.totalMiliSecondsElapsed > this.miliSecondsPerFrame)
                {
                    // reset frame timer
                    this.totalMiliSecondsElapsed = 0;

                    // check here if last frame, first time run true
                    if (this.lastFrameHere == false)
                    {
                        // advance here
                        this.currentColumn++;
                        if (this.currentColumn <= GameConstants.ColumnsPerSpriteSheet - 1)
                        {
                            this.SetSourceRectangleLocation(this.currentColumn, this.currentRow);
                            return;
                        }
                        else
                        {
                            // advance here
                            this.currentRow++;
                            this.currentColumn = 0;

                            if (this.currentRow <= GameConstants.RowsPerSpriteSheet - 1)
                            {
                                this.SetSourceRectangleLocation(this.currentColumn, this.currentRow);
                                return;
                            }
                            else
                            {
                                // reset here
                                this.currentRow = 0;

                                // draw last frame
                                this.SetSourceRectangleLocation(
                                    GameConstants.ColumnsPerSpriteSheet - 1,
                                    GameConstants.RowsPerSpriteSheet - 1);

                                // Set flag so next loop knows that the button reaches last frame, and need to wait before reset
                                this.lastFrameHere = true;
                            }
                        }
                    }
                    else
                    {
                        this.idleFrameCount++;

                        if (this.idleFrameCount == GameConstants.MaxNumIdleFrame)
                        {
                            this.idleFrameCount = 0;
                            this.lastFrameHere = false;

                            // Set source to draw back to first frame
                            this.SetSourceRectangleLocation(this.currentColumn, this.currentRow);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Play button animation in forward frames
        /// </summary>
        private void PlayForward()
        {
            this.currentColumn++;

            if (this.currentColumn <= GameConstants.ColumnsPerSpriteSheet - 1)
            {
                this.SetSourceRectangleLocation(this.currentColumn, this.currentRow);
                return;
            }
            else
            {
                this.currentRow++;
                this.currentColumn = 0;

                if (this.currentRow <= GameConstants.RowsPerSpriteSheet - 1)
                {
                    this.SetSourceRectangleLocation(this.currentColumn, this.currentRow);
                    return;
                }
                else
                {
                    this.currentRow = 0;

                    // Set flag so next loop knows that the button reaches last frame, and need to draw first frame first
                    this.animationLastFrame = true;
                    this.currentColumn = GameConstants.ColumnsPerSpriteSheet - 1;
                    this.currentRow = GameConstants.RowsPerSpriteSheet - 1;
                }
            }
        }

        /// <summary>
        /// Play button animation in backward frames
        /// </summary>
        private void PlayBackward()
        {
            if ((this.currentColumn == GameConstants.ColumnsPerSpriteSheet - 1) &&
                (this.currentRow == GameConstants.RowsPerSpriteSheet - 1) &&
                this.lastFrameDrawn == false)
            {
                this.lastFrameDrawn = true;
                return;
            }

            if (this.lastFrameDrawn == true)
            {
                this.currentColumn--;
            }

            if (this.currentColumn < 0)
            {
                this.currentRow--;
                this.currentColumn = GameConstants.ColumnsPerSpriteSheet - 1;

                if (this.currentRow < 0)
                {
                    this.currentRow = GameConstants.RowsPerSpriteSheet - 1;
                    this.animationLastFrame = false;
                    this.lastFrameDrawn = false;
                    this.currentColumn = 0;
                    this.currentRow = 0;
                    return;
                }
                else
                {
                    this.SetSourceRectangleLocation(this.currentColumn, this.currentRow);
                    return;
                }
            }
            else
            {
                this.SetSourceRectangleLocation(this.currentColumn, this.currentRow);
                return;
            }
        }

        ////private void PlayAnimation(float totalMiliSecondsElapsed)
        ////{
        ////    if (this.animationActiveMode == true)
        ////    {
        ////        if (this.totalMiliSecondsElapsed > GameConstants.ButtonMiliSecondsPerFrame)
        ////        {
        ////            // reset frame timer
        ////            this.totalMiliSecondsElapsed = 0;

        ////            // check here if last frame, first time run true
        ////            if (this.lastFrameHere == true)
        ////            {
        ////                this.SetSourceRectangleLocation(this.currentColumn, this.currentRow);

        ////                // not last frame anymore
        ////                this.lastFrameHere = false;
        ////            }
        ////            else
        ////            {
        ////                this.currentColumn++;
        ////            }

        ////            if (this.currentColumn <= GameConstants.ColumnsPerSpriteSheet - 1)
        ////            {
        ////                this.SetSourceRectangleLocation(this.currentColumn, this.currentRow);
        ////                return;
        ////            }
        ////            else
        ////            {
        ////                this.currentRow++;
        ////                this.currentColumn = 0;

        ////                if (this.currentRow <= GameConstants.RowsPerSpriteSheet - 1)
        ////                {
        ////                    this.SetSourceRectangleLocation(this.currentColumn, this.currentRow);
        ////                    return;
        ////                }
        ////                else
        ////                {
        ////                    this.currentRow = 0;
        ////                    this.SetSourceRectangleLocation(
        ////                        GameConstants.ColumnsPerSpriteSheet - 1,
        ////                        GameConstants.RowsPerSpriteSheet - 1);

        ////                    // Set flag so next loop knows that the button reaches last frame, and need to draw first frame first
        ////                    this.lastFrameHere = true;
        ////                }
        ////            }
        ////        }
        ////    }
        ////    else
        ////    {
        ////        if (this.totalMiliSecondsElapsed > GameConstants.ButtonMiliSecondsPerFrame)
        ////        {
        ////            // reset frame timer
        ////            this.totalMiliSecondsElapsed = 0;

        ////            // check here if last frame, first time run true
        ////            if (this.lastFrameHere == false)
        ////            {
        ////                // advance here
        ////                this.currentColumn++;
        ////                if (this.currentColumn <= GameConstants.ColumnsPerSpriteSheet - 1)
        ////                {
        ////                    this.SetSourceRectangleLocation(this.currentColumn, this.currentRow);
        ////                    return;
        ////                }
        ////                else
        ////                {
        ////                    // advance here
        ////                    this.currentRow++;
        ////                    this.currentColumn = 0;

        ////                    if (this.currentRow <= GameConstants.RowsPerSpriteSheet - 1)
        ////                    {
        ////                        this.SetSourceRectangleLocation(this.currentColumn, this.currentRow);
        ////                        return;
        ////                    }
        ////                    else
        ////                    {
        ////                        // reset here
        ////                        this.currentRow = 0;

        ////                        // draw last frame
        ////                        this.SetSourceRectangleLocation(
        ////                            GameConstants.ColumnsPerSpriteSheet - 1,
        ////                            GameConstants.RowsPerSpriteSheet - 1);

        ////                        // Set flag so next loop knows that the button reaches last frame, and need to wait before reset
        ////                        this.lastFrameHere = true;
        ////                    }
        ////                }
        ////            }
        ////            else
        ////            {
        ////                this.idleFrameCount++;

        ////                if (this.idleFrameCount == GameConstants.MaxNumIdleFrame)
        ////                {
        ////                    this.idleFrameCount = 0;
        ////                    this.lastFrameHere = false;

        ////                    // Set source to draw back to first frame
        ////                    this.SetSourceRectangleLocation(this.currentColumn, this.currentRow);
        ////                }
        ////            }
        ////        }
        ////    }
        ////}

        #endregion
    }
}