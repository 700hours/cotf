﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using CirclePrefect;
using CirclePrefect.Native;
using cotf;
using cotf.Assets;
using cotf.Base;
using cotf.ID;

namespace cotf.Legacy
{
    public class Fog
    {
        public static Fog Instance;
        public bool active;
        public bool lit;
        public int x, y, width = 8, height = 8;
        //  Old value: 8
        public static int Size = 10;
        public int size;
        public Vector2 position;
        public Rectangle hitbox => new Rectangle(x, y, width, height);
        public Vector2 Center => new Vector2(x + width / 2, y + height / 2);
        public static float range = 100f;
        public static bool updating;
        public bool onScreen;
        public const float AddLight = 1.2f;
        public static Texture2D fow => Game.fog;
        public static Color TorchLight = Color.Orange;
        public void Draw(SpriteBatch sb)
        {
            if (!lit && onScreen)
                sb.Draw(Game.fog, new Rectangle((int)position.X - 10 + Main.ScreenX, (int)position.Y - 10 + Main.ScreenY, Size * 3, Size * 3), Color.Black * (float)Math.Min(Helper.Distance(Center, Main.myPlayer.Center) / (range * 2f), 1f));
            updating = false;

            lit = false;
            //  Comment out for fog of war
            if (Main.myPlayer.lamp != null &&
                Main.myPlayer.lamp.active is false)
            {
                
            }
        }
        public void Update()
        {
            if (updating) return;

            onScreen =
                position.X >= Main.myPlayer.position.X - (Main.ScreenWidth + Size * 5) / 2 &&
                position.X <= Main.myPlayer.position.X + (Main.ScreenWidth + Size * 10) / 2 &&
                position.Y >= Main.myPlayer.position.Y - (Main.ScreenHeight + Size * 5) / 2 &&
                position.Y <= Main.myPlayer.position.Y + (Main.ScreenHeight + Size * 10) / 2;
            if (!onScreen) return;

            foreach (var ent in Main.lamp.Concat(new[] { Main.myPlayer.lamp }))
            {
                if (ent == null || !ent.active) continue;
                Vector2 center = ent.position + new CirclePrefect.Vector2(ent.width / 2, ent.height / 2);
                if (Helper.Distance(Center, center) < range)
                {
                    lit = true;
                }
            }
        }
        public static void Create(int x, int y, int width, int height)
        {
            updating = true;
            for (int i = x; i < x + width; i += Size)
            {
                for (int j = y; j < y + height; j += Size)
                {
                    Main.effect.Add(new Fog()
                    {
                        position = new Vector2(i, j),
                        x = i,
                        y = j,
                        active = true,
                        lit = false
                    });
                }
            }
        }
    }
}
