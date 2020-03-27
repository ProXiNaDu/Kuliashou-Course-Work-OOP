using System;
using System.Windows;
using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;
using GameEngineLibrary;
using GameLibrary;

namespace GameUserInterface
{
    /// <summary>
    /// Логика взаимодействия для главного окна игры.
    /// </summary>
    public partial class MainWindow : Window
    {
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

            scene = new BattleScene(this);

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
    }
}
