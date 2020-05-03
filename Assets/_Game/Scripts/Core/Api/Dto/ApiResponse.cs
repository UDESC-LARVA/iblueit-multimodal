namespace Assets._Game.Scripts.Core.Api.Dto
{
    public class ApiResponse<T>
    {
        public T Result { get; set; }

        public bool Success { get; set; }
    }
}