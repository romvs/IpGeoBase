using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IpGeoBase.Loader
{
    /// <summary>
    /// Небольшой хелпер по чтению данных
    /// </summary>
    public class DataReader: IDisposable
    {
        StreamReader _reader = null;
        string _buffer = null;

        public DataReader(Stream stream)
        {
            _reader = new StreamReader(stream, Encoding.GetEncoding(1251));
        }

        /// <summary>
        /// Имеются ли еще данные в источнике
        /// </summary>
        /// <returns></returns>
        public bool HasMore()
        {
            _buffer = _reader.ReadLine();
            return !string.IsNullOrEmpty(_buffer);
        }

        /// <summary>
        /// Возвращяет следующий кортеж данных
        /// </summary>
        /// <returns></returns>
        public string[] Next()
        {
            return _buffer.Split('\t');
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DataReader()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && _reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }
        }
    }
}
