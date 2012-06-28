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
    public abstract class Widget
    {
        //common to every instance of the class
        static protected GraphicsDevice device = null;
        static SpriteBatch batch;
        static Texture2D blank;
        static ContentManager content;
        
        protected static GraphicsDevice Device
        {
            get { return device; }
        }

        protected static SpriteBatch Batch
        {
            get { return batch; }
        }

        public static void Init(GraphicsDevice d, ContentManager c)
        {
            device = d;
            content = c;
            batch = new SpriteBatch(device);
        }

        protected static ContentManager Content
        {
            get { return content; }
        }

        //specific to each widget
        protected RenderTarget2D texture;
        protected bool dirty = true; //do we need to redraw the texture?
        protected Rectangle destination = Rectangle.Empty;
        protected List<Widget> children = new List<Widget>();
        protected Widget parent = null;

        public Widget(Widget p = null)
        {
            parent = p;
            if (device == null)
            {
                Console.WriteLine("Error: You must call Widget.Init(...) before using any widget.");
            }
            blank = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            if (parent != null)
            {
                parent.AddChild(this);
            }
            
        }

        public void Release()
        {
            texture.Dispose();
        }

        ~Widget()
        {
            texture.Dispose();
            foreach (Widget child in children)
            {
                child.Release();
            }
        }

        public bool Dirty
        {
            get { return dirty; }
        }

        public RenderTarget2D Texture
        {
            get
            {
                if (dirty)
                    _Render();
                dirty = false;
                return texture;
            }
        }

        public Rectangle Destination
        {
            get { return destination; }
        }

        public Vector2 Position
        {
            get { return new Vector2(destination.X, destination.Y); }
        }

        protected void _Render()
        {
            if (texture != null)
            {
                Console.WriteLine("disposing of old texture");
                texture.Dispose();
            }
            texture = new Microsoft.Xna.Framework.Graphics.RenderTarget2D(device,destination.Width, destination.Height);
            device.SetRenderTarget(texture);
            device.Clear(Color.Transparent);
            batch.Begin();
            Render();
            batch.End();
            device.SetRenderTarget(null);
            using (System.IO.Stream stream = System.IO.File.OpenWrite("tooltip.png"))
            {
                texture.SaveAsPng(stream, destination.Width, destination.Height);
            }
            dirty = false;
            
        }

        protected abstract void Render();

        protected static void DrawLine(float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }

        protected static void DrawRectangle(Rectangle dest, Color color)
        {
            batch.Draw(blank, new Rectangle(0, 0, dest.Width, dest.Height),color);
        }

        protected abstract void Tick(GameTime time);

        public void Update(GameTime time)
        {
            Tick(time);
            foreach (Widget child in children)
            {
                child.Update(time);
            }
        }

        public void AddChild(Widget w)
        {
            children.Add(w);
        }

        public void Draw()
        {
            if (parent == null)
            {
                Batch.Begin();
            }
            foreach (Widget child in children)
            {
                child.Draw();
            }
            
            Batch.Draw(Texture, destination, Color.White);
            if (parent == null)
            {
                Batch.End();
            }
        }
    }
}
