using System.Drawing;
using GameEngineLibrary;
using GameLibrary.Scripts;
using OpenTK;

namespace GameLibrary
{
    /// <summary>
    /// Сцена танкового сражения в игре.
    /// </summary>
    public class BattleScene : Scene
    {
        private const string TRACK_TEXTURE_PATH = @"../../../GameLibrary/Resources/Track.bmp";
        private const string TURRET_TEXTURE_PATH = @"../../../GameLibrary/Resources/Turret.bmp";
        private const string BACKGROUND_TEXTURE_PATH = @"../../../GameLibrary/Resources/BG.bmp";
        private const string ROCKET_TEXTURE_PATH = @"../../../GameLibrary/Resources/Rocket.bmp";

        /// <summary>
        /// Создание сцены.
        /// </summary>
        /// <param name="windowWidth">Ширина окна, в котором будет отображаться сцена.</param>
        /// <param name="windowHeight">Высота окна, в котором будет отображаться сцена.</param>
        public BattleScene(double windowWidth, double windowHeight) 
            : base (windowWidth, windowHeight)
        {
        }

        public override void Init()
        {
            Texture2D rocketTex = Texture2D.LoadTexture(ROCKET_TEXTURE_PATH);
            AddTexture(rocketTex);

            Texture2D backgroundTex = Texture2D.LoadTexture(BACKGROUND_TEXTURE_PATH);
            AddTexture(backgroundTex);
            GameObject background = new GameObject(backgroundTex);
            background.Scale = new Vector2(5, 5);
            background.Position = new Vector2((float)-windowWidth, (float)-windowHeight);
            AddGameObject(background);

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
            ShootKeyboardControlScript firstShootControl = new ShootKeyboardControlScript(this, rocketTex, 500);
            firstShootControl.SetKey(OpenTK.Input.Key.Space);
            ShootKeyboardControlScript secondShootControl = new ShootKeyboardControlScript(this, rocketTex, 500);
            secondShootControl.SetKey(OpenTK.Input.Key.Enter);

            GameObject firstPanzer = BuildPanzer(Color.FromArgb(200, 120, 60),
                new Vector2(-5, 5), firstPanzerControl, firstTurretControl, firstShootControl);
            GameObject secondPanzer = BuildPanzer(Color.FromArgb(20, 140, 120),
                new Vector2(5, 5), secondPanzerControl, secondTurretControl, secondShootControl);

            firstPanzer.Position = new Vector2((float) -windowWidth * 3 / 4,
                (float)windowHeight - firstPanzer.Texture.Height * 14);
            secondPanzer.Position = new Vector2((float) windowWidth * 3 / 4,
                (float)windowHeight - secondPanzer.Texture.Height * 14);

            AddGameObject(firstPanzer);
            AddGameObject(secondPanzer);
        }

        private GameObject BuildPanzer(Color color, Vector2 scale, Script trackController, Script turretController, Script shootController)
        {
            Texture2D trackTex = Texture2D.LoadTexture(TRACK_TEXTURE_PATH);
            Texture2D turretTex = Texture2D.LoadTexture(TURRET_TEXTURE_PATH);
            trackTex.Color = color;
            turretTex.Color = color;

            AddTexture(trackTex);
            AddTexture(turretTex);

            GameObject panzer = new GameObject(trackTex);
            GameObject turret = new GameObject(turretTex);
            panzer.AddInnerObject(turret);
            panzer.Scale = scale;
            turret.Scale = scale;
            turret.Position = new Vector2(5 * scale.X, -4 * scale.Y);
            turret.RotationPoint = new Vector2(16, 4);

            panzer.AddScript(trackController);
            turret.AddScript(turretController);
            turret.AddScript(shootController);

            return panzer;
        }
    }
}
