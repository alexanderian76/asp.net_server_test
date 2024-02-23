using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.EntityFrameworkCore;

public class CompanyRepository: IBaseEditableRepository<Company>
{
    private readonly MobileContext _db;
	public CompanyRepository(MobileContext db)
	{
        _db = db;
	}

    public async Task Create(Company obj)
    {
        await _db.Companies.AddAsync(obj);
        
        await _db.SaveChangesAsync();
    }

    public async Task<Company> Get(int id)
    {
        var company = await _db.Companies.Include(t => t.Phones).FirstAsync(x => x.Id == id);
        
        return company;
        //return await _db.Companies.Select(x => new Company() { Id = x.Id, Name = x.Name, Phones = x.Phones }).FirstAsync(x => x.Id == id);
    }
}


public interface ICompanyRepository : IBaseEditableRepository<Company>
{
    new Task<Company> Get(int id);
    new Task Create(Company phone);

}