using SCCC;
using Terminal.Gui;

namespace SCCC
{
    public class Program
    {
        public static MyView view = new MyView();
        public static void Main(string[] args)
        {

            Application.Init();
            try
            {
                Application.Run(view);
            }
            finally
            {
                Application.Shutdown();
            }
        }
    }
}
