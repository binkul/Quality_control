using Quality_Control.Commons;
using Quality_Control.Forms.Modification.Model;
using System.Collections.Generic;
using System.Linq;

namespace Quality_Control.Service
{
    public class ModificationService
    {
        private readonly QualityFields _fields = new QualityFields();

        public List<ModificationModel> Fields => _fields.Fields;

        internal bool Modified => Fields.Any(x => x.Modify);

        internal bool IsAnySet => Fields.Any(x => x.Visible);

        internal bool IsAnyUnSet => Fields.Any(x => x.Visible == false);

        internal void CheckFieldsInList(string fieldsList)
        {
            string[] fields = fieldsList.Split('|');
            foreach (string field in fields)
            {
                ModificationModel model = Fields.FirstOrDefault(x => x.DbName.Equals(field));
                if (model != null)
                    model.Visible = true;
            }
        }

        internal void CancelModify()
        {
            foreach (ModificationModel model in Fields)
            {
                model.Modify = false;
            }
        }

        internal void UnsetFields()
        {
            foreach (ModificationModel model in Fields)
            {
                model.Visible = false;
            }
        }

        internal void SetFields()
        {
            foreach (ModificationModel model in Fields)
            {
                model.Visible = true;
            }
        }

        internal void SetStandard()
        {
            CheckFieldsInList(DefaultData.DefaultDataFieldsNoPh);
        }

        internal void SetStandardWithpH()
        {
            CheckFieldsInList(DefaultData.DefaultDataFields);
        }

        internal string RecalculateFields()
        {
            return Fields
                .Where(x => x.Visible)
                .Select(x => x.DbName)
                .Aggregate((x, y) => x + "|" + y);
        }
    }
}
