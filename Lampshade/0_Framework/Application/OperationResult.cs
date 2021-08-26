namespace _0_Framework.Application
{
    public class OperationResult
    {
        public string Message { get; set; }
        public bool IsSuccessed { get; set; }
        public OperationResult()
        {
            IsSuccessed = false;
        }

        public OperationResult Successed(string message = "عملیات با موفقیت انجام شد")
        {
            IsSuccessed = true;
            Message = message;
            return this;
        }
        public OperationResult Failed(string message)
        {
            IsSuccessed = false;
            Message = message;
            return this;
        }
    }
}
