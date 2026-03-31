namespace DevKit.Utils
{
    public struct Response
    {
        public bool IsSuccess { get; private set; }
        public readonly bool IsFail => !IsSuccess;
        public string ErrorMessage { get; private set; }

        public static Response Success()
        {
            return new Response
            {
                IsSuccess = true
            };
        }

        public static Response Fail()
        {
            return new Response
            {
                IsSuccess = false
            };
        }

        public static Response Fail(string message)
        {
            return new Response
            {
                IsSuccess = false,
                ErrorMessage = message
            };
        }
    }

    public struct Response<T>
    {
        public T Value { get; private set; }
        public bool IsSuccess { get; private set; }
        public readonly bool IsFail => !IsSuccess;
        public string ErrorMessage { get; private set; }

        public static Response<T> Success(T value)
        {
            return new Response<T>
            {
                IsSuccess = true,
                Value = value
            };
        }

        public static Response<T> Fail()
        {
            return new Response<T>
            {
                IsSuccess = false
            };
        }

        public static Response<T> Fail(string message)
        {
            return new Response<T>
            {
                IsSuccess = false,
                ErrorMessage = message
            };
        }
    }
}