namespace Quality_Control.Forms.Navigation
{
    internal interface INavigation
    {
        int DgRowIndex { get; set; }

        int GetRowCount { get; }

        void Refresh();
    }
}
