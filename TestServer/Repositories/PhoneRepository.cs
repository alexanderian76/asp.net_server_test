using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.EntityFrameworkCore;

public class PhoneRepository: IPhoneRepository
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
        return await _db.Phones.FirstAsync(x => x.Id == id);
    }
}


public interface IPhoneRepository : IBaseRepository<Phone>
{
    new Task<Phone> Get(int id);
    new Task Create(Phone phone);

}