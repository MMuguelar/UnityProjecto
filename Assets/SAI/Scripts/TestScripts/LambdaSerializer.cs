using System;
using System.Linq.Expressions;
using Newtonsoft.Json;

public static class LambdaSerializer
{
    public static string Serialize<T>(Expression<Func<T>> expression)
    {
        return JsonConvert.SerializeObject(expression.ToString());
    }

}

//https://zzzcode.ai/answer-question?id=5378ad2b-449b-4ad2-9db6-abe103952ff4
//https://zzzcode.ai/answer-question?id=7ba1b2d2-8ad1-4804-92e9-01d416db6768
//https://zzzcode.ai/answer-question
//https://dynamic-linq.net/advanced-parse-lambda
//https://github.com/zzzprojects/System.Linq.Dynamic.Core