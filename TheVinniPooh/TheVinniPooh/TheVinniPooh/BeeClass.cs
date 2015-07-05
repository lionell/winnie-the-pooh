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
    class BeeClass
    {
        public Texture2D Image;
        public Vector2 Position;
        public Vector2 Center;
        public float Rotation;
        public Vector2 Speed;
        public bool Is;
        public BeeClass(Texture2D Img)
        {
            Image = Img;
            Speed = Vector2.Zero;
            Position = Vector2.Zero;
            Center = Vector2.Zero;
            Rotation = 0.0f;
            Is = true;
        }
        public void Cent()
        {
            Center = new Vector2(this.Image.Width / 2, this.Image.Height / 2);
        }
    }
}
