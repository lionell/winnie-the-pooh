using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace TheVinniPooh
{
    class BallClass
    {
        public Texture2D Image;
        public Vector2 Position;
        public Vector2 Center;
        public float Rotation;
        public Vector2 Speed;
        public char Symbol;
        public bool Is;
        public bool Upper;
        public bool Upped;
        public BallClass(Texture2D Img, char Sym)
        {
            Image = Img;
            Symbol = Sym;
            Speed = Vector2.Zero;
            Position = Vector2.Zero;
            Center = Vector2.Zero;
            Rotation = 0.0f;
            Is = true;
            Upper = false;
            Upped = false;
        }
        public void Cent()
        {
            Center = new Vector2(this.Image.Width / 2, this.Image.Height / 2);
        }
    }
}
