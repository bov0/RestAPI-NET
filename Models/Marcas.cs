using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RestAPI_NET.Models;

public partial class Marcas
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    [JsonIgnore]

    public virtual ICollection<Relojes> Relojes { get; set; } = new List<Relojes>();
}
