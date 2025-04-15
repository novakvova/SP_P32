using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaPoshtaParallelFor.Entity
{
    public class NewPostBaseEntity
    {
        [Key]
        [StringLength(36)]
        public string Ref { get; set; }

        [StringLength(128)]
        public string Description { get; set; }
    }
}
