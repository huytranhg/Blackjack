//-----------------------------------------------------------------------
// <copyright file="RoundResult.cs" company="HT">
//     Copyright (c) HT. All rights reserved.
// </copyright>
// <author>Huy Tran</author>
//-----------------------------------------------------------------------

namespace Blackjack
{
    /// <summary>
    /// An enumeration of the possible game states
    /// </summary>
    public enum RoundResult
    {
        /// <summary>
        /// Player Win.
        /// </summary>
        PlayerWon,

        /// <summary>
        /// Dealer Win.
        /// </summary>
        DealerWon,

        /// <summary>
        /// Draw state.
        /// </summary>
        Draw,

        /// <summary>
        /// Wait for player moves
        /// </summary>
        WaitingForPlayer,
    }
}