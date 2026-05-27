using System;

namespace SoapParserWCF.Exceptions
{
    [Serializable]
    public class AppException : Exception
    {
        public AppException() { }
        public AppException(string message) : base(message) { }
        public AppException(string message, Exception inner) : base(message, inner) { }
        protected AppException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class BusinessValidationException : AppException
    {
        public BusinessValidationException() { }
        public BusinessValidationException(string message) : base(message) { }
        public BusinessValidationException(string message, Exception inner) : base(message, inner) { }
        protected BusinessValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class ParameterMappingException : BusinessValidationException
    {
        public string ParameterName { get; set; }

        public ParameterMappingException(string parameterName) { this.ParameterName = parameterName; }
        public ParameterMappingException(string parameterName, string message) : base(message) { this.ParameterName = parameterName; }
        public ParameterMappingException(string parameterName, string message, Exception inner) : base(message, inner) { this.ParameterName = parameterName; }
        protected ParameterMappingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}