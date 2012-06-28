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
    public class Tooltip : Widget
    {
        string message;
        SpriteFont font = null;

        public Tooltip(string message, Vector2 position,Widget p = null):base(p)
        {
            destination.X = (int)position.X;
            destination.Y = (int)position.Y;
            
            if (font == null)
            {
                font = Content.Load<SpriteFont>("mainfont");
            }

            Message = message;
            Show = true;
            drawable = true;
        }

        public string Message
        {
            get { return message; }

            //if the message is changed we need to recalculate the new neccesary size
            //for the widget and recache it
            set 
            { 
                message = value; dirty = true;
                destination.Width = message.Length * Tools.FontX +10 ;
                destination.Height = Tools.FontY+5;
                _Render();
            }
        }

        override protected void Render()
        {
            
            //DrawRectangle(destination, Color.Black);
            DrawLine(1, Color.Yellow, new Vector2(0, 0), new Vector2(destination.Width, 0));
            DrawLine(1, Color.Yellow, new Vector2(destination.Width, 0), new Vector2(destination.Width, destination.Height));
            DrawLine(1, Color.Yellow, new Vector2(destination.Width,destination.Height), new Vector2(0, destination.Height));
            DrawLine(1, Color.Yellow, new Vector2(0, destination.Height), new Vector2(0, 0));
            Batch.DrawString(font, message, new Vector2(5,3), Color.White);
           // Console.WriteLine("[Tooltip]: rendering \"{2}\" to texture: {0}x{1}",destination.Width,destination.Height,message);
        }

        override protected void Tick(GameTime time)
        {
           // Console.WriteLine("tick from tooltip");
        }

        override protected void OnHover()
        {
        }
    }
}
