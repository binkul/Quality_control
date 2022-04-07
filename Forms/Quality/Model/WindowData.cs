namespace Quality_Control.Forms.Quality.Model
{
    class WindowData
    {
        public double FormXpos { get; set; }
        public double FormYpos { get; set; }
        public double FormWidth { get; set; }
        public double FormHeight { get; set; }

        public WindowData(double formXpos, double formYpos, double formWidth, double formHeight)
        {
            FormXpos = formXpos;
            FormYpos = formYpos;
            FormWidth = formWidth;
            FormHeight = formHeight;
        }
    }
}
