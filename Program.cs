using System;
using System.Windows.Forms;
using GameConnectFour.Model;
using GameConnectFour.Presenter;
using GameConnectFour.View;

namespace GameConnectFour
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Dependency Injection (Manual)
            ConnectFourForm view = new ConnectFourForm(); // The View
            GameModel model = new GameModel();       // The Model
            
            // The Presenter wires them together
            // We don't need to store the presenter, it stays alive by event subscriptions
            new GamePresenter(view, model);

            Application.Run(view);
        }
    }
}
