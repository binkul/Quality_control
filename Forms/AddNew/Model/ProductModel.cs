using Quality_Control.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Quality_Control.Forms.AddNew.Model
{
    public class ProductModel // : INotifyPropertyChanged
    {
        public long Id { get; set; } = -1;
        public long LabBookId { get; set; } = 1;
        public string Name { get; set; } = "";
        public string Index { get; set; } = "";
        public string Description { get; set; }
        public bool IsDanger { get; set; } = false;
        public bool IsArchive { get; set; } = false;
        public bool IsExperiment { get; set; } = false;
        public int PriceId { get; set; } = 1;
        public int TypeId { get; set; } = 1;
        public int GlossId { get; set; } = 1;
        public DateTime Created { get; set; } = DateTime.Today;
        public long LoginId { get; set; } = 1;
        public bool Modified { get; set; } = false;
        private string _activeFields = DefaultData.DefaultDataFields;

        //public event PropertyChangedEventHandler PropertyChanged;

        public ProductModel(long id, long labBookId, string name, string index, string description, bool isDanger, 
            bool isArchive, bool isExperiment, int priceId, int typeId, int glossId, DateTime created, long loginId, string fields)
        {
            Id = id;
            LabBookId = labBookId;
            Name = name;
            Index = index;
            Description = description;
            IsDanger = isDanger;
            IsArchive = isArchive;
            IsExperiment = isExperiment;
            PriceId = priceId;
            TypeId = typeId;
            GlossId = glossId;
            Created = created;
            LoginId = loginId;
            _activeFields = fields;
        }

        public ProductModel(long labBookId, string name, string index, string description, bool isDanger, 
            bool isArchive, bool isExperiment, int priceId, int typeId, int glossId, DateTime created, long loginId, string fields)
        {
            LabBookId = labBookId;
            Name = name;
            Index = index;
            Description = description;
            IsDanger = isDanger;
            IsArchive = isArchive;
            IsExperiment = isExperiment;
            PriceId = priceId;
            TypeId = typeId;
            GlossId = glossId;
            Created = created;
            LoginId = loginId;
            _activeFields = fields;
        }

        public string ActiveFields
        {
            get => _activeFields;
            set
            {
                _activeFields = value;
                Modified = true;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is ProductModel model &&
                   Id == model.Id &&
                   LabBookId == model.LabBookId &&
                   Name == model.Name &&
                   Index == model.Index &&
                   Description == model.Description &&
                   IsDanger == model.IsDanger &&
                   IsArchive == model.IsArchive &&
                   IsExperiment == model.IsExperiment &&
                   PriceId == model.PriceId &&
                   TypeId == model.TypeId &&
                   GlossId == model.GlossId &&
                   Created == model.Created &&
                   LoginId == model.LoginId;
        }

        public override int GetHashCode()
        {
            int hashCode = -735400064;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + LabBookId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Index);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + IsDanger.GetHashCode();
            hashCode = hashCode * -1521134295 + IsArchive.GetHashCode();
            hashCode = hashCode * -1521134295 + IsExperiment.GetHashCode();
            hashCode = hashCode * -1521134295 + PriceId.GetHashCode();
            hashCode = hashCode * -1521134295 + TypeId.GetHashCode();
            hashCode = hashCode * -1521134295 + GlossId.GetHashCode();
            hashCode = hashCode * -1521134295 + Created.GetHashCode();
            hashCode = hashCode * -1521134295 + LoginId.GetHashCode();
            return hashCode;
        }
    }
}
