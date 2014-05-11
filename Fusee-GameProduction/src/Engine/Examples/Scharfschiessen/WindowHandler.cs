using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fusee.Engine;

namespace Examples.Scharfschiessen
{
    /// <summary>
    /// Die Klassse WindowHandler regelt die Einstellung der Fenstergröße
    /// </summary>
    public class WindowHandler
    {

        public void SetWindowSettings(RenderCanvas rc)
        {
            //Höhe und Breite an Primären Bildschirm anpassen
            rc.Width = Screen.PrimaryScreen.Bounds.Width;
            rc.Height = Screen.PrimaryScreen.Bounds.Height /9;
        }

        public void Hide(RenderCanvas rc)
        {
            rc.Width = Screen.PrimaryScreen.Bounds.Width /9;
            rc.Height = Screen.PrimaryScreen.Bounds.Height / 8;
        }
    }
}
