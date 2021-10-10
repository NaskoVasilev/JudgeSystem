using System.Collections.Generic;
using System.Linq;

namespace JudgeSystem.Common.Models
{
    public class Result
    {
        public Result()
        {
            Errors = new List<string>();
        }

        public Result(IEnumerable<string> errors)
        {
            Errors = errors.ToList();
        }

        public bool Succeeded => Errors == null || Errors.Count() == 0;

        public List<string> Errors { get; set; }

        public static Result Success() => new Result();

        public static Result Failure(IEnumerable<string> errors) => new Result(errors);

        public static Result Failure(params string[] errors) => new Result(errors);

        public static implicit operator Result(string value) => new Result(new List<string>() { value });

        public static implicit operator Result(List<string> errors) => new Result(errors);
    }
}
