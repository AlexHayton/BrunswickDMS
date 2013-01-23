using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityFunctions
{
    public class MimeTypes
    {
        // MIME Type detection code from http://forums.asp.net/t/1041821.aspx/1
        [System.Runtime.InteropServices.DllImport("urlmon.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
        static extern int FindMimeFromData(IntPtr pBC,
        [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string pwzUrl,
        [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPArray, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1, SizeParamIndex = 3)] 
        byte[] pBuffer, int cbSize,
        [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string pwzMimeProposed, int dwMimeFlags, out IntPtr ppwzMimeOut, int dwReserved);

        /// <summary>
        /// Ensures that file exists and retrieves the content type 
        /// </summary>
        /// <param name="strfileName"></param>
        /// <returns>Returns the Content MimeType </returns>
        public static string GetMimeFromFile(string strFileName)
        {
            IntPtr mimeout;
            if (!System.IO.File.Exists(strFileName))
                throw new System.IO.FileNotFoundException(strFileName + " not found");

            int MaxContent = (int)new System.IO.FileInfo(strFileName).Length;
            if (MaxContent > 4096) MaxContent = 4096;
            System.IO.FileStream fs = System.IO.File.OpenRead(strFileName);


            byte[] buf = new byte[MaxContent];
            fs.Read(buf, 0, MaxContent);
            fs.Close();
            int result = FindMimeFromData(IntPtr.Zero, strFileName, buf, MaxContent, null, 0, out mimeout, 0);

            if (result != 0)
                throw System.Runtime.InteropServices.Marshal.GetExceptionForHR(result);

            string mime = System.Runtime.InteropServices.Marshal.PtrToStringUni(mimeout);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(mimeout);
            return mime;
        }
    }
}
