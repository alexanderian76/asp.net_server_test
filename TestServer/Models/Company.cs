using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

public class Company
{
    public int Id { get; set; }
    [Column("MyName")]
    public string Name { get; set; }

    
    public List<Phone>? Phones { get; set; }

    public override string ToString()
    {
        return Name;
    }
}