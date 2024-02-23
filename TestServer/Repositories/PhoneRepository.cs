using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.EntityFrameworkCore;

public class PhoneRepository: IBaseEditableRepository<Phone>
{
    private readonly MobileContext _db;
	public PhoneRepository(MobileContext db)
	{
        _db = db;
	}

    public async Task Create(Phone obj)
    {
        
        await _db.Phones.AddAsync(obj);
        await _db.SaveChangesAsync();
       
    }

    public async Task<Phone> Get(int id)
    {
        var phone = await _db.Phones.Include(p => p.Company).FirstAsync(x => x.Id == id);
        //await _db.Entry(phone).Reference(p => p.Company).LoadAsync();
       // phone.Company = await _db.Companies.Select(x =>  new Company() { Id = x.Id, Name = x.Name}).FirstAsync(x => x.Id == phone.CompanyId);
        return phone;
    }
}


public interface IPhoneRepository : IBaseEditableRepository<Phone>
{
    new Task<Phone> Get(int id);
    new Task Create(Phone phone);

}