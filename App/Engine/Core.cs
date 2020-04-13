﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using App.Engine.PhysicsEngine;
using App.Engine.PhysicsEngine.Collision;
using App.Engine.PhysicsEngine.RigidBody;
using App.Model;
using App.Model.LevelData;
using App.View;

namespace App.Engine
{
    public class Core : ContractCore
    {
        private ContractView view;
        
        private Stopwatch clock;
        
        private KeyStates keyState;
        
        private Level currentLevel;
        
        private LevelManager levelManager;
        
        private Player player;
        
        private Camera camera;
        
        private Vector cursorPosition;
        
        class KeyStates
        {
            public bool W, S, A, D;
        }

        public Core(ContractView view)
        {
            SetUpCamera();
            SetUpLevel();
            var playerStartPosition = new Vector(14 * 64, 6 * 64);
            cursorPosition = playerStartPosition;
            SetUpPlayer(playerStartPosition);
            keyState = new KeyStates();
            clock = new Stopwatch();
            
        }
        
        private void SetUpCamera()
        {
            var cameraSize = new Size(1280, 720);
            var p = cameraSize.Height / 3;
            var q = cameraSize.Height / 5;
            
            var walkableArea = new Rectangle(p, p, cameraSize.Width - 2 * p, cameraSize.Height - 2 * p);
            var cursorArea = new Rectangle(q, q, cameraSize.Width - 2 * q, cameraSize.Height - 2 * q);
            camera = new Camera(new Vector(250, 100), cameraSize, walkableArea, cursorArea);
        }
        
        private void SetUpLevel()
        {
            levelManager = new LevelManager();
            currentLevel = levelManager.CurrentLevel;
        }
        
        private void SetUpPlayer(Vector position)
        {
            var bmpPlayer = levelManager.GetTileMap("boroda");
            var playerLegs = new Sprite
            (
                position,
                bmpPlayer,
                4,
                7,
                new Size(64, 64),
                4);
            
            var playerTorso = new Sprite
            (
                position,
                bmpPlayer,
                0,
                3,
                new Size(64, 64),
                4);
            
            player = new Player
            {
                Shape = new RigidCircle(position, tileSize / 2, false),
                Torso = playerTorso,
                Legs = playerLegs
            };
        }

        public override void GameLoop(object sender, EventArgs args)
        {
            UpdateState();
            clock.Start();
            
            view.Render();
            
            
            clock.Stop();
            Console.WriteLine(clock.ElapsedMilliseconds);
            clock.Reset();
        }
        
        private void UpdatePlayer(int step)
        {
            var delta = Vector.ZeroVector;
            if (keyState.W) 
                delta.Y -= step;
            if (keyState.S)
                delta.Y += step;
            if (keyState.A)
                delta.X -= step;
            if (keyState.D)
                delta.X += step;
            player.Move(delta);
        }
        
        private void CorrectPlayer()
        {
            var delta = Vector.ZeroVector;
            var rightBorder = currentLevel.levelSizeInTiles.Width * tileSize;
            const int leftBorder = 0;
            var bottomBorder = currentLevel.levelSizeInTiles.Height * tileSize;
            const int topBorder = 0;

            var a = player.Center.Y - player.Radius - topBorder;
            var b = player.Center.Y + player.Radius - bottomBorder;
            var c = player.Center.X - player.Radius - leftBorder;
            var d = player.Center.X + player.Radius - rightBorder;

            if (a < 0)
                delta.Y -= a;
            if (b > 0)
                delta.Y -= b;
            if (c < 0)
                delta.X -= c;
            if (d > 0)
                delta.X -= d;
            
            player.Move(delta);
        }
        
        private void UpdateState()
        {
            const int step = 6;
            UpdatePlayer(step);
            CorrectPlayer();
            camera.CorrectCamera(cursorPosition, player, currentLevel.levelSizeInTiles, tileSize);
        }
        
        public override void OnMouseMove(Vector newPosition)
        {
            cursorPosition = newPosition;
        }
        
        public override void OnKeyDown(Keys keyPressed)
        {
            switch (keyPressed)
            {
                case Keys.W:
                    keyState.W = true;
                    break;

                case Keys.S:
                    keyState.S = true;
                    break;

                case Keys.A:
                    keyState.A = true;
                    break;

                case Keys.D:
                    keyState.D = true;
                    break;
            }
        }

        public override void OnKeyUp(Keys keyPressed)
        {
            switch (keyPressed)
            {
                case Keys.Escape:
                    Application.Exit();
                    break;
                
                case Keys.W:
                    keyState.W = false;
                    break;

                case Keys.S:
                    keyState.S = false;
                    break;

                case Keys.A:
                    keyState.A = false;
                    break;

                case Keys.D:
                    keyState.D = false;
                    break;
            }
        }

        public override List<RigidShape> GetSceneObjects()
        {
            return sceneObjects;
        }
        
        public override List<CollisionInfo> GetCollisions()
        {
            return collisions;
        }
    }
}