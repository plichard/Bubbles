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
        //common to every instance of the class ======================================
        static protected GraphicsDevice device = null;
        static SpriteBatch batch;
        static Texture2D blank;
        static ContentManager content;
        static protected int mouse_x = 0, mouse_y = 0;
        

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
            blank = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
        }

        protected static ContentManager Content
        {
            get { return content; }
        }

        //specific to each widget =====================================================

        Queue<Widget> to_erase = new Queue<Widget>();
        protected bool drawable = true; //is the widget actualy drawable? maybe we just want some clicable space. Doh. -__-
        //is the child necessarily inside its parent's boundaries ?
        //this is an important information as we can render everything(parent + children)onto a single texture
        protected bool strictly_bound = false; 

        protected RenderTarget2D texture;
        protected bool dirty = true; //do we need to redraw the texture?
        protected Rectangle destination = Rectangle.Empty;
        Rectangle previous_destination = Rectangle.Empty;
        protected List<Widget> children = new List<Widget>();
        protected Widget parent = null;

        protected int life_time = -1; //remaining time until the widget is destroyed (-1 = infinite)
        protected int show_time = -1; //remaining time until the widget is hidden

        protected bool hovered = false;

        protected bool Hovered
        {
            get
            {
                if(parent == null)
                return mouse_x >= destination.Left &&
                    mouse_x <= destination.Right &&
                    mouse_y <= destination.Bottom &&
                    mouse_y >= destination.Top;
                else
                    return mouse_x >= destination.Left  &&
                   mouse_x <= destination.Right  &&
                   mouse_y <= destination.Bottom &&
                   mouse_y >= destination.Top;
            }
        }
        

        public static void MousePick(int x, int y)
        {
            mouse_x = x;
            mouse_y = y;
        }
        

        /// <summary>
        /// Is the child inside its parent's render area?
        /// We can then cache the render of the parent + every bound child
        /// (Quite cool =) )
        /// </summary>
        public bool StrictlyBound
        {
            get { return strictly_bound; }
            set { strictly_bound = value; }
        }

        public Widget(Widget p = null)
        {
            parent = p;
            if (device == null)
            {
                Console.WriteLine("Error: You must call Widget.Init(...) before using any widget.");
            }
            
            if (parent != null)
            {
                parent.AddChild(this);
                strictly_bound = parent.strictly_bound;
            }
        }

        public bool Show
        {
            get { return (show_time == -1) || (show_time > 0); }
            set 
            {
                if (value)
                    show_time = -1;
                else
                    show_time = 0;
            }
        }

        public bool Kill
        {
            get { return life_time == 0; }
            set
            {
                if (value)
                    life_time = 0;
                else
                    life_time = -1;
            }
        }

        public void Release()
        {
            Console.WriteLine("Releasing {0}", this.ToString());
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

        public int LifeTime
        {
            set { life_time = value;}
            get { return life_time; }
        }

        public int ShowTime
        {
            set { show_time = value; }
            get { return show_time; }
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
            set { destination.X = (int)value.X; destination.Y = (int)value.Y; }
        }

        protected void _Render()
        {
            if (!drawable)
                return;
            Console.WriteLine("[{0}]: caching", this.ToString());
            if (texture != null && (destination.Width != previous_destination.Width || destination.Height != previous_destination.Height))
            {
                Console.WriteLine("[{0}]: Cannot re-use old texture, disposing",this);
                texture.Dispose();
                texture = new Microsoft.Xna.Framework.Graphics.RenderTarget2D(device, destination.Width, destination.Height);
            }
            else if(texture == null)
                texture = new Microsoft.Xna.Framework.Graphics.RenderTarget2D(device, destination.Width, destination.Height);

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
            previous_destination = destination;
        }

        public int Width
        {
            get { return destination.Width; }
        }

        public int Height
        {
            get { return destination.Height; }
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
            mouse_x = Mouse.GetState().X;
            mouse_y = Mouse.GetState().Y;
            
            Tick(time);
            
            if (show_time > 0)
            {
                show_time -= time.ElapsedGameTime.Milliseconds;
                if (show_time < 0)
                {
                    show_time = 0;
                }
            }

            if (life_time > 0)
            {
                life_time -= time.ElapsedGameTime.Milliseconds;
                if (life_time < 0)
                {
                    life_time = 0;
                }
            }
            foreach (Widget child in children)
            {
                child.Update(time);
                if (child.Kill)
                {
                    to_erase.Enqueue(child);
                }
            }
            foreach (Widget child in to_erase)
            {
                children.Remove(child);
                child.Release();
            }
            to_erase.Clear();
            
        }

        public void AddChild(Widget w)
        {
            children.Add(w);
            w.parent = this;
        }

        public void Draw()
        {
            if (!Show)
                return;
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

        protected virtual void OnHover()
        {
            Console.WriteLine("{0} hovered, should implement the method ^_^", this.ToString());
        }
    }
}
