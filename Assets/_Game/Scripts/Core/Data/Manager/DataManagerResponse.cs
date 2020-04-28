using Assets._Game.Scripts.Core.Api.Dto;

namespace Ibit.Core.Data.Manager
{
    public class DataManagerReponse<T, TU>
    {
        public ApiResponse<T> ApiResponse { get; set; }
        public TU LocalResponse { get; set; }
    }
}