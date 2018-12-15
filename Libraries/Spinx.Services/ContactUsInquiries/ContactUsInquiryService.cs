using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.ContactUsInquiries;
using Spinx.Domain.ContactUsInquiries;
using Spinx.Services.ContactUsInquiries.DTOs;
using Spinx.Services.ContactUsInquiries.Validators;
using Spinx.Services.Infrastructure;

namespace Spinx.Services.ContactUsInquiries
{
    public interface IContactUsInquiryService
    {
        Result SaveValidation(ContactUsDto dto);
        Result Save(ContactUsDto dto);
    }

    public class ContactUsInquiryService : IContactUsInquiryService
    {
        private readonly IContactUsInquiryRepository _contactUsInquiryRepository;
        private readonly ContactUsValidator _contactUsValidator;
        private readonly IUnitOfWork _unitOfWork;

        public ContactUsInquiryService(
            IContactUsInquiryRepository contactUsInquiryRepository,
            ContactUsValidator contactUsValidator,
            IUnitOfWork unitOfWork)
        {
            _contactUsInquiryRepository = contactUsInquiryRepository;
            _contactUsValidator = contactUsValidator;
            _unitOfWork = unitOfWork;
        }

        public Result SaveValidation(ContactUsDto dto)
        {
            return _contactUsValidator.ValidateResult(dto);
        }

        public Result Save(ContactUsDto dto)
        {
            var result = new Result();
            var entity = Mapper.Map<ContactUsInquiry>(dto);

            _contactUsInquiryRepository.Insert(entity);
            _unitOfWork.Commit();

            result.Data = dto.Email;
            result.SetSuccess("Thank you for contacting us. We will get back to you soon.");

            return result;
        }
    }
}