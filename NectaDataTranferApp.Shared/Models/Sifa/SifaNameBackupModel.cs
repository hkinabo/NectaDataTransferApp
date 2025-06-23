using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NectaDataTransfer.Shared.Models.Sifa
{
    public class SifaNameBackupModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ExamName { get; set; }
        public string ExamNumber { get; set; }
        public int ExamYear { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
