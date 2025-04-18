using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NovaPoshtaParalle.Entities;

[Table("Cities")]
public class City
{
    [Key]
    public string Ref { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SettlementTypeDescription { get; set; } = string.Empty;
    [ForeignKey("AreaObj")]
    public string Area { get; set; } = string.Empty;
    public Area AreaObj { get; set; }

}
