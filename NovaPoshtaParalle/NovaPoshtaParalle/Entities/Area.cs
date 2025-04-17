using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NovaPoshtaParalle.Entities;

[Table("tblAreas")]
public class Area
{
    [Key]
    //ідентифікатор області в Новій пошті
    public string Ref { get; set; }
    public string Description { get; set; }
}
