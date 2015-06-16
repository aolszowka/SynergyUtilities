// -----------------------------------------------------------------------
// <copyright file="ExternalReference.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2015. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ParseListELB.DOM
{
    using System;
    using System.Xml.Serialization;

    public class ExternalReference : IEquatable<ExternalReference>
    {
        [XmlAttribute]
        public string Name { get; set; }

        public override int GetHashCode()
        {
            if (this.Name != null)
            {
                return this.Name.GetHashCode();
            }
            else
            {
                return 0;
            }
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ExternalReference);
        }

        public bool Equals(ExternalReference other)
        {
            if (other == null)
            {
                return false;
            }
            else if (this.GetHashCode() != other.GetHashCode())
            {
                return false;
            }
            else
            {
                return this.Name.Equals(other.Name);
            }
        }
    }
}
