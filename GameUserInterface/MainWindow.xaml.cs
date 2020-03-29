using System;
using System.Windows;
using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;
using GameEngineLibrary;
using GameLibrary;
using GameLibrary.Scenes;
using System.Windows.Controls;

namespace GameUserInterface
{
    /// <summary>
    /// Логика взаимодействия для главного окна игры.
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int POWERFUL_ROCKET_COST = 5;
        public const int FAST_ROCKET_COST = 4;
        public const int ROCKET_COST = 3;

        /// <summary>
        /// Сцена, которая будет отрисовываться на экране.
        /// </summary>
        private Scene scene;

        /// <summary>
        /// Отрисовщик сцены.
        /// </summary>
        private Renderer renderer;

        public MainWindow()
        {
            InitializeComponent();

            scene = new BattleScene(this, new BattleSceneSettings());

            FirstPanzerInfo.Visibility = Visibility.Hidden;
            SecondPanzerInfo.Visibility = Visibility.Hidden;
            RocketShop.Visibility = Visibility.Hidden;

            var settings = new GLWpfControlSettings();
            settings.MajorVersion = 3;
            settings.MinorVersion = 6;
            OpenTKControl.Start(settings);
        }

        private void OpenTKControl_Render(TimeSpan delta)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-Width, Width, Height, -Height, 0d, 1d);

            scene.Update(delta);
            renderer.Render();
        }

        private void OpenTKControl_Ready()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            scene.Init();
            renderer = new Renderer(scene);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            scene.Dispose();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
        }

        private void PlayGameBtn_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Hidden;
            RocketShop.Visibility = Visibility.Visible;
        }

        private void QuitGameBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StartGameBtn_Click(object sender, RoutedEventArgs e)
        {
            RocketShop.Visibility = Visibility.Hidden;
            FirstPanzerInfo.Visibility = Visibility.Visible;
            SecondPanzerInfo.Visibility = Visibility.Visible;

            scene.Dispose();

            BattleSceneSettings settings = new BattleSceneSettings();
            int firstPanzerPowerfulRockets = int.Parse(FirstPanzerPowerfulRockets.Content.ToString());
            int firstPanzerFastRockets = int.Parse(FirstPanzerFastRockets.Content.ToString());
            int firstPanzerRockets = int.Parse(FirstPanzerRockets.Content.ToString());
            int secondPanzerPowerfulRockets = int.Parse(SecondPanzerPowerfulRockets.Content.ToString());
            int secondPanzerFastRockets = int.Parse(SecondPanzerFastRockets.Content.ToString());
            int secondPanzerRockets = int.Parse(SecondPanzerRockets.Content.ToString());
            settings.SetFirstPanzerAmounts(firstPanzerPowerfulRockets, firstPanzerFastRockets, firstPanzerRockets);
            settings.SetSecondPanzerAmounts(secondPanzerPowerfulRockets, secondPanzerFastRockets, secondPanzerRockets);
            scene = new BattleScene(this, settings);
            scene.Init();
            renderer = new Renderer(scene);
        }

        private void BuyBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Grid grid = button.Parent as Grid;
            Label outputLabel = grid.Children[0] as Label;
            Label moneyLable = (grid.Parent as Grid).Children[0] as Label;

            int amount = int.Parse(button.Content.ToString());
            int value = int.Parse(outputLabel.Content.ToString());
            int money = int.Parse(moneyLable.Content.ToString());
            int cost = int.Parse((grid.Children[1] as Label).Content.ToString()) * amount;

            if (value + amount >= 0 && money - cost >= 0)
            {
                value += amount;
                money -= cost;

                outputLabel.Content = value.ToString();
                moneyLable.Content = money.ToString();
            }
        }
    }
}
