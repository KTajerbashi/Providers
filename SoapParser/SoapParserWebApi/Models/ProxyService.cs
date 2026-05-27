using SoapParserWebApi.Extensions;
using System.Reflection;

namespace SoapParserWebApi.Models
{
    public class ProxyMember
    {

        #region Property

        public string Name { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }
        public bool IsHidden { get; set; }

        #endregion

        #region Ctors

        public ProxyMember(string Name, Type Type) { this.Name = Name; this.Type = Type; }
        public ProxyMember(string Name, Type Type, object Value) : this(Name, Type) { this.Value = Value; }
        public ProxyMember(string Name, Type Type, object Value, bool isHidden) : this(Name, Type) { this.Value = Value; this.IsHidden = isHidden; }
        public ProxyMember(string Name, object Value) : this(Name, Value.GetType(), Value) { }

        #endregion

    }
    public class ProxyMethod
    {
        #region Properties

        //private const string ErrorTypeMessage = "Unknown method parameter {0} type : {1}. Parameter type must be in generic types";
        private const string ErrorTypeMessage = "نوع پارامتر تابع {2} ناشناخته است {0}  : {1}\r\n نوع پارامتر باید از انواع عمومی سیستم باشد";

        public string Name { get; set; }
        public Type Type { get; set; }

        public List<ProxyMember> ServiceResults { get; set; }
        public List<ProxyMember> ServiceParameters { get; set; }

        #endregion

        #region Ctors

        public ProxyMethod(MethodInfo info)
        {
            this.Name = info.Name;
            this.Type = info.ReturnType;

            this.ServiceResults = new List<ProxyMember>();
            this.ServiceParameters = new List<ProxyMember>();

            this.FindParameters(info);
            this.FindResults(info);
        }

        #endregion

        #region Methods

        private void FindParameters(MethodInfo info)
        {
            foreach (var item in info.GetParameters())
            {
                var isOutParameter = item.IsOut;
                if (isOutParameter)
                    continue;

                string TypeName = item.ParameterType.ToString().ToLower();
                Type pType = item.ParameterType;
                if ((
                    pType.IsByRef || item.IsOut ///////////////////////////////////////////////
                    || pType.IsEnum || pType.IsInterface)
                    ||
                    (!TypeName.StartsWith("system.") && TypeName.EndsWith("[]")) ||
                    (TypeName.StartsWith("system.") && TypeName.EndsWith("[]")))
                {
                    // skip to next param
                    //throw new Exception(string.Format(ErrorTypeMessage, item.Name, item.ParameterType.ToString(), info.Name));
                }
                else if (!TypeName.StartsWith("system."))
                {
                    foreach (var prop in pType.GetProperties(BindingFlags.Public))
                    {
                        Type propType = prop.PropertyType;
                        if ((pType.IsByRef || pType.IsEnum || pType.IsInterface) ||
                            (!TypeName.StartsWith("system.")) ||
                            (TypeName.StartsWith("system.") && TypeName.EndsWith("[]")))
                            throw new Exception(string.Format(ErrorTypeMessage, item.Name, item.ParameterType.ToString(), info.Name));
                        else this.ServiceParameters.Add(new ProxyMember(prop.Name, prop.PropertyType));
                    }
                }
                else this.ServiceParameters.Add(new ProxyMember(item.Name, item.ParameterType));
            }
        }

        private void FindResults(MethodInfo info)
        {
            if (!info.ReturnType.ToString().ToLower().StartsWith("system."))
            {
                Type T = info.ReturnType;
                if (T.IsArray) T = T.GetEnumeratedType();
                foreach (var item in T.GetProperties())
                {
                    string TypeName = item.PropertyType.ToString().ToLower();
                    Type pType = item.PropertyType;
                    if (pType.IsByRef || pType.IsEnum || pType.IsInterface)
                    {
                        throw new Exception(string.Format(ErrorTypeMessage, item.Name, item.PropertyType.ToString(), info.Name));
                    }
                    else if (TypeName.StartsWith("system.") && !TypeName.EndsWith("[]"))
                    {
                        this.ServiceResults.Add(new ProxyMember(item.Name, item.PropertyType));
                    }
                    else if (!TypeName.StartsWith("system.") && !TypeName.EndsWith("[]"))
                    {
                        this.ServiceResults.Clear();
                        foreach (var inneritem in item.PropertyType.GetProperties())
                        {
                            if (pType.IsByRef || pType.IsEnum || pType.IsInterface)
                            {
                                throw new Exception(string.Format(ErrorTypeMessage, item.Name, item.PropertyType.ToString(), info.Name));
                            }
                            else
                            {
                                this.ServiceResults.Add(new ProxyMember(inneritem.Name, inneritem.PropertyType));
                            }
                        }

                        return;
                    }
                    else
                    {
                        // TODO: ex: system.string[] ro ham support kon
                        throw new Exception(string.Format(ErrorTypeMessage, item.Name, item.PropertyType.ToString(), info.Name));
                    }
                }
            }
            //else if (info.GetParameters().Any(x => x.IsOut))
            //{
            //    var subjectParamName = $"{info.Name}Result";
            //    var outParamAsReturnType = info.GetParameters().SingleOrDefault(x => x.IsOut && x.Name == subjectParamName);

            //    if (outParamAsReturnType != null)
            //    {
            //        Type T = outParamAsReturnType.ParameterType;
            //        if (T.IsArray) T = T.GetEnumeratedType();
            //        foreach (var item in T.GetProperties())
            //        {
            //            string TypeName = item.PropertyType.ToString().ToLower();
            //            Type pType = item.PropertyType;
            //            if (pType.IsByRef || pType.IsEnum || pType.IsInterface)
            //            {
            //                throw new Exception(string.Format(ErrorTypeMessage, item.Name, item.PropertyType.ToString(), info.Name));
            //            }
            //            else if (TypeName.StartsWith("system.") && !TypeName.EndsWith("[]"))
            //            {
            //                this.ServiceResults.Add(new ProxyMember(item.Name, item.PropertyType));
            //            }
            //            else if (!TypeName.StartsWith("system.") && !TypeName.EndsWith("[]"))
            //            {
            //                this.ServiceResults.Clear();
            //                foreach (var inneritem in item.PropertyType.GetProperties())
            //                {
            //                    if (pType.IsByRef || pType.IsEnum || pType.IsInterface)
            //                    {
            //                        throw new Exception(string.Format(ErrorTypeMessage, item.Name, item.PropertyType.ToString(), info.Name));
            //                    }
            //                    else
            //                    {
            //                        this.ServiceResults.Add(new ProxyMember(inneritem.Name, inneritem.PropertyType));
            //                    }
            //                }

            //                return;
            //            }
            //            else
            //            {
            //                throw new Exception(string.Format(ErrorTypeMessage, item.Name, item.PropertyType.ToString(), info.Name));
            //            }

            //        }
            //    }

            //}
        }

        #endregion

    }
    public class ProxyService
    {

        #region Properties

        private Assembly OutputAssembly { get; set; }
        public string ServiceName { get; set; }
        public byte[] ServiceFile { get; set; }

        public Dictionary<string, ProxyMethod> ServiceMethods { get; set; }

        #endregion

        #region Ctors

        public ProxyService(byte[] ServiceFile, string ServiceName, Assembly OutputAssembly)
        {
            this.ServiceFile = ServiceFile;
            this.ServiceName = ServiceName;
            this.OutputAssembly = OutputAssembly;
            this.ServiceMethods = new Dictionary<string, ProxyMethod>();

            this.FindMethods(this.OutputAssembly.GetType(ServiceName));
        }

        public ProxyService(byte[] ServiceFile, string ServiceName) :
            this(ServiceFile, ServiceName, Assembly.Load(ServiceFile))
        { }

        #endregion

        #region Methods

        private void FindMethods(Type T)
        {
            var methods = T.GetMethods();
            foreach (var item in methods)
            {
                if (item.Name == "Discover") break;
                this.ServiceMethods.Add(item.Name, new ProxyMethod(item));
            }
        }

        #endregion

    }
}
