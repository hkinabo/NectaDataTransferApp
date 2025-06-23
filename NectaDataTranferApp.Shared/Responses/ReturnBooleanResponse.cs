using NectaDataTransfer.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NectaDataTranferApp.Shared.Responses
{
    public class ReturnBooleanResponse : BaseResponse
    {
        public bool BooleanResponse { get; set; }
    }
}
