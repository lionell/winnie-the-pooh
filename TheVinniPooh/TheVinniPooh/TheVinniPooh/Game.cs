using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TheVinniPooh
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        #region Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D VinniPoohImg;
        Texture2D BackGroundImg;
        Texture2D SkyImg;
        Texture2D BallImg;
        Texture2D BeeImg;
        Texture2D NewGameImg;
        Texture2D MouseImg;
        Texture2D MenuImg;
        Texture2D ContinueImg;
        Texture2D ExitImg;
        Texture2D LifeImg;
        ObjectClass VinniPooh;
        ObjectClass BackGround;
        BallClass[] Balls;
        BeeClass[] Bees;
        Rectangle Vinni;
        Rectangle Ball;
        Rectangle BeeRect;
        Rectangle NewGameRect;
        Rectangle ContinueRect;
        Rectangle ExitRect;
        Rectangle MouseRect;
        Random RandomNum = new Random();
        SpriteFont Normal;
        SpriteFont Big;
        string Word;
        System.Text.Encoding Encode = System.Text.Encoding.GetEncoding(1251);
        #endregion


        #region Options
        int Lives = 0;
        bool FullScreen = true;
        bool UserResizing = false;
        bool Overed = false;
        int MaxScreenX = 800;
        int MaxScreenY = 600;
        Vector2 BGSpeed = new Vector2(-30.0f, 0.0f);
        Vector2 UpperSpeed = new Vector2(0.0f, -250.0f);
        int NextUpperBallCoord = 0;
        int K = 0;
        int BK = 0;
        int Level = 0;
        int MaxLevel = 0;
        int MinASCII = 0;
        string Symb = "";
        int MaxASCII = 0;
        string FinishWord = "";
        string Translation = "";
        bool Finished = false;
        bool IsGame = false;
        #endregion

        public Game()
        {
            this.Window.Title = "The Vinni Pooh";
            this.Window.AllowUserResizing = UserResizing;
            this.IsMouseVisible = false;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = MaxScreenX;
            graphics.PreferredBackBufferHeight = MaxScreenY;
            graphics.IsFullScreen = FullScreen;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected void InitMenu()
        {
            NewGameImg = Content.Load<Texture2D>("NewGame");
            ContinueImg = Content.Load<Texture2D>("Cont");
            ExitImg = Content.Load<Texture2D>("Exit");
            MouseImg = Content.Load<Texture2D>("Cursor");
            MenuImg = Content.Load<Texture2D>("MenuBG");
            NewGameRect = new Rectangle((MaxScreenX / 2 - NewGameImg.Width / 2), (MaxScreenY / 2 - NewGameImg.Height / 2) - 100, NewGameImg.Width, NewGameImg.Height);
            ContinueRect = new Rectangle((MaxScreenX / 2 - ContinueImg.Width / 2), (MaxScreenY / 2 - ContinueImg.Height / 2), ContinueImg.Width, ContinueImg.Height);
            ExitRect = new Rectangle((MaxScreenX / 2 - ExitImg.Width / 2), (MaxScreenY / 2 - ExitImg.Height / 2) + 100, ExitImg.Width, ExitImg.Height);
            // Menu
            //InitGame();
        }
        protected void SaveParams()
        {
            #region Reading Level
            System.IO.StreamWriter SaveFile =
                new System.IO.StreamWriter("SavedGame.txt", false, Encode);
            SaveFile.WriteLine(Convert.ToString(Level));
            SaveFile.WriteLine(Convert.ToString(Lives));
            SaveFile.WriteLine(Convert.ToString(MaxLevel));
            SaveFile.Close();
            #endregion
        }
        protected void LoadParams()
        {
            #region Reading Level
            System.IO.StreamReader LoadFile =
                new System.IO.StreamReader("SavedGame.txt", Encode);
            Level = Convert.ToInt32(LoadFile.ReadLine());
            Lives = Convert.ToInt32(LoadFile.ReadLine());
            MaxLevel = Convert.ToInt32(LoadFile.ReadLine());
            LoadFile.Close();
            if ((Level == 0) || (Lives == 0) || (MaxLevel == 0))
            {
                Level = 1;
                Lives = 5;
                MaxLevel = 2;
            }
            #endregion
        }
        protected void InitGame()
        {

            Finished = false;
            Overed = false;
            NextUpperBallCoord = 0;
            LoadParams();

            #region Reading Level
            System.IO.StreamReader File =
                new System.IO.StreamReader("levels/level" + Convert.ToString(Level) + ".txt", Encode);
            FinishWord = File.ReadLine();
            Translation = File.ReadLine();
            Symb = File.ReadLine();
            File.Close();
            #endregion

            #region Loading Textures
            VinniPoohImg = Content.Load<Texture2D>("TheVinniPooh");
            BackGroundImg = Content.Load<Texture2D>("BackGround");
            SkyImg = Content.Load<Texture2D>("Sky");
            LifeImg = Content.Load<Texture2D>("Life");
            #endregion

            #region Loading Fonts
            Normal = Content.Load<SpriteFont>("NormalFont");
            Big = Content.Load<SpriteFont>("BigFont");
            #endregion

            #region Creating Objects
            VinniPooh = new ObjectClass(VinniPoohImg);
            BackGround = new ObjectClass(BackGroundImg);
            #endregion

            #region Setting Fields
            VinniPooh.Cent();
            VinniPooh.Position = new Vector2(37.0f, 100.0f);
            BackGround.Position = new Vector2(0, (BackGroundImg.Height / 2) + 75);
            BackGround.Center = new Vector2(BackGroundImg.Width / 2, BackGroundImg.Height / 2 + 75);
            #endregion

            #region Creating Balls
            K = RandomNum.Next(10, 20);
            Balls = new BallClass[K];
            char C;
            int T = MinASCII;
            MaxASCII = Symb.Length - 1;
            for (int i = 0; i < K; i++)
            {
                C = (char)(Symb[T]);
                T++;
                if (T > MaxASCII) T = MinASCII;
                BallImg = Content.Load<Texture2D>("" + C);
                Balls[i] = new BallClass(BallImg, C);
                Balls[i].Speed = new Vector2(-(RandomNum.Next(40, 100)), 0);
                Balls[i].Position = new Vector2(RandomNum.Next(1280, 2664), RandomNum.Next(80, 560));
                Balls[i].Cent();
            }
            #endregion

            #region Creating Bees
            BK = RandomNum.Next(2, 5);
            Bees = new BeeClass[BK];
            BeeImg = Content.Load<Texture2D>("bee");
            for (int i = 0; i < BK; i++)
            {
                Bees[i] = new BeeClass(BeeImg);
                Bees[i].Speed = new Vector2(-(RandomNum.Next(100, 200)), 0);
                Bees[i].Position = new Vector2(RandomNum.Next(1280, 2664), RandomNum.Next(80, 560));
                Bees[i].Cent();
            }
            #endregion

            Word = "";

        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            InitMenu();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseRect = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, MouseImg.Width, MouseImg.Height);
            if (IsGame)
            {
                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    SaveParams();
                    IsGame = false;
                }
                if ((Keyboard.GetState().IsKeyDown(Keys.Enter)) && (Finished) && (!Overed))
                {
                    Level++;
                    if (Level == (MaxLevel + 1))
                    {
                        Level = 1;
                    }
                    SaveParams();
                    InitGame();
                }
                if ((Keyboard.GetState().IsKeyDown(Keys.Enter)) && (Finished) && (Overed))
                {
                    Level = 1;
                    Lives = 5;
                    SaveParams();
                    IsGame = false;
                }

                if ((Keyboard.GetState().IsKeyDown(Keys.Down)) && (!Finished))
                {
                    if ((VinniPooh.Position.Y + VinniPooh.Image.Height / 2) < MaxScreenY)
                        VinniPooh.Position.Y += 2.5f;
                }
                if ((Keyboard.GetState().IsKeyDown(Keys.Up)) && (!Finished))
                {
                    if ((VinniPooh.Position.Y - VinniPooh.Image.Height / 2) > 0)
                        VinniPooh.Position.Y -= 2.5f;
                }
                if ((Keyboard.GetState().IsKeyDown(Keys.Right)) && (!Finished))
                {
                    if (((VinniPooh.Position.X + VinniPooh.Image.Width / 2) < MaxScreenX) && (VinniPooh.Position.X < 300))
                        VinniPooh.Position.X += 2.5f;
                    if (VinniPooh.Position.X > 300)
                        BGSpeed.X = -70.0f;

                }
                if ((Keyboard.GetState().IsKeyDown(Keys.Left)) && (!Finished))
                {
                    if ((VinniPooh.Position.X - VinniPooh.Image.Width / 2) > 0)
                        VinniPooh.Position.X -= 2.5f;
                    if (VinniPooh.Position.X < 300)
                        BGSpeed.X = -30.0f;
                }
                if ((Keyboard.GetState().IsKeyDown(Keys.Space)) && (!Finished))
                {
                    NextUpperBallCoord = 0;
                    Word = "";
                    foreach (BallClass D in Balls)
                    {
                        if (D.Upped)
                        {
                            D.Upped = false;
                            D.Upper = false;
                        }
                    }
                }

                BackGround.Position +=
                    BGSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (BackGround.Position.X <= 0)
                {
                    BackGround.Position.X = BackGroundImg.Width / 2;
                }
                Vinni = new Rectangle(
                            (int)VinniPooh.Position.X - 20,
                            (int)VinniPooh.Position.Y + 25,
                            VinniPooh.Image.Width,
                            VinniPooh.Image.Height - 100);
                foreach (BeeClass Bee in Bees)
                {
                    if (Bee.Is)
                    {
                        Bee.Position +=
                        (Bee.Speed + BGSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (Bee.Position.X < -(Bee.Image.Width)) Bee.Is = false;
                        BeeRect = new Rectangle(
                                (int)Bee.Position.X + 10,
                                (int)Bee.Position.Y + 40,
                                Bee.Image.Width - 10,
                                Bee.Image.Height);
                        if (Vinni.Intersects(BeeRect) && (!Finished))
                        {
                            Bee.Is = false;
                            Lives--;
                            if (Lives == 0)
                            {
                                Finished = true;
                                Overed = true;
                            }
                            break;
                        }
                    }
                    else
                    {
                        Bee.Position = new Vector2(RandomNum.Next(1280, 2664), RandomNum.Next(80, 560));
                        Bee.Speed = new Vector2(-(RandomNum.Next(100, 200)), 0);
                        Bee.Is = true;
                    }


                }
                foreach (BallClass B in Balls)
                {
                    if (B.Is)
                    {
                        B.Position +=
                        (B.Speed + BGSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (B.Position.X < -(B.Image.Width)) B.Is = false;
                        Ball = new Rectangle(
                                (int)B.Position.X,
                                (int)B.Position.Y,
                                B.Image.Width,
                                B.Image.Height);
                        if (Vinni.Intersects(Ball) && (!Finished))
                        {
                            B.Is = false;
                            B.Upper = true;
                            break;
                        }
                    }
                    else
                    {
                        if (!B.Upper)
                        {
                            B.Position = new Vector2(RandomNum.Next(1280, 2664), RandomNum.Next(80, 560));
                            B.Speed = new Vector2(-(RandomNum.Next(40, 100)), 0);
                            B.Is = true;
                        }
                        else
                        {
                            if (!B.Upped)
                            {
                                if (B.Position.Y > 0) B.Position +=
                                (UpperSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                if (B.Position.Y < (B.Image.Height))
                                {
                                    if ((B.Symbol != '`') && (B.Symbol != '+')) Word += B.Symbol;
                                    else if (Word != "")
                                    {
                                        if (B.Symbol == '`')
                                        {
                                            Word = Word.Substring(0, Word.Length - 1);
                                            NextUpperBallCoord -= 1;
                                            foreach (BallClass D in Balls)
                                            {
                                                if ((D.Position.X == D.Image.Width + ((NextUpperBallCoord) * (D.Image.Width + D.Image.Width / 2))) && (D.Upped))
                                                {
                                                    D.Upper = false;
                                                    D.Upped = false;
                                                }
                                            }
                                        }

                                    }
                                    if (Word == FinishWord) Finished = true;
                                    B.Position.X = B.Image.Width + (NextUpperBallCoord * (B.Image.Width + B.Image.Width / 2));
                                    if ((B.Symbol != '`') && (B.Symbol != '+')) NextUpperBallCoord++;
                                    if (NextUpperBallCoord < 0) NextUpperBallCoord = 0;
                                    B.Upped = true;
                                    if (B.Symbol == '`')
                                    {
                                        B.Upped = false;
                                        B.Upper = false;
                                    }
                                    if (B.Symbol == '+')
                                    {
                                        B.Upped = false;
                                        B.Upper = false;
                                        Lives++;
                                        if (Lives == 7)
                                        {
                                            Lives = 6;
                                        }
                                    }
                                    if (NextUpperBallCoord == 9)
                                    {
                                        NextUpperBallCoord = 0;
                                        Word = "";
                                        foreach (BallClass D in Balls)
                                        {
                                            if (D.Upped)
                                            {
                                                D.Upped = false;
                                                D.Upper = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }


                    }

                }
                // TODO: Add your update logic here

            }
            else
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    MouseRect = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, MouseImg.Width, MouseImg.Height);
                    if (NewGameRect.Intersects(MouseRect))
                    {
                        IsGame = true;
                        Level = 1;
                        MaxLevel = 3;
                        Lives = 5;
                        SaveParams();
                        InitGame();
                    }
                    if (ContinueRect.Intersects(MouseRect))
                    {
                        IsGame = true;
                        InitGame();
                    }
                    if (ExitRect.Intersects(MouseRect))
                    {
                        this.Exit();
                    }
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            if (IsGame)
            {
                spriteBatch.Draw(SkyImg,
                   new Vector2(0.0f, 0.0f),
                   null,
                   Color.White,
                   0.0f,
                   new Vector2(0.0f, 0.0f), 1.0f,
                   SpriteEffects.None, 0);
                spriteBatch.Draw(BackGround.Image,
                    BackGround.Position,
                    null,
                    Color.White,
                    BackGround.Rotation,
                    BackGround.Center, 1.0f,
                    SpriteEffects.None, 0);

                spriteBatch.Draw(VinniPooh.Image,
                    VinniPooh.Position,
                    null,
                    Color.White,
                    VinniPooh.Rotation,
                    VinniPooh.Center, 0.8f,
                    SpriteEffects.None, 0);
                foreach (BallClass B in Balls)
                {
                    if ((B.Is) || (B.Upper) || (B.Upped))
                    {
                        spriteBatch.Draw(B.Image,
                        B.Position,
                        null,
                        Color.White,
                        B.Rotation,
                        B.Center, 1.3f,
                        SpriteEffects.None, 0);
                    }

                }
                foreach (BeeClass Bee in Bees)
                {
                    if (Bee.Is)
                    {
                        spriteBatch.Draw(Bee.Image,
                        Bee.Position,
                        null,
                        Color.White,
                        Bee.Rotation,
                        Bee.Center, 1.0f,
                        SpriteEffects.None, 0);
                    }

                }

                for (int y = Lives; y > 0; y--)
                {
                    spriteBatch.Draw(LifeImg,
                        new Vector2((MaxScreenY + 225) - (y * (LifeImg.Width)), LifeImg.Height),
                        null,
                        Color.White,
                        0.0f,
                        new Vector2((LifeImg.Width / 2), (LifeImg.Height / 2)), 1.0f,
                        SpriteEffects.None, 0);
                }

                if ((Finished) && (!Overed) && (Level != MaxLevel))
                {
                    spriteBatch.DrawString(Big,
                       "Ти пройшов рiвень" + Convert.ToChar(10) + "    Натисни Enter",
                       new Vector2((MaxScreenX / 2) - 150, (MaxScreenY / 2) - 25),
                       Color.Red);
                }
                if ((Finished) && (!Overed) && (Level == MaxLevel))
                {
                    spriteBatch.DrawString(Big,
                       "Вiтаю! Ти пройшов всi рiвнi в цiй грi",
                       new Vector2((MaxScreenX / 2) - 300, (MaxScreenY / 2) - 25),
                       Color.Red);
                }
                if ((Finished) && (Overed)) spriteBatch.DrawString(Big,
                    "Ти програв рiвень",
                    new Vector2((MaxScreenX / 2) - 150, (MaxScreenY / 2) - 25),
                    Color.Red);
                spriteBatch.DrawString(Big,
                    Translation,
                    new Vector2((MaxScreenX / 2) - 50, MaxScreenY - 50),
                    Color.Red);
            }
            else
            {
                spriteBatch.Draw(MenuImg, new Rectangle(0, 0, MaxScreenX, MaxScreenY), Color.White);
                spriteBatch.Draw(NewGameImg, NewGameRect, Color.White);
                spriteBatch.Draw(ContinueImg, ContinueRect, Color.White);
                spriteBatch.Draw(ExitImg, ExitRect, Color.White);
                spriteBatch.Draw(MouseImg, MouseRect, Color.White);
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
