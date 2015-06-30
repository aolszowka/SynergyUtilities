// -----------------------------------------------------------------------
// <copyright file="LinkedELB.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2015. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ParseListELB.Library.DOM
{
    using System.Xml.Serialization;

    public class LinkedELB
    {
        [XmlAttribute]
        public string Name { get; set; }
    }
}
