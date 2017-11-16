using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MVCHomeWork.Infrastructure.Helpers {
    public class ListToDatatable {
        public ListToDatatable() { }
        public static DataTable ListToDataTable<T>(List<T> entitys) {

            // 檢查實體集合不能為空
            if (entitys == null || entitys.Count < 1) {
                return new DataTable();
            }

            // 取出第一個實體的所有Propertie 
            Type entityType = entitys[0].GetType();
            
            //列出排除ICollection的屬性物件
            var entityCollectionProperties = entityType.GetProperties().Where(p => (typeof(IEnumerable).IsAssignableFrom(p.PropertyType) && p.PropertyType != typeof(string)) || p.PropertyType.Namespace == entityType.Namespace).Select(p => p.Name).ToList();


            PropertyInfo[] entityProperties = entityType.GetProperties().Where(p => !entityCollectionProperties.Contains(p.Name) ).ToArray();

            // 生成DataTable的structure
            // 生產代碼中，應將生成的DataTable結構Cache起來，此處略 
            DataTable dt = new DataTable(typeof(T).Name);
            for (int i = 0; i < entityProperties.Length; i++) {
                // dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType); 
                dt.Columns.Add(entityProperties[i].Name);
            }

            // 將所有entity添加到DataTable中
            foreach (object entity in entitys) {
                // 檢查所有的的實體都為同一類型
                if (entity.GetType() != entityType) {
                    throw new Exception(" 要轉換的集合元素類型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++) {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);

                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }
    }
}
