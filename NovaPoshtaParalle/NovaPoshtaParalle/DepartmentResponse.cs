using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NovaPoshtaParalle.Entities;

namespace NovaPoshtaParalle
{
    public class DepartmentResponse
    {
        public bool Success { get; set; }
        public List<DepartmentEntity> Data { get; set; }
        public List<object>? Errors { get; set; }
        public List<object>? Warnings { get; set; }
        [JsonPropertyName("info")]
        public NovaPoshtaInfoDepartment Info { get; set; }
        public List<object>? MessageCodes { get; set; }
        public List<object>? ErrorCodes { get; set; }
        public List<object>? WarningCodes { get; set; }
        public List<object>? InfoCodes { get; set; }
    }

    public class NovaPoshtaInfoDepartment
    {
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }
    }
}
