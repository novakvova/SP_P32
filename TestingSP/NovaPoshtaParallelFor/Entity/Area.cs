using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaPoshtaParallelFor.Entity;

[Table("tbl_Areas")]
public class Area : NewPostBaseEntity
{
    [StringLength(50)]
    public string RegionType { get; set; }
    public ICollection<Region> Regions { get; set; } = new HashSet<Region>();
}

