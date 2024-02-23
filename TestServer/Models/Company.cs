using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

public class Company
{
    public Company()
    {
        Phones = new List<Phone>();
    }
    public int Id { get; set; }
    [Column("MyName")]
    public string Name { get; set; }


    public ICollection<Phone> Phones { get; }

    public override string ToString()
    {
        return Name;
    }
}