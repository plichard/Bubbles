using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bubbles
{
    public class Frame: Widget
    {
        public bool resizable = false;
        Color color = Color.White;
        public Frame(Rectangle geometry)
        {
            destination = geometry;
            _Render();
        }


        protected override void Render()
        {
            DrawLine(1, Color.White, new Vector2(0, 0), new Vector2(destination.Width, 0));
            DrawLine(1, Color.White, new Vector2(destination.Width, 0), new Vector2(destination.Width, destination.Height));
            DrawLine(1, Color.White, new Vector2(destination.Width, destination.Height), new Vector2(0, destination.Height));
            DrawLine(1, Color.White, new Vector2(0, destination.Height), new Vector2(0, 0));
        }

        protected override void Tick(GameTime time)
        {
            
        }

        protected override void OnHover()
        {
            
        }

        public Color Color
        {
            get { return color; }
            set { color = value; _Render(); }
        }
    }
}
