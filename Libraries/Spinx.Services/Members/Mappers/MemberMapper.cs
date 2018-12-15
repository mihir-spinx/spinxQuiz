using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using Spinx.Domain.Members;
using Spinx.Services.Members.DTOs;

namespace Spinx.Services.Members.Mappers
{
    public static class MemberMapper
    {
        public static void Init()
        {
            Mapper.AddMap<Member, MemberDto>(src =>
            {
                var memberViewModel = new MemberDto();
                memberViewModel.InjectFrom(src);
                memberViewModel.Password = null;

                return memberViewModel;
            });

            Mapper.AddMap<MemberDto, Member>((from, to) =>
            {
                var existing = to as Member ?? new Member();
                existing.InjectFrom(new LoopInjection(new[] { "Password" }), from);
                return existing;
            });
        }
    }
}
