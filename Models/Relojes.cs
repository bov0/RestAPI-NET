using System;
using System.Collections.Generic;

namespace RestAPI_NET.Models;

public partial class Relojes
{
    public int Id { get; set; }

    public string Modelo { get; set; } = null!;

    public decimal Precio { get; set; }

    public byte[]? Imagen { get; set; }

    public string? Imagenblob { get; set; }

    public int? IdMarca { get; set; }

    public virtual Marcas? oMarca { get; set; }
}
