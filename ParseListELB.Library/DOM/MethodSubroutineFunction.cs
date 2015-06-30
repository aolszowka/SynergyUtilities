// -----------------------------------------------------------------------
// <copyright file="MethodSubroutineFunction.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2015. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ParseListELB.Library.DOM
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class MethodSubroutineFunction : IEquatable<MethodSubroutineFunction>
    {
        private List<ExternalReference> externalReferences;

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Size { get; set; }

        [XmlAttribute]
        public string Position { get; set; }

        [XmlAttribute]
        public string ExternalReferenceCount { get; set; }

        [XmlAttribute]
        public string GlobalNameSize { get; set; }

        [XmlArray]
        public List<ExternalReference> ExternalReferences
        {
            get
            {
                return this.externalReferences;
            }
            set
            {
                this.externalReferences = value;
            }
        }

        public void AddExternalReference(ExternalReference externalReference)
        {
            if (this.externalReferences == null)
            {
                this.externalReferences = new List<ExternalReference>();
            }

            this.externalReferences.Add(externalReference);
        }

        public override int GetHashCode()
        {
            if (this.Name != null)
            {
                return this.Name.GetHashCode();
            }
            else
            {
                // This is a bad HashCode implementation; but in practice we
                // should never have an object that doesn't have Name defined.
                return 0;
            }
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as MethodSubroutineFunction);
        }

        public bool Equals(MethodSubroutineFunction other)
        {
            if (other == null)
            {
                return false;
            }
            else if (other.GetHashCode() != this.GetHashCode())
            {
                return false;
            }
            else
            {
                // WARNING: We make an assumption that if everything, with the
                // exception of the External References are equal then the obj
                // is equal.
                return
                    this.Name.Equals(other.Name) &&
                    this.Size.Equals(other.Size) &&
                    this.Position.Equals(other.Position) &&
                    this.ExternalReferenceCount.Equals(other.ExternalReferenceCount);
            }
        }
    }
}
