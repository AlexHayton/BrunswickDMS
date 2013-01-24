using System;
using System.Collections.Generic;
using System.IO;
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
        public static string GetMimeFromFile(string fileName, string filePath)
        {
            // Handle Office 2007 documents specially.
            // The auto-detection code treats them as regular ZIP files!
            string extension = Path.GetExtension(fileName);
            string mimeType = string.Empty;

            switch (extension)
            {
                // Word 2007
                case ".docx":
                    mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;

                case ".docm":
                    mimeType = "application/vnd.ms-word.document.macroEnabled.12";
                    break;

                case ".dotx":
                    mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                    break;

                case ".dotm":
                    mimeType = "application/vnd.ms-word.template.macroEnabled.12";
                    break;

                // Excel 2007
                case ".xlsx":
                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;

                case ".xltx":
                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.template";
                    break;

                case ".xlsm":
                    mimeType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                    break;

                case ".xltm":
                    mimeType = "application/vnd.ms-excel.template.macroEnabled.12";
                    break;

                case ".xlam":
                    mimeType = "application/vnd.ms-excel.addin.macroEnabled.12";
                    break;

                case ".xlsb":
                    mimeType = "application/vnd.ms-excel.sheet.binary.macroEnabled.12";
                    break;

                // Powerpoint
                case ".pptx":
                    mimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;

                case ".potx":
                    mimeType = "application/vnd.openxmlformats-officedocument.presentationml.template";
                    break;

                case ".ppsx":
                    mimeType = "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
                    break;

                case ".ppam":
                    mimeType = "application/vnd.ms-powerpoint.addin.macroEnabled.12";
                    break;

                case ".pptm":
                case ".potm":
                    mimeType = "application/vnd.ms-powerpoint.presentation.macroEnabled.12";
                    break;

                case ".ppsm":
                    mimeType = "application/vnd.ms-powerpoint.slideshow.macroEnabled.12";
                    break;

                default:
                    mimeType = GetMimeFromFileRaw(filePath);
                    break;
            }

            return mimeType;
        }

        /// <summary>
        /// This handles cases where we can't determine the MIME Type by extension
        /// </summary>
        /// <param name="strFileName">The filename</param>
        /// <returns>The detected MIME type</returns>
        private static string GetMimeFromFileRaw(string strFileName)
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
