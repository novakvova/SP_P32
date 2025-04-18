using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaPoshtaParalle.Entities
{
    [Table("tblDepartments")]
    public class DepartmentEntity
    {
        [Key]
        public string Ref { get; set; } = string.Empty;
        [ForeignKey(nameof(City))]
        public string CityRef { get; set; } = string.Empty;
        public City City { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ShortAddress { get; set; } = string.Empty;
    }
}
