using System.ServiceModel;
using System.Threading;
using WcfServiceLibrary;

namespace GameUserInterface
{
    /// <summary>
    /// Игровой сервер
    /// </summary>
    public class Server
    {
        private Thread hostThread;
        private ServiceHost connectServiceHost;

        private bool started;
        private bool closed;

        /// <summary>
        /// Создает сервер
        /// </summary>
        public Server()
        {
            hostThread = new Thread(new ThreadStart(Host));
            hostThread.Start();

            while(!started)
            {
                Thread.Sleep(100);
            }
        }

        private void ListenForConnections()
        {
            connectServiceHost = new ServiceHost(typeof(ConnectService));

            connectServiceHost.Open();

            started = true;

            while (!closed)
            {
                Thread.Sleep(100);
            }
        }

        private void Host()
        {
            connectServiceHost = new ServiceHost(typeof(ConnectService));

            connectServiceHost.Open();

            started = true;

            while (!closed)
            {
                Thread.Sleep(10000);
            }
        }

        /// <summary>
        /// Указывает, закрыт ли сервер
        /// </summary>
        /// <returns>true - если сервер закрыт; в противно случае - false</returns>
        public bool IsClosed()
        {
            return closed;
        }

        /// <summary>
        /// Освобождает занятые сервером ресурсы
        /// </summary>
        public void Close()
        {
            if (closed) return;

            connectServiceHost.Close();

            closed = true;
            started = false;
        }

        /// <summary>
        /// Освобождает занятые сервером ресурсы
        /// </summary>
        ~Server()
        {
            Close();
        }
    }
}
