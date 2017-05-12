using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public class File
    {
        public static bool saveNewFileSimple( String path , String content ) {
            try
            {
                System.IO.File.WriteAllText( path , content);
                return true;
            }
            catch (Exception err) {
                return false;
            }
            
        }

        public static bool createFolder( String dir)
        {
            try {
                System.IO.DirectoryInfo dirCreated = System.IO.Directory.CreateDirectory(dir);
                return true;
            }
            catch (Exception err) {
                return false;
            }
        }

    }
}
