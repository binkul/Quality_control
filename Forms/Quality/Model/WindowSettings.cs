using System;
using System.Collections.Generic;
using Quality_Control.Forms.Tools;

namespace Quality_Control.Forms.Quality.Model
{
    internal class WindowSettings
    {
        private static readonly string _path = @"\Data\Forms\QualityForm.xml";
        private static readonly WindowData defaultData = new WindowData(50d, 50d, 900d, 600d);

        public static WindowData Read()
        {
            IList<double> list = WindowsOperation.LoadWindowSettings(_path);
            try
            {
                return list != null ? new WindowData(list[0], list[1], list[2], list[3]) : defaultData;
            }
            catch (Exception)
            {
                return defaultData;
            }
        }

        public static void Save(WindowData data)
        {
            IList<double> list = new List<double>
            {
                data.FormXpos,
                data.FormYpos,
                data.FormWidth,
                data.FormHeight,
            };
            WindowsOperation.SaveWindowsSettings(list, _path);
        }
    }
}
