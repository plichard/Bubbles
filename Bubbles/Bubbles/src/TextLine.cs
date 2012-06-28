using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Bubbles
{
    public class TextLine: Widget
    {
        static SpriteFont font = null;
        public Color font_color = Color.White;
        public Color bg_color = Color.Transparent;
        bool was_hovered = false;
        string text;
        public TextLine(string t):base()
        {
            if(font == null)
                font = Content.Load<SpriteFont>("mainfont");
            text = t;
            destination.Width = (int)font.MeasureString(text).X;
            destination.Height = font.LineSpacing;
            _Render();
           // dirty = true;
        }
        protected override void Render()
        {
            DrawRectangle(destination, bg_color);
            Batch.DrawString(font, text, Vector2.Zero, font_color);
            bg_color = Color.Transparent;
        }

        protected override void Tick(Microsoft.Xna.Framework.GameTime time)
        {
            if (!was_hovered && Hovered)
            {
                bg_color = Color.Red;
                _Render();
                was_hovered = true;
            }
            else if (was_hovered && !Hovered)
            {
                bg_color = Color.Transparent;
                _Render();
                was_hovered = false;
            }
        }

        protected override void OnHover()
        {
           
        }
    }
}
