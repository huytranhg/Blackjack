//-----------------------------------------------------------------------
// <copyright file="Game1.cs" company="HT">
//     Copyright (c) HT. All rights reserved.
// </copyright>
// <author>Huy Tran</author>
//-----------------------------------------------------------------------

namespace Blackjack
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        /// <summary>
        /// Static GameSate object for default
        /// </summary>
        private static GameState currentState;

        /// <summary>
        /// Flag for a round result
        /// </summary>
        private static RoundResult currentResult;

        /// <summary>
        /// This is the variable that hold graphics object of the Game1 class
        /// </summary>
        private GraphicsDeviceManager graphics;

        /// <summary>
        /// Target Render, native resolution
        /// </summary>
        private RenderTarget2D targetRenderNative;

        /// <summary>
        /// This is the variable that hold sprite batch object of the Game1 class
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// List of cards object that holds dealer cards each round
        /// </summary>
        private List<Card> dealerHand;

        /// <summary>
        /// List of cards object that holds player cards each round
        /// </summary>
        private List<Card> playerHand;

        /// <summary>
        /// Card Draw Rectangle
        /// </summary>
        private Rectangle cardDrawRectangle;

        /// <summary>
        /// List of fixed loaded texture for cards in game
        /// </summary>
        private Dictionary<string, Texture2D> allCardsTextures;

        /// <summary>
        /// List of fixed textures for buttons in game
        /// </summary>
        private Dictionary<string, Texture2D> allButtonsTexture;

        /// <summary>
        /// Sprite object that holds drawing sprite of message font
        /// </summary>
        private SpriteFont messageFont;

        /// <summary>
        /// game only one deck object
        /// </summary>
        private Deck oneDeck;

        /// <summary>
        /// List of message objects that holds all of the game allMessages
        /// </summary>
        private Dictionary<string, Message> allMessages;

        /// <summary>
        /// List of AnimatedButton objects that holds all buttons of Game1 class
        /// </summary>
        private Dictionary<string, AnimatedButton> animatedButtons;

        /// <summary>
        /// Player Score
        /// </summary>
        private int playerScore;

        /// <summary>
        /// Dealer Score
        /// </summary>
        private int dealerScore;

        /// <summary>
        /// Counter for Player won
        /// </summary>
        private int playerWonCount;

        /// <summary>
        /// Counter for Dealer won
        /// </summary>
        private int dealerWonCount;

        /// <summary>
        /// Counter for draw
        /// </summary>
        private int drawCount;

        /// <summary>
        /// Flag for a round result
        /// </summary>
        private bool playerWon;

        /// <summary>
        /// Flag for a round result
        /// </summary>
        private bool dealerWon;

        /// <summary>
        /// Flag for a round result
        /// </summary>
        private bool drawWon;

        /// <summary>
        /// Flag for active mode of buttons
        /// </summary>
        private bool hitOrNot;

        /// <summary>
        /// Mouse State
        /// </summary>
        private MouseState mouseState;

        /// <summary>
        /// Keyboard State 
        /// </summary>
        private KeyboardState keyboardState;

        /// <summary>
        /// Previous Keyboard State 
        /// </summary>
        private KeyboardState previousKeyboardState;

        /// <summary>
        /// Scale Factor
        /// </summary>
        private ushort scaleFactor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game1"/> class.
        /// </summary>
        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // set resolution and show mouse
            //this.graphics.PreferredBackBufferWidth = GameConstants.WindowWidth;
            //this.graphics.PreferredBackBufferHeight = GameConstants.WindowHeight;
            this.IsMouseVisible = true;

            int userDisplayWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int userDisplayHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            if ((userDisplayWidth > GameConstants.MHDWidth) &&
                (userDisplayWidth <= GameConstants.FullHDWidth) &&
                (userDisplayHeight > GameConstants.MHDHeight) &&
                (userDisplayHeight <= GameConstants.FullHDHeight))
            {
                this.graphics.PreferredBackBufferWidth = GameConstants.MHDWidth;
                this.graphics.PreferredBackBufferHeight = GameConstants.MHDHeight;
                this.scaleFactor = 2;
            }
            else if ((userDisplayWidth > GameConstants.FullHDWidth) &&
                (userDisplayHeight > GameConstants.FullHDHeight))
            {
                this.graphics.PreferredBackBufferWidth = GameConstants.FullHDWidth;
                this.graphics.PreferredBackBufferHeight = GameConstants.FullHDHeight;
                this.scaleFactor = 3;
            }
            else
            {
                this.graphics.PreferredBackBufferWidth = GameConstants.WindowWidth;
                this.graphics.PreferredBackBufferHeight = GameConstants.WindowHeight;
                this.scaleFactor = 1;
            }
        }

        /// <summary>
        /// Changes the state of the game
        /// </summary>
        /// <param name="newState">the new game state</param>
        public static void ChangeState(GameState newState)
        {
            currentState = newState;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // initialize game objects
            currentState = GameState.WaitingForPlayer;
            currentResult = RoundResult.WaitingForPlayer;
            this.dealerHand = new List<Card>(5);
            this.playerHand = new List<Card>(5);

            // Card textures for 52 and one back texture
            this.allCardsTextures = new Dictionary<string, Texture2D>(53);
            this.allButtonsTexture = new Dictionary<string, Texture2D>(3);
            this.oneDeck = new Deck();
            this.allMessages = new Dictionary<string, Message>(5);
            this.animatedButtons = new Dictionary<string, AnimatedButton>(3);
            this.playerWonCount = 0;
            this.dealerWonCount = 0;
            this.drawCount = 0;
            this.playerWon = false;
            this.dealerWon = false;
            this.drawWon = false;
            this.hitOrNot = false;
            this.playerScore = 0;
            this.dealerScore = 0;
            this.previousKeyboardState = Keyboard.GetState();
            this.targetRenderNative = new RenderTarget2D(
                GraphicsDevice,
                GameConstants.WindowWidth,
                GameConstants.WindowHeight);

            base.Initialize();

            // Start shuffling deck cards and distribute to dealer and player
            if (this.oneDeck != null)
            {
                this.StartNewRound();
            }

            // Add buttons
            if (this.animatedButtons != null)
            {
                this.UpdateAllButtons();
            }

            // Add Messages
            if (this.allMessages != null)
            {
                this.UpdatePlayerMessages();
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            // load sprite font
            this.messageFont = Content.Load<SpriteFont>(@"fonts\BlackjackPixel");

            // load hit button sprite
            this.allButtonsTexture.Add("Hit Button", Content.Load<Texture2D>(@"graphics\animations\hitbuttonspritesheet"));

            // load stand button sprite
            this.allButtonsTexture.Add("Stand Button", Content.Load<Texture2D>(@"graphics\animations\standbuttonspritesheet"));

            // load power button sprite
            this.allButtonsTexture.Add("Power Button", Content.Load<Texture2D>(@"graphics\animations\powerbuttonspritesheet"));

            // load 52 cards texture
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    this.allCardsTextures.Add(
                        suit.ToString() + rank.ToString(),
                        Content.Load<Texture2D>(@"graphics" + "\\" + suit.ToString() + "\\" + rank.ToString()));
                }
            }

            // add card back texture
            this.allCardsTextures.Add(
                        "Back",
                        Content.Load<Texture2D>(@"graphics\Back"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            if (this.targetRenderNative != null)
            {
                this.targetRenderNative.Dispose();
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            this.mouseState = Mouse.GetState();
            this.keyboardState = Keyboard.GetState();
            this.UpdateScore();
            this.UpdateAllMessages(gameTime);

            // game state-specific processing
            switch (currentState)
            {
                case GameState.WaitingForPlayer:
                    {
                        this.UpdatePlayerMessages();

                        int playerScore = this.GetBlackjackScore(this.playerHand);

                        if (playerScore > GameConstants.MaxHandValue)
                        {
                            ChangeState(GameState.WaitingForDealer);
                        }
                        else
                        {
                            // update animated buttons
                            this.animatedButtons["Hit Button"].Update(gameTime, this.mouseState, this.keyboardState, this.previousKeyboardState);
                            this.animatedButtons["Stand Button"].Update(gameTime, this.mouseState, this.keyboardState, this.previousKeyboardState);
                        }

                        break;
                    }

                case GameState.PlayerHitting:
                    {
                        this.PlayerHitCard();

                        if ((this.playerHand.Count < GameConstants.MaxCards) &&
                            (this.playerScore < GameConstants.MaxHandValue))
                        {
                            ChangeState(GameState.WaitingForPlayer);
                        }
                        else
                        {
                            ChangeState(GameState.WaitingForDealer);
                        }
                        
                        if (this.playerScore < GameConstants.PlayerRecommendedScore)
                        {
                            this.animatedButtons["Hit Button"].ChangeToActiveMode(true);
                            this.animatedButtons["Stand Button"].ChangeToActiveMode(false);
                        }
                        else
                        {
                            this.animatedButtons["Hit Button"].ChangeToActiveMode(false);
                            this.animatedButtons["Stand Button"].ChangeToActiveMode(true);
                        }

                        break;
                    }

                case GameState.WaitingForDealer:
                    {
                        if (this.GetBlackjackScore(this.dealerHand) < GameConstants.DealerMinimumScore)
                        {
                            ChangeState(GameState.DealerHitting);
                        }
                        else
                        {
                            ChangeState(GameState.CheckingHandOver);
                        }

                        break;
                    }

                case GameState.DealerHitting:
                    {
                        this.DealerHitCard();

                        if ((this.GetBlackjackScore(this.dealerHand) < GameConstants.DealerMinimumScore)
                            && (this.dealerHand.Count < GameConstants.MaxCards))
                        {
                            ChangeState(GameState.DealerHitting);
                        }
                        else
                        {
                            ChangeState(GameState.CheckingHandOver);
                        }

                        break;
                    }

                case GameState.CheckingHandOver:
                    {
                        RoundResult result = this.CheckScore();

                        if (result == RoundResult.DealerWon)
                        {
                            this.dealerWon = true;
                            this.dealerWonCount++;
                            currentResult = RoundResult.DealerWon;
                            ChangeState(GameState.DisplayingHandResults);
                        }
                        else if (result == RoundResult.Draw)
                        {
                            this.drawWon = true;
                            this.drawCount++;
                            currentResult = RoundResult.Draw;
                            ChangeState(GameState.DisplayingHandResults);
                        }
                        else if ((result == RoundResult.PlayerWon)
                            && (this.dealerHand.Count < GameConstants.MaxCards)
                            && (this.GetBlackjackScore(this.dealerHand) < GameConstants.MaxHandValue))
                        {
                            ChangeState(GameState.DealerHitting);
                        }
                        else if ((result == RoundResult.PlayerWon)
                            && ((this.dealerHand.Count == GameConstants.MaxCards) 
                            || this.GetBlackjackScore(this.dealerHand) > GameConstants.MaxHandValue))
                        {
                            this.playerWon = true;
                            this.playerWonCount++;
                            currentResult = RoundResult.PlayerWon;
                            ChangeState(GameState.DisplayingHandResults);
                        }

                        break;
                    }

                case GameState.DisplayingHandResults:
                    {
                        // update animated buttons
                        this.animatedButtons["Power Button"].Update(gameTime, this.mouseState, this.keyboardState, this.previousKeyboardState);

                        switch (currentResult)
                        {
                            case RoundResult.DealerWon:
                                {
                                    this.allMessages["Dealer Count"].ChangeMode(this.dealerWon);
                                    break;
                                }

                            case RoundResult.PlayerWon:
                                {
                                    this.allMessages["Player Count"].ChangeMode(this.playerWon);
                                    break;
                                }

                            case RoundResult.Draw:
                                {
                                    this.allMessages["Draw Count"].ChangeMode(this.drawWon);
                                    break;
                                }
                        }

                        if ((this.dealerHand != null) && (this.dealerHand.Count >= 2))
                        {
                            this.dealerHand[1].FlipOver();
                        }

                        break;
                    }

                case GameState.Exiting:
                    {
                        break;
                    }

                case GameState.StartOver:
                    {
                        currentResult = RoundResult.WaitingForPlayer;
                        this.playerHand.Clear();
                        this.dealerHand.Clear();            
                        this.playerWon = false;
                        this.dealerWon = false;
                        this.drawWon = false;
                        this.playerScore = 0;
                        this.dealerScore = 0;
                        this.StartNewRound();
                        break;
                    }
            }

            base.Update(gameTime);

            this.previousKeyboardState = this.keyboardState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Set to use native target render
            GraphicsDevice.SetRenderTarget(this.targetRenderNative);
            GraphicsDevice.Clear(Color.ForestGreen);
            this.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            this.DrawCards();
            this.DrawMessages();
            this.DrawButtons();
            this.spriteBatch.End();

            // Set render target to backbuffer
            GraphicsDevice.SetRenderTarget(null);
            this.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Rectangle actualScreen = new Rectangle(
                0,
                0,
                this.graphics.PreferredBackBufferWidth,
                this.graphics.PreferredBackBufferHeight);
            this.spriteBatch.Draw(this.targetRenderNative, actualScreen, Color.White);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Start new round, shuffle new deck and deal cards to dealer and player hand
        /// </summary>
        private void StartNewRound()
        {
            if ((this.playerWonCount == GameConstants.MaxCount - 1) ||
                (this.dealerWonCount == GameConstants.MaxCount - 1) ||
                (this.drawCount == GameConstants.MaxCount - 1))
            {
                this.playerWonCount = 0;
                this.dealerWonCount = 0;
                this.drawCount = 0;
            }

            // add cards and shuffle deck
            this.oneDeck.AddCards();
            this.oneDeck.Shuffle();

            // first player card
            Card firstPlayerCard = this.oneDeck.TakeTopCard();
            firstPlayerCard.FlipOver();
            firstPlayerCard.X = GameConstants.WidthtFirstLineOffset;
            firstPlayerCard.Y = GameConstants.HeightBottomLineOffset;
            this.playerHand.Add(firstPlayerCard);

            // first dealer card
            Card firstDealerCard = this.oneDeck.TakeTopCard();
            firstDealerCard.X = GameConstants.WidthtFirstLineOffset;
            firstDealerCard.Y = GameConstants.HeightTopLineOffset;
            firstDealerCard.FlipOver();
            this.dealerHand.Add(firstDealerCard);

            // second player card
            Card secondPlayerCard = this.oneDeck.TakeTopCard();
            secondPlayerCard.X = GameConstants.WidthtFirstLineOffset + GameConstants.VerticalCardSpacing;
            secondPlayerCard.Y = GameConstants.HeightBottomLineOffset;
            secondPlayerCard.FlipOver();
            this.playerHand.Add(secondPlayerCard);

            // second dealer card
            Card secondDealerCard = this.oneDeck.TakeTopCard();
            secondDealerCard.X = GameConstants.WidthtFirstLineOffset + GameConstants.VerticalCardSpacing;
            secondDealerCard.Y = GameConstants.HeightTopLineOffset;
            
            // No flip over here
            this.dealerHand.Add(secondDealerCard);

            this.playerScore = this.GetBlackjackScore(this.playerHand);

            if (this.allMessages != null)
            {
                this.UpdatePlayerMessages();                
            }

            if (this.playerScore < GameConstants.PlayerRecommendedScore)
            {
                this.hitOrNot = true;
            }
            else
            {
                this.hitOrNot = false;
            }

            if (this.playerScore < GameConstants.PlayerRecommendedScore)
            {
                if (this.animatedButtons.Count >= 3)
                {
                    this.animatedButtons["Hit Button"].ChangeToActiveMode(this.hitOrNot);
                    this.animatedButtons["Stand Button"].ChangeToActiveMode(!this.hitOrNot);
                    this.animatedButtons["Hit Button"].Reset();
                    this.animatedButtons["Stand Button"].Reset();
                    this.animatedButtons["Power Button"].Reset();
                }
            }
            else
            {
                if (this.animatedButtons.Count >= 3)
                {
                    this.animatedButtons["Hit Button"].ChangeToActiveMode(this.hitOrNot);
                    this.animatedButtons["Stand Button"].ChangeToActiveMode(!this.hitOrNot);
                    this.animatedButtons["Hit Button"].Reset();
                    this.animatedButtons["Stand Button"].Reset();
                    this.animatedButtons["Power Button"].Reset();
                }
            }

            RoundResult tempResult = this.CheckScoreFirstHand();
            if (tempResult != RoundResult.WaitingForPlayer)
            {
                if (tempResult == RoundResult.PlayerWon)
                {
                    this.playerWon = true;
                    this.playerWonCount++;
                    currentResult = RoundResult.PlayerWon;
                }

                if (tempResult == RoundResult.Draw)
                {
                    this.drawWon = true;
                    this.drawCount++;
                    currentResult = RoundResult.Draw;
                }

                if (tempResult == RoundResult.DealerWon)
                {
                    this.dealerWon = true;
                    this.dealerWonCount++;
                    currentResult = RoundResult.DealerWon;
                }

                ChangeState(GameState.DisplayingHandResults);
            }
            else
            {
                ChangeState(GameState.WaitingForPlayer);
            }
        }

        /// <summary>
        /// update all messages
        /// </summary>
        private void UpdatePlayerMessages()
        {
            this.playerScore = this.GetBlackjackScore(this.playerHand);

            if (!this.allMessages.ContainsKey("Player Score"))
            {
                this.allMessages.Add(
                    "Player Score",
                    new Message(
                        this.playerScore.ToString(),
                        this.messageFont,
                        new Vector2(
                            GameConstants.WidthtFirstLineOffset / 2,
                            GameConstants.HeightBottomLineOffset),
                        false));
            }
            else
            {
                this.allMessages["Player Score"].Text = this.playerScore.ToString();
            }
        }

        /// <summary>
        /// update all messages
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void UpdateAllMessages(GameTime gameTime)
        {
            if (this.allMessages.ContainsKey("Player Score"))
            {
                this.allMessages["Player Score"].Text = this.playerScore.ToString();
            }

            if (!this.allMessages.ContainsKey("Dealer Score"))
            {
                this.allMessages.Add(
                "Dealer Score",
                new Message(
                    this.dealerScore.ToString(),
                    this.messageFont,
                    new Vector2(
                        GameConstants.WidthtFirstLineOffset / 2,
                        GameConstants.HeightTopLineOffset),
                    false));
            }
            else
            {
                this.allMessages["Dealer Score"].Text = this.dealerScore.ToString();
            }

            if (!this.allMessages.ContainsKey("Dealer Count"))
            {
                this.allMessages.Add(
                "Dealer Count",
                new Message(
                    this.dealerWonCount.ToString(),
                    this.messageFont,
                    new Vector2(
                        GameConstants.WidthtSecondLineOffset + (GameConstants.WidthtFirstLineOffset / 2),
                        GameConstants.HeightTopLineOffset),
                    this.dealerWon));
            }
            else
            {
                this.allMessages["Dealer Count"].Text = this.dealerWonCount.ToString();
                this.allMessages["Dealer Count"].Update(gameTime);
                this.allMessages["Dealer Count"].ChangeMode(this.dealerWon);
            }

            if (!this.allMessages.ContainsKey("Player Count"))
            {
                this.allMessages.Add(
                "Player Count",
                new Message(
                    this.playerWonCount.ToString(),
                    this.messageFont,
                    new Vector2(
                        GameConstants.WidthtSecondLineOffset + (GameConstants.WidthtFirstLineOffset / 2),
                        GameConstants.HeightBottomLineOffset),
                    this.playerWon));
            }
            else
            {
                this.allMessages["Player Count"].Text = this.playerWonCount.ToString();
                this.allMessages["Player Count"].Update(gameTime);
                this.allMessages["Player Count"].ChangeMode(this.playerWon);
            }

            if (!this.allMessages.ContainsKey("Draw Count"))
            { 
                this.allMessages.Add(
                    "Draw Count",
                new Message(
                    this.drawCount.ToString(),
                    this.messageFont,
                    new Vector2(
                        GameConstants.WidthtSecondLineOffset + (GameConstants.WidthtFirstLineOffset / 2),
                        GameConstants.HeightMidLineOffset),
                    this.drawWon));
        }
            else
            {
                this.allMessages["Draw Count"].Text = this.drawCount.ToString();
                this.allMessages["Draw Count"].Update(gameTime);
                this.allMessages["Draw Count"].ChangeMode(this.drawWon);
            }
        }

        /// <summary>
        /// add buttons to list
        /// </summary>
        private void UpdateAllButtons()
        {
            if (!this.animatedButtons.ContainsKey("Hit Button"))
            {
                AnimatedButton hitButton = new AnimatedButton(
                    new Vector2(
                        GameConstants.WidthtFirstLineOffset,
                        GameConstants.ScoreMessageBottomOffset),
                    GameState.PlayerHitting,
                    GameConstants.ColumnsPerSpriteSheet,
                    GameConstants.RowsPerSpriteSheet,
                    this.hitOrNot,
                    Keys.Space,
                    this.allButtonsTexture["Hit Button"].Width,
                    this.allButtonsTexture["Hit Button"].Height,
                    this.scaleFactor);
                this.animatedButtons.Add("Hit Button", hitButton);
            }

            if (!this.animatedButtons.ContainsKey("Stand Button"))
            {
                AnimatedButton standButton = new AnimatedButton(
                    new Vector2(
                        GameConstants.WidthtSecondLineOffset,
                        GameConstants.ScoreMessageBottomOffset),
                    GameState.WaitingForDealer,
                    GameConstants.ColumnsPerSpriteSheet,
                    GameConstants.RowsPerSpriteSheet,
                    !this.hitOrNot,
                    Keys.Enter,
                    this.allButtonsTexture["Stand Button"].Width,
                    this.allButtonsTexture["Stand Button"].Height,
                    this.scaleFactor);
                this.animatedButtons.Add("Stand Button", standButton);
            }

            if (!this.animatedButtons.ContainsKey("Power Button"))
            {
                AnimatedButton powerButton = new AnimatedButton(
                     new Vector2(GameConstants.WidthtMidLineOffset, GameConstants.HeightMidLineOffset),
                     GameState.StartOver,
                     GameConstants.ColumnsPerSpriteSheet,
                     GameConstants.RowsPerSpriteSheet,
                     true,
                     Keys.Enter,
                     this.allButtonsTexture["Power Button"].Width,
                     this.allButtonsTexture["Power Button"].Height,
                     this.scaleFactor);
                this.animatedButtons.Add("Power Button", powerButton);
            }
        }

        /// <summary>
        /// Calculates the Blackjack score for the given hand
        /// </summary>
        /// <param name="hand">the hand</param>
        /// <returns>the Blackjack score for the hand</returns>
        private int GetBlackjackScore(List<Card> hand)
        {
            // add up score excluding Aces
            int numAces = 0;
            int score = 0;
            foreach (Card card in hand)
            {
                if (card.Rank != Rank.Ace)
                {
                    score += this.GetBlackjackCardValue(card);
                }
                else
                {
                    numAces++;
                }
            }

            // if more than one ace, only one should ever be counted as 11
            if (numAces > 1)
            {
                // make all but the first ace count as 1
                score += numAces - 1;
                numAces = 1;
            }

            // if there's an Ace, score it the best way possible
            if (numAces > 0)
            {
                if (score + 11 <= GameConstants.MaxHandValue)
                {
                    // counting Ace as 11 doesn't bust
                    score += 11;
                }
                else
                {
                    // count Ace as 1
                    score++;
                }
            }

            return score;
        }

        /// <summary>
        /// Gets the Blackjack value for the given card
        /// </summary>
        /// <param name="card">the card</param>
        /// <returns>the Blackjack value for the card</returns>
        private int GetBlackjackCardValue(Card card)
        {
            switch (card.Rank)
            {
                case Rank.Ace:
                    return 11;
                case Rank.King:
                case Rank.Queen:
                case Rank.Jack:
                case Rank.Ten:
                    return 10;
                case Rank.Nine:
                    return 9;
                case Rank.Eight:
                    return 8;
                case Rank.Seven:
                    return 7;
                case Rank.Six:
                    return 6;
                case Rank.Five:
                    return 5;
                case Rank.Four:
                    return 4;
                case Rank.Three:
                    return 3;
                case Rank.Two:
                    return 2;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Add new card to player hand and recalculate player score
        /// </summary>
        private void PlayerHitCard()
        {
            if ((this.playerHand.Count < GameConstants.MaxCards) &&
                (this.playerScore < GameConstants.MaxHandValue))
            {
                Card playerNewCard = this.oneDeck.TakeTopCard();
                playerNewCard.X = GameConstants.WidthtFirstLineOffset + (this.playerHand.Count * GameConstants.VerticalCardSpacing);
                playerNewCard.Y = GameConstants.HeightBottomLineOffset;
                playerNewCard.FlipOver();
                this.playerHand.Add(playerNewCard);
                this.playerScore = this.GetBlackjackScore(this.playerHand);
            }
        }

        /// <summary>
        /// Add new card to dealer hand and recalculate dealer score
        /// </summary>
        private void DealerHitCard()
        {
            if ((this.dealerHand.Count < GameConstants.MaxCards)
                && (this.GetBlackjackScore(this.dealerHand) < GameConstants.MaxHandValue))
            {
                Card dealerNewCard = this.oneDeck.TakeTopCard();
                dealerNewCard.X = GameConstants.WidthtFirstLineOffset + (this.dealerHand.Count * GameConstants.VerticalCardSpacing);
                dealerNewCard.Y = GameConstants.HeightTopLineOffset;
                dealerNewCard.FlipOver();
                this.dealerHand.Add(dealerNewCard);
                this.dealerScore = this.GetBlackjackScore(this.dealerHand);
            }
        }

        /// <summary>
        /// Check the score of dealer hand and player hand for instant Blackjack to return the value of RoundResult Enum
        /// </summary>
        /// <returns>the RoundResult enum value for the round</returns>
        private RoundResult CheckScoreFirstHand()
        {
            int playerScore = this.GetBlackjackScore(this.playerHand);
            int dealerScore = this.GetBlackjackScore(this.dealerHand);

            if ((this.playerHand != null) && (this.dealerHand != null))
            {
                if ((this.playerHand.Count == 2) && (this.dealerHand.Count == 2))
                {
                    if ((playerScore == GameConstants.MaxHandValue) && (dealerScore < GameConstants.MaxHandValue))
                    {
                        return RoundResult.PlayerWon;
                    }

                    if ((playerScore < GameConstants.MaxHandValue) && (dealerScore == GameConstants.MaxHandValue))
                    {
                        return RoundResult.DealerWon;
                    }

                    if ((playerScore == GameConstants.MaxHandValue) && (dealerScore == GameConstants.MaxHandValue))
                    {
                        return RoundResult.Draw;
                    }
                }
            }

            return RoundResult.WaitingForPlayer;
        }

        /// <summary>
        /// Check the score of dealer hand and player hand to return the value of RoundResult Enum
        /// </summary>
        /// <returns>the RoundResult enum value for the round</returns>
        private RoundResult CheckScore()
        {
            int playerScore = this.GetBlackjackScore(this.playerHand);
            int dealerScore = this.GetBlackjackScore(this.dealerHand);

            if ((dealerScore <= GameConstants.MaxHandValue)
                && (playerScore <= GameConstants.MaxHandValue))
            {
                if ((dealerScore > playerScore)
                    && (dealerScore >= GameConstants.DealerMinimumScore))
                {
                    return RoundResult.DealerWon;
                }
                else if ((dealerScore == playerScore)
                    || ((dealerScore > playerScore)
                    && (dealerScore < GameConstants.DealerMinimumScore)))
                {
                    return RoundResult.Draw;
                }
                else
                {
                    return RoundResult.PlayerWon;
                }
            }
            else if ((dealerScore > GameConstants.MaxHandValue)
                && (playerScore <= GameConstants.MaxHandValue))
            {
                return RoundResult.PlayerWon;
            }
            else if ((dealerScore <= GameConstants.MaxHandValue)
                && (dealerScore >= GameConstants.DealerMinimumScore)
                && (playerScore > GameConstants.MaxHandValue))
            {
                return RoundResult.DealerWon;
            }
            else
            {
                return RoundResult.Draw;
            }
        }

        /// <summary>
        /// Draw dealer and played cards
        /// </summary>
        private void DrawCards()
        {
            foreach (Card card in this.dealerHand)
            {
                if (card.FaceUp)
                {
                    this.cardDrawRectangle = new Rectangle(
                        card.X - (GameConstants.CardWidth / 2),
                        card.Y - (GameConstants.CardHeight / 2),
                        GameConstants.CardWidth,
                        GameConstants.CardHeight);

                    this.spriteBatch.Draw(
                        this.allCardsTextures[card.Suit.ToString() + card.Rank.ToString()],
                        this.cardDrawRectangle,
                        Color.White);
                }
                else
                {
                    this.cardDrawRectangle = new Rectangle(
                        card.X - (GameConstants.CardWidth / 2),
                        card.Y - (GameConstants.CardHeight / 2),
                        GameConstants.CardWidth,
                        GameConstants.CardHeight);

                    this.spriteBatch.Draw(
                        this.allCardsTextures["Back"],
                        this.cardDrawRectangle,
                        Color.White);
                }
            }

            foreach (Card card in this.playerHand)
            {
                if (card.FaceUp)
                {
                    this.cardDrawRectangle = new Rectangle(
                        card.X - (GameConstants.CardWidth / 2),
                        card.Y - (GameConstants.CardHeight / 2),
                        GameConstants.CardWidth,
                        GameConstants.CardHeight);

                    this.spriteBatch.Draw(
                        this.allCardsTextures[card.Suit.ToString() + card.Rank.ToString()],
                        this.cardDrawRectangle,
                        Color.White);
                }
                else
                {
                    this.cardDrawRectangle = new Rectangle(
                        card.X - (GameConstants.CardWidth / 2),
                        card.Y - (GameConstants.CardHeight / 2),
                        GameConstants.CardWidth,
                        GameConstants.CardHeight);

                    this.spriteBatch.Draw(
                        this.allCardsTextures["Back"],
                        this.cardDrawRectangle,
                        Color.White);
                }
            }
        }

        /// <summary>
        /// Draw messages
        /// </summary>
        private void DrawMessages()
        {
            switch (currentState)
            {
                case GameState.WaitingForPlayer:
                    {
                        this.spriteBatch.DrawString(
                            this.messageFont,
                            this.allMessages["Player Score"].Text.ToString(),
                            this.allMessages["Player Score"].Position,
                            this.allMessages["Player Score"].WhiteOrFlash);
                    }

                    break;
                case GameState.DisplayingHandResults:
                    {
                        this.spriteBatch.DrawString(
                            this.messageFont,
                            this.allMessages["Player Score"].Text.ToString(),
                            this.allMessages["Player Score"].Position,
                            this.allMessages["Player Score"].WhiteOrFlash);
                        this.spriteBatch.DrawString(
                            this.messageFont,
                            this.allMessages["Dealer Score"].Text.ToString(),
                            this.allMessages["Dealer Score"].Position,
                            this.allMessages["Dealer Score"].WhiteOrFlash);
                        this.spriteBatch.DrawString(
                            this.messageFont,
                            this.allMessages["Player Count"].Text.ToString(),
                            this.allMessages["Player Count"].Position,
                            this.allMessages["Player Count"].WhiteOrFlash);
                        this.spriteBatch.DrawString(
                            this.messageFont,
                            this.allMessages["Dealer Count"].Text.ToString(),
                            this.allMessages["Dealer Count"].Position,
                            this.allMessages["Dealer Count"].WhiteOrFlash);
                        this.spriteBatch.DrawString(
                            this.messageFont,
                            this.allMessages["Draw Count"].Text.ToString(),
                            this.allMessages["Draw Count"].Position,
                            this.allMessages["Draw Count"].WhiteOrFlash);
                    }

                    break;
            } 
        }

        /// <summary>
        /// Draw buttons
        /// </summary>
        private void DrawButtons()
        {
            switch (currentState)
            {
                case GameState.WaitingForPlayer:
                    {
                        this.spriteBatch.Draw(
                            this.allButtonsTexture["Hit Button"],
                            this.animatedButtons["Hit Button"].DrawRectangle,
                            this.animatedButtons["Hit Button"].SourceRectangle,
                            Color.White);
                        this.spriteBatch.Draw(
                          this.allButtonsTexture["Stand Button"],
                          this.animatedButtons["Stand Button"].DrawRectangle,
                          this.animatedButtons["Stand Button"].SourceRectangle,
                          Color.White);
                    }

                    break;
                case GameState.DisplayingHandResults:
                    {
                        this.spriteBatch.Draw(
                            this.allButtonsTexture["Power Button"],
                            this.animatedButtons["Power Button"].DrawRectangle,
                            this.animatedButtons["Power Button"].SourceRectangle,
                            Color.White);
                    }

                    break;
            }
        }

        /// <summary>
        /// Draw scores
        /// </summary>
        private void UpdateScore()
        {
            if ((this.playerHand.Count >= 2) && (this.dealerHand.Count >= 2))
            {
                this.playerScore = this.GetBlackjackScore(this.playerHand);
                this.dealerScore = this.GetBlackjackScore(this.dealerHand);
            }
        }
    }
}