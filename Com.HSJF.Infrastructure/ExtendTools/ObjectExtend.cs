using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace Com.HSJF.Infrastructure.ExtendTools
{
    public static class ObjectExtend
    {
        /// <summary>
        /// 将指定对象转为Json
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToJson(this object o)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;

            return Newtonsoft.Json.JsonConvert.SerializeObject(o);
        }

        /// <summary>
        /// 从当前对象中向目标对象复制同名且类型相同的public属性
        /// 即，仅复制两者共有的属性
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void CopyTo(this object from, object to)
        {
            if (from == null || to == null)
            {
                return;
            }
            var tFrom = from.GetType();
            var tTo = to.GetType();

            var propFrom = tFrom.GetProperties();
            foreach (var pFrom in propFrom)
            {
                if (!pFrom.CanRead) continue;
                var pTo = tTo.GetProperty(pFrom.Name, pFrom.PropertyType);
                if (pTo == null || !pTo.CanWrite) continue;
                pTo.SetValue(to, pFrom.GetValue(from));
            }
        }

        /// <summary>
        /// 从当前对象中向目标对象复制指定名称且类型相同的public属性
        /// 如指定的名称不存在或无法读/写，将抛出异常
        /// 即，仅复制两者共有且出现在properties中的属性
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="properties"></param>
        public static void CopyTo(this object from, object to, ISet<string> properties)
        {
            var tFrom = from.GetType();
            var tTo = to.GetType();

            foreach (var propName in properties)
            {
                var pFrom = tFrom.GetProperty(propName);
                if (pFrom == null || !pFrom.CanRead)
                {
                    throw new Exception("属性{0}在源对象中未发现或不可读。".DoFormat(propName));
                };
                var pTo = tTo.GetProperty(propName, pFrom.PropertyType);
                if (pTo == null || !pTo.CanWrite)
                {
                    throw new Exception("属性{0}在源对象中未发现或不可写。".DoFormat(propName));
                }
                pTo.SetValue(to, pFrom.GetValue(from));
            }
        }

        /// <summary>
        /// 从当前对象中向目标对象复制指定类型所拥有且类型相同的public属性
        /// 即仅复制三者共有的属性。典型场景：from和to继承同一个类，又各自定义了同名同类型属性，但不希望拷贝这些属性。
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="type"></param>
        public static void CopyTo(this object from, object to, Type type)
        {
            var tFrom = from.GetType();
            var tTo = to.GetType();

            var propFrom = type.GetProperties();
            foreach (var pFrom in propFrom)
            {
                if (!pFrom.CanRead) continue;
                var pTo = tTo.GetProperty(pFrom.Name, pFrom.PropertyType);
                if (pTo == null || !pTo.CanWrite) continue;
                pTo.SetValue(to, pFrom.GetValue(from));
            }
        }

        /// <summary>
        /// 从当前对象中向目标对象复制同名且类型相同的public属性，但不包含指定属性
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="excludes"></param>
        public static void CopyExclude(this object from, object to, IEnumerable<string> excludes)
        {
            var tFrom = from.GetType();
            var tTo = to.GetType();

            var propFrom = tFrom.GetProperties();
            foreach (var pFrom in propFrom)
            {
                if (excludes.Contains(pFrom.Name)) continue;
                if (!pFrom.CanRead) continue;
                var pTo = tTo.GetProperty(pFrom.Name, pFrom.PropertyType);
                if (pTo == null || !pTo.CanWrite) continue;
                pTo.SetValue(to, pFrom.GetValue(from));
            }
        }

        /// <summary>
        /// 从当前对象中向目标对象复制指定类型所拥有且类型相同的public属性，但不包含指定属性
        /// 即仅复制三者共有的属性。典型场景：from和to继承同一个类，又各自定义了同名同类型属性，但不希望拷贝这些属性。
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="type"></param>
        /// <param name="excludes"></param>
        public static void CopyExclude(this object from, object to, Type type, ISet<string> excludes)
        {
            var tFrom = from.GetType();
            var tTo = to.GetType();

            var propFrom = type.GetProperties();
            foreach (var pFrom in propFrom)
            {
                if (excludes.Contains(pFrom.Name)) continue;
                if (!pFrom.CanRead) continue;
                var pTo = tTo.GetProperty(pFrom.Name, pFrom.PropertyType);
                if (pTo == null || !pTo.CanWrite) continue;
                pTo.SetValue(to, pFrom.GetValue(from));
            }
        }
    }
}