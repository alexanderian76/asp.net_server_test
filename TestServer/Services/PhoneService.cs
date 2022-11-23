using System;

public class PhoneService: IPhoneService
{

    private readonly IPhoneRepository _repository;
	public PhoneService(IPhoneRepository repository)
	{
        _repository = repository;
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


