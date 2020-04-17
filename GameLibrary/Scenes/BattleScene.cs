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

        private GameObject firstPanzer;
        private GameObject secondPanzer;

        /// <summary>
        /// Создание сцены.
        /// </summary>
        /// <param name="window">Окно, в котором будет отрисовываться сцена.</param>
        /// <param name="settings">Настройки игровой сцены.</param>
        public BattleScene(Window window, BattleSceneSettings settings) 
            : base (window)
        {
            this.settings = settings;
            firstPanzer = new GameObject();
            secondPanzer = new GameObject();
        }

        /// <summary>
        /// Инициализация сцены.
        /// </summary>
        public override void Init()
        {
            AddGameObject(CreateBackground());
            AddGameObject(CreateMountain());

            Inventory.RocketBuilder[] rockets = CreateRockets();
            Inventory firstPanzerInventory = new WpfInventory(
                (StackPanel) GameWindow.FindName("FirstPanzerInventory"), rockets);
            Inventory secondPanzerInventory = new WpfInventory(
                (StackPanel)GameWindow.FindName("SecondPanzerInventory"), rockets);

            settings.FillFirtsPanzerInventory(firstPanzerInventory);
            settings.FillSecondPanzerInventory(secondPanzerInventory);

            BuildFirstPanzer(firstPanzerInventory);
            BuildSecondPanzer(secondPanzerInventory);

            GameObject winChecker = new GameObject();
            winChecker.AddScript(new WinCheckerScript(firstPanzer, secondPanzer,
                GameWindow.FindName("WinMenu") as StackPanel));

            AddGameObject(firstPanzer);
            AddGameObject(secondPanzer);
            AddGameObject(winChecker);
        }

        private void BuildPanzer(GameObject panzer, Texture2D trackTex,
            Texture2D turretTex, Vector2 position, Vector2 scale,
            ProgressBar healthBar, Inventory inventory,
            Script[] panzerScripts, Script[] turretScripts)
        {
            Transform transform = panzer.GetComponent("transform") as Transform;
            transform.Position = position;
            transform.Scale = scale;

            panzer.AddComponent("texture", trackTex);
            panzer.AddComponent("health",
                new ProgressBarHealth(new Health(), healthBar));
            foreach (Script script in panzerScripts)
                panzer.AddScript(script);

            GameObject turret = new GameObject(turretTex, new Vector2(
                5 * scale.X, -4 * scale.Y), new Vector2(16, 4), scale, MathHelper.Pi / 4);
            panzer.AddInnerObject(turret);

            turret.AddComponent("inventory", inventory);
            foreach (Script script in turretScripts)
                turret.AddScript(script);
        }

        private void BuildFirstPanzer(Inventory inventory)
        {
            Texture2D trackTexture = Texture2D.LoadTexture(TRACK_TEXTURE_PATH);
            Texture2D turretTexture = Texture2D.LoadTexture(TURRET_TEXTURE_PATH);
            trackTexture.Color = Color.FromArgb(200, 120, 60);
            turretTexture.Color = Color.FromArgb(200, 120, 60);
            AddTexture(trackTexture);
            AddTexture(turretTexture);

            Vector2 position = new Vector2((float)-GameWindow.Width * 3 / 4,
                (float)GameWindow.Height - trackTexture.Height * 14);

            Script[] trackScripts = null;
            Script[] turretScripts = null;

            ProgressBar cooldownBar = (ProgressBar)GameWindow.FindName("FirstPanzerCooldown");
            ProgressBar healthBar = (ProgressBar)GameWindow.FindName("FirstPanzerHealth");

            switch (settings.FirstPanzerControlType)
            {
                case BattleSceneSettings.PanzerControlType.AI:
                    trackScripts = CreateTrackAIScripts();
                    turretScripts = CreateTurretAIScripts(cooldownBar, secondPanzer);
                    break;
                case BattleSceneSettings.PanzerControlType.Keyboard:
                    trackScripts = CreateTrackKeyboardScripts(OpenTK.Input.Key.A, OpenTK.Input.Key.D);
                    turretScripts = CreateTurretKeyboardScripts(cooldownBar, 
                        OpenTK.Input.Key.W, OpenTK.Input.Key.S, OpenTK.Input.Key.Space, 
                        OpenTK.Input.Key.E, OpenTK.Input.Key.Q);
                    break;
            }

            BuildPanzer(firstPanzer, trackTexture,
                turretTexture, position, new Vector2(-5, 5), 
                healthBar, inventory, trackScripts, turretScripts);
        }

        private void BuildSecondPanzer(Inventory inventory)
        {
            Texture2D trackTexture = Texture2D.LoadTexture(TRACK_TEXTURE_PATH);
            Texture2D turretTexture = Texture2D.LoadTexture(TURRET_TEXTURE_PATH);
            trackTexture.Color = Color.FromArgb(20, 140, 120);
            turretTexture.Color = Color.FromArgb(20, 140, 120);
            AddTexture(trackTexture);
            AddTexture(turretTexture);

            Vector2 position = new Vector2((float)GameWindow.Width * 3 / 4,
                (float)GameWindow.Height - trackTexture.Height * 14);

            Script[] trackScripts = null;
            Script[] turretScripts = null;

            ProgressBar cooldownBar = (ProgressBar)GameWindow.FindName("SecondPanzerCooldown");
            ProgressBar healthBar = (ProgressBar)GameWindow.FindName("SecondPanzerHealth");

            switch (settings.FirstPanzerControlType)
            {
                case BattleSceneSettings.PanzerControlType.AI:
                    trackScripts = CreateTrackAIScripts();
                    turretScripts = CreateTurretAIScripts(cooldownBar, firstPanzer);
                    break;
                case BattleSceneSettings.PanzerControlType.Keyboard:
                    trackScripts = CreateTrackKeyboardScripts(OpenTK.Input.Key.Left, OpenTK.Input.Key.Right);
                    turretScripts = CreateTurretKeyboardScripts(cooldownBar,
                        OpenTK.Input.Key.Up, OpenTK.Input.Key.Down, OpenTK.Input.Key.Enter,
                        OpenTK.Input.Key.Plus, OpenTK.Input.Key.Minus);
                    break;
            }

            BuildPanzer(secondPanzer, trackTexture,
                turretTexture, position, new Vector2(5, 5),
                healthBar, inventory, trackScripts, turretScripts);
        }

        private Inventory.RocketBuilder[] CreateRockets()
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

            return new Inventory.RocketBuilder[]
            {
                new Inventory.RocketBuilder(this, powerfulRocketTex, explosionAnim,
                new DoubleDamageRocket(new DoubleCooldownRocket(new Rocket()))),
                new Inventory.RocketBuilder(this, fastRocketTex, explosionAnim,
                new HalfDamageRocket(new HalfCooldownRocket(new Rocket()))),
                new Inventory.RocketBuilder(this, rocketTex, explosionAnim,
                new Rocket())
            };
        }

        private GameObject CreateMountain()
        {
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
            return mountain;
        }

        private GameObject CreateBackground()
        {
            Texture2D backgroundTex = Texture2D.LoadTexture(BACKGROUND_TEXTURE_PATH);
            AddTexture(backgroundTex);
            return new GameObject(backgroundTex,
                new Vector2((float)-GameWindow.Width, (float)-GameWindow.Height),
                Vector2.Zero, new Vector2(5, 5), 0);
        }

        private Script[] CreateTrackKeyboardScripts(OpenTK.Input.Key left, OpenTK.Input.Key right)
        {
            TrackKeyboardControlScript panzerControl = new TrackKeyboardControlScript(this, 300f);
            panzerControl.SetKeyToMoveLeft(left);
            panzerControl.SetKeyToMoveRight(right);
            return new Script[] { panzerControl };
        }

        private Script[] CreateTrackAIScripts()
        {
            return new Script[] { new TrackAIControlScript(300f, 200) };
        }

        private Script[] CreateTurretKeyboardScripts(ProgressBar cooldownBar,
            OpenTK.Input.Key up, OpenTK.Input.Key down, OpenTK.Input.Key shoot,
            OpenTK.Input.Key next, OpenTK.Input.Key previous)
        {
            TurretKeyboardControlScript turretControl = new TurretKeyboardControlScript(2);
            turretControl.SetKeyToTurnUp(up);
            turretControl.SetKeyToTurnDown(down);
            ShootKeyboardControlScript shootControl = new ShootKeyboardControlScript(this);
            shootControl.SetKey(shoot);
            KeyboardRocketSwitcherScript rocketSwitcher = new KeyboardRocketSwitcherScript();
            rocketSwitcher.SetKeyToSelectNext(next);
            rocketSwitcher.SetKeyToSelectPrevious(previous);

            WpfShootControlScript wpfShootControl = new WpfShootControlScript(this, cooldownBar, shootControl);
            return new Script[] { turretControl, rocketSwitcher, wpfShootControl };
        }

        private Script[] CreateTurretAIScripts(ProgressBar cooldownBar, GameObject target)
        {
            TurretAIControlScript turretControl = new TurretAIControlScript(2);
            ShootAIControlScript shootControl = new ShootAIControlScript(this, 500);
            AIRocketSwitcherScript rocketSwitcher = new AIRocketSwitcherScript(100, 1000);
            turretControl.SetTarget(target);

            WpfShootControlScript wpfShootControl = new WpfShootControlScript(this, cooldownBar, shootControl);
            return new Script[] { turretControl, rocketSwitcher, wpfShootControl };
        }
    }
}
