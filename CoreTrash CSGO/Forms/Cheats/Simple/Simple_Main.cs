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
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;
using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices;

namespace Ruby_CSGO.Forms.Cheats.Simple
{

    public partial class Simple_Main : MetroFramework.Forms.MetroForm
    {
        #region Defines
        public static int ClientAdress = 0x0000000;
        public static int EngineAdress = 0x0000000;
        public static Process gameProc;
        public static int EngineBase;
        public static int LocalBase;
        public static int GameState;
        public static int xorBase;
        public static VAMemory mem = new VAMemory();
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr OpenProcess(uint dwDesiredAcess, bool bInheritHandle, int dwProcessId);
        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);
        [DllImport("user32", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        #endregion

        #region Form Shits
        public Simple_Main()
        {
            InitializeComponent();
            GetModules();
        }
        private static string RandomString(int length)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            string input = "#$!%&?}{][ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, length).Select(x => input[random.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }
        private void Simple_Main_Load(object sender, EventArgs e)
        {
            //Define Threads
            Thread titleChg = new Thread(new ThreadStart(titleChanger));
            //Start Threads
            titleChg.Start();
            //Etc
            delayMS.Text = Convert.ToString(delayTrack.Value) + " -MS";
            
        }
        private void exitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private void prtctButton_Click(object sender, EventArgs e)
        {
            prtctPnl.Visible = false;
            Thread AFlashThread = new Thread(new ThreadStart(AntFlashIt));
            AFlashThread.Start();
            Thread TrigThread = new Thread(new ThreadStart(TriggerIt));
            TrigThread.Start();
        }
        private void delayTrack_Scroll(object sender, ScrollEventArgs e)
        {
            if (delayTrack.Value < 1000)
            {
                delayMS.Text = Convert.ToString(delayTrack.Value) + " -MS";
            }
            else
            {
                delayMS.Text = Convert.ToString(delayTrack.Value / 1000) + " -Sec";
            }
        }
        private void Simple_Main_Shown(object sender, EventArgs e)
        {
            this.Refresh();
        }
        private void titleChanger()
        {
            while (true)
            {
                //this.Text = RandomString(10);
            }
        } //DISABLED FOR A WHILE
        #endregion

        #region Modules
        private void GetModules()
        {
            IEnumerator enumerator;
            try
            {
                gameProc = Process.GetProcessesByName(Loader.Loader.GameName)[0];
                IntPtr Handle = OpenProcess(0x1f0fff, false, gameProc.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Where Is Trash:GO? Exception =>\n" + ex, "Ruby");
            }
            enumerator = gameProc.Modules.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ProcessModule aaa = (ProcessModule)enumerator.Current;
                if (aaa.ModuleName == "client_panorama.dll")
                {
                    ClientAdress = (int)aaa.BaseAddress;
                }
                if (aaa.ModuleName == "engine.dll")
                {
                    EngineAdress = (int)aaa.BaseAddress;
                }
            }
            Debug.Print("client_panorama.dll ==>" + ClientAdress);
            Debug.Print("engine.dll ==>" + EngineAdress);
            Classes.Global_Functions.Wait(50);
            mem.processName = Forms.Loader.Loader.GameName;
            EngineBase = mem.ReadInt32((IntPtr)EngineAdress + Offsets.OffsetList.ClientBase);
            LocalBase = mem.ReadInt32((IntPtr)ClientAdress + Offsets.OffsetList.LocalPlayer);
            Thread GlowThread = new Thread(new ThreadStart(GlowIt));
            Thread ChamThread = new Thread(new ThreadStart(ChamIt));
            Thread RadarThread = new Thread(new ThreadStart(RadarIt));
            GlowThread.Start();
            ChamThread.Start();
            RadarThread.Start();
        }
        #endregion

        #region Cheats

        #region Glow Normal
        public struct GlowStruct
        {
            public float r;
            public float g;
            public float b;
            public float a;
            public bool rwo;
            public bool rwuo;
        }
        public void GlowIt()
        {
            GlowStruct colorsE = new GlowStruct()
            {
                r = 255.0f,
                g = 0f,
                b = 0f,
                a = 122.5f,
                rwo = true,
                rwuo = false
            };
            GlowStruct colorsT = new GlowStruct()
            {
                r = 0f,
                g = 103.0f,
                b = 221.0f,
                a = 122.5f,
                rwo = true,
                rwuo = false
            };
            while (true)
            {
                if (wallCHCK.Checked)
                {
                    GameState = mem.ReadInt32((IntPtr)EngineBase + Offsets.OffsetList.GameState);
                    if (GameState == 6)
                    {
                        Classes.Global_Functions.Wait(100);
                        int i = 1;
                        do
                        {
                            int MyTeam = mem.ReadInt32((IntPtr)LocalBase + Offsets.OffsetList.Team);
                            int EntityList = mem.ReadInt32((IntPtr)ClientAdress + Offsets.OffsetList.EntityList + (i - 1) * 0x10);
                            int HisTeam = mem.ReadInt32((IntPtr)EntityList + Offsets.OffsetList.Team);
                            if (!mem.ReadBoolean((IntPtr)EntityList + Offsets.OffsetList.Dormant))
                            {
                                if (MyTeam != HisTeam)
                                {
                                    int GlowIndex = mem.ReadInt32((IntPtr)EntityList + Offsets.OffsetList.GlowIndex);
                                    int GlowObject = mem.ReadInt32((IntPtr)ClientAdress + Offsets.OffsetList.GlowObject);
                                    mem.WriteByte((IntPtr)EntityList + 0x70, (byte)colorsE.r);
                                    mem.WriteByte((IntPtr)EntityList + 0x71, (byte)colorsE.g);
                                    mem.WriteByte((IntPtr)EntityList + 0x72, (byte)colorsE.b);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x4), colorsE.r);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x8), colorsE.g);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0xC), colorsE.b);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x10), colorsE.a);
                                    mem.WriteBoolean((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x24), true);
                                    mem.WriteBoolean((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x2C), false);
                                }
                                if(MyTeam == HisTeam)
                                {
                                    int GlowIndex = mem.ReadInt32((IntPtr)EntityList + Offsets.OffsetList.GlowIndex);
                                    int GlowObject = mem.ReadInt32((IntPtr)ClientAdress + Offsets.OffsetList.GlowObject);
                                    mem.WriteByte((IntPtr)EntityList + 0x70, (byte)colorsT.r);
                                    mem.WriteByte((IntPtr)EntityList + 0x71, (byte)colorsT.g);
                                    mem.WriteByte((IntPtr)EntityList + 0x72, (byte)colorsT.b);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x4), colorsT.r);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x8), colorsT.g);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0xC), colorsT.b);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x10), colorsT.a);
                                    mem.WriteBoolean((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x24), true);
                                    mem.WriteBoolean((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x2C), false);

                                }
                            }
                            i++;
                        } while (i < 65);
                        int thisPtr = EngineBase + Offsets.OffsetList.ModelAmbientMin - 0x2c;
                        xorBase = Convert.ToInt32(255.0f) ^ thisPtr;
                        mem.WriteInt32((IntPtr)EngineBase + Offsets.OffsetList.ModelAmbientMin - 0x2c, xorBase);
                    }
                }
            }
        }

        #endregion

        #region Glow Health
        public void GlowItHB() 
        {
            while (true)
            {
                if (wallCHCK.Checked)
                {
                    GameState = mem.ReadInt32((IntPtr)EngineBase + Offsets.OffsetList.GameState);
                    if (GameState == 6)
                    {
                        Classes.Global_Functions.Wait(100);
                        int i = 1;
                        do
                        {
                            int MyTeam = mem.ReadInt32((IntPtr)LocalBase + Offsets.OffsetList.Team);
                            int EntityList = mem.ReadInt32((IntPtr)ClientAdress + Offsets.OffsetList.EntityList + (i - 1) * 0x10);
                            int HisTeam = mem.ReadInt32((IntPtr)EntityList + Offsets.OffsetList.Team);
                            if (!mem.ReadBoolean((IntPtr)EntityList + Offsets.OffsetList.Dormant))
                            {
                                if (MyTeam != HisTeam)
                                {
                                    int GlowIndex = mem.ReadInt32((IntPtr)EntityList + Offsets.OffsetList.GlowIndex);
                                    int GlowObject = mem.ReadInt32((IntPtr)ClientAdress + Offsets.OffsetList.GlowObject);
                                    int EnemyHealth = mem.ReadInt32((IntPtr)EntityList + Offsets.OffsetList.Health);
                                    float r = (255.0f * 2.0f * (1 - ((float)EnemyHealth / 100.0f)));
                                    float g = 255;
                                    if (EnemyHealth < 50)
                                    {
                                        r = 255.0f;
                                        g = (255.0f * 2.0f * ((float)EnemyHealth / 100.0f));
                                    }
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x4), r);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x8), g);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0xC), 0f);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x10), 255f);
                                    mem.WriteBoolean((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x24), true);
                                    mem.WriteBoolean((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x25), false);
                                }
                            }
                            i++;
                        } while (i < 65);
                    }
                }
            }
        }
        #endregion

        #region Chams
        public void ChamIt()
        {
            GlowStruct colorsE = new GlowStruct()
            {
                r = 255.0f,
                g = 0f,
                b = 0f,
                a = 122.5f,
                rwo = true,
                rwuo = false
            };
            GlowStruct colorsT = new GlowStruct()
            {
                r = 0f,
                g = 103.0f,
                b = 221.0f,
                a = 155.5f,
                rwo = true,
                rwuo = false
            };
            while (true)
            {
                if (chamCHCK.Checked)
                {
                    GameState = mem.ReadInt32((IntPtr)EngineBase + Offsets.OffsetList.GameState);
                    if (GameState == 6)
                    {
                        int i = 1;
                        do
                        {
                            int MyTeam = mem.ReadInt32((IntPtr)LocalBase + Offsets.OffsetList.Team);
                            int EntityList = mem.ReadInt32((IntPtr)ClientAdress + Offsets.OffsetList.EntityList + (i - 1) * 0x10);
                            int HisTeam = mem.ReadInt32((IntPtr)EntityList + Offsets.OffsetList.Team);
                            if (!mem.ReadBoolean((IntPtr)EntityList + Offsets.OffsetList.Dormant))
                            {
                                if (MyTeam != HisTeam)
                                {
                                    int GlowIndex = mem.ReadInt32((IntPtr)EntityList + Offsets.OffsetList.GlowIndex);
                                    int GlowObject = mem.ReadInt32((IntPtr)ClientAdress + Offsets.OffsetList.GlowObject);
                                    mem.WriteByte((IntPtr)EntityList + 0x70, (byte)colorsE.r);
                                    mem.WriteByte((IntPtr)EntityList + 0x71, (byte)colorsE.g);
                                    mem.WriteByte((IntPtr)EntityList + 0x72, (byte)colorsE.b);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x4), colorsE.r);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x8), colorsE.g);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0xC), colorsE.b);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x10), colorsE.a);
                                    mem.WriteBoolean((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x24), true);
                                    mem.WriteBoolean((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x2C), true);
                                    //0x26 -> True -----> FULL BEAM
                                    //0x25 -> False -----> Typical Glow
                                    //0x2C -> True ------> Cool Beam
                                }
                                if (MyTeam == HisTeam)
                                {
                                    int GlowIndex = mem.ReadInt32((IntPtr)EntityList + Offsets.OffsetList.GlowIndex);
                                    int GlowObject = mem.ReadInt32((IntPtr)ClientAdress + Offsets.OffsetList.GlowObject);
                                    mem.WriteByte((IntPtr)EntityList + 0x70, (byte)colorsT.r);
                                    mem.WriteByte((IntPtr)EntityList + 0x71, (byte)colorsT.g);
                                    mem.WriteByte((IntPtr)EntityList + 0x72, (byte)colorsT.b);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x4), colorsT.r);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x8), colorsT.g);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0xC), colorsT.b);
                                    mem.WriteFloat((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x10), colorsT.a);
                                    mem.WriteBoolean((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x24), true);
                                    mem.WriteBoolean((IntPtr)GlowObject + (GlowIndex * 0x38 + 0x2C), true);

                                }
                            }
                            i++;
                        } while (i < 65);
                    }
                }
            }
        }

        #endregion

        #region AntiFlash
        public static float flashMaxAlpha
        {
            get
            {
                return mem.ReadFloat((IntPtr)LocalBase + Offsets.OffsetList.FlashMaxAlpha);
            }
            set
            {
                mem.WriteFloat((IntPtr)LocalBase + Offsets.OffsetList.FlashDuration, (float)value);
                mem.WriteFloat((IntPtr)LocalBase + Offsets.OffsetList.FlashMaxAlpha, (float)value);
            }
        }
        public void AntFlashIt()
        {
            while (true)
            {
                if (antiflCHCK.Checked)
                {
                    if (wallCHCK.Checked)
                    { }
                    else
                    {
                        GameState = mem.ReadInt32((IntPtr)EngineBase + Offsets.OffsetList.GameState);
                    }
                    if (GameState == 6)
                    {
                        flashMaxAlpha = 0f;
                        Classes.Global_Functions.Wait(15);
                    }
                    else
                    {
                        flashMaxAlpha = 255f;
                        Classes.Global_Functions.Wait(15);
                    }
                }
                else
                {
                    flashMaxAlpha = 255f;
                    Classes.Global_Functions.Wait(15);
                }
            }
        }
        #endregion

        #region Trigger
        private void TriggerIt()
        {
            while (true)
            {
                if (triggerCHCK.Checked)
                {
                    if (wallCHCK.Checked || antiflCHCK.Checked)
                    { }
                    else
                    {
                        GameState = mem.ReadInt32((IntPtr)EngineBase + Offsets.OffsetList.GameState);
                    }
                    if (GameState == 6)
                    {
                        int WeaponHandle = mem.ReadInt32((IntPtr)LocalBase + 0x2EE8);
                        WeaponHandle &= 0xFFF;
                        int WeaponEntity = mem.ReadInt32((IntPtr)ClientAdress + (WeaponHandle - 1) * 0x10);
                        int IsScoped = mem.ReadInt32((IntPtr)LocalBase + Offsets.OffsetList.Scope);
                        int WeaponIndex = mem.ReadInt32((IntPtr)WeaponEntity + 0x2F9A);
                        if (noScopeCHCK.Checked)
                        {
                            int MyTeam = mem.ReadInt32((IntPtr)LocalBase + Offsets.OffsetList.Team);
                            int PlayerInCross = mem.ReadInt32((IntPtr)LocalBase + Offsets.OffsetList.Cross);
                            if (PlayerInCross > 0 && PlayerInCross < 65)
                            {
                                int PtrToPIC = mem.ReadInt32((IntPtr)ClientAdress + Offsets.OffsetList.EntityList + (PlayerInCross - 1) * 0x10);
                                int PICHealth = mem.ReadInt32((IntPtr)PtrToPIC + Offsets.OffsetList.Health);
                                int PICTeam = mem.ReadInt32((IntPtr)PtrToPIC + Offsets.OffsetList.Team);
                                if ((PICTeam != MyTeam) && (PICTeam > 1) && (PICHealth > 0))
                                {
                                    for (int i = 0; i < 2; i++)
                                    {
                                        mouse_event(2, 0, 0, 0, 1);
                                        Classes.Global_Functions.Wait(15);
                                        mouse_event(4, 0, 0, 0, 1);
                                    }
                                }

                            }
                            Thread.Sleep(10);
                        }
                        else
                        {
                            if (IsScoped != 1)
                            {
                                if (WeaponIndex == 9) //AWP
                                {}
                                else if (WeaponIndex == 11) //T AUTO
                                {}
                                else if (WeaponIndex == 40) //SSG
                                {}
                                else if (WeaponIndex == 38) //CT AUTO
                                {}
                                else
                                {
                                    int MyTeam = mem.ReadInt32((IntPtr)LocalBase + Offsets.OffsetList.Team);
                                    int PlayerInCross = mem.ReadInt32((IntPtr)LocalBase + Offsets.OffsetList.Cross);
                                    if (PlayerInCross > 0 && PlayerInCross < 65)
                                    {
                                        int PtrToPIC = mem.ReadInt32((IntPtr)ClientAdress + Offsets.OffsetList.EntityList + (PlayerInCross - 1) * 0x10);
                                        int PICHealth = mem.ReadInt32((IntPtr)PtrToPIC + Offsets.OffsetList.Health);
                                        int PICTeam = mem.ReadInt32((IntPtr)PtrToPIC + Offsets.OffsetList.Team);
                                        if ((PICTeam != MyTeam) && (PICTeam > 1) && (PICHealth > 0))
                                        {
                                            for (int i = 0; i < 2; i++)
                                            {
                                                mouse_event(2, 0, 0, 0, 1);
                                                Classes.Global_Functions.Wait(15);
                                                mouse_event(4, 0, 0, 0, 1);
                                            }
                                        }
                                    }
                                    Thread.Sleep(10);
                                }
                            }
                            else
                            {
                                int MyTeam = mem.ReadInt32((IntPtr)LocalBase + Offsets.OffsetList.Team);
                                int PlayerInCross = mem.ReadInt32((IntPtr)LocalBase + Offsets.OffsetList.Cross);
                                if (PlayerInCross > 0 && PlayerInCross < 65)
                                {
                                    int PtrToPIC = mem.ReadInt32((IntPtr)ClientAdress + Offsets.OffsetList.EntityList + (PlayerInCross - 1) * 0x10);
                                    int PICHealth = mem.ReadInt32((IntPtr)PtrToPIC + Offsets.OffsetList.Health);
                                    int PICTeam = mem.ReadInt32((IntPtr)PtrToPIC + Offsets.OffsetList.Team);
                                    if ((PICTeam != MyTeam) && (PICTeam > 1) && (PICHealth > 0))
                                    {
                                        for (int i = 0; i < 2; i++)
                                        {
                                            mouse_event(2, 0, 0, 0, 1);
                                            Classes.Global_Functions.Wait(15);
                                            mouse_event(4, 0, 0, 0, 1);
                                        }
                                    }
                                }
                                Thread.Sleep(10);
                            }
                        }

                    };
                }
                }
            }
        #endregion

        #region RadarHack
        public void RadarIt()
        {
            while (true)
            {
                if (radarCHCK.Checked == true)
                {
                    int i = 1;
                    int EntityList = mem.ReadInt32((IntPtr)ClientAdress + Offsets.OffsetList.EntityList + (i - 1) * 0x10);
                    do
                    {
                        mem.WriteBoolean((IntPtr)EntityList + Offsets.OffsetList.SpottedByMask, true);
                    } while (i < 65);
                }
            }
        }
        #endregion

        #endregion
    }
}
