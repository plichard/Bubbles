using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bubbles
{
    public class Tooltip2: Widget
    {
        public Tooltip2(string msg, Vector2 position)
        {
            Position = position;
            TextLine text = new TextLine(msg);
            AddChild(text);
            text.Position = Position + new Vector2(5, 3);
            destination.Width = text.Width + 10;
            destination.Height = text.Height + 6;
            Frame frame = new Frame(destination);
            AddChild(frame);
            _Render();
        }
        protected override void Render()
        {
            
        }

        protected override void Tick(Microsoft.Xna.Framework.GameTime time)
        {
            
        }

        protected override void OnHover()
        {
            
        }
    }
}
