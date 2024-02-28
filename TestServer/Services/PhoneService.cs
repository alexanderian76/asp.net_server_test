using System;
using System.Diagnostics;

public class PhoneService: IPhoneService
{

    private readonly IBaseEditableRepository<Phone> _repository;
    private readonly IBaseEditableRepository<Company> _repositoryCompany;
    public PhoneService(IBaseEditableRepository<Phone> repository, IBaseEditableRepository<Company> repositoryCompany)
	{
        _repository = repository;
        _repositoryCompany = repositoryCompany;
	}

    public async Task<IBaseResponse<Phone>> Create(Phone phone)
    {
        var baseResponse = new BaseResponse<Phone>();
        baseResponse.Description = "Phone created";
        baseResponse.StatusCode = StatusCode.OK;
        
        await _repository.Create(phone);
        return baseResponse;
    }

    public async Task<IBaseResponse<Phone>> GetById(int id)
    {
        var baseResponse = new BaseResponse<Phone>();
        Console.WriteLine("GET BY ID");
        try
        {
            baseResponse.Description = "Hello";
            baseResponse.StatusCode = StatusCode.OK;
            baseResponse.Data = await _repository.Get(id);
            //baseResponse.Data.Company = await _repositoryCompany.Get(baseResponse.Data.CompanyId);
            return baseResponse;
        }
        catch(Exception)
        {
            baseResponse.Description = "Fuck u no data";
            baseResponse.StatusCode = StatusCode.InternalServerError;
            return baseResponse;
        }
    }
}


