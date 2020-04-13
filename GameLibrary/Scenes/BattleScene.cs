using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using GameEngineLibrary;
using GameLibrary.Components;
using GameLibrary.Components.HealthDecorators;
using GameLibrary.Components.RocketDecorators;
using GameLibrary.Scenes;
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
        private const string POWERFULROCKET_TEXTURE_PATH = @"../../../GameLibrary/Resources/PowerfulRocket.bmp";
        private const string FASTROCKET_TEXTURE_PATH = @"../../../GameLibrary/Resources/FastRocket.bmp";
        private const string MOUNTAIN_TEXTURE_PATH = @"../../../GameLibrary/Resources/Mountain.bmp";
        private const string EXPLOSION_ANIMATION_PATH = @"../../../GameLibrary/Resources/Explosion.bmp";

        private BattleSceneSettings settings;

        /// <summary>
        /// Создание сцены.
        /// </summary>
        /// <param name="window">Окно, в котором будет отрисовываться сцена.</param>
        /// <param name="settings">Настройки игровой сцены.</param>
        public BattleScene(Window window, BattleSceneSettings settings) 
            : base (window)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Инициализация сцены.
        /// </summary>
        public override void Init()
        {
            Texture2D rocketTex = Texture2D.LoadTexture(ROCKET_TEXTURE_PATH);
            AddTexture(rocketTex);
            Texture2D powerfulRocketTex = Texture2D.LoadTexture(POWERFULROCKET_TEXTURE_PATH);
            AddTexture(powerfulRocketTex);
            Texture2D fastRocketTex = Texture2D.LoadTexture(FASTROCKET_TEXTURE_PATH);
            AddTexture(fastRocketTex);

            Animation2D explosionAnim = Animation2D.LoadAnimation(EXPLOSION_ANIMATION_PATH, 5);
            explosionAnim.AnimationTime = 120;
            AddTexture(explosionAnim);

            Texture2D backgroundTex = Texture2D.LoadTexture(BACKGROUND_TEXTURE_PATH);
            AddTexture(backgroundTex);
            GameObject background = new GameObject(backgroundTex, 
                new Vector2((float)-GameWindow.Width, (float)-GameWindow.Height),
                Vector2.Zero, new Vector2(5, 5), 0);
            AddGameObject(background);
            Texture2D mountainTex = Texture2D.LoadTexture(MOUNTAIN_TEXTURE_PATH);
            AddTexture(mountainTex);
            GameObject mountain = new GameObject(mountainTex,
                new Vector2(-mountainTex.Width * 5 / 2, 80),
                Vector2.Zero, new Vector2(5, 5), 0);
            mountain.SetCollider(new Collider(new Vector2[] {
                new Vector2(-20, 80),
                new Vector2(mountainTex.Width * 5 / 2, 80 + mountainTex.Height * 5),
                new Vector2(-20, 80 + mountainTex.Height * 10),
                new Vector2(-mountainTex.Width * 5 / 2, 80 + mountainTex.Height * 5)
                
            }));
            AddGameObject(mountain);

            TrackKeyboardControlScript firstPanzerControl = new TrackKeyboardControlScript(this, 300f);
            firstPanzerControl.SetKeyToMoveLeft(OpenTK.Input.Key.A);
            firstPanzerControl.SetKeyToMoveRight(OpenTK.Input.Key.D);
            TurretKeyboardControlScript firstTurretControl = new TurretKeyboardControlScript(2);
            firstTurretControl.SetKeyToTurnUp(OpenTK.Input.Key.W);
            firstTurretControl.SetKeyToTurnDown(OpenTK.Input.Key.S);
            TrackKeyboardControlScript secondPanzerControl = new TrackKeyboardControlScript(this, 300f);
            secondPanzerControl.SetKeyToMoveLeft(OpenTK.Input.Key.Left);
            secondPanzerControl.SetKeyToMoveRight(OpenTK.Input.Key.Right);
            TurretKeyboardControlScript secondTurretControl = new TurretKeyboardControlScript(2);
            secondTurretControl.SetKeyToTurnUp(OpenTK.Input.Key.Up);
            secondTurretControl.SetKeyToTurnDown(OpenTK.Input.Key.Down);
            ShootKeyboardControlScript firstShootControl = new WpfShootControlScript(this,
                (ProgressBar) GameWindow.FindName("FirstPanzerCooldown"));
            firstShootControl.SetKey(OpenTK.Input.Key.Space);
            ShootKeyboardControlScript secondShootControl = new WpfShootControlScript(this,
                (ProgressBar)GameWindow.FindName("SecondPanzerCooldown"));
            secondShootControl.SetKey(OpenTK.Input.Key.Enter);
            KeyboardRocketSwitcherScript firstPanzerRocketSwitcher = new KeyboardRocketSwitcherScript();
            firstPanzerRocketSwitcher.SetKeyToSelectNext(OpenTK.Input.Key.E);
            firstPanzerRocketSwitcher.SetKeyToSelectPrevious(OpenTK.Input.Key.Q);
            KeyboardRocketSwitcherScript secondPanzerRocketSwitcher = new KeyboardRocketSwitcherScript();
            secondPanzerRocketSwitcher.SetKeyToSelectNext(OpenTK.Input.Key.Plus);
            secondPanzerRocketSwitcher.SetKeyToSelectPrevious(OpenTK.Input.Key.Minus);

            Inventory.RocketBuilder[] rockets = new Inventory.RocketBuilder[]
            {
                new Inventory.RocketBuilder(this, powerfulRocketTex, explosionAnim,
                new DoubleDamageRocket(new DoubleCooldownRocket(new Rocket()))),
                new Inventory.RocketBuilder(this, fastRocketTex, explosionAnim,
                new HalfDamageRocket(new HalfCooldownRocket(new Rocket()))),
                new Inventory.RocketBuilder(this, rocketTex, explosionAnim,
                new Rocket())
            };

            Inventory firstPanzerInventory = new WpfInventory(
                (StackPanel) GameWindow.FindName("FirstPanzerInventory"), rockets);
            Inventory secondPanzerInventory = new WpfInventory(
                (StackPanel)GameWindow.FindName("SecondPanzerInventory"), rockets);

            settings.FillFirtsPanzerInventory(firstPanzerInventory);
            settings.FillSecondPanzerInventory(secondPanzerInventory);

            GameObject firstPanzer = BuildPanzer(Color.FromArgb(200, 120, 60),
                new Vector2(-5, 5), firstPanzerControl, firstTurretControl,
                firstShootControl, firstPanzerRocketSwitcher,
                (ProgressBar)GameWindow.FindName("FirstPanzerHealth"), firstPanzerInventory);
            GameObject secondPanzer = BuildPanzer(Color.FromArgb(20, 140, 120),
                new Vector2(5, 5), secondPanzerControl, secondTurretControl, 
                secondShootControl, secondPanzerRocketSwitcher,
                (ProgressBar)GameWindow.FindName("SecondPanzerHealth"), secondPanzerInventory);

            Transform transform;
            Texture2D texture;
            transform = firstPanzer.GetComponent("transform") as Transform;
            texture = firstPanzer.GetComponent("texture") as Texture2D;
            transform.Position = new Vector2((float) -GameWindow.Width * 3 / 4,
                (float)GameWindow.Height - texture.Height * 14);

            transform = secondPanzer.GetComponent("transform") as Transform;
            texture = secondPanzer.GetComponent("texture") as Texture2D;
            transform.Position = new Vector2((float)GameWindow.Width * 3 / 4,
                (float)GameWindow.Height - texture.Height * 14);

            GameObject winChecker = new GameObject();
            winChecker.AddScript(new WinCheckerScript(firstPanzer, secondPanzer,
                GameWindow.FindName("WinMenu") as StackPanel));

            AddGameObject(firstPanzer);
            AddGameObject(secondPanzer);
            AddGameObject(winChecker);
        }

        private GameObject BuildPanzer(Color color, Vector2 scale, Script trackController,
            Script turretController, Script shootController, Script rocketSwitcher, ProgressBar healthBar, Inventory inventory)
        {
            Texture2D trackTex = Texture2D.LoadTexture(TRACK_TEXTURE_PATH);
            Texture2D turretTex = Texture2D.LoadTexture(TURRET_TEXTURE_PATH);
            trackTex.Color = color;
            turretTex.Color = color;

            AddTexture(trackTex);
            AddTexture(turretTex);

            GameObject panzer = new GameObject(trackTex, Vector2.Zero, Vector2.Zero, scale, 0);
            GameObject turret = new GameObject(turretTex, new Vector2(5 * scale.X, -4 * scale.Y), new Vector2(16, 4), scale, 0);
            panzer.AddInnerObject(turret);
            panzer.AddComponent("health", 
                new ProgressBarHealth(new Health(), healthBar));
            panzer.AddScript(trackController);
            turret.AddComponent("inventory", inventory);
            turret.AddScript(turretController);
            turret.AddScript(turretController);
            turret.AddScript(shootController);
            turret.AddScript(rocketSwitcher);

            return panzer;
        }
    }
}
