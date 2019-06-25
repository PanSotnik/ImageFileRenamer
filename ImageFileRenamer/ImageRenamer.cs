using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageFileRenamer
{
    class ImageRenamer
    {
        static void Main()
        {
            ImageRenamer imageRenamer = new ImageRenamer();
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Укажите путь, по которому хранятся файлы (пример: C:\\Users\\panso\\Desktop\\New photos)");
            string dir = Console.ReadLine();

            if (!Directory.Exists(dir))
            {
                Console.WriteLine("Указанной папки не существует на диске.");
                Main();
            }

            string[] fileNames = Directory.GetFiles(dir);

            if (fileNames.Length == 0)
            {
                Console.WriteLine("В указанной папке нет содержимого.");
                Main();
            }

            imageRenamer.PerformOp(dir, fileNames);

            Console.WriteLine("Все операции выполнены.");
            Console.ReadLine();
        }

        private void PerformOp(string dir, string[] fileNames)
        {
            List<string> imageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

            foreach (var file in fileNames)
            {
                if (!imageExtensions.Contains(Path.GetExtension(file)?.ToUpperInvariant()))
                {
                    continue;
                }

                string dateTaken = "";
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BitmapSource img = BitmapFrame.Create(fs);
                    BitmapMetadata md = (BitmapMetadata) img.Metadata;

                    if (md == null || string.IsNullOrEmpty(md.DateTaken))
                    {
                        Console.WriteLine($"Файл {file} не переименован. Отсутствуют метаданные.");
                        continue;
                    }

                    dateTaken = md.DateTaken.Replace(":", "_");
                }

                File.Move(file, $"{dir}\\{dateTaken}{Path.GetExtension(file)}");
            }
        }
    }
}
