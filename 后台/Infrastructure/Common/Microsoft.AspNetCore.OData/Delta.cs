// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Microsoft.AspNetCore.OData
{
    /// <summary>
    /// A class the tracks changes (i.e. the Delta) for an entity.
    /// </summary>
    [NonValidatingParameterBinding]
    public abstract class Delta : DynamicObject, IDelta
    {
        /// <summary>
        /// Clears the Delta and resets the underlying Entity.
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Attempts to set the Property called <paramref name="name"/> to the <paramref name="value"/> specified.
        /// <remarks>
        /// Only properties that exist on Entity can be set.
        /// If there is a type mismatch the request will fail.
        /// </remarks>
        /// </summary>
        /// <param name="name">The name of the Property</param>
        /// <param name="value">The new value of the Property</param>
        /// <returns>True if successful</returns>
        public abstract bool TrySetPropertyValue(string name, object value);

        /// <summary>
        /// Attempts to get the value of the Property called <paramref name="name"/> from the underlying Entity.
        /// <remarks>
        /// Only properties that exist on Entity can be retrieved.
        /// Both modified and unmodified properties can be retrieved.
        /// </remarks>
        /// </summary>
        /// <param name="name">The name of the Property</param>
        /// <param name="value">The value of the Property</param>
        /// <returns>True if the Property was found</returns>
        public abstract bool TryGetPropertyValue(string name, out object value);

        /// <summary>
        /// Attempts to get the <see cref="Type"/> of the Property called <paramref name="name"/> from the underlying Entity.
        /// <remarks>
        /// Only properties that exist on Entity can be retrieved.
        /// Both modified and unmodified properties can be retrieved.
        /// </remarks>
        /// </summary>
        /// <param name="name">The name of the Property</param>
        /// <param name="type">The type of the Property</param>
        /// <returns>Returns <c>true</c> if the Property was found and <c>false</c> if not.</returns>
        public abstract bool TryGetPropertyType(string name, out Type type);

        /// <summary>
        /// Overrides the DynamicObject TrySetMember method, so that only the properties
        /// of Entity can be set.
        /// </summary>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (binder == null)
            {
                throw Error.ArgumentNull("binder");
            }

            return TrySetPropertyValue(binder.Name, value);
        }

        /// <summary>
        /// Overrides the DynamicObject TryGetMember method, so that only the properties
        /// of Entity can be got.
        /// </summary>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder == null)
            {
                throw Error.ArgumentNull("binder");
            }

            return TryGetPropertyValue(binder.Name, out result);
        }

        /// <summary>
        /// Returns the Properties that have been modified through this Delta as an 
        /// enumeration of Property Names 
        /// </summary>
        public abstract IEnumerable<string> GetChangedPropertyNames();

        /// <summary>
        /// Returns the Properties that have not been modified through this Delta as an 
        /// enumeration of Property Names 
        /// </summary>
        public abstract IEnumerable<string> GetUnchangedPropertyNames();
    }

    public class Delta<T> : IDelta
    {
        private T _Value;
        private Type _Type = typeof(T);
        public Dictionary<string, object> ChangeProperty = new Dictionary<string, object>();
        public MethodInfo IsConvertDefault = typeof(Delta<T>).GetMethod(nameof(IsDefault));
        public Delta(T value)
        {
            _Value = value;
        }

        public void Patch(object obj, HttpContext httpContext)
        {
            ChangeProperties(obj, httpContext);
        }


        public void Patch(object obj)
        {
            ChangeProperties(obj);
        }

        

        public bool IsDefault<V>(V value)
        {
            V dv = default(V);

            if (typeof(V) is ValueType) return dv.Equals(value);

            return Object.Equals(dv, value);
        }

        /// <summary>
        /// 获取上传的属性名
        /// </summary>
        /// <param name="httpContent"></param>
        /// <returns></returns>
        public List<string> GetProperties(HttpContext httpContent)
        {
            List<string> list = new List<string>();

            if (httpContent == null) return list;

            httpContent.Request.Body.Position = 0;
            StreamReader reder = new StreamReader(httpContent.Request.Body);
            var bodyStr = reder.ReadToEnd();
            var jsonObj = JObject.Parse(bodyStr);
            foreach (var item in jsonObj)
            {
                list.Add(item.Key);
            }

            return list;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetChangedPropertyNames()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetUnchangedPropertyNames()
        {
            throw new NotImplementedException();
        }

        public bool TryGetPropertyType(string name, out Type type)
        {
            throw new NotImplementedException();
        }

        public bool TryGetPropertyValue(string name, out object value)
        {
            throw new NotImplementedException();
        }

        public bool TrySetPropertyValue(string name, object value)
        {
            throw new NotImplementedException();
        }



        private void ChangeProperties(object obj,HttpContext httpContext=null)
        {
            var sourceProperties = new List<PropertyInfo>(_Type.GetProperties());
            var targetProperties = new List<PropertyInfo>(obj.GetType().GetProperties());

            var keys =  GetProperties(httpContext);

            for (int i = 0; i < sourceProperties.Count; i++)
            {
                var property = sourceProperties[i];

                var value = property.GetValue(_Value);
                if (value == null) continue;
                bool trueValue = false;
                
                if(httpContext==null) trueValue = (bool)IsConvertDefault.MakeGenericMethod(property.PropertyType).Invoke(this, new object[] { value });

                if (trueValue == true) continue;

                //var name = property.Name.ToLower();

                if (httpContext!=null) trueValue = keys.Any(x =>string.Equals(x,property.Name, StringComparison.OrdinalIgnoreCase));

                if (trueValue ==false) continue;

                var targetProperty = targetProperties.FirstOrDefault(x => x.Name == property.Name);// && x.PropertyType == property.PropertyType);

                if (targetProperty == null) continue;

                ChangeProperty.Add(targetProperty.Name, targetProperty.GetValue(obj));

                targetProperty.SetValue(obj, value);
            }
        }
    }
}