using System;

public class CompanyService: ICompanyService
{

    private readonly IBaseEditableRepository<Company> _repository;
	public CompanyService(IBaseEditableRepository<Company> repository)
	{
        _repository = repository;
	}

    public async Task<IBaseResponse<Company>> Create(Company phone)
    {
        var baseResponse = new BaseResponse<Company>();
        baseResponse.Description = "Company created";
        baseResponse.StatusCode = StatusCode.OK;
        
        await _repository.Create(phone);
        return baseResponse;
    }

    public async Task<IBaseResponse<Company>> GetById(int id)
    {
        var baseResponse = new BaseResponse<Company>();
        try
        {
            baseResponse.Description = "Hello";
            baseResponse.StatusCode = StatusCode.OK;
            baseResponse.Data = await _repository.Get(id);
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


