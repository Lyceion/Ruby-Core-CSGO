using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ruby_CSGO.Classes;

namespace Ruby_CSGO.Classes
{
    class Global_Functions
    {
        public static bool isGameOpen(string GameName)
        {
            if (Process.GetProcessesByName(GameName).Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool detectModule(string ModuleName, string GameName)
        {
            try
            {
                Process[] p = Process.GetProcessesByName(GameName);
                if (p.Length > 0)
                {
                    foreach (ProcessModule m in p[0].Modules)
                    {
                        if (m.ModuleName == ModuleName)
                        {
                            return true;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public static void Wait(int ms)
        {
            DateTime start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < ms)
                Application.DoEvents();
        }
        public static List<string> ScanShit()
        {
            GetModuleAdress();
            List<string> outdatedSignatures = new List<string> { };

            Offsets.OffsetList.GlowObject = Memory.SigScan.ScanPattern((int)Forms.Cheats.Simple.Simple_Main.ClientAdress, "A1????A801754B", 1, 4, true);
            {
                if (Offsets.OffsetList.GlowObject == 0) outdatedSignatures.Add("dwGlowObjectManager");
                //Extensions.Information($"dwGlowObjectManager: 0x{ dwGlowObjectManager.ToString("X") }", true);
            }

            Offsets.OffsetList.EntityList = Memory.SigScan.ScanPattern((int)Forms.Cheats.Simple.Simple_Main.ClientAdress, "BB????83FF010F8C????3BF8", 0, 1, true);
            {
                if (Offsets.OffsetList.EntityList == 0) outdatedSignatures.Add("dwEntityList");
            }

            Offsets.OffsetList.ClientBase = Memory.SigScan.ScanPattern((int)Forms.Cheats.Simple.Simple_Main.EngineAdress, "A1????33D26A006A0033C989B0", 0, 1, true);
            {
                if (Offsets.OffsetList.ClientBase == 0) outdatedSignatures.Add("dwClientState");
            }

            Offsets.OffsetList.LocalPlayer = Memory.SigScan.ScanPattern((int)Forms.Cheats.Simple.Simple_Main.ClientAdress, "8D3485????8915????8B41088B480483F9FF", 4, 3, true);
            {
                if (Offsets.OffsetList.LocalPlayer == 0) outdatedSignatures.Add("dwLocalPlayer");
            }

            return outdatedSignatures;
        }
        public static void GetModuleAdress()
        {
            IEnumerator enumerator;
            try
            {
                Forms.Cheats.Simple.Simple_Main.gameProc = Process.GetProcessesByName(Forms.Loader.Loader.GameName)[0];
                IntPtr Handle = Forms.Cheats.Simple.Simple_Main.OpenProcess(0x1f0fff, false, Forms.Cheats.Simple.Simple_Main.gameProc.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Where Is Trash:GO? Exception =>\n" + ex, "Ruby");
            }
            enumerator = Forms.Cheats.Simple.Simple_Main.gameProc.Modules.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ProcessModule aaa = (ProcessModule)enumerator.Current;
                if (aaa.ModuleName == "client_panorama.dll")
                {
                    Forms.Cheats.Simple.Simple_Main.ClientAdress = (int)aaa.BaseAddress;
                }
                if (aaa.ModuleName == "engine.dll")
                {
                    Forms.Cheats.Simple.Simple_Main.EngineAdress = (int)aaa.BaseAddress;
                }
            }
        }
    }
}
