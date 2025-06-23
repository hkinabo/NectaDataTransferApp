using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NectaDataTransfer.Shared.Models
{
    public class RegStatusModel
    {
        public int ClassId { get; set; }
        public string Comments { get; set; }
        public int RegStatus { get; set; } = 0;
        public string Username { get; set; }
    }
}
