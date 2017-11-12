using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Histacom2.Engine;
using System.Drawing.Text;
using static Histacom2.Engine.SaveSystem;
using Histacom2.OS.Win95;

namespace Histacom2.OS.Win95.Win95Apps._12padamsViruses
{
    public partial class StartRunner95 : UserControl
    {
        Point beginLocation;
        private Windows95 _win;
        int xLocation;
        private static PrivateFontCollection pfc = new PrivateFontCollection();
        public StartRunner95(Windows95 w)
        {
            _win = w;

            InitializeComponent();
            pfc.AddFontFile(DataDirectory + "\\LeviWindows.ttf");
            label2.Font = new Font(pfc.Families[0], 16F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            beginLocation = w.startbutton.Location;
            Random random = new Random();
            
        }


        private void classicButton1_Click(object sender, EventArgs e)
        {
            Timer1.Start();
        }


        private void classicButton2_Click(object sender, EventArgs e)
        {
            beginLocation = new Point(2, 4);
            Timer1.Stop();
            ResetStartButton();
        }

        private void classicButton3_Click(object sender, EventArgs e)
        {
            ResetStartButton();
            ParentForm.Close();
        }

        
        private void ResetStartButton()
        {
            _win.startbutton.Location = beginLocation;
            Timer1.Stop();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            xLocation = r.Next(1, _win.taskbar.Width);
            _win.startbutton.Location = new Point(xLocation, _win.startbutton.Location.Y);
        }

        private void StartRunner95_Load(object sender, EventArgs e)
        {

        }
    }
}
