using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;
using GameEngineLibrary;
using System.Drawing;

namespace GameUserInterface
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IScene scene;
        private Renderer renderer;

        public MainWindow()
        {
            InitializeComponent();

            var settings = new GLWpfControlSettings();
            settings.MajorVersion = 3;
            settings.MinorVersion = 6;
            OpenTKControl.Start(settings);
        }

        private void OpenTKControl_Render(TimeSpan delta)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color4.SkyBlue);

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
