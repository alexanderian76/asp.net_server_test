using System;

public interface ICompanyService
{
	Task<IBaseResponse<Company>> GetById(int id);
	Task<IBaseResponse<Company>> Create(Company phone);
}


