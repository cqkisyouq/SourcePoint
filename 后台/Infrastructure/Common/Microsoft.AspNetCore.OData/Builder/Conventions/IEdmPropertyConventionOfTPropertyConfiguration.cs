﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.OData.Builder.Conventions
{
    /// <summary>
    /// Convention to process properties of <see cref="StructuralTypeConfiguration"/>.
    /// </summary>
    /// <typeparam name="TPropertyConfiguration"></typeparam>
    internal interface IEdmPropertyConvention<TPropertyConfiguration> : IEdmPropertyConvention where TPropertyConfiguration : PropertyConfiguration
    {
        /// <summary>
        /// Applies the convention.
        /// </summary>
        /// <param name="edmProperty">The property the convention is applied on.</param>
        /// <param name="structuralTypeConfiguration">The <see cref="StructuralTypeConfiguration"/> the edmProperty belongs to.</param>
        /// <param name="model">The <see cref="ODataConventionModelBuilder"/>that contains the type this property is being applied to.</param>
        void Apply(TPropertyConfiguration edmProperty, StructuralTypeConfiguration structuralTypeConfiguration,
            ODataConventionModelBuilder model);
    }
}
