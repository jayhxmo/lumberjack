using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lumberjack.Source.UI
{
    class Label
    {
        public Texture2D tex;
        public Vector2 pos;

        public Label(Texture2D tex, Vector2 pos, Screen screen)
        {
            this.tex = tex;
            this.pos = pos;
            screen.labels.Add(this);
        }
    }
}
