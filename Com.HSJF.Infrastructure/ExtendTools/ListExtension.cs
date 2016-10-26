using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.Collections;

namespace Com.HSJF.Infrastructure.ExtendTools
{
	public static class ListExtension
	{
		/// <summary>
		/// 将IList转化为DataTable
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static DataTable ToDataTable<T>(this IList<T> list)
		{
			DataTable result = new DataTable();
			if (list.Count > 0)
			{
				FieldInfo[] fileinfos = list[0].GetType().GetFields();
				foreach (FieldInfo field in fileinfos)
				{
					if (field.FieldType == typeof(string) || field.FieldType == typeof(int) || field.FieldType == typeof(decimal) || field.FieldType == typeof(DateTime))
					{
						DataColumn col = new DataColumn(field.Name, field.FieldType);
						result.Columns.Add(col);
					}
					if (field.FieldType.IsEnum)
					{
						DataColumn col = new DataColumn(field.Name, typeof(string));
						result.Columns.Add(col);
					}
				}

				PropertyInfo[] propertys = list[0].GetType().GetProperties();
				foreach (PropertyInfo pi in propertys)
				{
					if (pi.PropertyType == typeof(string) || pi.PropertyType == typeof(int) || pi.PropertyType == typeof(decimal) || pi.PropertyType == typeof(DateTime))
					{
						result.Columns.Add(pi.Name, pi.PropertyType);
					}
					if (pi.PropertyType.IsEnum)
					{
						DataColumn col = new DataColumn(pi.Name, typeof(string));
						result.Columns.Add(col);
					}
				}
				for (int i = 0; i < list.Count; i++)
				{
					ArrayList tempList = new ArrayList();
					foreach (PropertyInfo pi in propertys)
					{
						if (pi.PropertyType == typeof(string) || pi.PropertyType == typeof(int) || pi.PropertyType == typeof(decimal) || pi.PropertyType == typeof(DateTime) || pi.PropertyType.IsEnum)
						{
							object obj = pi.GetValue(list[i], null);
							tempList.Add(obj);
						}
					}
					foreach (FieldInfo field in fileinfos)
					{
						if (field.FieldType == typeof(string) || field.FieldType == typeof(int) || field.FieldType == typeof(decimal) || field.FieldType == typeof(DateTime) || field.FieldType.IsEnum)
						{
							object obj = field.GetValue(list[i]);
							tempList.Add(obj);
						}
					}
					object[] array = tempList.ToArray();
					result.LoadDataRow(array, false);
				}
			}
			return result;
		}
	}
}
