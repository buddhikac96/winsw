using System;
using System.Collections.Generic;
using System.IO;
#if !NET20
using System.Linq;
#endif

namespace WinSW.Util
{
    public class YamlQuery
    {
        private readonly object yamlDic;
        private string? key;
        private object? current;

        public YamlQuery(object yamlDic)
        {
            this.yamlDic = yamlDic;
        }

        public YamlQuery On(string key)
        {
            this.key = key;
            this.current = this.Query<object>(this.current ?? this.yamlDic, this.key, null);
            return this;
        }

        public YamlQuery Get(string prop)
        {
            if (this.current == null)
            {
                throw new InvalidOperationException();
            }

            this.current = this.Query<object>(this.current, null, prop, this.key);
            return this;
        }

        private void Reset()
        {
            this.current = null;
            this.key = null;
        }

        private string LeafValue()
        {
            if (this.current == null)
            {
                throw new InvalidOperationException();
            }

            var result = this.current as List<object>;

            if (result == null)
            {
                throw new InvalidOperationException();
            }

            if (result.Count == 1)
            {
                var output = result[0] as string;
                return output == null ? string.Empty : output;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public bool ToBoolean()
        {
            try
            {
                return this.GetBoolean();
            }
            finally
            {
                this.Reset();
            }
        }

        public new string ToString()
        {
            try
            {
                return this.GetString();
            }
            finally
            {
                this.Reset();
            }
        }

        public List<T> ToList<T>()
        {
            try
            {
                return this.GetList<T>();
            }
            finally
            {
                this.Reset();
            }
        }

        private string GetString()
        {
            return this.LeafValue();
        }

        private bool GetBoolean()
        {
            var value = this.LeafValue().ToLower();
            if (value == "true" || value == "yes" || value == "on")
            {
                return true;
            }
            else if (value == "false" || value == "no" || value == "off")
            {
                return false;
            }
            else
            {
                throw new InvalidDataException(value + " cannot convert into bool");
            }
        }

        private List<T> GetList<T>()
        {
            if (this.current == null)
            {
                throw new InvalidOperationException();
            }
#if NET20
            var list = this.current as List<object>;

            if (list is null)
            {
                this.Reset();
                return new List<T>(0);
            }
            else
            {
                var result = new List<T>();
                foreach (var item in list)
                {
                    result.Add((T)item);
                }

                return result;
            }
#else
            return (this.current as List<object>).Cast<T>().ToList();
#endif
        }

        private IEnumerable<T> Query<T>(object dictionary, string? key, string? prop, string? fromKey = null)
        {
            var result = new List<T>();
            if (dictionary == null)
            {
                return result;
            }

            var dic = dictionary as IDictionary<object, object>;
            if (dic != null)
            {
#if NET20
                IDictionary<object, object> d = new Dictionary<object, object>();
                foreach (KeyValuePair<object, object> kvp in dic)
                {
                    d.Add(kvp);
                }
#else
                var d = dic.Cast<KeyValuePair<object, object>>();
#endif
                foreach (var dd in d)
                {
                    if (dd.Key as string == key)
                    {
                        if (prop == null)
                        {
                            result.Add((T)dd.Value);
                        }
                        else
                        {
                            result.AddRange(this.Query<T>(dd.Value, key, prop, dd.Key as string));
                        }
                    }
                    else if (fromKey == key && dd.Key as string == prop)
                    {
                        result.Add((T)dd.Value);
                    }
                    else
                    {
                        result.AddRange(this.Query<T>(dd.Value, key, prop, dd.Key as string));
                    }
                }

                return result;
            }

            var t = dictionary as IEnumerable<object>;
            if (t != null)
            {
                foreach (var tt in t)
                {
                    result.AddRange(this.Query<T>(tt, key, prop, key));
                }

                return result;
            }

            return result;
        }
    }
}
