﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.TestObjects
{
    public class DerivedPolymorphicObject2Data : BasePolymorphicObjectData
    {
        public DerivedPolymorphicObject2Data()
            : base()
        { }

        public DerivedPolymorphicObject2Data(string name)
            : base(name, typeof(DerivedPolymorphicObject2))
        { }
    }
}
