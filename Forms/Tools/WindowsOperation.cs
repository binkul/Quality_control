using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Xml.Linq;

namespace Quality_Control.Forms.Tools
{
    internal static class WindowsOperation
    {
        public static void SaveWindowsSettings(IList<double> data, string file)
        {
            XDeclaration declaration = new XDeclaration("1.0", "utf-8", "yes");
            XComment comment = new XComment("Window position, size and DataGrid columns width");
            XElement parameters = new XElement("parameters");
            XElement window = new XElement("window");
            XElement dataGrid = new XElement("datagrid");
            XElement position = new XElement("position");
            XElement size = new XElement("size");
            XElement x = new XElement("x", data[0]);
            XElement y = new XElement("y", data[1]);
            XElement width = new XElement("width", data[2]);
            XElement height = new XElement("height", data[3]);

            position.Add(x);
            position.Add(y);
            size.Add(width);
            size.Add(height);
            window.Add(position);
            window.Add(size);

            if (data.Count > 4)
            {
                int col = 0;
                for (int i = 4; i < data.Count; i++)
                {
                    XElement columnWidth = new XElement("W" + col.ToString(), data[i]);
                    dataGrid.Add(columnWidth);
                    col++;
                }
                window.Add(dataGrid);
            }

            parameters.Add(window);

            XDocument xml = new XDocument { Declaration = declaration };
            xml.Add(comment);
            xml.Add(parameters);

            string path = Directory.GetCurrentDirectory() + file;
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                _ = Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            xml.Save(path);
        }

        public static IList<double> LoadWindowSettings(string file)
        {
            string path = Directory.GetCurrentDirectory() + file;
            IList<double> result = new List<double>();

            if (!File.Exists(path))
                return null;
            try
            {
                XDocument xml = XDocument.Load(path);

                XElement position = xml.Root.Element("window").Element("position");
                result.Add(double.Parse(position.Element("x").Value, CultureInfo.InvariantCulture));
                result.Add(double.Parse(position.Element("y").Value, CultureInfo.InvariantCulture));

                XElement size = xml.Root.Element("window").Element("size");
                result.Add(double.Parse(size.Element("width").Value, CultureInfo.InvariantCulture));
                result.Add(double.Parse(size.Element("height").Value, CultureInfo.InvariantCulture));

                return LoadGridData(result, xml);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Błąd w czasie pobierania formularza danych z pliku xlm: '" + ex.Message + "'", "Błąd odczytu",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }
        }

        private static IList<double> LoadGridData(IList<double> list, XDocument xml)
        {
            XElement datagrid = xml.Root.Element("window").Element("datagrid");

            if (list != null && list.Count != 0 && datagrid != null)
            {
                int col = 0;
                for (int i = 0; i < 10000; i++)
                {
                    XElement read = datagrid.Element("W" + col.ToString());
                    if (read != null)
                    {
                        list.Add(double.Parse(datagrid.Element("W" + col.ToString()).Value, CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        return list;
                    }
                    col++;
                }
            }
            return list;
        }
    }
}
