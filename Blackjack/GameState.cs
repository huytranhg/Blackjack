//-----------------------------------------------------------------------
// <copyright file="GameState.cs" company="HT">
//     Copyright (c) HT. All rights reserved.
// </copyright>
// <author>Huy Tran</author>
//-----------------------------------------------------------------------

namespace Blackjack
{
    /// <summary>
    /// An enumeration of the possible game states
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// Checking hand over state.
        /// </summary>
        CheckingHandOver,

        /// <summary>
        /// Dealer Hitting state.
        /// </summary>
        DealerHitting,

        /// <summary>
        /// Display Hand Results state.
        /// </summary>
        DisplayingHandResults,

        /// <summary>
        /// Exiting state.
        /// </summary>
        Exiting,

        /// <summary>
        /// Player Hitting state.
        /// </summary>
        PlayerHitting,

        /// <summary>
        /// Checking hand over state.
        /// </summary>
        WaitingForDealer,

        /// <summary>
        /// Waiting For Player state.
        /// </summary>
        WaitingForPlayer,

        /// <summary>
        /// Start Over state.
        /// </summary>
        StartOver
    }
}
