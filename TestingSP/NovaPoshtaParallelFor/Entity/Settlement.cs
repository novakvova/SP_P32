using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaPoshtaParallelFor.Entity
{
    [Table("tbl_Settlements")]
    public class Settlement : NewPostBaseEntity
    {
        [StringLength(100)]
        public string SettlementTypeDescription { get; set; } = string.Empty;

        [StringLength(36)]
        public string? Region { get; set; }
        public int Warehouse { get; set; }
        public Region? SettlementRegion { get; set; }
    }
}
