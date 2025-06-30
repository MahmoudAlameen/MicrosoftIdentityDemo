namespace AM_Supplement.Contracts.ResultModel
{
    public class ResultModel
    {
        public bool IsValid {  get; set; }
        public List<string> ErrorMessages {  get; set; }

    }
    public class ResultModel<T> : ResultModel
    {
       public T Model {  get; set; }
    }
    public class ResultList<T> : ResultModel
    {
        public List<T> ModelList { get; set; }
        public int TotalPages { get; set; }
       
    }
}
