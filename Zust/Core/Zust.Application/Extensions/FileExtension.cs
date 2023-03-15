using Microsoft.AspNetCore.Http;
using System.Text;

namespace Zust.Application.Extensions
{
    public static class FileExtension
    {

        public static bool CheckType(this IFormFile file, params string[] types)
        {
            foreach (string type in types)
                if(file.ContentType.Contains(type)) return true;
            return false;
        }

        public static bool CheckSize(this IFormFile file, int mb)
            => mb * 1024 * 1024 > file.Length;


        public static string SaveFile(this IFormFile file,string path)
        {
            string filename = file.FileName;
            filename = ChangeName(filename);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            using(FileStream fs = new FileStream(Path.Combine(path,filename), FileMode.Create))
            {
                file.CopyTo(fs);
            }
            return filename;
        }

        static string ChangeName(string filename)
        {
            filename = Guid.NewGuid() + filename.Substring(filename.LastIndexOf('.'));
            return filename;
        }

        public static void Delete(this string filename,string path)
        {
            path = Path.Combine(path,filename);
            if(File.Exists(path))
                File.Delete(path);
        }


        public static string CheckValidate(this IFormFile file, int mb,params string[] types)
        {
            string result = "";
            if (!file.CheckSize(mb)) result += $"{file.FileName} size bigger than {mb} mb";
            StringBuilder typeList = new();
            foreach(string type in types)
            {
                typeList.Append(type);
                typeList.Append("or");
            }
            if (!file.CheckType(types)) result += $"{file.FileName} type not {typeList}";
            return result;
        }


        public static void ChangeDirectoryName(this string usersfolders,string sourceusername,string destinationusername)
        {
            if(sourceusername != destinationusername)
            {
                string sourcepath = Path.Combine(usersfolders, sourceusername);
                string destinationpath = Path.Combine(usersfolders, destinationusername);

                if(Directory.Exists(sourcepath))
                    Directory.Move(sourcepath,destinationpath);
            }
        }


    }
}
