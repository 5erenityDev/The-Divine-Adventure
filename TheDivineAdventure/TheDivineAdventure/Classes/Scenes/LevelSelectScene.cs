﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System;

namespace TheDivineAdventure
{
    public class LevelSelectScene : Scene
    {

        private Texture2D backdrop, frontPlate, actionButton, level1ButtonTex, levelPreview1, emberSheet01;
        private AnimatedSprite[] titleEmbers;
        private Button backButton, selectButton, level1Button;
        private int activeLevel;
        private string[,] levelText;
        private List<(string Role, int Score)> highscores;


        public LevelSelectScene(SpriteBatch sb, GraphicsDeviceManager graph, Game1 game, ContentManager cont) : base(sb, graph, game, cont)
        {

            //starter text for level select menu
            levelText = new string[8, 8];
            levelText[0, 0] = "Welcome to your Divine Adventure,";
            levelText[0, 1] = "  select a level to enter.";

            //level one text
            levelText[1, 0] = "   You and your companions have been brought to";
            levelText[1, 1] = "the underworld to aid the divine being in their";
            levelText[1, 2] = "attempt to belay the assaults of the Great Seven ";
            levelText[1, 3] = "Demons. You now find yourself on the first level, ";
            levelText[1, 4] = "Pride, facing against Lucifer. Can your party";
            levelText[1, 5] = "of adventurers defeat this daunting threat?";
        }

        public override void Initialize()
        {
            base.Initialize();

            //set active level
            activeLevel = 0;

            //load buttons
            backButton = new Button(actionButton, actionButton,"Back", parent.smallFont, new Vector2(226, 148),
                new Vector2(210, 76), parent.currentScreenScale);
            selectButton = new Button(actionButton, actionButton, "Continue", parent.smallFont, new Vector2(1383, 762),
                new Vector2(210, 76), parent.currentScreenScale);
            level1Button = new Button(level1ButtonTex, level1ButtonTex, " ", parent.smallFont, new Vector2(226, 228),
                new Vector2(545, 78), parent.currentScreenScale);


            //create embers
            titleEmbers = new AnimatedSprite[30];
            for (int i = 0; i < titleEmbers.Length; i++)
            {
                titleEmbers[i] = new AnimatedSprite(174, 346, emberSheet01, 6);
                titleEmbers[i].Pos = new Vector2(rand.Next(1920) * parent.currentScreenScale.X, rand.Next(450, 750) * parent.currentScreenScale.Y);
                titleEmbers[i].Scale = 1 - (rand.Next(-200, 50) / 100f);
                titleEmbers[i].Frame = rand.Next(6);
            }

            highscores = GetHighScore();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            //Load 2d textures
            backdrop = Content.Load<Texture2D>("TEX_CharacterSelect_Backdrop");
            emberSheet01 = Content.Load<Texture2D>("TEX_EmberSHeet01");
            frontPlate = Content.Load<Texture2D>("TEX_LevelSelect_Plate");
            actionButton = Content.Load<Texture2D>("TEX_Settings_Button2");
            level1ButtonTex = Content.Load<Texture2D>("TEX_Level1Button");
            levelPreview1 = Content.Load<Texture2D>("TEX_Level1Preview");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            //click detection

            if (parent.mouseState.LeftButton == ButtonState.Pressed && parent.lastMouseState.LeftButton != ButtonState.Pressed)
            {
                if (level1Button.IsPressed())
                {
                    activeLevel = 1;
                    level1Button.IsActive =true;
                    return;
                }
                if (selectButton.IsPressed() && activeLevel!=0)
                {
                    parent.level = activeLevel;
                    parent.currentScene = "CHARACTER_SELECT";
                    parent.ReloadContent();
                    parent.characterSelectScene.Initialize();
                    return;
                }
                if (backButton.IsPressed())
                {
                    parent.currentScene = "TITLE";
                    parent.ReloadContent();
                    parent.titleScene.Initialize();
                    return;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin();
            //draw backdrop
            _spriteBatch.Draw(backdrop, Vector2.Zero, null, Color.White, 0, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 0);

            //draw embers
            foreach (AnimatedSprite ember in titleEmbers)
            {
                if (ember.Frame == 0)
                {
                    ember.Pos = new Vector2((rand.Next(1920)) * parent.currentScreenScale.X, rand.Next(450, 750) * parent.currentScreenScale.Y);
                    ember.Scale = 1 - (rand.Next(-200, 50) / 100f);
                }
                ember.Draw(_spriteBatch, parent.currentScreenScale);
            }

            //draw front plate
            _spriteBatch.Draw(frontPlate, Vector2.Zero, null, Color.White, 0, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 0);

            //draw level preview
            switch (activeLevel)
            {
                case 1:
                    _spriteBatch.Draw(levelPreview1, new Vector2(889, 229) * parent.currentScreenScale, null,
                        Color.White, 0, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 0);
                    break;
                default:
                    _spriteBatch.Draw(levelPreview1, new Vector2(889, 229)*parent.currentScreenScale, null,
                        new Color(Color.DarkRed,100), 0, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 0);
                    break;

            }

            //draw level preview text
            for(int i = 0; i <= 7; i++)
            {
                if(levelText[activeLevel,i] != null)
                {
                    //drop shadow
                    _spriteBatch.DrawString(parent.smallFont, levelText[activeLevel, i], new Vector2(903, (30 * i) + 517) * parent.currentScreenScale,
                        new Color(Color.Black, 255), 0f, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 1);
                    //main text
                    _spriteBatch.DrawString(parent.smallFont, levelText[activeLevel, i], new Vector2(905, (30*i)+515) * parent.currentScreenScale,
                        parent.textGold, 0f, Vector2.Zero, parent.currentScreenScale, SpriteEffects.None, 1);
                }
            }

            //write current level highscore
            //drop shadow
            _spriteBatch.DrawString(parent.BigFont, "High Score : " + highscores[0].Score, new Vector2(903, 765) * parent.currentScreenScale,
                new Color(Color.Black, 255), 0f, Vector2.Zero, parent.currentScreenScale * 0.5f, SpriteEffects.None, 1);
            //main text
            _spriteBatch.DrawString(parent.BigFont, "High Score : " + highscores[0].Score, new Vector2(905, 763) * parent.currentScreenScale,
                parent.textGold, 0f, Vector2.Zero, parent.currentScreenScale*0.5f, SpriteEffects.None, 1);


            //draw buttons
            backButton.DrawButton(_spriteBatch);
            selectButton.DrawButton(_spriteBatch);
            level1Button.DrawButton(_spriteBatch);

            FadeIn();

            _spriteBatch.End();
        }

        private List<(string Role, int Score)> GetHighScore()
        {
            //define file path
            string filePath = (Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString() + @"\Level1_HighScores.txt");
            //write file to array
            string[] text = File.ReadAllLines(filePath);
            //creat empty tuple list
            List<(string Role, int Score)> scoreList = new List<(string Role, int Score)>();

            //populate Tuple list
            foreach (string line in text)
            {
                (string Role, int Score) output;
                output.Role = line.Substring(0, line.LastIndexOf(':'));
                output.Score = Int32.Parse(line.Substring(line.LastIndexOf('-') + 2));
                scoreList.Add(output);
            }
            //sort Tuple List
            sortScoreList(scoreList);

            return scoreList;
        }

        private void sortScoreList(List<(string Role, int Score)> list)
        {
            int length = list.Count;
            for (int i = 1; i < length; i++)
            {
                (string Role, int Score) key = list[i];
                int j = i - 1;

                while (j >= 0 && list[j].Score < key.Score)
                {
                    list[j + 1] = list[j];
                    j = j - 1;
                }
                list[j + 1] = key;
            }

        }
    }
}
