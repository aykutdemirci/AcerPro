using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace AcerPro.Models
{
    public class DataTableAjaxViewModel<T> where T : class
    {
        public int Draw { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public string OrderColumnName { get { return Columns[Order[0].Column].Name; } }

        public Search Search { get; set; }

        public Column[] Columns { get; set; }

        public Order[] Order { get; set; }

        public IQueryable<T> DataTableQuery { get; set; }

        public DataTableResponse<T> GetDataTableResponse()
        {
            if (!(string.IsNullOrEmpty(this.OrderColumnName) && string.IsNullOrEmpty(this.Order[0].Dir)))
            {
                DataTableQuery = DataTableQuery.OrderBy($"{this.OrderColumnName} {this.Order[0].Dir}");
            }

            return new DataTableResponse<T>
            {
                Draw = this.Draw,
                RecordsTotal = DataTableQuery.Count(),
                RecordsFiltered = DataTableQuery.Count(),
                Data = DataTableQuery.Skip(this.Start).Take(this.Length).ToList()
            };
        }

        public JsonResult DataTableResult()
        {
            var response = this.GetDataTableResponse();

            return new JsonResult
            {
                Data = new { response.Draw, response.RecordsFiltered, response.RecordsTotal, response.Data }
            };
        }
    }

    public class Column
    {
        public string Data { get; set; }

        public string Name { get; set; }

        public bool Searchable { get; set; }

        public bool Orderable { get; set; }

        public Search Search { get; set; }
    }

    public class Search
    {
        public string Value { get; set; }

        public string Regex { get; set; }
    }

    public class Order
    {
        public string Dir { get; set; }

        public int Column { get; set; }
    }

    public class DataTableResponse<T>
    {
        public int Draw { get; set; }

        public int RecordsTotal { get; set; }

        public int RecordsFiltered { get; set; }

        public List<T> Data { get; set; }
    }
}