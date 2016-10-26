using Com.HSJF.Infrastructure.DoMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Localizations
{
    public class LocalField : ObjectModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }

    public class ClassField : LocalField
    {
        public ClassField()
        {
            this.PropFields = new HashSet<PropField>();
        }

        public IEnumerable<PropField> PropFields { get; set; }
        public IEnumerable<Attribute> Attributes { get; set; }
    }

    public class PropField : LocalField
    {
        public int? Order { get; set; }
        public string Group1Name { get; set; }
        public string Group2Name { get; set; }
        public string Group3Name { get; set; }
        public bool Hidden { get; set; }
        public bool Editable { get; set; }
        public bool IsKeyId { get; set; }
        public bool IsKeyName { get; set; }
        public bool IsRequired { get; set; }
        public string DisplayFormat { get; set; }
        public string DataProperty { get; set; }
        public bool IsFormula { get; set; }
        public PropertyInfo PropInfo { get; set; }
        public IEnumerable<Attribute> Attributes { get; set; }
    }

    public class FlatPropField : ObjectModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int? Order { get; set; }
        public string GroupName { get; set; }
        public bool Hidden { get; set; }
        public bool Editable { get; set; }
        public bool IsKeyId { get; set; }
        public bool IsKeyName { get; set; }
        public bool IsRequired { get; set; }
        public string DisplayFormat { get; set; }
        public string DataProperty { get; set; }

        public static explicit operator FlatPropField(PropField propField)
        {
            FlatPropField fpf = null;
            if (propField != null)
            {
                fpf = new FlatPropField
                {
                    Name = propField.Name,
                    DisplayName = propField.DisplayName,
                    Order = propField.Order,
                    GroupName = propField.Group1Name,
                    Hidden = propField.Hidden,
                    Editable = propField.Editable,
                    IsKeyId = propField.IsKeyId,
                    IsKeyName = propField.IsKeyName,
                    IsRequired = propField.IsRequired,
                    DisplayFormat = propField.DisplayFormat,
                    DataProperty = propField.DataProperty
                };
            }

            return fpf;
        }
    }
}
