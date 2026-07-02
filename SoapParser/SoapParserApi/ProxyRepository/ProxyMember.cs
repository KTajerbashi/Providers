using System;

namespace SoapParserApi.ProxyRepository
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
}
