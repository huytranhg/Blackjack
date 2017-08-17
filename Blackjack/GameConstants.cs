//-----------------------------------------------------------------------
// <copyright file="GameConstants.cs" company="HT">
//     Copyright (c) HT. All rights reserved.
// </copyright>
// <author>Huy Tran</author>
//-----------------------------------------------------------------------

namespace Blackjack
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// All the constants used in the game
    /// </summary>
    public static class GameConstants
    {
        /// <summary>
        /// Max valid Blackjack score for a hand
        /// </summary>
        public const int MaxHandValue = 21;

        /// <summary>
        /// Max cards to be taken
        /// </summary>
        public const int MaxCards = 5;

        /// <summary>
        /// Dealer min score to win a round
        /// </summary>
        public const int DealerMinimumScore = 17;

        /// <summary>
        /// Recommended player Hit score
        /// </summary>
        public const int PlayerRecommendedScore = 15;

        /// <summary>
        /// This is the variable that hold constant Windows Width of Game1 class
        /// </summary>
        public const int WindowWidth = 640;

        /// <summary>
        /// This is the variable that hold constant Windows Height of Game1 class
        /// </summary>
        public const int WindowHeight = 360;

        /// <summary>
        /// Full HD Width
        /// </summary>
        public const int FullHDWidth = 1920;

        /// <summary>
        /// Full HD Height
        /// </summary>
        public const int FullHDHeight = 1080;

        /// <summary>
        /// mHD Width
        /// </summary>
        public const int MHDWidth = 1280;

        /// <summary>
        /// mHD Height
        /// </summary>
        public const int MHDHeight = 720;

        /// <summary>
        /// Offset for row of cards of Dealer on screen
        /// </summary>
        public const int HeightTopLineOffset = WindowHeight / 3;

        /// <summary>
        /// Offset for row of cards of Dealer on screen
        /// </summary>
        public const int HeightBottomLineOffset = (WindowHeight / 3) * 2;

        /// <summary>
        /// Middle line offset for showing buttons on screen
        /// </summary>
        public const int HeightMidLineOffset = WindowHeight / 2;

        /// <summary>
        /// Offset for width first line on screen
        /// </summary>
        public const int WidthtFirstLineOffset = WindowWidth / 3;

        /// <summary>
        /// Offset for width second line on screen
        /// </summary>
        public const int WidthtSecondLineOffset = (WindowWidth / 3) * 2;

        /// <summary>
        /// Offset for width mid line on screen
        /// </summary>
        public const int WidthtMidLineOffset = WindowWidth / 2;

        /// <summary>
        /// Offset for placement of cards and button on Windows Screen
        /// </summary>
        public const int VerticalCardSpacing = 53;

        /// <summary>
        /// Offset value for cards and messages drawing
        /// </summary>
        public const int ScoreMessageTopOffset = 45;

        /// <summary>
        /// Offset value for cards and messages drawing
        /// </summary>
        public const int ScoreMessageBottomOffset = WindowHeight - ScoreMessageTopOffset;

        /// <summary>
        /// Offset value for cards and messages drawing
        /// </summary>
        public const int HorizontalMessageOffset = 150;

        /// <summary>
        /// Offset value for cards and messages drawing
        /// </summary>
        public const int TopMenuButtonOffset = HeightTopLineOffset;

        /// <summary>
        /// Offset value for buttons drawing
        /// </summary>
        public const int QuitMenuButtonOffset = WindowHeight - HeightTopLineOffset;

        /// <summary>
        /// Offset value for buttons drawing
        /// </summary>
        public const int HorizontalMenuButtonOffset = WindowWidth / 2;

        /// <summary>
        /// Number of frames per second when running button animation
        /// </summary>
        public const ushort ButtonFramePerSecond = 6;

        /// <summary>
        /// Amount of time for each frame
        /// </summary>
        public const int ButtonMiliSecondsPerFrame = 1000 / ButtonFramePerSecond;

        /// <summary>
        /// Number of columns per sprite sheet
        /// </summary>
        public const ushort ColumnsPerSpriteSheet = 2;
        
        /// <summary>
        /// Number of rows per sprite sheet
        /// </summary>
        public const ushort RowsPerSpriteSheet = 3;

        /// <summary>
        /// Idle frame to stop in passive mode
        /// </summary>
        public const ushort MaxNumIdleFrame = ButtonFramePerSecond * 6;

        /// <summary>
        /// Times to flash each second for winner count number
        /// </summary>
        public const ushort MessageMiliSecondsPerFlashing = 1000 / 6 * 2;

        /// <summary>
        /// Card Width
        /// </summary>
        public const ushort CardWidth = 45;

        /// <summary>
        /// Card Height
        /// </summary>
        public const ushort CardHeight = 63;

        /// <summary>
        /// Maximum Counts of winning times before reset
        /// </summary>
        public const ushort MaxCount = 9999;
    }
}