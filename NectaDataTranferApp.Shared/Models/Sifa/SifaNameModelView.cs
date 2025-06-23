using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NectaDataTransfer.Shared.Models.Sifa
{
    public class SifaNameModelView
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Fname { get; set; }
        public string? Oname { get; set; }

        public string? Sname { get; set; }
        public string? Sex { get; set; }
        public string? Dbirth { get; set; }
        public int ExamType { get; set; }
        public string? ExamName { get; set; }
        public string? ExamNumber { get; set; }
        public int ExamYear { get; set; }
        public string? SifaTable { get; set; }
        public string? EmisDB { get; set; }
        public string? UserName { get; set; }
        public bool Selected { get; set; }
    }
}
