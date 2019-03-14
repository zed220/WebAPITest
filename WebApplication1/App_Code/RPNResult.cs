namespace WebAPIBooks.App_Code {
    public class RPNResult {
        public bool IsValid { get; }
        public double Result { get; }

        RPNResult() { }
        RPNResult(double result) {
            IsValid = true;
            Result = result;
        }


        public static RPNResult CreateError() {
            return new RPNResult();
        }
        public static RPNResult Create(double result) {
            return new RPNResult(result);
        }
    }
}