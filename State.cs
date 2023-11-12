using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace SCCC
{
    public enum State
    {
        LOCAL,
        SERVER,
        CLIENT,
        CHANGING,
        ERROR,
        UNKNOWN
    }

    public class StateConvertor
    {
        public static ColorScheme GetScheme(State state)
        {
            var scheme = new ColorScheme();
            var driver = Application.Driver;
            switch (state)
            {
                case State.LOCAL:
                    scheme.Normal = driver.MakeAttribute(Color.White, Color.Blue);
                    scheme.Focus = driver.MakeAttribute(Color.White, Color.Blue);
                    break;
                case State.SERVER:
                    scheme.Normal = driver.MakeAttribute(Color.BrightYellow, Color.Blue);
                    scheme.Focus = driver.MakeAttribute(Color.BrightYellow, Color.Blue);
                    break;
                case State.CLIENT:
                    scheme.Normal = driver.MakeAttribute(Color.BrightGreen, Color.Blue);
                    scheme.Focus = driver.MakeAttribute(Color.BrightGreen, Color.Blue);
                    break;
                case State.CHANGING:
                    scheme.Normal = driver.MakeAttribute(Color.Red, Color.Blue);
                    scheme.Focus = driver.MakeAttribute(Color.Red, Color.Blue);
                    break;
                case State.ERROR:
                    scheme.Normal = driver.MakeAttribute(Color.BrightRed, Color.Blue);
                    scheme.Focus = driver.MakeAttribute(Color.BrightRed, Color.Blue);
                    break;
                case State.UNKNOWN:
                    scheme.Normal = driver.MakeAttribute(Color.Gray, Color.Blue);
                    scheme.Focus = driver.MakeAttribute(Color.Gray, Color.Blue);
                    break;
            }

            return scheme;
        }
    }
}
