// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Concurrent;
using Microsoft.OData.Edm;

namespace Microsoft.AspNetCore.OData.Query.Expressions
{
    using System;
    using System.Linq;

    /// <summary>
    /// EntityFramework does not let you inject non primitive constant values (like IEdmModel) in Select queries. Primitives like strings and guids can be
    /// injected as they can be translated into a SQL query. This container associates a unique string with each EDM model, so that, given the string the model
    /// can be retrieved later.
    /// </summary>
    internal static class ModelContainer
    {
        private static ConcurrentDictionary<IEdmModel, string> _map = new ConcurrentDictionary<IEdmModel, string>();
        private static ConcurrentDictionary<string, IEdmModel> _reverseMap = new ConcurrentDictionary<string, IEdmModel>();

        public static string GetModelID(IEdmModel model)
        {
            string index = _map.GetOrAdd(model, m => Guid.NewGuid().ToString());
            _reverseMap.TryAdd(index, model);
            return index;
        }

        public static IEdmModel GetModel(string id)
        {
            return _reverseMap[id];
        }

        public static IEdmModel GetModel(Type type)
        {
            IEdmModel model=null;
            var name = type.Name;
            foreach (var item in _reverseMap)
            {
               var itemModel= item.Value.SchemaElements.FirstOrDefault(x => x.SchemaElementKind == EdmSchemaElementKind.TypeDefinition && string.Equals(name, x.Name, StringComparison.CurrentCultureIgnoreCase));
                if (itemModel != null)
                {
                    model = item.Value;
                    break;
                }
            }

            return model;
        }
    }
}
