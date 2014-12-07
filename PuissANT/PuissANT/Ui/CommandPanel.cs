﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using PuissANT.Pheromones;

namespace PuissANT.Ui
{
    public class CommandPanel : IPanel
    {
        public Vector2 Dimensions, ButtonDimensions, PheremoneStartPosition;
        public Image Image;
        public List<Button> Buttons;

        private string imgPath, text;
        private int _buttonOffset = 20;


        public CommandPanel()
        {
            Dimensions = Vector2.Zero;
            ButtonDimensions = Vector2.Zero;
            PheremoneStartPosition = Vector2.Zero;
            Image = new Image();
            Buttons = new List<Button>();

            imgPath = "ui/commandBar";
            text = "this is the command bar";
        }

        public void LoadContent()
        {
            Dimensions = new Vector2(240, 720);
            ButtonDimensions = new Vector2(120, 30);
            Image.Position = new Vector2(ScreenManager.Instance.ScreenSize.X - Dimensions.X, ScreenManager.Instance.ScreenSize.Y - Dimensions.Y);
            PheremoneStartPosition = new Vector2(Image.Position.X + (Dimensions.X / 2) - (ButtonDimensions.X / 2), Image.Position.Y + 100);
            Image.LoadContent(imgPath, text);

            // Create Pheremone buttons
            Array array = Enum.GetValues(typeof(PheromoneType));
            foreach (var v in array)
            {
                Buttons.Add(CreateButton(v.ToString()));
            }
        }

        public Button CreateButton(string text)
        {
            Button b = new Button();
            b.Location = new Rectangle((int)PheremoneStartPosition.X, (int)PheremoneStartPosition.Y + Buttons.Count * (_buttonOffset + (int)ButtonDimensions.Y),
                (int)ButtonDimensions.X, (int)ButtonDimensions.Y);
            // Create the text
            b.Font = Image.Font;
            b.ButtonText = text;
            // Set the image textures...this is retarded, fix later
            Image temp = new Image();
            temp.LoadContent("ui/pherButtonActive", String.Empty);
            b.SetTexture(temp.Texture, Button.ButtonState.Neutral);
            temp = new Image();
            temp.LoadContent("ui/pherButtonPressed", String.Empty);
            b.SetTexture(temp.Texture, Button.ButtonState.Over);
            temp = new Image();
            temp.LoadContent("ui/pherButtonInactive", String.Empty);
            b.SetTexture(temp.Texture, Button.ButtonState.Pressed);
            b.ButtonClicked = HandleButtonClick;
            return b;
        }

        public void HandleButtonClick(string text)
        {
            Console.WriteLine(text + " was clicked");
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.Update(gameTime);
            foreach (Button b in Buttons)
                b.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
            foreach (Button b in Buttons)
                b.Draw(spriteBatch);
        }
    }
}
