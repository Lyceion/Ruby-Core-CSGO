using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Ruby_CSGO.Forms.Loader
{
    public partial class Loader : MetroFramework.Forms.MetroForm
    {
        #region Defines
        public static string GameName = "csgo";
        string client_module = "client_panorama.dll";
        string engine_module = "engine.dll";
        #endregion

        #region Command Shortcuts
        public void addStrToLog(string stringToAdd)
        {
            logTextBox.Text = logTextBox.Text + "\n[" + DateTime.Now.ToString("dd-MM-yyy || HH:mm:ss") + "] => " + stringToAdd;
        }
        public void Wait(int ms)
        {
            DateTime start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < ms)
            Application.DoEvents();
        }
        #endregion

        #region Form Things
        public Loader()
        {
            InitializeComponent();
        }
        private void Loader_Shown(object sender, EventArgs e)
        {
            logTextBox.Text = "[" + DateTime.Now.ToString("dd-MM-yyy || HH:mm:ss") + "] => " + "Cheat Loaded.";
            Wait(1500);
            addStrToLog("Waiting For CS:GO...");
            Wait(2000);
            Stage1();
        }
        private void Loader_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion

        #region Launching Process
        private void Stage1()
        {
            //Detecting game statuts...
            while (true)
            {
                if (Classes.Global_Functions.isGameOpen(GameName))
                {
                    addStrToLog("CSGO Found! Now, we should look to moduels...");
                    Stage2();
                    break;
                }
                else
                {
                    //WTF are you looking there? Next step! 
                }
            }
        }
        private void Stage2()
        {
            while (true)
            {
                if(Classes.Global_Functions.detectModule(client_module, GameName))
                {
                    addStrToLog("W0w, u r amazing! Client module found. Now, engine's turn...");
                    Stage3();
                    break;
                }
                else
                {
                    //Do u have brain? It should be... OK, now switch to the next step
                }
            }
        }
        private void Stage3()
        {
            while (true)
            {
                if (Classes.Global_Functions.detectModule(engine_module, GameName))
                {
                    addStrToLog("All complate! Now, launching the rockets (Like Elon LMAO)");
                    Stage5();
                    break;
                }
                else
                {
                    //Next!
                }
            }
        }
        //private void Stage4()
        //{
        //    //SigScan Boi
        //    Wait(2000);
        //    Classes.Global_Functions.ScanShit();
        //    addStrToLog("SigScan ---- OK!");
        //    addStrToLog(Convert.ToString(Offsets.OffsetList.EntityList));
        //    addStrToLog(Convert.ToString(Offsets.OffsetList.GlowObject));
        //    addStrToLog(Convert.ToString(Offsets.OffsetList.LocalPlayer));
        //    addStrToLog(Convert.ToString(Offsets.OffsetList.ClientBase));
        //    Stage5();
        //}
        private void Stage5()
        {
            Wait(10000);
            //Only showing the cheat form LMAOOOOOOOOO xd
            Forms.Cheats.Simple.Simple_Main chtFrm = new Forms.Cheats.Simple.Simple_Main();
            Wait(5000);
            this.Hide();
            chtFrm.Show();
        }
        #endregion
    }
}
