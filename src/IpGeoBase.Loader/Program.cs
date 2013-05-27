using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Ionic.Zip;
using IpGeoBase.Services;
using IpGeoBase.Domain;
using System.Globalization;
using System.Threading;
using System.Collections.Concurrent;

namespace IpGeoBase.Loader
{
    class Program
    {
        static void Main(string[] args)
        {
            // Настраиваем культуру
            CultureInfo culture = new CultureInfo(Properties.Settings.Default.DeafultCulture);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Генерируем имя файла для архива
            string filename = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".zip");
         
            // Обрабатываем его
            DownloadArchive(filename);
            ProceedArchive(filename);
            RemoveArchive(filename);
        }

        /// <summary>
        /// Загружает архив с сервера в локальную копию
        /// </summary>
        /// <param name="filename"></param>
        static void DownloadArchive(string filename)
        {
            Console.Write("Загружаем архив... ");

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(Properties.Settings.Default.IpGeoBaseUrl, filename);
            }

            Console.WriteLine("Архив успешно загружен.");
        }

        /// <summary>
        /// Обрабатываем архив
        /// </summary>
        /// <param name="filename"></param>
        static void ProceedArchive(string filename)
        {
            using (ZipFile archive = ZipFile.Read(filename))
            {
                using(Stream cityStream = new MemoryStream())
                {
                    archive["cities.txt"].Extract(cityStream);
                    cityStream.Seek(0, SeekOrigin.Begin);
                    ProceedCities(cityStream);
                }

                using (Stream rangeStream = new MemoryStream())
                {
                    archive["cidr_optim.txt"].Extract(rangeStream);
                    rangeStream.Seek(0, SeekOrigin.Begin);
                    ProceedRanges(rangeStream);
                }
            }
        }

        /// <summary>
        /// Обрабатываем базу городов
        /// </summary>
        /// <param name="cityStream"></param>
        static void ProceedCities(Stream cityStream)
        {
            Console.Write("Загружаем локации... ");

            LoadService service = new LoadService();
            string[] data;
            Int32 id;
            string locationName, regionName, areaName;
            Decimal latitlude, longitude;

            using (DataReader reader = new DataReader(cityStream))
            {
                while (reader.HasMore())
                {
                    data = reader.Next();
                    locationName = data[1];
                    regionName = data[2];
                    areaName = data[3];

                    if (!Int32.TryParse(data[0], out id))
                    {
                        Console.WriteLine("Ошибка при чтении локаций: неверный формат идентификатора локации");
                        Environment.Exit(1);
                    }

                    if (!Decimal.TryParse(data[4].Replace('.', ','), out latitlude))
                    {
                        Console.WriteLine("Ошибка при чтении локаций: неверный формат широты центра локации");
                        Environment.Exit(1);
                    }

                    if (!Decimal.TryParse(data[5].Replace('.', ','), out longitude))
                    {
                        Console.WriteLine("Ошибка при чтении локаций: неверный формат долготы центра локации");
                        Environment.Exit(1);
                    }

                    Area area = service.GetOrCreateArea(areaName);
                    Region region = service.GetOrCreateRegion(regionName, area);
                    service.UpdateOrCreateLocation(id, locationName, latitlude, longitude, region);
                }
            }

            Console.WriteLine("Локации успешно загружены.");
        }

        /// <summary>
        /// Обрабатываем базу диапазонов ip-адресов
        /// </summary>
        /// <param name="cityStream"></param>
        static void ProceedRanges(Stream rangeStream)
        {
            Console.Write("Загружаем диапазоны ip-адресов... ");

            using (DataReader reader = new DataReader(rangeStream))
            {
                using (BlockingCollection<string[]> bc = new BlockingCollection<string[]>())
                {
                    using (Task loadTask = Task.Factory.StartNew(() =>
                        {
                            while (reader.HasMore())
                            {
                                bc.Add(reader.Next());
                            }

                            bc.CompleteAdding();
                        }))
                    {
                        Action storeAction = () =>
                        {
                            LoadService service = new LoadService();
                            string[] data;
                            long start, end;
                            string description, country;
                            Location location = null;
                            int locationId;

                            while (bc.TryTake(out data))
                            {
                                description = data[2];
                                country = data[3];

                                if (!long.TryParse(data[0], out start))
                                {
                                    Console.WriteLine("Ошибка при чтении диапазонов ip-фдресов: неверный формат начала диапазона");
                                    Environment.Exit(1);
                                }

                                if (!long.TryParse(data[1], out end))
                                {
                                    Console.WriteLine("Ошибка при чтении диапазонов ip-фдресов: неверный формат конца диапазона");
                                    Environment.Exit(1);
                                }

                                location = (data[4] != "-") && Int32.TryParse(data[4], out locationId) ? service.GetLocation(locationId) : null;
                                service.UpdateOrCreateRange(start, end, description, country, location);
                            }
                        };

                        Parallel.Invoke(storeAction, storeAction, storeAction);
                    }
                }
            }

            //string[] data;
            //long start, end;
            //string description, country;
            //Location location = null;
            //int locationId;

            //while (reader.HasMore())
            //{
            //    data = reader.Next();
            //    description = data[2];
            //    country = data[3];

            //    if (!long.TryParse(data[0], out start))
            //    {
            //        Console.WriteLine("Ошибка при чтении диапазонов ip-фдресов: неверный формат начала диапазона");
            //        Environment.Exit(1);
            //    }

            //    if (!long.TryParse(data[1], out end))
            //    {
            //        Console.WriteLine("Ошибка при чтении диапазонов ip-фдресов: неверный формат конца диапазона");
            //        Environment.Exit(1);
            //    }

            //    location = (data[4] != "-") && Int32.TryParse(data[4], out locationId) ? service.GetLocation(locationId) : null;
            //    service.UpdateOrCreateRange(start, end, description, country, location);
            //}

            Console.WriteLine("Диапазоны ip-адресов успешно загружены.");
        }

        /// <summary>
        /// Удаляет локальную копию архива
        /// </summary>
        /// <param name="filename"></param>
        static void RemoveArchive(string filename)
        {
            Console.Write("Удаляем архив... ");

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            Console.WriteLine("Архив успешно удален.");
        }
    }
}
