using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Ruby_CSGO.Classes;

namespace Ruby_CSGO.Memory
{
    public class SigScan
    {
        #region Defines
        private static IntPtr m_pProcessHandle;

        private static int m_iNumberOfBytesRead;
        private static int m_iNumberOfBytesWritten;

        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_VM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;
        #endregion

        #region LOAD
        public static void Initialize(int ProcessID) => m_pProcessHandle = Global_Bois.Imports.OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, false, ProcessID);
        #endregion

        #region RWM
        public static byte[] ReadMemory(int offset, int size)
        {
            byte[] buffer = new byte[size];

            Global_Bois.Imports.ReadProcessMemory((int)m_pProcessHandle, offset, buffer, size, ref m_iNumberOfBytesRead);

            return buffer;
        }
        #endregion

        #region SigScanner

        public static int ScanPattern(int dll, string pattern, int extra, int offset, bool modeSubtract)
        {
            int tempOffset = BitConverter.ToInt32(ReadMemory(AobScan(dll, 0x1800000, pattern, 0) + extra, 4), 0) + offset;

            if (modeSubtract) tempOffset -= dll;

            return tempOffset;
        }

        private static int AobScan(int dll, int range, string signature, int instance)
        {
            if (signature == string.Empty) return -1;

            string tempSignature = Regex.Replace(signature.Replace("??", "3F"), "[^a-fA-F0-9]", "");

            int count = -1;

            byte[] searchRange = new byte[(tempSignature.Length / 2)];

            for (int i = 0; i <= searchRange.Length - 1; i++) searchRange[i] = byte.Parse(tempSignature.Substring(i * 2, 2), NumberStyles.HexNumber);

            byte[] readMemory = ReadMemory(dll, range);

            int temp1 = 0;
            int iEnd = searchRange.Length < 0x20 ? searchRange.Length : 0x20;

            for (int j = 0; j <= iEnd - 1; j++)
            {
                if ((searchRange[j] == 0x3f)) temp1 = (temp1 | (Convert.ToInt32(1) << ((iEnd - j) - 1)));
            }

            int[] sBytes = new int[0x100];

            if ((temp1 != 0))
            {
                for (int k = 0; k <= sBytes.Length - 1; k++) sBytes[k] = temp1;
            }

            temp1 = 1;

            int index = (iEnd - 1);

            while ((index >= 0))
            {
                sBytes[searchRange[index]] = (sBytes[searchRange[index]] | temp1);

                index -= 1;

                temp1 = (temp1 << 1);
            }

            int temp2 = 0;

            while ((temp2 <= (readMemory.Length - searchRange.Length)))
            {
                int last = searchRange.Length;

                temp1 = (searchRange.Length - 1);

                int temp3 = -1;

                while ((temp3 != 0))
                {
                    temp3 = (temp3 & sBytes[readMemory[temp2 + temp1]]);

                    if ((temp3 != 0))
                    {
                        if ((temp1 == 0))
                        {
                            count += 1;

                            if (count == instance) return dll + temp2;

                            temp2 += 2;
                        }

                        last = temp1;
                    }

                    temp1 -= 1;

                    temp3 = (temp3 << 1);
                }

                temp2 += last;
            }

            return -1;
        }

        #endregion

    }
}
