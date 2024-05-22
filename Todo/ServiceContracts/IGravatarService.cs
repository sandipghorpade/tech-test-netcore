using System.Threading.Tasks;
using System;

namespace Todo.ServiceContracts
{
    public interface IGravatarService
    {
        Task<Tuple<string, string>> GetProfileInformation(string emailAddress);
    }
}
