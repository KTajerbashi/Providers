using SoapParserApi.Exceptions;
using SoapParserApi.Extensions;
using SoapParserApi.ProxyRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace SoapParserApi.ProxyServices
{
    public class Proxy
    {

        #region Properties

        public Assembly OutputAssembly { get; set; }
        public string ServiceName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ServiceUri { get; set; }

        #endregion

        #region Ctors

        public Proxy(byte[] ServiceFile, string ServiceUri, string ServiceName, string Username, string Password)
        {
            this.ServiceName = ServiceName;
            this.Username = Username;
            this.Password = Password;
            this.ServiceUri = ServiceUri;
            this.OutputAssembly = Assembly.Load(ServiceFile);
        }

        #endregion

        #region Methods

        public List<ProxyResult> InvokeMethod(string MethodName)
        {
            return this.InvokeMethod(MethodName, null);
        }

        public List<ProxyResult> InvokeMethod(string MethodName, params ProxyMember[] Parameters)
        {
            List<ProxyResult> result = new List<ProxyResult>();

            var objService = this.OutputAssembly.GetType(this.ServiceName);
            var newobj = Activator.CreateInstance(objService);

            if (this.Username.IsNotEmpty())
            {
                PropertyInfo piClientCreds = objService.GetProperty("Credentials");
                piClientCreds.SetValue(newobj, (ICredentials)new NetworkCredential(this.Username, this.Password));
            }

            var method = objService.GetMethod(MethodName);
            var param = MapParameters(method, Parameters);

            object retobj;
            try
            {
                retobj = method.Invoke(newobj, param);
            }
            catch (TargetInvocationException tex)
            {
                if (!(tex.InnerException is WebException wex))
                    throw;

                if (!(wex.InnerException is SocketException sex))
                    throw;

                if (sex.SocketErrorCode == SocketError.TimedOut)
                {
                    throw new AppException($"امکان اتصال و فراخوانی وب سرویس '{this.ServiceName}.{MethodName}' وجود ندارد");
                }

                throw;
            }

            if (retobj == null)
            {
                throw new AppException("اشکالی در دریافت پاسخ از وب سرویس رخ داده است");
            }

            string retType = method.ReturnType.ToString().ToLower();
            if (retType.StartsWith("system."))
            {
                if (retType.EndsWith("[]"))
                {
                    ProxyResult member = new ProxyResult(retType);
                    foreach (var item in (Array)retobj)
                        member.Members.Add("value", new ProxyMember("value", item.GetType(), item));
                    result.Add(member);
                }
                else if (retType == "system.void") return result;
                else
                {
                    ProxyResult member = new ProxyResult(retType);
                    member.Members.Add("value", new ProxyMember("value", retobj.GetType(), retobj));
                    result.Add(member);
                }
            }
            else
            {
                if (retType.EndsWith("[]"))
                {
                    foreach (var item in (Array)retobj)
                    {
                        ProxyResult member = new ProxyResult(item.GetType());

                        foreach (var prop in item.GetType().GetProperties())
                            member.Members.Add(prop.Name, new ProxyMember(prop.Name, prop.PropertyType, prop.GetValue(item)));

                        result.Add(member);
                    }
                }
                else
                {
                    ProxyResult member = new ProxyResult(retType);
                    foreach (var prop in retobj.GetType().GetProperties())
                        member.Members.Add(prop.Name, new ProxyMember(prop.Name, prop.PropertyType, prop.GetValue(retobj)));
                    result.Add(member);
                }
            }

            return result;
        }

        private object[] MapParameters(MethodBase method, ProxyMember[] namedParameters)
        {
            var paraminfo = method.GetParameters();

            if (paraminfo.Length == 1 && !paraminfo[0].ParameterType.ToString().ToLower().StartsWith("system."))
            {
                var paramtype = paraminfo[0].ParameterType;
                var newobj = Activator.CreateInstance(paramtype);

                PropertyInfo[] Parameters = newobj.GetType().GetProperties();
                if (namedParameters != null)
                    foreach (var item in namedParameters)
                    {
                        var paramName = item.Name;
                        Type T = Parameters.First(r => r.Name == paramName).PropertyType;

                        try
                        {
                            Parameters.SetValue(CastTo(T, item.Value), 0);
                        }
                        catch (FormatException fex)
                        {
                            throw new FormatException($"خطا در تبدیل نوع پارامترها ({fex.Message}): paramName:{item.Name} - paramValue:{item.Value}");
                        }
                    }
                //CreateMapInnerProperties(ref Parameters, namedParameters);
                return new[] { newobj };
            }
            else return CreateMapPatameters(paraminfo, namedParameters);
        }

        private object[] CreateMapPatameters(ParameterInfo[] Parameters, ProxyMember[] namedParameters)
        {
            string[] paramNames = Parameters.Select(p => p.Name.ToLower()).ToArray();
            object[] parameters = new object[paramNames.Length];

            for (int i = 0; i < parameters.Length; ++i)
                parameters[i] = Type.Missing;

            if (namedParameters != null)
            {
                foreach (var item in namedParameters)
                {
                    var paramName = item.Name.ToLower();
                    var paramIndex = Array.IndexOf(paramNames, paramName);

                    Type destinationType = null;
                    try
                    {
                        destinationType = Parameters.First(r => r.Name.ToLower() == paramName).ParameterType;
                        parameters[paramIndex] = CastTo(destinationType, item.Value);
                    }
                    catch (FormatException fex)
                    {
                        throw new ParameterMappingException(item.Name, $"خطا در نگاشت پارامترها به وب سرویس مقصد : ({fex.Message}). فیلد '{item.Name}' با مقدار '{item.Value}' به نوع مقصد '{destinationType?.ToString() ?? "--"}' قابل نگاشت نمی باشد");
                        //throw new FormatException($"خطا در نگاشت پارامترها به وب سرویس مقصد : ({fex.Message}). فیلد '{item.Name}' با مقدار '{item.Value}' به نوع مقصد '{destinationType?.ToString() ?? "--"}' قابل نگاشت نمی باشد");
                    }
                    catch (OverflowException ovex)
                    {
                        throw new ParameterMappingException(item.Name, $"خطا در نگاشت پارامترها به وب سرویس مقصد : ({ovex.Message}). فیلد '{item.Name}' با مقدار '{item.Value}' بزرگتر از نوع مقصد می باشد :'{destinationType?.ToString() ?? "--"}'", ovex);
                    }
                    catch (Exception ex)
                    {
                        throw new ParameterMappingException(item.Name, $"خطا در نگاشت پارامترها به وب سرویس مقصد : ({ex.Message}). فیلد '{item.Name}' با مقدار '{item.Value}'. نوع مقصد: '{destinationType?.ToString() ?? "--"}'", ex);
                    }
                }
            }
            return parameters;
        }

        private void CreateMapInnerProperties(ref PropertyInfo[] Properties, ProxyMember[] namedParameters)
        {
            if (namedParameters != null)
                foreach (var item in namedParameters)
                {
                    var paramName = item.Name;
                    Type T = Properties.First(r => r.Name == paramName).PropertyType;
                    Properties.SetValue(CastTo(T, item.Value), 0);
                }
        }


        private object CastTo(Type t, object value)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null ||
                    value.ToString() == "null")
                    return null;
                else
                    return Convert.ChangeType(value, Nullable.GetUnderlyingType(t));
            }
            else return Convert.ChangeType(value, t);
        }

        #endregion

    }
}
