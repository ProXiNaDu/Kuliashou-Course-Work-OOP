using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using GameEngineLibrary;
using GameLibrary.Components;
using GameLibrary.Components.HealthDecorators;
using GameLibrary.Components.RocketDecorators;
using GameLibrary.Scenes;
using GameLibrary.Scripts;
using GameLibrary.Scripts.RemoteKeyboardControlScripts;
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
            Inventory firstPanzerInventory;
            Inventory secondPanzerInventory;

            if (GameWindow == null)
            {
                firstPanzerInventory = new Inventory(rockets);
                secondPanzerInventory = new Inventory(rockets);
            }
            else
            {
                firstPanzerInventory = new WpfInventory(
                    (StackPanel)GameWindow.FindName("FirstPanzerInventory"), rockets);
                secondPanzerInventory = new WpfInventory(
                    (StackPanel)GameWindow.FindName("SecondPanzerInventory"), rockets);
            }

            settings.FillFirtsPanzerInventory(firstPanzerInventory);
            settings.FillSecondPanzerInventory(secondPanzerInventory);

            BuildFirstPanzer(firstPanzerInventory);
            BuildSecondPanzer(secondPanzerInventory);

            GameObject winChecker = null;
            if (GameWindow != null)
            {
                winChecker = new GameObject();
                winChecker.AddScript(new WinCheckerScript(firstPanzer, secondPanzer,
                    GameWindow.FindName("WinMenu") as StackPanel));

            }

            AddGameObject(firstPanzer);
            AddGameObject(secondPanzer);

            if (winChecker != null)
            {
                AddGameObject(winChecker);
            }
        }

        private void BuildPanzer(GameObject panzer, Texture2D trackTex,
            Texture2D turretTex, Vector2 position, Vector2 scale,
            int health, ProgressBar healthBar, Inventory inventory,
            Script[] panzerScripts, Script[] turretScripts)
        {
            Transform transform = panzer.GetComponent("transform") as Transform;
            transform.Position = position;
            transform.Scale = scale;

            panzer.AddComponent("texture", trackTex);
            var healthComponent = new Health(health);
            if (healthBar != null)
            {
                healthComponent = new ProgressBarHealth(healthComponent, healthBar);
            }
            panzer.AddComponent("health", healthComponent);
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

            Vector2 position = new Vector2(
                ((GameWindow != null) ? (float)-GameWindow.Width : -800f) * 3 / 4,
                ((GameWindow != null) ? (float)GameWindow.Height : 450f) - trackTexture.Height * 14);

            Script[] trackScripts = null;
            Script[] turretScripts = null;

            ProgressBar cooldownBar = (ProgressBar)GameWindow?.FindName("FirstPanzerCooldown");
            ProgressBar healthBar = (ProgressBar)GameWindow?.FindName("FirstPanzerHealth");

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
                case BattleSceneSettings.PanzerControlType.Remote:
                    trackScripts = CreateTrackRemoteKeyboardScripts(settings.FirstPanzerRemoteState);
                    turretScripts = CreateTurretRemoteKeyboardScripts(settings.FirstPanzerRemoteState);
                    break;
            }

            BuildPanzer(firstPanzer, trackTexture,
                turretTexture, position, new Vector2(-5, 5), 
                settings.FirstPanzerHealth, healthBar, 
                inventory, trackScripts, turretScripts);
        }

        private void BuildSecondPanzer(Inventory inventory)
        {
            Texture2D trackTexture = Texture2D.LoadTexture(TRACK_TEXTURE_PATH);
            Texture2D turretTexture = Texture2D.LoadTexture(TURRET_TEXTURE_PATH);
            trackTexture.Color = Color.FromArgb(20, 140, 120);
            turretTexture.Color = Color.FromArgb(20, 140, 120);
            AddTexture(trackTexture);
            AddTexture(turretTexture);

            Vector2 position = new Vector2(
                ((GameWindow != null) ? (float)GameWindow.Width : 800) * 3 / 4,
                ((GameWindow != null) ? (float)GameWindow.Height : 450) - trackTexture.Height * 14);

            Script[] trackScripts = null;
            Script[] turretScripts = null;

            ProgressBar cooldownBar = (ProgressBar)GameWindow?.FindName("SecondPanzerCooldown");
            ProgressBar healthBar = (ProgressBar)GameWindow?.FindName("SecondPanzerHealth");

            switch (settings.SecondPanzerControlType)
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
                case BattleSceneSettings.PanzerControlType.Remote:
                    trackScripts = CreateTrackRemoteKeyboardScripts(settings.SecondPanzerRemoteState);
                    turretScripts = CreateTurretRemoteKeyboardScripts(settings.SecondPanzerRemoteState);
                    break;
            }

            BuildPanzer(secondPanzer, trackTexture,
                turretTexture, position, new Vector2(5, 5), 
                settings.SecondPanzerHealth, healthBar, 
                inventory, trackScripts, turretScripts);
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
                new DoubleDamageRocket(new DoubleCooldownRocket(new BaseRocket()))),
                new Inventory.RocketBuilder(this, fastRocketTex, explosionAnim,
                new HalfDamageRocket(new HalfCooldownRocket(new BaseRocket()))),
                new Inventory.RocketBuilder(this, rocketTex, explosionAnim,
                new BaseRocket())
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
                new Vector2(
                    (GameWindow != null) ? (float)-GameWindow.Width : -800f,
                    (GameWindow != null) ? (float)-GameWindow.Height : -450f),
                Vector2.Zero, new Vector2(5, 5), 0);
        }

        private Script[] CreateTrackKeyboardScripts(OpenTK.Input.Key left, OpenTK.Input.Key right)
        {
            TrackKeyboardControlScript panzerControl = new TrackKeyboardControlScript(this, 300f);
            panzerControl.SetKeyToMoveLeft(left);
            panzerControl.SetKeyToMoveRight(right);
            return new Script[] { panzerControl };
        }

        private Script[] CreateTrackRemoteKeyboardScripts(RemoteState remoteState)
        {
            RemoteTrackKeyboardControlScript panzerControl = new RemoteTrackKeyboardControlScript(this, 300f);
            panzerControl.SetRemoteState(remoteState);
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

            if (cooldownBar == null)
            {
                return new Script[] { turretControl, rocketSwitcher, shootControl };
            }

            WpfShootControlScript wpfShootControl = new WpfShootControlScript(this, cooldownBar, shootControl);
            return new Script[] { turretControl, rocketSwitcher, wpfShootControl };
        }

        private Script[] CreateTurretRemoteKeyboardScripts(RemoteState remoteState)
        {
            RemoteTurretKeyboardControlScript turretControl = new RemoteTurretKeyboardControlScript(2);
            turretControl.SetRemoteState(remoteState);
            RemoteShootKeyboardControlScript shootControl = new RemoteShootKeyboardControlScript(this);
            shootControl.SetRemoteState(remoteState);
            RemoteKeyboardRocketSwitcherScript rocketSwitcher = new RemoteKeyboardRocketSwitcherScript();
            rocketSwitcher.SetRemoteState(remoteState);

            return new Script[] { turretControl, rocketSwitcher, shootControl };
        }

        private Script[] CreateTurretAIScripts(ProgressBar cooldownBar, GameObject target)
        {
            TurretAIControlScript turretControl = new TurretAIControlScript(2);
            ShootAIControlScript shootControl = new ShootAIControlScript(this, 500);
            AIRocketSwitcherScript rocketSwitcher = new AIRocketSwitcherScript(100, 1000);
            turretControl.SetTarget(target);

            if (cooldownBar == null)
            {
                return new Script[] { turretControl, rocketSwitcher, shootControl };
            }

            WpfShootControlScript wpfShootControl = new WpfShootControlScript(this, cooldownBar, shootControl);
            return new Script[] { turretControl, rocketSwitcher, wpfShootControl };
        }
    }
}
