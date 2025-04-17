using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NovaPoshtaParalle.Entities;

[Table("Cities")]
public class City
{
    [Key] 
    public string Ref { get; set; }
    
    [Required] 
    public string Description { get; set; }

    [Required, ForeignKey("AreaObj"), JsonPropertyName("Area")] 
    public string Area { get; set; }

    public Area AreaObj { get; set; }

    public override string ToString()
    {
        return $"City: {Description}, Ref: {Ref}, AreaRef: {Area}, Area: {AreaObj?.Description}";
    }
}
