using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Localizations
{
    public class Localizer : ILocalizer
    {
        private static readonly IDictionary<string, ResourceManager> _resMgrMap;

        static Localizer()
        {
            _resMgrMap = new Dictionary<string, ResourceManager>();
        }

        /// <summary>
        /// 根据类型获得其对应的DisplayingAttribute名称
        /// </summary>
        /// <param name="typeOfClass"></param>
        /// <returns></returns>
        public ClassField GetLocalField(Type typeOfClass)
        {
            var attrs = typeOfClass.GetCustomAttributes(true).OfType<Attribute>();
            var dispAttr = attrs.OfType<DisplayingAttribute>().FirstOrDefault();
            var dispName = dispAttr == null ? null : GetResource(dispAttr.Name, dispAttr.ResourceType);
            var field = new ClassField
            {
                Name = typeOfClass.Name,
                DisplayName = dispName,
                PropFields = this.GetPropFields(typeOfClass),
                Attributes = attrs
            };

            return field;
        }

        /// <summary>
        /// 根据类型获得其对应的属性列表
        /// </summary>
        /// <param name="typeOfClass"></param>
        /// <returns></returns>
        private IEnumerable<PropField> GetPropFields(Type typeOfClass)
        {
            var fields = new List<PropField>();
            typeOfClass.GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .ToList()
               .ForEach(p =>
               {
                   var attrs = p.GetCustomAttributes(true).OfType<Attribute>();
                   var field = new PropField
                   {
                       Name = p.Name,
                       PropInfo = p,
                       Attributes = attrs
                   };
                   field.Hidden = attrs.OfType<HiddenAttribute>().Any() || attrs.OfType<FlatFormatAttribute>().Any();
                   field.Editable = attrs.OfType<EditableAttribute>().Any(ea => ea.AllowEdit);
                   field.IsKeyId = attrs.OfType<KeyAttribute>().Any();
                   field.IsKeyName = attrs.OfType<KeyNameAttribute>().Any();
                   field.IsRequired = attrs.OfType<RequiredAttribute>().Any();
                   field.DisplayFormat = attrs.OfType<DisplayFormatAttribute>()
                       .Select(dfa => dfa.DataFormatString)
                       .FirstOrDefault();
                   field.DataProperty = attrs.OfType<DataListAttribute>()
                       .Select(dla => dla.DataProperty)
                       .FirstOrDefault();
                   var dispAttr = attrs.OfType<DisplayAttribute>().FirstOrDefault();
                   if (dispAttr != null)
                   {
                       var dispName = dispAttr.ResourceType == null ?
                            dispAttr.Name : GetResource(dispAttr.Name, dispAttr.ResourceType);
                       field.DisplayName = dispName;
                       field.Order = dispAttr.GetOrder();
                       field.Group1Name = dispAttr.GetGroupName();
                   }
                   var flatDispAttr = attrs.OfType<FlatDisplayAttribute>().FirstOrDefault();
                   if (flatDispAttr != null)
                   {
                       var dispName = flatDispAttr.ResourceType == null ?
                            flatDispAttr.Name : GetResource(flatDispAttr.Name, flatDispAttr.ResourceType);
                       field.DisplayName = dispName;
                       field.Order = flatDispAttr.Order;
                       field.Group1Name = flatDispAttr.Group1Name;
                       field.Group2Name = flatDispAttr.Group2Name;
                       field.Group3Name = flatDispAttr.Group3Name;
                   }
                   field.IsFormula = attrs.OfType<FormulaAttribute>().Any();
                   fields.Add(field);
               });

            return fields.OrderBy(lf => lf.Order);
        }



        /// <summary>
        /// 从Resource中获得名称
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        private static string GetResource(string resourceName, Type resourceType)
        {
            if (resourceType == null)
            {
                return resourceName;
            }

            ResourceManager resMgr = null;
            var resFullName = resourceType.FullName;
            if (_resMgrMap.ContainsKey(resFullName))
            {
                resMgr = _resMgrMap[resFullName];
            }
            else
            {
                resMgr = new ResourceManager(resourceType);
                _resMgrMap.Add(resFullName, resMgr);
            }

            return resMgr.GetString(resourceName);
        }

    }
}
