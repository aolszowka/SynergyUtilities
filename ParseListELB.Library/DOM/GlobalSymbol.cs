// -----------------------------------------------------------------------
// <copyright file="GlobalSymbol.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2015. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ParseListELB.Library.DOM
{
    using System;
    using System.Xml.Serialization;

    public class GlobalSymbol : IEquatable<GlobalSymbol>
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Type { get; set; }

        [XmlElement]
        public string InitialValues { get; set; }

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
            return this.Equals(obj as GlobalSymbol);
        }

        public bool Equals(GlobalSymbol other)
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
                return
                    this.Name.Equals(other.Name) &&
                    this.InitialValues.Equals(other.InitialValues) &&
                    this.Type.Equals(other.Type);
            }

        }
    }
}
