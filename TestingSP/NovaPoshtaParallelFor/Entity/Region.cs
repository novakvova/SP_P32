using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace NovaPoshtaParallelFor.Entity
{
    [Table("tbl_Regions")]
    public class Region : NewPostBaseEntity
    {
        [StringLength(50)]
        public string RegionType { get; set; }

        [StringLength(36)]
        public string AreaRef { get; set; }

        public string? AreasCenter { get; set; }

        [JsonIgnore]
        public Area Area { get; set; }

        public ICollection<Settlement> Settlements = new HashSet<Settlement>();
    }
}
