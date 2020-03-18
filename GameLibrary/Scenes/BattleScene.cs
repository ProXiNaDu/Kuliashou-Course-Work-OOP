﻿using System;
using System.Collections.Generic;
using System.Drawing;
using GameEngineLibrary;
using GameLibrary.Scripts;
using OpenTK;

namespace GameLibrary
{
    /// <summary>
    /// Сцена танкового сражения в игре.
    /// </summary>
    public class BattleScene : IScene
    {
        private const string TRACK_TEXTURE_PATH = @"../../../GameLibrary/Resources/Track.bmp";
        private const string TURRET_TEXTURE_PATH = @"../../../GameLibrary/Resources/Turret.bmp";

        /// <summary>
        /// Список созданных текстур.
        /// </summary>
        private List<Texture2D> textures;

        /// <summary>
        /// Список объектов на сцене.
        /// </summary>
        private List<GameObject> objects;

        /// <summary>
        /// Список объектов на сцене для отрисовки.
        /// </summary>
        private List<GameObject> objectsToRender;

        public void Init()
        {
            textures = new List<Texture2D>();
            objects = new List<GameObject>();
            objectsToRender = new List<GameObject>();

            TrackKeyboardControlScript firstPanzerControl = new TrackKeyboardControlScript(300f);
            firstPanzerControl.SetKeyToMoveLeft(OpenTK.Input.Key.A);
            firstPanzerControl.SetKeyToMoveRight(OpenTK.Input.Key.D);
            TurretKeyboardControlScript firstTurretControl = new TurretKeyboardControlScript(2);
            firstTurretControl.SetKeyToTurnUp(OpenTK.Input.Key.W);
            firstTurretControl.SetKeyToTurnDown(OpenTK.Input.Key.S);
            TrackKeyboardControlScript secondPanzerControl = new TrackKeyboardControlScript(300f);
            secondPanzerControl.SetKeyToMoveLeft(OpenTK.Input.Key.Left);
            secondPanzerControl.SetKeyToMoveRight(OpenTK.Input.Key.Right);
            TurretKeyboardControlScript secondTurretControl = new TurretKeyboardControlScript(2);
            secondTurretControl.SetKeyToTurnUp(OpenTK.Input.Key.Up);
            secondTurretControl.SetKeyToTurnDown(OpenTK.Input.Key.Down);

            GameObject firstPanzer = BuildPanzer(Color.Green, new Vector2(-5, 5), firstPanzerControl, firstTurretControl);
            GameObject secondPanzer = BuildPanzer(Color.Red, new Vector2(5, 5), secondPanzerControl, secondTurretControl);
            objectsToRender.Add(firstPanzer);
            objectsToRender.Add(secondPanzer);
        }

        private GameObject BuildPanzer(Color color, Vector2 scale, Script trackController, Script turretController)
        {
            Texture2D trackTex = Texture2D.LoadTexture(TRACK_TEXTURE_PATH);
            Texture2D turretTex = Texture2D.LoadTexture(TURRET_TEXTURE_PATH);
            trackTex.Color = color;
            turretTex.Color = color;
            textures.Add(trackTex);
            textures.Add(turretTex);

            GameObject panzer = new GameObject(trackTex);
            GameObject turret = new GameObject(turretTex);
            panzer.AddInnerObject(turret);
            panzer.Scale = scale;
            turret.Scale = scale;
            turret.Position = new Vector2(5 * scale.X, -4 * scale.Y);
            turret.RotationPoint = new Vector2(16, 4);

            panzer.AddScript(trackController);
            turret.AddScript(turretController);

            objects.Add(panzer);
            objects.Add(turret);

            return panzer;
        }

        public void Update(TimeSpan delta)
        {
            foreach (GameObject gameObject in objects)
            {
                foreach (Script script in gameObject.Scripts)
                {
                    script.Update(delta);
                }
            }
        }

        public List<GameObject> GetGameObjects()
        {
            return objectsToRender;
        }

        public void Dispose()
        {
            foreach (Texture2D texture in textures)
            {
                texture.Dispose();
            }

            foreach (GameObject gameObject in objects)
            {
                gameObject.Dispose();
            }
        }
    }
}
