using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.API.ElisaIntegration.Dtos
{
    public partial class APIResponseDto
    {
        public APIResponseDto() 
        {
            Errors = new List<Error>();
        }

        public bool IsSuccess { get; set; }

        public dynamic Data { get; set; }

        public IList<Error> Errors { get; set; }
        public class Error 
        {
            public string Message { get; set; }
        }
    }
}
