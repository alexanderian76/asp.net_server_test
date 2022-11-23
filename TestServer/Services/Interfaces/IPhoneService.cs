using System;

public interface IPhoneService
{
	Task<IBaseResponse<Phone>> GetById(int id);
	Task<IBaseResponse<Phone>> Create(Phone phone);
}


