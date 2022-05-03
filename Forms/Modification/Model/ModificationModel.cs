using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Quality_Control.Forms.Modification.Model
{
    public class ModificationModel : INotifyPropertyChanged, IComparable<ModificationModel>
    {
        private bool _visible;
        public event PropertyChangedEventHandler PropertyChanged;
        public int Number { get; set; }
        public string Name { get; set; }
        public string DbName { get; set; }
        public bool Modify { get; set; }

        public ModificationModel(int number, string name, string dbName)
        {
            Number = number;
            Name = name;
            DbName = dbName;
            Modify = false;
        }

        protected void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public bool Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                Modify = true;
                OnPropertyChanged(nameof(Visible));
            }
        }

        public int CompareTo(ModificationModel other)
        {
            return Number.CompareTo(other.Number);
        }

        public override bool Equals(object obj)
        {
            return obj is ModificationModel model &&
                   _visible == model._visible &&
                   Number == model.Number &&
                   Name == model.Name &&
                   DbName == model.DbName;
        }

        public override int GetHashCode()
        {
            int hashCode = 609588734;
            hashCode = hashCode * -1521134295 + _visible.GetHashCode();
            hashCode = hashCode * -1521134295 + Number.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DbName);
            return hashCode;
        }
    }
}
