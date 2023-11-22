namespace VisualStudioKeyExport
{
    struct Product
    {
        public string Name { get; }
        public string GUID { get; }
        public string MPC { get; }
        public Product(string Name, string GUID, string MPC)
        {
            this.Name = Name;
            this.GUID = GUID;
            this.MPC = MPC;
        }
    }
}