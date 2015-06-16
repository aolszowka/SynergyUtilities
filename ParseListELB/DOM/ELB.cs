// -----------------------------------------------------------------------
// <copyright file="ELB.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2015. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ParseListELB.DOM
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Domain Object Model for ELB Information.
    /// </summary>
    public class ELB
    {
        private List<MethodSubroutineFunction> methods;
        private List<MethodSubroutineFunction> subroutineFunctions;
        private List<GlobalSymbol> globalSymbols;
        private List<LinkedELB> linkedELBs;

        [XmlAttribute]
        public string Name { get; set; }
        
        [XmlAttribute]
        public string Path { get; set; }

        [XmlAttribute]
        public string StructureVersion { get; set; }

        [XmlArray]
        public List<MethodSubroutineFunction> Methods
        {
            get
            {
                return this.methods;
            }
            set
            {
                this.methods = value;
            }
        }

        [XmlArray]
        public List<MethodSubroutineFunction> SubroutineFunctions
        {
            get
            {
                return this.subroutineFunctions;
            }
            set
            {
                this.subroutineFunctions = value;
            }
        }

        [XmlArray]
        public List<GlobalSymbol> GlobalSymbols
        {
            get
            {
                return this.globalSymbols;
            }
            set
            {
                this.globalSymbols = value;
            }
        }

        [XmlArray]
        public List<LinkedELB> LinkedELBs
        {
            get
            {
                return this.linkedELBs;
            }
            set
            {
                this.linkedELBs = value;
            }
        }

        public void AddMethod(MethodSubroutineFunction method)
        {
            if (this.methods == null)
            {
                this.methods = new List<MethodSubroutineFunction>();
            }

            this.methods.Add(method);
        }

        public void AddSubroutineFunction(MethodSubroutineFunction subroutineFunction)
        {
            if (this.subroutineFunctions == null)
            {
                this.subroutineFunctions = new List<MethodSubroutineFunction>();
            }

            this.subroutineFunctions.Add(subroutineFunction);
        }

        public void AddGlobalSymbol(GlobalSymbol globalSymbol)
        {
            if (this.globalSymbols == null)
            {
                this.globalSymbols = new List<GlobalSymbol>();
            }

            this.globalSymbols.Add(globalSymbol);
        }

        public void AddLinkedELB(LinkedELB linkedELB)
        {
            if (this.linkedELBs == null)
            {
                this.linkedELBs = new List<LinkedELB>();
            }

            this.linkedELBs.Add(linkedELB);
        }
    }
}
