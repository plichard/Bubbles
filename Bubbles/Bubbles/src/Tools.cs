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
    public class Tools
    {
        static GraphicsDevice device;
        static SpriteBatch batch;
        static Texture2D blank;
        static ContentManager content;
        static SpriteFont font;

        static Vector2 font_size = Vector2.Zero;

        public static void Init(GraphicsDevice dev,SpriteBatch b,ContentManager c)
        {
            content = c;
            font = c.Load<SpriteFont>("mainfont");
            font_size = font.MeasureString("a");
            blank = new Texture2D(dev, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            device = dev;
            batch = b;
        }

        public static SpriteBatch Batch
        {
            get { return batch; }
        }

        public static int FontX
        {
            get { return (int)font_size.X; }
        }

        public static int FontY
        {
            get { return (int)font_size.Y; }
        }

        public static void DrawLine(float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }

        public static void DrawString(string text, Vector2 position,Color color)
        {
            batch.DrawString(font, text, position, color);
        }
    }
}
